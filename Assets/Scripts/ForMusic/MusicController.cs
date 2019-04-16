using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MusicController : MonoBehaviour
{

    public StudioEventEmitter myEmitter;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.U))
        {
            myEmitter.EventInstance.triggerCue();
        }

        if (Input.GetKeyUp(KeyCode.I))
        {
            myEmitter.SetParameter("Restart", 1f);

        }
    }
}
