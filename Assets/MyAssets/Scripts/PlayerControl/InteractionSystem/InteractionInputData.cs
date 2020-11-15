using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractionInputData", menuName = "InteractionSystem/InputData")]
public class InteractionInputData : ScriptableObject
{
    bool m_interactedClicked;
    bool m_interactedReleased;

    public bool InteractedClicked
    {
        get => m_interactedClicked;
        set => m_interactedClicked = value;
    }

    public bool InteractedReleased
    {
        get => m_interactedReleased;
        set => m_interactedReleased = value;
    }

    public void Reset() {
        m_interactedClicked = false;
        m_interactedReleased = false;
    }

    
}
