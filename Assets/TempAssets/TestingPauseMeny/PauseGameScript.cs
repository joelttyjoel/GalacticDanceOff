using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameScript : MonoBehaviour {

	public GameObject OptionMenu;
	private bool pauseable = true;

	private void PauseGame()
	{
		InputManager.instance.isInputsDisabled = true;
		pauseable = false;
		UnityEngine.EventSystems.EventSystem.current.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(OptionMenu.transform.GetChild (0).gameObject);
		Time.timeScale = 0f;
		MusicController.instance.PauseMusic ();
		OptionMenu.SetActive (true);
	}

	public void ResumeGame()
	{
		pauseable = true;
		StartCoroutine (StartDelay(3f));
		OptionMenu.SetActive (false);
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
			if (!OptionMenu.activeInHierarchy) 
			{
				PauseGame ();
			}
		}
		if (Input.GetKeyDown (KeyCode.KeypadEnter)) 
		{
			Debug.Log (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject);
		}
		if (Input.GetKeyDown (KeyCode.Backspace) || Input.GetButtonDown ("Back Button")) 
		{
			if (OptionMenu.activeInHierarchy) 
			{
				ResumeGame ();
			}
		}
	}

}
