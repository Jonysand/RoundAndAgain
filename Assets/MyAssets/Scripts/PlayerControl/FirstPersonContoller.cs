using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;
using UnityEngine.UI;

public class FirstPersonContoller : NetworkBehaviour
{
    // --------
    // Movement
    // --------
    private CharacterController controller;
    private Vector3 playerVelocity;
    [SerializeField]
    float playerSpeed = 2.0f;
    float runningScaler = 2.0f;
    InputManager inputManager;
    CinemachineVirtualCamera virtualCamera;

    // ---------
    // Animation
    // ---------
    Animator animator;
    int isRunnningHash;
    // float velocity = 0f;
    int isMovingHash;
    int velocityHash;
    int velocityXHash;
    int velocityYHash;
    public Image IdentityColor;
    public Text IdentityText;
    public BillboardText IdentityLable = null;
    CanvasGroup MainMenu = null;

    // ------
    // Player
    // ------
    Toggle isAdminUI = null;
    GameObject minimap = null;
    public GameObject miniMapSpot = null;
    float MapRealRatio = 242f/26f;
    int MatID;

    // -----
    // Admin
    // -----
    


    void Awake() {
        animator = GetComponentInChildren<Animator>();
        isAdminUI = GameObject.FindGameObjectWithTag("AdminToggle").GetComponent<Toggle>();
        minimap = GameObject.FindGameObjectWithTag("minimap");
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        isMovingHash = Animator.StringToHash("isMoving");
        velocityHash = Animator.StringToHash("Velocity");
        velocityXHash = Animator.StringToHash("VelocityX");
        velocityYHash = Animator.StringToHash("VelocityY");
        IdentityColor = GameObject.FindGameObjectWithTag("Identity").GetComponentInChildren<Image>();
        IdentityText = GameObject.FindGameObjectWithTag("Identity").GetComponentInChildren<Text>();
        MainMenu = GameObject.FindGameObjectWithTag("MainMenu").GetComponentInChildren<CanvasGroup>();
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if(isLocalPlayer)
            CommandSendPlayerInfos(isAdminUI.isOn);
    }

    private void Start()
    {
        // ---------------
        // synchronization
        // ---------------
        MainMenu.interactable = false;
        if(isLocalPlayer){
            LocalPlayerInit();
            if(isAdminUI.isOn){
                InitAdmin();
                InitAdminOnServer();
            }
        }
        SyncAllPlayers();
    }

    void Update()
    {
        // -------
        // MiniMap
        // -------
        if(miniMapSpot!=null && minimap.activeInHierarchy){
            SyncMinimap(miniMapSpot);
        }

        // is admin
        if(transform.localScale == Vector3.zero){
            IdentityLable.text = "";
            if(miniMapSpot){
                miniMapSpot.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
                miniMapSpot = null;
            }
        }

        // ---------------------
        // Local Player Specific
        // ---------------------
        if(!isLocalPlayer) return;
        // --------
        // movement
        // --------
        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = Camera.main.transform.forward * movement.y + Camera.main.transform.right * movement.x;
        move.y = 0f;

        // ---------
        // animation
        // ---------
        if(movement.magnitude > 0){
            animator.SetBool(isMovingHash, true);
            if(inputManager.isRun()) move *= runningScaler;
        }
        else{
            animator.SetBool(isMovingHash, false);
        }
        Vector3 moveLocal = transform.InverseTransformVector(move);
        animator.SetFloat(velocityXHash, moveLocal.x);
        animator.SetFloat(velocityYHash, moveLocal.z);
        controller.Move(move * Time.deltaTime * playerSpeed);

        // ------
        // cursor
        // ------
        if(inputManager.ShowMenu()){
            if(Cursor.visible == false){
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                virtualCamera.enabled = false;
                SetUIVisible(MainMenu, true);
            }else{
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                virtualCamera.enabled = true;
                SetUIVisible(MainMenu, false);
            }
        }
    }

    private void OnDestroy() {
        int matIndex;
        if(GameManager.Instance.Dict_PlayerID_MatID.TryGetValue(netId, out matIndex)){
            GameManager.Instance.MatIndexList.Add(matIndex);
            GameManager.Instance.Dict_PlayerID_MatID.Remove(netId);
        }
        if(isLocalPlayer)
            CommandRemovePlayer(matIndex, netId);
    }

    void LocalPlayerInit(){
        GetComponentInChildren<CinemachinePOVExtension>().enabled = true;
        virtualCamera.enabled = true;
        GetComponentInChildren<AudioListener>().enabled = true;
        GetComponentInChildren<AudioSource>().enabled = false;
        GetComponent<InteractionController>().enabled = true;
        minimap.GetComponent<CanvasGroup>().alpha = 0;
        GameManager.Instance.LocalPlayer = gameObject;
        // Hide UI
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SetUIVisible(MainMenu, false);
    }

    void InitAdmin(){
        gameObject.layer = 10;
        gameObject.tag = "Admin";
        transform.position = virtualCamera.transform.position;
        transform.localScale = Vector3.zero;
        if(miniMapSpot){
            miniMapSpot.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            miniMapSpot = null;
        }
        IdentityColor.color = new Color(0f, 0f, 0f, 0f);
        IdentityText.text = "";
        IdentityLable.text = "";
        miniMapSpot = null;
        transform.GetChild(0).gameObject.SetActive(false);
        minimap.GetComponent<CanvasGroup>().alpha = 1;
        SetUIVisible(GameObject.FindGameObjectWithTag("AdminControl").GetComponent<CanvasGroup>(), true);
    }

