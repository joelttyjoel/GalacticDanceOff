using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FMODUnity;
using FMOD.Studio;

public class PauseGameScript : MonoBehaviour {
	Text countDown;

	//[FMODUnity.EventRef]
	//public string PauseEventEventPath;
	//private EventInstance Pause;

	void Start ()
	{
		countDown = this.transform.GetChild (0).GetChild (0).GetComponent<Text> ();
	}

	[Header("Menus")]
	public GameObject optionMenu;
	public GameObject parentMenu;

	private bool pauseable = true;
	private void PauseGame()
	{
		this.transform.GetChild (0).gameObject.SetActive (false);
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
		StartCoroutine (CountDown());
		optionMenu.SetActive (false);
	}
		
	IEnumerator CountDown()
	{
		this.transform.GetChild (0).gameObject.SetActive (true);
		//yield return WaitToResumeGame (0.25f);

        AudioController.instance.PlayPauseSound(3f);
		countDown.text = "3";

        yield return WaitToResumeGame (1f);
        AudioController.instance.PlayPauseSound(2f);
		countDown.text = "2";

		yield return WaitToResumeGame (1f);
        AudioController.instance.PlayPauseSound(1f);
		countDown.text = "1";

        yield return WaitToResumeGame (1f);
        AudioController.instance.PlayPauseSound(0f);
		countDown.text = "0";

        InputManager.instance.isInputsDisabled = false;
		MusicController.instance.ResumeMusic ();
		Time.timeScale = 1f;

		yield return WaitToResumeGame (1f);


		this.transform.GetChild (0).gameObject.SetActive (false);
	}

	IEnumerator WaitToResumeGame(float timer)
	{
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + timer) 
		{
			if (!pauseable) 
			{
				StopAllCoroutines ();
			}
			yield return 0;
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

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
    }

}
