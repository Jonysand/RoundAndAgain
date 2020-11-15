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

    public override void OnStartHost()
    {
        base.OnStartHost();
        Debug.Log("Host Started");
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        Debug.Log("Server Stopped");
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server Started");
    }
    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        Debug.Log("Server Ready: " + conn.address + ";");
    }
}
