using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameScript : MonoBehaviour {

	public GameObject OptionMenu;
	private bool pauseable = true;

	private void PauseGame()
	{
		pauseable = false;
		Time.timeScale = 0f;
		MusicController.instance.PauseMusic ();
		OptionMenu.SetActive (true);
	}

	public void ResumeGame()
	{
		pauseable = true;
		StartCoroutine (StartDelay(3f));
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
		MusicController.instance.ResumeMusic ();
		Time.timeScale = 1f;
		}

	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape) || Input.GetButtonDown ("Start Button")) 
		{
			if (!OptionMenu.activeInHierarchy) 
			{
				PauseGame ();
			}
		}
	}

}
