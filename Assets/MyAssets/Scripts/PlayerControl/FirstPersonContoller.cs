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
    Transform camTransform;

    // ---------
    // Animation
    // ---------
    Animator animator;
    int isRunnningHash;
    // float velocity = 0f;
    [SerializeField]
    // private float animationAcc = 0.1f;
    int isMovingHash;
    int velocityHash;
    int velocityXHash;
    int velocityYHash;
    [SerializeField] Image IdentityColor;

    // -----
    // Admin
    // -----
    Toggle isAdmin = null;
    [SerializeField]GameObject avatarMesh = null;

    void Awake() {
        animator = GetComponentInChildren<Animator>();
        isAdmin = GameObject.FindGameObjectWithTag("AdminToggle").GetComponent<Toggle>();
        isMovingHash = Animator.StringToHash("isMoving");
        velocityHash = Animator.StringToHash("Velocity");
        velocityXHash = Animator.StringToHash("VelocityX");
        velocityYHash = Animator.StringToHash("VelocityY");
        IdentityColor = GameObject.FindGameObjectWithTag("Identity").GetComponent<Image>();
    }

    void LocalPlayerInit(){
        GetComponentInChildren<CinemachinePOVExtension>().enabled = true;
        GetComponentInChildren<CinemachineVirtualCamera>().enabled = true;
        GetComponentInChildren<AudioListener>().enabled = true;
        GetComponentInChildren<AudioSource>().enabled = false;
        GetComponent<InteractionController>().enabled = true;
    }

    void InitAdmin(){
        GetComponentInChildren<AudioSource>().enabled = false;
        gameObject.layer = 10;
        gameObject.tag = "Admin";
        avatarMesh.SetActive(false);
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        camTransform = Camera.main.transform;
        Material mat = GameManager.Instance.MatList[GameObject.FindGameObjectsWithTag("Player").Length-1];
        GetComponentInChildren<SkinnedMeshRenderer>().material = mat;
        if(isLocalPlayer){
            LocalPlayerInit();
            IdentityColor.color = mat.GetColor("_BaseColor");
        }
        if(isAdmin.isOn) InitAdmin();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if(!isLocalPlayer) return;
        // --------
        // movement
        // --------
        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = Camera.main.transform.forward * movement.y + Camera.main.transform.right * movement.x;
        move.y = 0f;
        transform.right = Camera.main.transform.right;

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
            }else{
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
