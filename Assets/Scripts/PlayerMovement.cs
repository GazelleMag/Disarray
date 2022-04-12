using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class PlayerMovement : NetworkBehaviour
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

  public float movementSpeed = 6f;
  public float turnSmoothTime = 0.1f;
  float turnSmoothVelocity;
  private bool thirdPersonCamera;

  void Start()
  {
    cam = GameObject.Find("Main Camera");
    vcam = GameObject.Find("Third Person Camera");
    lcam = GameObject.Find("Lock On Camera");
    scam = GameObject.Find("State Driven Camera");
    freeLookCam = vcam.GetComponent<CinemachineFreeLook>();
    lockOnCam = lcam.GetComponent<CinemachineVirtualCamera>();
    cinemachineAnimator = scam.GetComponent<Animator>();
    thirdPersonCamera = false;

    if (isLocalPlayer)
    {
      freeLookCam.LookAt = transform;
      freeLookCam.Follow = transform;
      lockOnCam.Follow = transform;
    }
  }

  void Update()
  {
    if (isLocalPlayer)
    {
      Move();
      if (Input.GetKeyDown("space"))
      {
        SwitchState();
      }
    }

  }

  void Move()
  {
    Vector3 inputDirection = GetPlayerInputDirection();

    if (inputDirection.magnitude >= 0.1f)
    {
      Vector3 camDirection = GetPlayerCamDirection(inputDirection);
      characterController.Move(camDirection.normalized * movementSpeed * Time.deltaTime);
      animationController.WalkingAnimTransition();
    }
    else
    {
      animationController.IdleAnimTransition();
    }
  }

  Vector3 GetPlayerInputDirection()
  {
    float horizontal = Input.GetAxisRaw("Horizontal");
    float vertical = Input.GetAxisRaw("Vertical");
    return new Vector3(horizontal, 0f, vertical).normalized;
  }

  Vector3 GetPlayerCamDirection(Vector3 inputDirection)
  {
    float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
    transform.localRotation = Quaternion.Euler(0f, angle, 0f);
    return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
  }

  private void SwitchState()
  {
    if (thirdPersonCamera)
    {
      cinemachineAnimator.Play("ThirdPersonCamera");
    }
    else
    {
      cinemachineAnimator.Play("LockOnCamera");
    }
    thirdPersonCamera = !thirdPersonCamera;
  }

}
