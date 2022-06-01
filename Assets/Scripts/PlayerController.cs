using UnityEngine;
using Mirror;
using Cinemachine;

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
  public override void OnStartClient()
  {
    playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
    targetGroupManager = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroupManager>();
    if (!isLocalPlayer)
    {
      playerManager.AddPlayer(gameObject);
      targetGroupManager.AddPlayer(transform);
    }
  }

  public override void OnStopClient()
  {
    playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
    targetGroupManager = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroupManager>();
    if (!isLocalPlayer)
    {
      playerManager.RemovePlayer(gameObject);
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

}
