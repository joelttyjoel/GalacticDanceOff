﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour {

	public bool isInputsDisabled;
	Dictionary <string, KeyCode> buttonKeys;

	void OnEnable()
	{
		buttonKeys = new Dictionary<string, KeyCode> ();	

		buttonKeys ["W"] = KeyCode.W;
		buttonKeys ["A"] = KeyCode.A;
		buttonKeys ["S"] = KeyCode.S;
		buttonKeys ["D"] = KeyCode.D;
		buttonKeys ["Up"] = KeyCode.UpArrow;
		buttonKeys ["Left"] = KeyCode.LeftArrow;
		buttonKeys ["Down"] = KeyCode.DownArrow;
		buttonKeys ["Right"] = KeyCode.RightArrow;

	}
		


    //for creating singleton, love easy referencing
    public static InputManager instance = null;

    void Awake()
    {

		//Cursor.visible = false;
		//Cursor.lockState = CursorLockMode.Locked;
        if (instance == null)
            instance = this;
        //now replaces already existing gameManager instead
        else if (instance != this)
            Destroy(instance.gameObject);
    }

    // Use this for initialization
    void Start () {
		isInputsDisabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//GetbuttonDown is like input.geykeydown
	public bool GetButtonDown(string buttonName)
	{
		if (buttonKeys.ContainsKey (buttonName) == false) 
		{
			Debug.LogError ("Getbuttondown -- no button named: " + buttonName);
			return false;
		}
		return Input.GetKeyDown (buttonKeys [buttonName]);
	}

	//return array of all keycodes in dictionary
	public string[] GetButtonNames()
	{
		return buttonKeys.Keys.ToArray ();
	}

	//returns the name of the keycodes inside dictionary
	public string GetKeyNameForButton( string buttonName)
	{
		if (buttonKeys.ContainsKey (buttonName) == false) 
		{
			Debug.Log ("GetKeynameForButton no button named" + buttonName);
			return "N/A";
		}
		return buttonKeys [buttonName].ToString ();
	}

	//Sets the name to corresponding keycode
	public void SetButtonForKey ( string buttonName, KeyCode keyCode)
	{
		buttonKeys [buttonName] = keyCode;
	}

}
