using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractionData", menuName = "InteractionSystem/InteractionData")]
public class InteractionData : ScriptableObject
{
    private InteractableBase m_interactable;
    public InteractionUIPanel uiPanel = null;

    public InteractableBase Interactable{
        get => m_interactable;
        set => m_interactable = value;
    }

    public void Interact(){
        m_interactable.OnInteract();
        ResetData();
    }

    public bool IsSameInteractable(InteractableBase _newInteractable) => m_interactable == _newInteractable;

    public bool IsEmpty() => m_interactable == null;

    public void ResetData() => m_interactable = null;
}
