using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance{
        get{
            return _instance;
        }
    }
    PlayerControls playerControls;

    private void Awake() {
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }
        else{
            _instance = this;
        }
        playerControls = new PlayerControls();
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
}
