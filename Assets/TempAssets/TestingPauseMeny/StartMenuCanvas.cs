using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartMenuCanvas : MonoBehaviour {

	public Button soloButton;
	public Button versusButton;

	public GameObject OptionButton;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if(Input.GetKeyDown(KeyCode.M))
		{
			Debug.Log ("fix");
			OpenOptions();
		}
		if (Input.GetKeyDown(KeyCode.Z)) 
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
		for (int i = OptionButton.transform.childCount-1; i >= 0; i--)
		{
			if (!OptionButton.transform.GetChild (i).gameObject.activeInHierarchy) 
			{
				Debug.Log ("Opens Options");
				OptionButton.transform.GetChild (0).gameObject.SetActive (true);
				soloButton.interactable = false;
				versusButton.interactable = false;

				EventSystem.current.GetComponent<EventSystem> ().SetSelectedGameObject (
					OptionButton.transform.GetChild (0).transform.GetChild (0).gameObject);
			}
		}
	}

	public void CloseOptions()
	{
		Debug.Log ("closes options");
		for (int i = OptionButton.transform.childCount-1; i >= 0; i--) 
		{
			OptionButton.transform.GetChild (i).gameObject.SetActive (false);
		}
		soloButton.interactable = true;
		versusButton.interactable = true;
		EventSystem.current.GetComponent<EventSystem> ().SetSelectedGameObject (soloButton.gameObject);

	}


	public void ExitGame()
	{
		Debug.Log ("Quit");
	}

}
