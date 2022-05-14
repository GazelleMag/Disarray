using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : MonoBehaviour
{
  public static List<GameObject> playersList = new List<GameObject>();

  public void AddPlayer(GameObject player)
  {
    playersList.Add(player);
  }

  public void RemovePlayer(GameObject player)
  {
    playersList.Remove(player);
  }
}

