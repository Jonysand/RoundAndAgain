using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : InteractableBase
{
    bool opened = false;
    float timer = 0f;
    [SerializeField]float animDuration = 1f;
    [SerializeField]float openAngle = 120f;
    public override void OnInteract(){
        base.OnInteract();
        if(!opened) StartCoroutine(openTheDoor());
        if(opened) StartCoroutine(closeTheDoor());
    }

    IEnumerator openTheDoor(){
        IsInteractable = false;
        while(timer < animDuration){
            // transform.rotation = Quaternion.Euler(0f, openAngle * timer/animDuration, 0f);
            transform.Rotate(0f, openAngle * Time.deltaTime / animDuration, 0f, Space.Self);
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        opened = true;
        timer = 0f;
        IsInteractable = true;
    }

    IEnumerator closeTheDoor(){
        IsInteractable = false;
        while(timer < animDuration){
            // transform.rotation = Quaternion.Euler(0f, openAngle * timer/animDuration, 0f);
            transform.Rotate(0f, -openAngle * Time.deltaTime / animDuration, 0f, Space.Self);
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        opened = false;
        timer = 0f;
        IsInteractable = true;
    }
}
