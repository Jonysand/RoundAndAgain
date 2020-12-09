using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InteractableDoor : InteractableBase
{
    bool opened = false;
    [SerializeField]float openAngle = -120f;

    public override void OnInteract(){
        base.OnInteract();
        if(!opened){
            transform.Rotate(0f, openAngle, 0f);
            opened = true;
        }else{
            transform.Rotate(0f, -openAngle, 0f);
            opened = false;
        }
    }

    // void SyncRequest(Quaternion clientRotation){
    //     transform.rotation = clientRotation;
    //     SyncDistribute(transform.rotation);
    // }

    // [ClientRpc]
    // void SyncDistribute(Quaternion serverRotation){
    //     transform.rotation = serverRotation;
    // }
}
