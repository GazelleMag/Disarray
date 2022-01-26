using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public CharacterController characterController;
  public Transform cam;

  public float movementSpeed = 6f;

  void Update()
  {
    float horizontal = Input.GetAxisRaw("Horizontal");
    float vertical = Input.GetAxisRaw("Vertical");
    Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

    if (direction.magnitude >= 0.1f)
    {
      float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
      transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

      Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
      characterController.Move(moveDirection.normalized * movementSpeed * Time.deltaTime);
    }
  }
}
