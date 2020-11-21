#pragma warning disable 0649
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
    [SerializeField] BillboardText IdentityLable = null;
    CanvasGroup MainMenu = null;

    // ------
    // Player
    // ------
    Toggle isAdminUI = null;
    public bool isAdmin = false;
    GameObject minimap = null;
    public GameObject miniMapSpot = null;
    float MapRealRatio = 242f/26f;
    Material mat;

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
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
    }

    private void Start()
    {
        // ---------------
        // synchronization
        // ---------------
        SyncAllPlayers();
        if(!isAdmin) isAdmin = isAdminUI.isOn;
        if(isLocalPlayer){
            LocalPlayerInit();
            if(isAdmin){
                InitAdmin();
                InitAdminOnServer();
            }
        }
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
        if(miniMapSpot){
            miniMapSpot.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            miniMapSpot = null;
        }
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

    void SyncAllPlayers(){
        List<GameObject> PlayerList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        // remove admins
        for(int i = 0; i < PlayerList.Count;){
            if (PlayerList[i].transform.localScale == Vector3.zero){
                PlayerList.RemoveAt(i);
            }else i++;
        }
        // players full
        if(PlayerList.Count >= GameManager.Instance.MatList.Count) isAdmin = true;
        else{
            for (int i = 0; i < PlayerList.Count; i++){
                // sync material
                mat = GameManager.Instance.MatList[i];
                PlayerList[i].GetComponentInChildren<SkinnedMeshRenderer>().material = mat;
                // connect corresponding spots on the minimap
                GameManager.Instance.MinimapSpots[i].GetComponent<Image>().color = mat.GetColor("_BaseColor");
                PlayerList[i].GetComponent<FirstPersonContoller>().miniMapSpot = GameManager.Instance.MinimapSpots[i];
                PlayerList[i].GetComponent<FirstPersonContoller>().IdentityLable.text = System.Char.ConvertFromUtf32((65+i)).ToString();
                // UI update
                if(PlayerList[i].GetComponent<FirstPersonContoller>().isLocalPlayer){
                    PlayerList[i].GetComponent<FirstPersonContoller>().IdentityColor.color = mat.GetColor("_BaseColor");
                    PlayerList[i].GetComponent<FirstPersonContoller>().IdentityText.text = System.Char.ConvertFromUtf32((65+i)).ToString();
                }
            }
        }
        GameManager.Instance.Players = PlayerList;
    }
    void SyncMinimap(GameObject spot){
        Vector3 pos = spot.GetComponent<RectTransform>().anchoredPosition;
        pos.x = transform.position.z * MapRealRatio;
        pos.y = transform.position.x * -MapRealRatio;
        miniMapSpot.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    [Command]
    public void CommandShowKillerMap(int playerID, bool isShow){
        GameManager.Instance.Players[playerID].GetComponent<FirstPersonContoller>().RPCShowKillerMap(isShow);
    }

    [TargetRpc]
    public void RPCShowKillerMap(bool isShow){
        if(isShow)
            minimap.GetComponent<CanvasGroup>().alpha = 1;
        else
            minimap.GetComponent<CanvasGroup>().alpha = 0;
    }

    void SetUIVisible(CanvasGroup UIgroup, bool show){
        if(show){
            UIgroup.alpha = 1;
            UIgroup.interactable = true;
            UIgroup.blocksRaycasts = true;
        }else{
            UIgroup.alpha = 0;
            UIgroup.interactable = false;
            UIgroup.blocksRaycasts = false;
        }
    }
}
