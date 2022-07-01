using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : MonoBehaviour
{
  public static List<int> playerConnectionIds = new List<int>();

  public void AddPlayer(int connectionId)
  {
    playerConnectionIds.Add(connectionId);
  }

  public void RemovePlayer(int connectionId)
  {
    playerConnectionIds.Remove(connectionId);
  }

  public List<int> GetPlayersList()
  {
    return playerConnectionIds;
  }
}

