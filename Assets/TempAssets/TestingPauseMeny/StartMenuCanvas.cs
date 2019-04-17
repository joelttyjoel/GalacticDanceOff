using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartMenuCanvas : MonoBehaviour {

	public Button soloButton;
	public Button versusButton;
	public Button OptionButton;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if input esc.
		if(Input.GetButtonDown("Cancel"))
		{
			Options();
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

	public void Options()
	{
		
		if (!OptionButton.transform.GetChild(0).gameObject.activeInHierarchy) 
		{
			Debug.Log ("Opens Options");
			soloButton.interactable = false;
			versusButton.interactable = false;
			OptionButton.transform.GetChild (0).gameObject.SetActive (true);

			EventSystem.current.GetComponent<EventSystem> ().SetSelectedGameObject( 
				OptionButton.transform.GetChild (0).transform.GetChild (0).gameObject);


		} else if (OptionButton.transform.GetChild(0).gameObject.activeInHierarchy) 
		{
			Debug.Log ("closes options");
			soloButton.interactable = true;
			versusButton.interactable = true;
			OptionButton.transform.GetChild (0).gameObject.SetActive (false);
			EventSystem.current.GetComponent<EventSystem> ().SetSelectedGameObject (soloButton.gameObject);
		}
	}

	public void ExitGame()
	{
		//exit game?
	}

}
