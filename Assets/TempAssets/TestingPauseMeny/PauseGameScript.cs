﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseGameScript : MonoBehaviour {
    public List<Sprite> countdownSprite3;

	public GameObject optionMenu;
	public GameObject parentMenu;

	private bool pauseable = true;
	private void PauseGame()
	{
		InputManager.instance.isInputsDisabled = true;
		pauseable = false;
		Time.timeScale = 0f;
		MusicController.instance.PauseMusic ();
		optionMenu.SetActive (true);


		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(
			optionMenu.transform.GetChild(0).gameObject);

	}

	public void ResumeGame()
	{
		pauseable = true;
		StartCoroutine (StartDelay(3f));
		optionMenu.SetActive (false);
	}

	IEnumerator StartDelay(float countdown)
	{
		float pauseTime = countdown + Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < pauseTime) 
		{
			Debug.Log ("Resuming in " + Time.realtimeSinceStartup);
			if (!pauseable) 
			{
				break;
			}
			yield return 0;
		}

		if(pauseable)
		{
			InputManager.instance.isInputsDisabled = false;
			MusicController.instance.ResumeMusic ();
			Time.timeScale = 1f;
		}

	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			int count = 0;
			for (int i = parentMenu.transform.childCount-2; i >= 1; i--) 
			{
				if (!parentMenu.transform.GetChild (i).transform.gameObject.activeInHierarchy) 
				{
					count += 1;
				}	
			}
			if (count >= parentMenu.transform.childCount-2) 
			{
				count = 0;
				PauseGame ();
			}
		}
		if (Input.GetKeyDown (KeyCode.KeypadEnter)) 
		{
			Debug.Log (EventSystem.current.currentSelectedGameObject);
		}
		if (Input.GetKeyDown (KeyCode.Backspace) || Input.GetButtonDown ("Back Button")) 
		{
			if (optionMenu.activeInHierarchy) 
			//{
				ResumeGame ();
			//}
		}
	}

}
