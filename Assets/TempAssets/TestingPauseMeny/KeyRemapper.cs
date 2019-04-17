using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class KeyRemapper : MonoBehaviour {

	Dictionary<string,Text> buttonToLabel;

	InputManager inputManager;
	public GameObject KeyItemPrefab;
	public GameObject keyList;

	string buttonToRebind = null;

	// Use this for initialization
	void Start () 
	{
		inputManager = GameObject.FindObjectOfType<InputManager> ();
		string[] buttonNames = inputManager.GetButtonNames ();
		buttonToLabel = new Dictionary<string, Text> ();

	
		foreach (string bn in buttonNames) 
		{
			GameObject go = (GameObject)Instantiate (KeyItemPrefab);
			go.transform.SetParent (keyList.transform);

			Text buttonNameText = go.transform.Find ("ButtonNames").GetComponent<Text>();
			buttonNameText.text = bn;


			Text keyNameText = go.transform.Find ("Button/Keyname").GetComponent<Text>();
			keyNameText.text = inputManager.GetKeyNameForButton(bn);
			buttonToLabel [bn] = keyNameText;

			Button keybindButton = go.transform.Find ("Button").GetComponent<Button> ();
			keybindButton.onClick.AddListener ( () => { StartRebindFor(bn); } );
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (buttonToRebind != null) 
		{
			if (Input.anyKeyDown) 
			{
				
				foreach (KeyCode kc in Enum.GetValues (typeof(KeyCode))) 
				{
					if (Input.GetKeyDown (kc)) 
					{
						inputManager.SetButtonForKey (buttonToRebind, kc);
						buttonToLabel [buttonToRebind].text = kc.ToString ();
						buttonToRebind = null;
						break;
					}
				}
			}
		}
	}

	void StartRebindFor(string buttonName)
	{
		Debug.Log (buttonName);
		buttonToRebind = buttonName;
	}

}
