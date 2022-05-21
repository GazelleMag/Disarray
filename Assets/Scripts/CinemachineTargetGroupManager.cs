using UnityEngine;
using Cinemachine;
using Mirror;

public class CinemachineTargetGroupManager : MonoBehaviour
{
  public CinemachineTargetGroup targetGroup;

  void Start()
  {
    targetGroup = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroup>();
  }

  public void AddPlayer(Transform playerTransform)
  {
    targetGroup.AddMember(playerTransform, 1f, 0f);
  }

  public void RemovePlayer(Transform playerTransform)
  {
    targetGroup.RemoveMember(playerTransform);
  }
}
