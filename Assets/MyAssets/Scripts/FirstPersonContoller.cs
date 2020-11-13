#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

public class FirstPersonContoller : NetworkBehaviour
{
    // --------
    // Movement
    // --------
    private CharacterController controller;
    private Vector3 playerVelocity;
    [SerializeField]
    private float playerSpeed = 2.0f;
    InputManager inputManager;
    Transform camTransform;

    // ---------
    // Animation
    // ---------
    Animator animator;
    int isRunnningHash;
    float velocity = 0f;
    [SerializeField]
    private float animationAcc = 0.1f;
    int isMovingHash;
    int velocityHash;
    int velocityXHash;
    int velocityYHash;
    [SerializeField]
    Transform upBody;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
        isMovingHash = Animator.StringToHash("isMoving");
        velocityHash = Animator.StringToHash("Velocity");
        velocityXHash = Animator.StringToHash("VelocityX");
        velocityYHash = Animator.StringToHash("VelocityY");
    }
    void LocalPlayerInit(){
        GetComponentInChildren<CinemachinePOVExtension>().enabled = true;
        GetComponentInChildren<CinemachineVirtualCamera>().enabled = true;
        GetComponentInChildren<AudioListener>().enabled = true;
        GetComponentInChildren<AudioSource>().enabled = false;
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        camTransform = Camera.main.transform;
        if(isLocalPlayer) LocalPlayerInit();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // --------
        // movement
        // --------
        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = camTransform.forward * movement.y + camTransform.right * movement.x;
        move.y = 0f;
        transform.right = camTransform.right;

        // ---------
        // animation
        // ---------
        // if(movement.magnitude > 0){
        //     animator.SetBool(isMovingHash, true);
        //     if(inputManager.isRun()) velocity += animationAcc;
        //     else velocity -= animationAcc;
        //     if(velocity > 1.0f) velocity = 1.0f;
        //     if(velocity < 0.0f) velocity = 0.0f;
        // }
        // else{
        //     velocity -= animationAcc;
        //     if(velocity<0.0f) velocity = 0.0f;
        //     animator.SetBool(isMovingHash, false);
        // }
        // animator.SetFloat(velocityHash, velocity);
        // controller.Move(move * Time.deltaTime * playerSpeed * (1f+0.5f * velocity));

        if(movement.magnitude > 0){
            animator.SetBool(isMovingHash, true);
            if(inputManager.isRun()) move *= 1.5f;
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
        if(inputManager.ShowMenu()) Cursor.lockState = CursorLockMode.None;
        if(inputManager.LeftMouseClicked()) Cursor.lockState = CursorLockMode.Locked;
    }
}
