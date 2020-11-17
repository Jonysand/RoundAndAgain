using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InteractableBase : NetworkBehaviour, IInteractable
{
    [Header("Interactable Settings")]
    [SerializeField] float holdDuration = 0;
    [SerializeField] bool holdInteract = false;
    [SerializeField] bool multipleUse = false;
    [SerializeField] bool isInteractable = true;
    [SerializeField] string tooltipMessage = "Press 'E' to interact";

    public float HoldDuration => holdDuration;
    public bool HoldInteract => holdInteract;
    public string ToolTipMessage => tooltipMessage;
    public bool MultipleUse
    {
        get{return multipleUse;}
        set{multipleUse = value;}
    }
    public bool IsInteractable
    {
        get{return isInteractable;}
        set{isInteractable = value;}
    }

    public virtual void OnInteract(){
        Debug.Log("INTERACTED: " + gameObject.name);
    }
}
