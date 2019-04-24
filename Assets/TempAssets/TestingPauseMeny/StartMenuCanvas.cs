using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartMenuCanvas : MonoBehaviour {

	public Button soloButton;
	public Button versusButton;
	public GameObject OptionButton;
	public GameObject InputButtons;

	public Button keyBindButton; //open keybindmenu
	public Button volumeButton; //open volumemenu
	public Button ExitButton; //exits

	public Button keyBoardButton;

	private Stack<GameObject> menuQueue = new Stack<GameObject>();


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if input esc.
		if(Input.GetKeyDown(KeyCode.M))
		{
			OpenOptions();
		}
		if (Input.GetButtonDown ("Back Button")) 
		{
			
				CloseOptions ();
				
		
		}
	}

	public void SoloMode() 
	{
		Debug.Log ("Play");

	}

	//resumes the game
	public void VersusMode()
	{
		Debug.Log ("Multiplay");
	}
		
	public void OpenOptions ()
	{
		
		if (!OptionButton.transform.GetChild (0).gameObject.activeInHierarchy) 
		{
			menuQueue.Push (OptionButton.transform.GetChild (0).gameObject);
			Debug.Log ("Opens Options");
			soloButton.interactable = false;
			versusButton.interactable = false;
			OptionButton.transform.GetChild (0).gameObject.SetActive (true);

			EventSystem.current.GetComponent<EventSystem> ().SetSelectedGameObject (
				OptionButton.transform.GetChild (0).transform.GetChild (0).gameObject);

		}
	}

	public void CloseOptions()
	{
		if (OptionButton.transform.GetChild(0).gameObject.activeInHierarchy) 
		{
			menuQueue.Clear ();
			Debug.Log ("closes options");
			soloButton.interactable = true;
			versusButton.interactable = true;
			OptionButton.transform.GetChild (0).gameObject.SetActive (false);
			EventSystem.current.GetComponent<EventSystem> ().SetSelectedGameObject (soloButton.gameObject);
		}
	}
		
	public void OpenKeyBind()
	{
		menuQueue.Push (InputButtons);
		InputButtons.SetActive (true);
		OptionButton.transform.GetChild(0).gameObject.SetActive (false);
		EventSystem.current.SetSelectedGameObject (
			InputButtons.transform.GetChild (0).gameObject);
	}




	public void ExitGame()
	{
		//exit game?
	}

}
