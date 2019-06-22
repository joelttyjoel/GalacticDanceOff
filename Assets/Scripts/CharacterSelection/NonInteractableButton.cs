using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NonInteractableButton : MonoBehaviour {

	public Button back;
	public Transform backTransform;
	public Image image;
	public Sprite backsprite;
	private string currentBack;
	private string PS4_controller = "Button X"; 
	private string XBOX_controller = "Button B";


	// Use this for initialization
	void Start () {
		back = back.GetComponent<Button> ();
		backTransform = back.GetComponent<Transform> ();
		image = back.GetComponent<Image> ();

		currentBack = "BackKeyboard";
		if (SceneSwitchereController.instance.Ps4)
			currentBack = PS4_controller;
		if (SceneSwitchereController.instance.xBox)
			currentBack = XBOX_controller;

	}
	
	// Update is called once per frame
	void Update () {
		

		if ((Input.GetKeyDown (KeyCode.Backspace) ||  Input.GetButtonDown(currentBack)) || Input.GetKeyDown(KeyCode.Escape) && !SceneSwitchereController.instance.dissableAllInputs) 
		{
			image.sprite = backsprite;
			backTransform.localScale *= 1.2f;
			back.onClick.Invoke ();
		}
	}

}
