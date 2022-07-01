using UnityEngine;
using Mirror;
using Cinemachine;
using System.Collections.Generic;

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

  private float movementSpeed = 3f;
  private float turnSmoothTime = 0.1f;
  float turnSmoothVelocity;
  private bool lockOnCamera;

  public CinemachineTargetGroupManager targetGroupManager;
  private Transform lockedTargetTransform;
  private PlayerManager playerManager;

  [SyncVar]
  public int health = 100;

  //
  //public GameObject projectile;
  //float projectileSpeed = 100f;

  void Start()
  {
    InitializeCameraProperties();
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
        Debug.Log("Punching!");
        animationController.Punching();
      }
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

    if (isLocalPlayer)
    {
      freeLookCam.LookAt = transform;
      freeLookCam.Follow = transform;
      lockOnCam.Follow = transform;
    }
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
      //CmdTakeDamage(10);
      //CmdInflictDamage();
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
    playerManager.AddPlayer(connectionToClient.connectionId);
  }

  public override void OnStopServer()
  {
    playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
    playerManager.RemovePlayer(connectionToClient.connectionId);
  }

  public override void OnStartClient()
  {
    //playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
    targetGroupManager = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroupManager>();
    if (!isLocalPlayer)
    {
      targetGroupManager.AddPlayer(transform);
    }
  }

  public override void OnStopClient()
  {
    //playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
    targetGroupManager = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroupManager>();
    if (!isLocalPlayer)
    {
      targetGroupManager.RemovePlayer(transform);
    }
  }

  /*public void TakeDamage(int damage)
  {
    if (isServer)
    {
      health -= damage;
      Debug.Log(connectionToClient + " current health: " + health);
    }
    else
    {
      CmdTakeDamage(damage);
    }
  }

  [Command]
  public void CmdTakeDamage(int damage)
  {
    TakeDamage(damage);
  }

  [Command]
  public void CmdInflictDamage()
  {
    List<int> playerConnectionIds = playerManager.GetPlayersList();
    foreach (var playerConnectionId in playerConnectionIds)
    {
      if (connectionToClient.connectionId != playerConnectionId)
      {
        Debug.Log(playerConnectionId + " is my target!");
      }
    }

  }*/

  // ANIMATIONS
  void HandleWalkingStanceAnimations(float horizontal, float vertical)
  {
    if (vertical > 0) { animationController.WalkingForward(); }
    if (vertical < 0) { animationController.WalkingBackwards(); }
    if (horizontal > 0) { animationController.WalkingRight(); }
    if (horizontal < 0) { animationController.WalkingLeft(); }
  }

  //
  /*private void Shoot()
  {

    GameObject tempProjectile = Instantiate(projectile, transform.position, transform.rotation);
    Physics.IgnoreCollision(tempProjectile.GetComponent<Collider>(), GetComponent<Collider>());
    Rigidbody tempRigidBodyProjectile = tempProjectile.GetComponent<Rigidbody>();
    tempRigidBodyProjectile.AddForce(tempRigidBodyProjectile.transform.forward * projectileSpeed);
    Destroy(tempProjectile, 2f);
  }*/

}
