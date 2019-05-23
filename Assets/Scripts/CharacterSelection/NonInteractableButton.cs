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
		back = GetComponent<Button> ();
	}
	
	// Update is called once per frame
	/*void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) // or Back later 
		{
			EventSystem.current.SetSelectedGameObject(back.gameObject);
			image.sprite = backsprite;
			backTransform.localScale *= 1.2f;
			back.onClick.Invoke ();
		}
	}
	*/
}
