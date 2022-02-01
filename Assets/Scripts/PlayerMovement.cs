using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class PlayerMovement : NetworkBehaviour
{
  public CharacterController characterController;
  public GameObject cam;
  //public GameObject vcam;

  public float movementSpeed = 6f;

  void Start()
  {
    cam = GameObject.Find("Main Camera");
    Debug.Log(cam);
    //vcam = transform.Find("Third Person Camera").gameObject;
  }

  void Update()
  {
    if (isLocalPlayer)
    {
      Move();
    }

  }

  void Move()
  {
    Vector3 inputDirection = GetPlayerInputDirection();

    if (inputDirection.magnitude >= 0.1f)
    {
      Vector3 camDirection = GetPlayerCamDirection(inputDirection);
      characterController.Move(camDirection.normalized * movementSpeed * Time.deltaTime);
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
    transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
    return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
  }

}
