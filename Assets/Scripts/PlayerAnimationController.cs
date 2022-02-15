using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
  public Animator playerAnimator;

  void Update()
  {
    transform.localPosition = Vector3.down;
  }

  void Start()
  {
    playerAnimator = GetComponent<Animator>();
  }

  public void IdleAnimTransition()
  {
    playerAnimator.SetBool("Walking", false);
  }

  public void WalkingAnimTransition()
  {
    playerAnimator.SetBool("Walking", true);
  }
}
