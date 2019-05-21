using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.EventSystems;

public class SelectPreview : MonoBehaviour, ISelectHandler
{
    [Header("Reference To event on music manager")]
    public StudioEventEmitter musicEventReference;
    public float valueOnSelected;

    public void OnSelect(BaseEventData eventData)
    {
        musicEventReference.SetParameter("Selected Song", valueOnSelected);
    }
}
