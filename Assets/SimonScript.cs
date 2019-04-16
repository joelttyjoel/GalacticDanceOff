using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


public class SimonScript : MonoBehaviour {

    public StudioEventEmitter myEmitter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.S))
        {
            myEmitter.EventInstance.triggerCue();
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            myEmitter.SetParameter("Restart", 1f);
            
        }
	}
}
