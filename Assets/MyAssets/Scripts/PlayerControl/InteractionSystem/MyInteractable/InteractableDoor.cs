using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : InteractableBase
{
    [SerializeField]float animDuration = 1f;
    [SerializeField]float openAngle = 120f;
    public override void OnInteract(){
        base.OnInteract();
        StartCoroutine(openTheDoor());
    }

    IEnumerator openTheDoor(){
        float timer = 0f;
        while(timer < animDuration){
            // transform.rotation = Quaternion.Euler(0f, openAngle * timer/animDuration, 0f);
            transform.Rotate(0f, openAngle * Time.deltaTime / animDuration, 0f, Space.Self);
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
