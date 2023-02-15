using UnityEngine;
using Mirror;
using Cinemachine;
using System.Runtime.InteropServices;
using System.Collections;

public class PlayerController : NetworkBehaviour
{
  public CharacterController characterController;
  public PlayerAnimationController animationController;
  private Animator cinemachineAnimator;

  public GameObject cam;
  public GameObject vcam;
  public GameObject lcam;
  public GameObject scam;
  private CinemachineFreeLook freeLookCam;
  private CinemachineVirtualCamera lockOnCam;
  private bool lockOnCamera;

  public CinemachineTargetGroupManager targetGroupManager;
  private Transform lockedTargetTransform;

  private PlayerManager playerManager;

  private float movementSpeed = 3f;
  private float turnSmoothTime = 0.1f;
  float turnSmoothVelocity;
  [SyncVar]
  public int health = 100;
  private bool canGetPunched = true;
  public Transform leftHandHitbox, rightHandHitbox;
  public float handHitboxRange = 0.075f;

  void Start()
  {
    if (isLocalPlayer)
    {
      InitializeCameraProperties();
    }
  }

  void Update()
  {
    if (isLocalPlayer)
    {
      Move();
      if (Input.GetKeyDown("space"))
      {
        SwitchCamera();
        GetLockedTarget();
      }

      if (lockOnCamera)
      {
        RotateTowardsLockedTarget();
      }

      if (Input.GetMouseButtonDown(0))
      {
        if (lockOnCamera)
        {
          Punch();
        }

      }

      if (animationController.punchingAnimationIsPlaying())
      {
        CmdEnableHandColliders();
      }
      else
      {
        CmdDisableHandColliders();
      }
      Debug.Log("Current health: " + health);
    }

  }

  // MOVEMENT
  void Move()
  {
    Vector3 inputDirection = GetPlayerInputDirection();

    if (inputDirection.magnitude >= 0.1f)
    {
      Vector3 camDirection = GetPlayerCamDirection(inputDirection);
      characterController.Move(camDirection.normalized * movementSpeed * Time.deltaTime);
      animationController.Walking();
    }
    else
    {
      animationController.NotWalking();
    }
  }

  Vector3 GetPlayerInputDirection()
  {
    float horizontal = Input.GetAxisRaw("Horizontal");
    float vertical = Input.GetAxisRaw("Vertical");

    if (lockOnCamera)
    {
      HandleWalkingStanceAnimations(horizontal, vertical);
    }

    return new Vector3(horizontal, 0f, vertical).normalized;
  }

  private void RotateTowardsLockedTarget()
  {
    if (lockedTargetTransform)
    {
      Vector3 direction = lockedTargetTransform.position - transform.position;
      Quaternion rotation = Quaternion.LookRotation(direction);
      transform.rotation = rotation;
    }
  }

  // CAMERA
  private void InitializeCameraProperties()
  {
    cam = GameObject.Find("Main Camera");
    vcam = GameObject.Find("Third Person Camera");
    lcam = GameObject.Find("Lock On Camera");
    scam = GameObject.Find("State Driven Camera");

    freeLookCam = vcam.GetComponent<CinemachineFreeLook>();
    lockOnCam = lcam.GetComponent<CinemachineVirtualCamera>();

    cinemachineAnimator = scam.GetComponent<Animator>();
    lockOnCamera = false;

    freeLookCam.LookAt = transform;
    freeLookCam.Follow = transform;
    lockOnCam.Follow = transform;
  }

  private Vector3 GetPlayerCamDirection(Vector3 inputDirection)
  {
    float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
    transform.localRotation = Quaternion.Euler(0f, angle, 0f);
    return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
  }

  private void SwitchCamera()
  {
    lockOnCamera = !lockOnCamera;
    if (lockOnCamera)
    {
      cinemachineAnimator.Play("LockOnCamera");
      animationController.Stance();
      movementSpeed = 1f;
    }
    else
    {
      cinemachineAnimator.Play("ThirdPersonCamera");
      animationController.NotStance();
      movementSpeed = 3f;
    }
  }

