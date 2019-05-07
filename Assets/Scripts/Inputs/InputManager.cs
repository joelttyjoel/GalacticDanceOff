using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InputManager : MonoBehaviour {

	public bool isInputsDisabled;
	Dictionary <string, KeyCode> buttonKeys;
	public Dictionary <string, KeyCode> xboxButton;
	public Dictionary <string, KeyCode> PS4Button;



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

		xboxButton = new Dictionary<string, KeyCode> ();

		xboxButton ["A"] = KeyCode.JoystickButton0;
		xboxButton ["B"] = KeyCode.JoystickButton1;
		xboxButton ["X"] = KeyCode.JoystickButton2;
		xboxButton ["Y"] = KeyCode.JoystickButton3;

		PS4Button = new Dictionary<string, KeyCode> ();

		PS4Button ["Square"] = KeyCode.JoystickButton0;
		PS4Button ["Cross"] = KeyCode.JoystickButton1;
		PS4Button ["Circe"] = KeyCode.JoystickButton2;
		PS4Button ["Triangle"] = KeyCode.JoystickButton3;
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
		//isInputsDisabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(isInputsDisabled);
	}

	public void KeyBoard()
	{
		SceneSwitchereController.instance.KeyBoard ();
	}

	public void XBOX()
	{
		SceneSwitchereController.instance.XBOX ();
	}

	public void PS4()
	{
		SceneSwitchereController.instance.PS4 ();
	}

	public float GetAxis(string axis)
	{
		return Input.GetAxis (axis);
	}

	//GetbuttonDown is like input.geykeydown
	public bool GetButtonDown(string buttonName)
	{
		if (buttonKeys.ContainsKey (buttonName) == false) 
		{
			Debug.LogError ("Getbuttondown -- no button named: " + buttonName);
			return false;
		}
		if (SceneSwitchereController.instance.xBox)
			return Input.GetKeyDown (xboxButton [buttonName]);

		if (SceneSwitchereController.instance.Ps4)
			return Input.GetKeyDown (PS4Button [buttonName]);

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
