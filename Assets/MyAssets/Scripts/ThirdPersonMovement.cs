#pragma warning disable 0649
using Mirror;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : NetworkBehaviour
{
    // Movement
    public CharacterController controller;
    public float speed = 6f;
    [SerializeField]
    private float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;
    [SerializeField]
    Transform cam;

    // Camera init
    private CinemachineFreeLook freeLook;

    // Animation
    Animator animator;
    int isRunnningHash;
    float velocity = 0f;
    [SerializeField]
    private float animationAcc = 0.001f;
    int isMovingHash;
    int velocityHash;


    private void Awake() {
        animator = GetComponentInChildren<Animator>();
        isMovingHash = Animator.StringToHash("isMoving");
        velocityHash = Animator.StringToHash("Velocity");

        freeLook = GetComponent<CinemachineFreeLook>();
        cam = Camera.main.transform;
    }

    void LocalPlayerInit(){
        GetComponentInChildren<CinemachineFreeLook>().enabled = true;
        // GetComponentInChildren<CinemachineCollider>().enabled = true;
        GetComponent<AudioListener>().enabled = true;
        GetComponentInChildren<AudioSource>().enabled = false;
    }
    private void Start() {
        if(isLocalPlayer) LocalPlayerInit();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // --------
        // movement
        // --------
        if(!isLocalPlayer) return;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        bool runPressed = Input.GetKey("left shift");
        if(direction.magnitude >=1f){
            // direction
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // animation
            animator.SetBool(isMovingHash, true);
            if(runPressed) velocity += animationAcc;
            else velocity -= animationAcc;
            if(velocity > 1.0f) velocity = 1.0f;
            if(velocity < 0.0f) velocity = 0.0f;

            controller.Move(moveDir.normalized * speed * (1f+0.5f * velocity) * Time.deltaTime);
        }
        else{
            velocity -= animationAcc;
            if(velocity<0.0f) velocity = 0.0f;
            animator.SetBool(isMovingHash, false);
        }
        animator.SetFloat(velocityHash, velocity);

        // ------
        // cursor
        // ------
        if(Input.GetKeyDown(KeyCode.Escape)) Cursor.lockState = CursorLockMode.None;
        if(Input.GetMouseButtonDown(0)) Cursor.lockState = CursorLockMode.Locked;
    }
}
