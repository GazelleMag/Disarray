using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineSwitcher : MonoBehaviour
{
  private Animator cinemachineAnimator;
  private bool thirdPersonCamera = true;

  private void Awake()
  {
    cinemachineAnimator = GetComponent<Animator>();
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
    Debug.Log("Im clicking!");
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
