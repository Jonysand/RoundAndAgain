using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardText : MonoBehaviour
{
    Camera cam;
    [SerializeField] GUIStyle nameStyle = null;
    public string text = "Test";
    void OnGUI ()
    {
        if(!cam) cam = Camera.main;
        if(Vector3.Dot(cam.transform.forward, (transform.position - cam.transform.position)) > 0){
            Vector2 worldPoint = cam.WorldToScreenPoint (transform.position);
            GUI.Label (new Rect (worldPoint.x - 100, (Screen.height - worldPoint.y) - 50, 200, 100), text, nameStyle);
        }
    }
}
