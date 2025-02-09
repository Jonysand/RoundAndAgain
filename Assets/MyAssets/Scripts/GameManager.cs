﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{   
    // -----
    // Admin
    // -----
    // Map Control
    public List<int> MatIndexList = new List<int>();
    public Dictionary<uint, int> Dict_PlayerID_MatID = new Dictionary<uint, int>();
    public ToggleGroup ShowMapToggles = null;
    public void ShowMap(int playerID){
        if(playerID < Dict_PlayerID_MatID.Count)
            LocalPlayer.GetComponent<FirstPersonContoller>().CommandShowKillerMap(playerID, ShowMapToggles.AnyTogglesOn());
    }

    public Toggle ShowLableToggle = null;
    public void ShowNames(){
        LocalPlayer.GetComponent<FirstPersonContoller>().CommandShowLable(ShowLableToggle.isOn);
    }

    int State = 0; // 0:Morning; 1:Evening; 2:Night

    [Header("Morning Time")]
    int atTableCount = 0;

    [Header("Evening Time")]
    int inRoomCount = 0;

    [Header("Night Time")]
    float NightTimer = 0f;
    float NightDuration = 60f;
    [SyncVar]public int KillerID = 0;
    public List<Material> MatList = new List<Material>(6);
    public List<string> NameList;
    public GameObject LocalPlayer = null;
    public List<GameObject> MinimapSpots;

    private static GameManager _instance;
    public static GameManager Instance{
        get{
            return _instance;
        }
    }
    private void Awake() {
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }
        else{
            _instance = this;
        }
    }

    private void Update() {
        switch (State)
        {
            // ------------
            // Morning Time
            // ------------
            case 0:
                if(atTableCount >= 6){ // all people at the table
                    MorningEnds();
                }
                break;
            
            // ------------
            // Evening Time
            // ------------
            case 1:
                if(inRoomCount >= 6){ // all people in the room
                    EveningEnds();
                }
                break;
            
            // ------------
            // Night Time
            // ------------
            case 2:
                NightTimer += Time.deltaTime;
                if(NightTimer >= NightDuration){ // the night ends
                    NightEnds();
                }
                break;
            default:
                break;
        }
    }

    void MorningEnds(){
        State = 1;
        inRoomCount = 0;
    }
    void EveningEnds(){
        State = 2;
    }
    void NightEnds(){
        State = 0;
        atTableCount = 0;
    }
}
