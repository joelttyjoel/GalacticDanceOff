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
	private GameObject currentMenu;

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
			if (currentMenu == OptionButton) 
			{
				CloseOptions ();
				currentMenu = null;
			} 
			else 
			{
				currentMenu.SetActive (false);
			}
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
		currentMenu = OptionButton;
		if (!OptionButton.transform.GetChild (0).gameObject.activeInHierarchy) 
		{
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
			Debug.Log ("closes options");
			soloButton.interactable = true;
			versusButton.interactable = true;
			OptionButton.transform.GetChild (0).gameObject.SetActive (false);
			EventSystem.current.GetComponent<EventSystem> ().SetSelectedGameObject (soloButton.gameObject);
		}
	}

	public void KeyMapping()
	{
		InputButtons.SetActive (true);
		currentMenu = InputButtons;
		EventSystem.current.GetComponent<EventSystem> ().SetSelectedGameObject (InputButtons.transform.GetChild (0).gameObject);

	}



	public void ExitGame()
	{
		//exit game?
	}

}