  private void GetLockedTarget() // this needs to be changed for multiple targets
  {
    for (int i = 0; i < targetGroupManager.targetGroup.m_Targets.Length; i++)
    {
      lockedTargetTransform = targetGroupManager.targetGroup.m_Targets[i].target.transform;
    }
  }

  // NETWORKING
  public override void OnStartServer()
  {
    playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
    playerManager.AddPlayer(connectionToClient);
  }

  public override void OnStopServer()
  {
    playerManager.RemovePlayer(connectionToClient);
  }

  public override void OnStartClient()
  {
    targetGroupManager = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroupManager>();
    if (!isLocalPlayer)
    {
      SetPlayerLayer("Enemy");
      targetGroupManager.AddPlayer(transform);
    }
  }

  public override void OnStopClient()
  {
    targetGroupManager = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroupManager>();
    if (!isLocalPlayer)
    {
      targetGroupManager.RemovePlayer(transform);
    }
  }

  // ANIMATIONS
  void HandleWalkingStanceAnimations(float horizontal, float vertical)
  {
    if (vertical > 0) { animationController.WalkingForward(); }
    if (vertical < 0) { animationController.WalkingBackwards(); }
    if (horizontal > 0) { animationController.WalkingRight(); }
    if (horizontal < 0) { animationController.WalkingLeft(); }
  }

  // COMBAT
  public void Punch()
  {
    animationController.Punching();
  }

  [Command]
  private void CmdEnableHandColliders()
  {
    RpcEnableHandColliders();
  }

  [Command]
  private void CmdDisableHandColliders()
  {
    RpcDisableHandColliders();
  }

  [ClientRpc]
  private void RpcEnableHandColliders()
  {
    leftHandHitbox.gameObject.GetComponent<SphereCollider>().enabled = true;
    rightHandHitbox.gameObject.GetComponent<SphereCollider>().enabled = true;
  }

  [ClientRpc]
  private void RpcDisableHandColliders()
  {
    leftHandHitbox.gameObject.GetComponent<SphereCollider>().enabled = false;
    rightHandHitbox.gameObject.GetComponent<SphereCollider>().enabled = false;
  }

  [Client]
  private void TakeDamage(int damage)
  {
    CmdSyncHP(damage);
  }

  [Command]
  private void CmdSyncHP(int damage)
  {
    health -= damage;
  }

  IEnumerator WaitToGetPunched()
  {
    yield return new WaitForSeconds(1f);
    canGetPunched = true;
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (isLocalPlayer && canGetPunched)
    {
      canGetPunched = false;
      TakeDamage(10);
      StartCoroutine(WaitToGetPunched()); // doing this so a punch doesn't trigger collision twice
    }
  }

  // MISC
  private void SetPlayerLayer(string layerName)
  {
    int layer = LayerMask.NameToLayer(layerName);
    gameObject.layer = layer;
    if (!isClientOnly) // this distinction must be made because find game objects with tag on server finds every player's hand hitboxes
    {
      int connectionId = gameObject.GetComponent<NetworkIdentity>().connectionToClient.connectionId;
      SetPlayerHandHitboxesLayer(layer, connectionId);
    }
    else
    {
      SetPlayerHandHitboxesLayer(layer);
    }
  }

  private void SetPlayerHandHitboxesLayer(int layer, [Optional] int connectionId)
  {
    GameObject[] hitboxes = GameObject.FindGameObjectsWithTag("Hitbox");
    if (connectionId != 0)
    {
      foreach (GameObject hitbox in hitboxes)
      {
        if (connectionId == hitbox.transform.root.gameObject.GetComponent<NetworkIdentity>().connectionToClient.connectionId)
        {
          hitbox.gameObject.layer = layer;
        }
      }
    }
    else
    {
      foreach (GameObject hitbox in hitboxes)
      {
        hitbox.gameObject.layer = layer;
      }
    }
  }
}
