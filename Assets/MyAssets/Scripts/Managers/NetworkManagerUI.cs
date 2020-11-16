#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

[RequireComponent(typeof(NetworkManager))]
public class NetworkManagerUI : MonoBehaviour
{
    NetworkManager manager;
    [SerializeField]
    Text InfoText;

    InputManager inputManager;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }
    
    void Start() {
        inputManager = InputManager.Instance;
    }

    public void LocalHost(){
        manager.StartHost();
        Cursor.lockState = CursorLockMode.Locked;
        InfoText.text = "Local Host Mode";
    }

    public void ConnectSercer(){
        manager.StartClient();
        Cursor.lockState = CursorLockMode.Locked;
        InfoText.text = "Connected to server";
    }

    public void ExitGame(){
        Application.Quit();
    }

    
}