    [Command]
    void InitAdminOnServer(){
        InitAdmin();
    }

    void SyncMinimap(GameObject spot){
        Vector3 pos = spot.GetComponent<RectTransform>().anchoredPosition;
        pos.x = transform.position.z * MapRealRatio;
        pos.y = transform.position.x * -MapRealRatio;
        miniMapSpot.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    // ---------------
    // Admin Functions
    // ---------------
    [Command]
    public void CommandShowKillerMap(int playerID, bool isShow){
        uint playerNetID = 0;
        foreach (KeyValuePair<uint, int> pair in GameManager.Instance.Dict_PlayerID_MatID){
            if(pair.Value == playerID){
                playerNetID = pair.Key;
                break;
            }
        }
        if(playerNetID == 0) return;
        foreach (GameObject playerObj in GameObject.FindGameObjectsWithTag("Player")){
            FirstPersonContoller fc = playerObj.GetComponent<FirstPersonContoller>();
            if(fc.netId == playerNetID){
                fc.RPCShowKillerMap(isShow);
                return;
            }
        }
    }

    [TargetRpc]
    public void RPCShowKillerMap(bool isShow){
        if(isShow)
            minimap.GetComponent<CanvasGroup>().alpha = 1;
        else
            minimap.GetComponent<CanvasGroup>().alpha = 0;
    }

    [Command]
    public void CommandShowLable(bool isShow){
        RPCShowLable(isShow);
    }
    [ClientRpc]
    public void RPCShowLable(bool isShow){
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            player.GetComponent<FirstPersonContoller>().IdentityLable.show = isShow;
    }


    void SetUIVisible(CanvasGroup UIgroup, bool show){
        if(show){
            UIgroup.alpha = 1;
            UIgroup.blocksRaycasts = true;
        }else{
            UIgroup.alpha = 0;
            UIgroup.blocksRaycasts = false;
        }
    }


    public void SyncAllPlayers(){
        List<GameObject> PlayerList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        foreach (GameObject playerObject in PlayerList)
        {
            int matIndex;
            if(GameManager.Instance.Dict_PlayerID_MatID.TryGetValue(playerObject.GetComponent<FirstPersonContoller>().netId, out matIndex)){
                string name = GameManager.Instance.NameList[matIndex];
                Material mat = GameManager.Instance.MatList[matIndex];
                playerObject.GetComponentInChildren<SkinnedMeshRenderer>().material = mat;
                GameManager.Instance.MinimapSpots[matIndex].GetComponent<Image>().color = mat.GetColor("_BaseColor");
                playerObject.GetComponent<FirstPersonContoller>().miniMapSpot = GameManager.Instance.MinimapSpots[matIndex];
                playerObject.GetComponent<FirstPersonContoller>().IdentityLable.text = name;
                // UI update
                if(playerObject.GetComponent<FirstPersonContoller>().isLocalPlayer){
                    playerObject.GetComponent<FirstPersonContoller>().IdentityColor.color = mat.GetColor("_BaseColor");
                    playerObject.GetComponent<FirstPersonContoller>().IdentityText.text = name;
                }
            }
        }
    }

    [Command]
    void CommandSendPlayerInfos(bool isAdmin){
        foreach (KeyValuePair<uint, int> pair in GameManager.Instance.Dict_PlayerID_MatID){
            RpcReceivePlayerInfos(pair.Key, pair.Value);
        }

        if(GameManager.Instance.MatIndexList.Count > 0 && !isAdmin){
            int matIndex = GameManager.Instance.MatIndexList[0];
            GameManager.Instance.MatIndexList.RemoveAt(0);
            GameManager.Instance.Dict_PlayerID_MatID.Add(netId, matIndex);
            RpcAddPlayer(matIndex, netId);
        }
    }

    [TargetRpc]
    void RpcReceivePlayerInfos(uint NetID, int MatID){
        GameManager.Instance.MatIndexList.Remove(MatID);
        GameManager.Instance.Dict_PlayerID_MatID.Add(NetID, MatID);
    }


    [Command]
    void CommandAddPlayer(int matID, uint NetID){
        if(!GameManager.Instance.Dict_PlayerID_MatID.ContainsKey(NetID)){
            GameManager.Instance.Dict_PlayerID_MatID.Add(NetID, matID);
            GameManager.Instance.MatIndexList.Remove(matID);
        }
        RpcAddPlayer(matID, NetID);
    }

    [ClientRpc]
    void RpcAddPlayer(int matID, uint NetID){
        if(!GameManager.Instance.Dict_PlayerID_MatID.ContainsKey(NetID)){
            GameManager.Instance.Dict_PlayerID_MatID.Add(NetID, matID);
            GameManager.Instance.MatIndexList.Remove(matID);
        }
        SyncAllPlayers();
    }

    [Command]
    void CommandRemovePlayer(int matID, uint NetID){
        if(!GameManager.Instance.MatIndexList.Contains(matID)){
            GameManager.Instance.Dict_PlayerID_MatID.Remove(key: NetID);
            GameManager.Instance.MatIndexList.Add(matID);
            GameManager.Instance.MatIndexList.Sort();
        }
        RpcRemovePlayer(matID, NetID);
    }

    [ClientRpc]
    void RpcRemovePlayer(int matID, uint NetID){
        if(!GameManager.Instance.MatIndexList.Contains(matID)){
            GameManager.Instance.Dict_PlayerID_MatID.Remove(key: NetID);
            GameManager.Instance.MatIndexList.Add(matID);
            GameManager.Instance.MatIndexList.Sort();
        }
        SyncAllPlayers();
    }
}
