using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnSelectSetLeaderboard : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private bool hasBeenOnce = false;
	// Update is called once per frame
	void Update () {
        if (EventSystem.current.currentSelectedGameObject == this.gameObject && hasBeenOnce == false)
        {
            hasBeenOnce = true;
        }
        else
        {

        }
    }
}
