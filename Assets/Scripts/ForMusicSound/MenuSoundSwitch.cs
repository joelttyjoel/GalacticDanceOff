using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSoundSwitch : MonoBehaviour {

    private GameObject currentSelected;
	// Use this for initialization
	void Start () {
        //if(EventSystem.current.currentSelectedGameObject != null)
        //currentSelected = EventSystem.current.currentSelectedGameObject;
    }
	
	// Update is called once per frame
	void Update () {

        //if(EventSystem.current.currentSelectedGameObject != currentSelected)
        //{
        //    Debug.Log("Switch sound play");
        //    //currentSelected has changed
        //    currentSelected = EventSystem.current.currentSelectedGameObject;
        //    AudioController.instance.PlayMainMenueSound(0f);
        //}
    }
}
