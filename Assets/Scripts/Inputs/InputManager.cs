using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;


public class InputManager : MonoBehaviour {

	public bool isInputsDisabled;
	Dictionary <string, KeyCode> buttonKeys;
	Dictionary <string, KeyCode> xboxButton;
	Dictionary <string, KeyCode> PS4Button;

    //Bool that says if these buttons were pressed last fixedupdate
    Dictionary <string, bool>    keyboardBool;
    Dictionary <string, bool>    xboxBool;
    Dictionary <string, bool>    ps4Bool;


    public EventSystem eventSys;

    [HideInInspector]
    public int Xbox_One_Controller = 0;
    [HideInInspector]
    public int PS4_Controller = 0;


    void OnEnable()
	{
        //Keyboard
		buttonKeys = new Dictionary<string, KeyCode> ();
		buttonKeys ["W"] = KeyCode.W;
		buttonKeys ["A"] = KeyCode.A;
		buttonKeys ["S"] = KeyCode.S;
		buttonKeys ["D"] = KeyCode.D;
		buttonKeys ["Up"] = KeyCode.UpArrow;
		buttonKeys ["Left"] = KeyCode.LeftArrow;
		buttonKeys ["Down"] = KeyCode.DownArrow;
		buttonKeys ["Right"] = KeyCode.RightArrow;

        keyboardBool = new Dictionary<string, bool>();
        keyboardBool["W"] = false;
        keyboardBool["A"] = false;
        keyboardBool["S"] = false;
        keyboardBool["D"] = false;
        keyboardBool["Up"] = false;
        keyboardBool["Left"] = false;
        keyboardBool["Down"] = false;
        keyboardBool["Right"] = false;


        //Xbox
        xboxButton = new Dictionary<string, KeyCode> ();
		xboxButton ["A"] = KeyCode.JoystickButton0;
		xboxButton ["B"] = KeyCode.JoystickButton1;
		xboxButton ["X"] = KeyCode.JoystickButton2;
		xboxButton ["Y"] = KeyCode.JoystickButton3;

        xboxBool = new Dictionary<string, bool>();
        xboxBool["A"] = false;
        xboxBool["B"] = false;
        xboxBool["X"] = false;
        xboxBool["Y"] = false;


        //PS4
        PS4Button = new Dictionary<string, KeyCode> ();
		PS4Button ["Square"] = KeyCode.JoystickButton0;
		PS4Button ["Cross"] = KeyCode.JoystickButton1;
		PS4Button ["Circle"] = KeyCode.JoystickButton2;
		PS4Button ["Triangle"] = KeyCode.JoystickButton3;

        ps4Bool = new Dictionary<string, bool>();
        ps4Bool["Square"] = false;
        ps4Bool["Cross"] = false;
        ps4Bool["Circle"] = false;
        ps4Bool["Triangle"] = false;
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
		//inputSystem.currentInputModule.
	}

    void Update()
    {
        if (SceneSwitchereController.instance.dissableAllInputs) return;

		if (Input.GetKeyDown(KeyCode.Return) && EventSystem.current.currentSelectedGameObject.GetComponent<Button>().IsActive())
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }
        //eventSys.GetComponent<MyInputModule>().horizontalAxis = "Q";
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            //print(names[x].Length);
            if (names[x].Length == 19)
            {
                print("PS4 CONTROLLER IS CONNECTED");
                PS4_Controller = 1;
                Xbox_One_Controller = 0;
            }
            if (names[x].Length == 33)
            {
                print("XBOX ONE CONTROLLER IS CONNECTED");
                //set a controller bool to true
                PS4_Controller = 0;
                Xbox_One_Controller = 1;

            }
        }


        if (Xbox_One_Controller == 1)
        {
            eventSys.GetComponent<MyInputModule>().horizontalAxis = "XHorizontal";
            eventSys.GetComponent<MyInputModule>().verticalAxis = "XVertical";
            eventSys.GetComponent<MyInputModule>().submitButton = "Button A";
        }

        else if (PS4_Controller == 1)
        {
            eventSys.GetComponent<MyInputModule>().horizontalAxis = "PHorizontal";
            eventSys.GetComponent<MyInputModule>().verticalAxis = "PVertical";
            eventSys.GetComponent<MyInputModule>().submitButton = "Button B";
        }
    }


	// Update is called once per frame
	void FixedUpdate ()
    {
        if (SceneSwitchereController.instance.dissableAllInputs) return;

        if (SceneSwitchereController.instance.xBox)
        {
            List<string> xboxKeysReset = new List<string>();
            foreach (string key in xboxBool.Keys)
            {
                if (Input.GetKey(xboxButton[key]) == false)
                {
                    xboxKeysReset.Add(key);
                    //xboxBool[key] = false;
                }
            }

            for (int i = 0; i < xboxKeysReset.Count(); i++)
            {
                xboxBool[xboxKeysReset[i]] = false;
            }
        }


        else if (SceneSwitchereController.instance.Ps4)
        {
            List<string> ps4KeysReset = new List<string>();
            foreach (string key in ps4Bool.Keys)
            {
                if (Input.GetKey(PS4Button[key]) == false)
                {
                    ps4KeysReset.Add(key);
                    //ps4Bool[key] = false;
                }
            }

            for (int i = 0; i < ps4KeysReset.Count(); i++)
            {
                ps4Bool[ps4KeysReset[i]] = false;
            }
        }

        else
        {
            List<string> keyboardKeysReset = new List<string>();
            foreach (string key in keyboardBool.Keys)
            {
                if (Input.GetKey(buttonKeys[key]) == false)
                {
                    keyboardKeysReset.Add(key);
                    //keyboardBool[key] = false;
                }
            }

            for (int i = 0; i < keyboardKeysReset.Count(); i++)
            {
                keyboardBool[keyboardKeysReset[i]] = false;
            }
        }

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
		return Input.GetAxisRaw (axis);
	}


	//GetbuttonDown is like input.geykeydown
	public bool GetButtonDown(string buttonName)
	{
		if (SceneSwitchereController.instance.xBox) 
		{
			if (xboxButton.ContainsKey (buttonName) == false)
			{
				Debug.LogError ("Getbuttondown -- no button named: " + buttonName);
				return false;
			}

			if (Input.GetKey (xboxButton[buttonName]))
            {
                if (!xboxBool[buttonName])
                {
                    xboxBool[buttonName] = true;

                    return true;
                }
            }

            return false;

        }
		else if (SceneSwitchereController.instance.Ps4)
		{
			if (PS4Button.ContainsKey (buttonName) == false)
			{
				Debug.LogError ("Getbuttondown -- no button named: " + buttonName);
				//return false;
			}

            if (Input.GetKey(PS4Button[buttonName]))
            {
                if (!ps4Bool[buttonName])
                {
                    ps4Bool[buttonName] = true;

                    return true;
                }
            }

            return false;

        } 
		else 
		{
			if (buttonKeys.ContainsKey (buttonName) == false)
			{
				Debug.LogError ("Getbuttondown -- no button named: " + buttonName);
				//return false;
			}


            if (Input.GetKey(buttonKeys[buttonName]))
            {
                if (!keyboardBool[buttonName])
                {
                    keyboardBool[buttonName] = true;

                    return true;
                }
            }

            return false;
        }
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
