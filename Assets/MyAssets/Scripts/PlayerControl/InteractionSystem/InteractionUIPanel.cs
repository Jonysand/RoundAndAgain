using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUIPanel : MonoBehaviour
{
    [SerializeField]
    Image progressBar = null;
    [SerializeField]
    Text tooltipText = null;

    string defaultString = "";

    public void SetToolTip(string tooltip){
        tooltipText.text = tooltip;
    }

    public void UpdateProgressBar(float fillAmount){
        progressBar.fillAmount = fillAmount;
    }

    public void ResetUI(){
        progressBar.fillAmount = 0;
        tooltipText.text = defaultString;
    }
}
