using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkipIntroButton : MonoBehaviour {

	public Button skipButton;

	// Use this for initialization
	void Start () {
		skipButton = GetComponent<Button> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return) && !SceneSwitchereController.instance.dissableAllInputs) 
		{
			skipButton.onClick.Invoke ();
		}
	}
}
