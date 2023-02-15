using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
  public static List<NetworkConnection> playerConnectionList = new List<NetworkConnection>();

  public void AddPlayer(NetworkConnection connection)
  {
    playerConnectionList.Add(connection);
  }

  public void RemovePlayer(NetworkConnection connection)
  {
    playerConnectionList.Remove(connection);
  }

  public List<NetworkConnection> GetPlayerConnections()
  {
    return playerConnectionList;
  }
}

