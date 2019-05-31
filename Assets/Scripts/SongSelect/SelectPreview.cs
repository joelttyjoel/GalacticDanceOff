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
    
    public void Awake()
    {
        musicEventReference = MusicManagerScript.instance.GetComponent<StudioEventEmitter>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Set it to: " + valueOnSelected);
        musicEventReference.SetParameter("Selected Song", valueOnSelected);
    }
}
