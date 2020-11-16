using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [Header("Data")]
    public InteractionInputData interactionInputData;
    public InteractionData interactionData;

    InteractionUIPanel uiPanel = null;

    [Space, Header("Ray Settings")]
    public float rayDistance;
    public float raySphereRadius;
    public LayerMask interactableLayer;

    Camera m_cam;
    bool m_interacting;
    float m_holderTimer = 0f;


    private void OnEnable() {
        m_cam = Camera.main;
        uiPanel = GameObject.FindGameObjectWithTag("LocalPlayerUIPanel").GetComponent<InteractionUIPanel>();
    }

    private void Update() {
        CheckForInteractable();
        CheckForInteractableInput();
    }

    void CheckForInteractable(){
        Ray _ray = new Ray(Camera.main.transform.position, m_cam.transform.forward);
        RaycastHit _hitInfo;

        bool _hitSomething = Physics.SphereCast(_ray, raySphereRadius, out _hitInfo, rayDistance, interactableLayer);

        if(_hitSomething){
            InteractableBase _interactable = _hitInfo.transform.GetComponent<InteractableBase>();
            if(_interactable != null){
                if(interactionData.IsEmpty() || !interactionData.IsSameInteractable(_interactable)){
                    interactionData.Interactable = _interactable;
                }

                uiPanel.SetToolTip(_interactable.ToolTipMessage);
            }
        }else{
            uiPanel.ResetUI();
            interactionData.ResetData();
        }

        Debug.DrawRay(_ray.origin, _ray.direction * rayDistance, _hitSomething ? Color.green:Color.red);
    }

    void CheckForInteractableInput(){
        if(interactionData.IsEmpty()) return;
        
        if(InputManager.Instance.InteractedTriggered()){
            m_interacting = true;
            m_holderTimer = 0f;
        }

        if(interactionInputData.InteractedReleased){
            m_interacting = false;
            m_holderTimer = 0f;
            uiPanel.UpdateProgressBar(m_holderTimer);
        }

        if(m_interacting){
            if(!interactionData.Interactable.IsInteractable) return;
            if(interactionData.Interactable.HoldInteract){
                m_holderTimer += Time.deltaTime;
                float heldPercent = m_holderTimer / interactionData.Interactable.HoldDuration;
                uiPanel.UpdateProgressBar(heldPercent);
                if(heldPercent > 1f){
                    interactionData.Interact();
                    m_interacting = false;
                }
            }else{
                interactionData.Interact();
                m_interacting = false;
            }
        }
    }
}
