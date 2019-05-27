using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.EventSystems;

public class SelectPreview : MonoBehaviour, ISelectHandler
{
    [Header("Reference To event on music manager")]
    private StudioEventEmitter musicEventReference;
    public float valueOnSelected;

    private StudioEventEmitter[] emitters;
    public void Awake()
    {
        emitters = MusicManagerScript.instance.GetComponents<StudioEventEmitter>();
        musicEventReference = emitters[1];
    }

    public void OnSelect(BaseEventData eventData)
    {
        musicEventReference.SetParameter("Selected Song", valueOnSelected);
    }
}
