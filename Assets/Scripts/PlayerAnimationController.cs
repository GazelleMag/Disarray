using UnityEngine;
using Mirror;

public class PlayerAnimationController : NetworkBehaviour
{
  public Animator playerAnimator;
  public NetworkAnimator networkAnimator;

  public void Walking()
  {
    playerAnimator.SetBool("Walking", true);
  }

  public void NotWalking()
  {
    playerAnimator.SetBool("Walking", false);
  }

  public void Stance()
  {
    playerAnimator.SetBool("Stance", true);
  }

  public void NotStance()
  {
    playerAnimator.SetBool("Stance", false);
  }

  public void WalkingForward()
  {
    playerAnimator.SetBool("WalkingForward", true);
    playerAnimator.SetBool("WalkingBackwards", false);
    playerAnimator.SetBool("WalkingRight", false);
    playerAnimator.SetBool("WalkingLeft", false);
  }

  public void WalkingBackwards()
  {
    playerAnimator.SetBool("WalkingForward", false);
    playerAnimator.SetBool("WalkingBackwards", true);
    playerAnimator.SetBool("WalkingRight", false);
    playerAnimator.SetBool("WalkingLeft", false);
  }

  public void WalkingRight()
  {
    playerAnimator.SetBool("WalkingForward", false);
    playerAnimator.SetBool("WalkingBackwards", false);
    playerAnimator.SetBool("WalkingRight", true);
    playerAnimator.SetBool("WalkingLeft", false);
  }

  public void WalkingLeft()
  {
    playerAnimator.SetBool("WalkingForward", false);
    playerAnimator.SetBool("WalkingBackwards", false);
    playerAnimator.SetBool("WalkingRight", false);
    playerAnimator.SetBool("WalkingLeft", true);
  }

  public void Punching()
  {
    networkAnimator.SetTrigger("Punching");
  }

  public bool punchingAnimationIsPlaying()
  {
    return networkAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName("Punching") ? true : false;
  }
}
