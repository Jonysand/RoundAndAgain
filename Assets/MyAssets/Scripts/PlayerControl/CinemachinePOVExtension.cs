using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField]
    private float horizontalSpeed = 20f;
    [SerializeField]
    private float verticalSpeed = 20f;
    [SerializeField]
    private float clampAngle = 80f;

    InputManager inputManager;
    private Vector3 startingRotation;

    protected override void Awake() {
        inputManager = InputManager.Instance;
        base.Awake();
    }

    // smooth turn
    [SerializeField]float turnDampingTime = 0.01f;
    float turnX;
    float turnY;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime){
        if (vcam.Follow){
            if (stage == CinemachineCore.Stage.Aim){
                if(startingRotation == null) startingRotation = transform.localRotation.eulerAngles;
                Vector2 deltaInput = inputManager.GetMouseDelta();

                // startingRotation.x = Mathf.SmoothDampAngle(startingRotation.x, startingRotation.x + deltaInput.x * verticalSpeed * Time.deltaTime, ref turnX, turnDampingTime);
                // startingRotation.y = Mathf.SmoothDampAngle(startingRotation.y, startingRotation.y + deltaInput.y * horizontalSpeed * Time.deltaTime, ref turnY, turnDampingTime);
                startingRotation.x += deltaInput.x * verticalSpeed * Time.deltaTime;
                startingRotation.y += deltaInput.y * horizontalSpeed * Time.deltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                transform.parent.rotation = Quaternion.Euler(0f, startingRotation.x, 0f);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
            }
        }
    }
}
