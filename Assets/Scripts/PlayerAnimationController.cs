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

  public void WalkingLeft()
  {
    playerAnimator.SetBool("WalkingLeft", true);
  }

  public void WalkingRight()
  {
    playerAnimator.SetBool("WalkingLeft", false);
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
