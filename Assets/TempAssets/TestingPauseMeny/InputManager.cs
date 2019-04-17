﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour {


	Dictionary <string, KeyCode> buttonKeys;

	void OnEnable()
	{
		buttonKeys = new Dictionary<string, KeyCode> ();	

		buttonKeys ["W"] = KeyCode.W;
		buttonKeys ["A"] = KeyCode.A;
		buttonKeys ["S"] = KeyCode.S;
		buttonKeys ["D"] = KeyCode.D;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool GetButtonDown(string buttonName)
	{
		if (buttonKeys.ContainsKey (buttonName) == false) 
		{
			Debug.LogError ("Getbuttondown -- no button named: " + buttonName);
			return false;
		}
		return Input.GetKeyDown (buttonKeys [buttonName]);
	}

	public string[] GetButtonNames()
	{
		return buttonKeys.Keys.ToArray ();
	}

	public string GetKeyNameForButton( string buttonName)
	{
		if (buttonKeys.ContainsKey (buttonName) == false) 
		{
			Debug.Log ("GetKeynameForButton no button named" + buttonName);
			return "N/A";
		}
		return buttonKeys [buttonName].ToString ();
	}

	public void SetButtonForKey ( string buttonName, KeyCode keyCode)
	{
		buttonKeys [buttonName] = keyCode;
	}

}
