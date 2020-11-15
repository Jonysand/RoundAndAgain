using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractable
{
    float HoldDuration {get;}
    bool HoldInteract {get;}
    bool MultipleUse {get; set;}
    bool IsInteractable {get;set;}

    string ToolTipMessage{get;}

    void OnInteract();
}
