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


	// Use this for initialization
	void Start () {
		back = back.GetComponent<Button> ();
		backTransform = back.GetComponent<Transform> ();
		image = back.GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Backspace)) // or Back later 
		{
			image.sprite = backsprite;
			backTransform.localScale *= 1.2f;
			back.onClick.Invoke ();
		}
	}

}
