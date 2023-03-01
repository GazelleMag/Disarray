using UnityEngine;
// this scrip is to make the health bar always point to the camera
public class Billboard : MonoBehaviour
{
  public Transform cam;

  void Start()
  {
    cam = GameObject.Find("Main Camera").transform;
  }

  void LateUpdate()
  {
    transform.LookAt(transform.position + cam.forward);
  }
}
