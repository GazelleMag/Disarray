using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
  public Animator playerAnimator;

  void Start()
  {
    playerAnimator = GetComponent<Animator>();
  }

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

  /*public void IdleAnimTransition()
  {
    playerAnimator.SetBool("Walking", false);
  }

  public void WalkingAnimTransition()
  {
    playerAnimator.SetBool("Walking", true);
  }

  public void StanceAnimTransition()
  {
    playerAnimator.SetBool("Stance", true);
  }

  public void NoStanceAnimTransition()
  {
    playerAnimator.SetBool("Stance", false);
  }*/
}
