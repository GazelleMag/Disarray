using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
  public Animator playerAnimator;

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
