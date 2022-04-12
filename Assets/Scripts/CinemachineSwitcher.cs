using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineSwitcher : MonoBehaviour
{
  private Animator cinemachineAnimator;
  private bool thirdPersonCamera = true;
  private CinemachineStateDrivenCamera stateDrivenCam;
  private CinemachineVirtualCamera lockOnCam;
  private GameObject player;

  private void Awake()
  {
    cinemachineAnimator = GetComponent<Animator>();
    stateDrivenCam = GetComponent<CinemachineStateDrivenCamera>();
    lockOnCam = stateDrivenCam.GetComponentInChildren<CinemachineVirtualCamera>();
  }

  void Start()
  {

    player = GameObject.Find("Player");
    lockOnCam.Follow = player.transform;
  }

  void Update()
  {
    if (Input.GetKeyDown("space"))
    {
      SwitchState();
    }
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
