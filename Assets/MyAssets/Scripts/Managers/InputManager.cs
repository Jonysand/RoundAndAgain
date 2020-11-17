using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance{
        get{
            return _instance;
        }
    }
    PlayerControls playerControls;

    // -----------
    // InputAssets
    // -----------
    [SerializeField]
    InteractionInputData interactionInputData = null;

    private void Awake() {
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }
        else{
            _instance = this;
        }
        playerControls = new PlayerControls();
        
        // ----------------------
        // interaction input init
        // ----------------------
        playerControls.Player.Interact.performed += ctx => {
            interactionInputData.InteractedClicked = true;
            interactionInputData.InteractedReleased = false;
        };
        playerControls.Player.Interact.canceled += ctx => {
            interactionInputData.InteractedReleased = true;
            interactionInputData.InteractedClicked = false;
        };
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    public Vector2 GetPlayerMovement(){
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta(){
        return playerControls.Player.Look.ReadValue<Vector2>();
    }

    public bool isRun(){
        // return playerControls.Player.Run.triggered;
        return UnityEngine.InputSystem.Keyboard.current.leftShiftKey.isPressed;
    }

    public bool ShowMenu(){
        return playerControls.UI.Escape.triggered;
    }

    public bool LeftMouseClicked(){
        return playerControls.UI.LeftClick.triggered;
    }

    public bool InteractedTriggered(){
        // return interactionInputData.InteractedClicked;
        return playerControls.Player.Interact.triggered;
    }

    public bool InteractedReleased(){
        return interactionInputData.InteractedReleased;
    }
}
