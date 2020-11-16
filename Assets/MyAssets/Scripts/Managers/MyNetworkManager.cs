#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class MyNetworkManager : NetworkManager
{
    
    public override void OnStartClient()
    {
        base.OnStartClient();
        // playerPrefab.GetComponentInChildren<CinemachineFreeLook>().enabled = true;
        // playerPrefab.GetComponentInChildren<CinemachineCollider>().enabled = true;
    }
    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        
        // GameManager.Instance.spawnedPlayerID += 1;
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        Debug.Log(GameObject.FindGameObjectsWithTag("Player").Length);
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log(GameObject.FindGameObjectsWithTag("Player").Length);
    }
}
