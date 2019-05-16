using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FMODUnity;
using FMOD.Studio;

public class PauseGameScript : MonoBehaviour {
	private Text countDown;
	private string startButton;
	private string backButton;

	[Header("Menus")]
	public GameObject optionMenu;
	public GameObject parentMenu;
	public GameObject menuBoard;


	public Button[] buttons;

	//[FMODUnity.EventRef]
	//public string PauseEventEventPath;
	//private EventInstance Pause;

	void Start ()
	{
		countDown = this.transform.GetChild (0).GetChild (0).GetComponent<Text> ();
		startButton = "Start Button";
		backButton = "Back Button";
		if (SceneSwitchereController.instance.Ps4) 
		{
			startButton = "PS4 Start Button";
			backButton = "PS4 Back Button";
		} 
		else if(SceneSwitchereController.instance.xBox)
		{
			startButton = "Start Button";
			backButton = "Back Button";
		}
	}



	private bool pauseable = true;
	private void PauseGame()
	{
        //start sound to play in pause menu thing
        AudioController.instance.SetParamPauseSound(4f);
        AudioController.instance.PlayPauseSound();

        this.transform.GetChild (0).gameObject.SetActive (false);

		InputManager.instance.isInputsDisabled = true;
		pauseable = false;
		Time.timeScale = 0f;
		MusicController.instance.PauseMusic ();
		menuBoard.SetActive (true);
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
		menuBoard.SetActive (false);
	}
		
	IEnumerator CountDown()
	{
		this.transform.GetChild (0).gameObject.SetActive (true);
		//yield return WaitToResumeGame (0.25f);

        AudioController.instance.SetParamPauseSound(3f);
		countDown.text = "3";

        yield return WaitToResumeGame (1f);
        AudioController.instance.SetParamPauseSound(2f);
		countDown.text = "2";

		yield return WaitToResumeGame (1f);
        AudioController.instance.SetParamPauseSound(1f);
		countDown.text = "1";

        yield return WaitToResumeGame (1f);
        AudioController.instance.SetParamPauseSound(0f);
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
		

		if (Input.GetButtonDown(startButton)) 
		{
			if (!menuBoard.activeInHierarchy) 
			{
				PauseGame();
			}
		}

		if (Input.GetButtonDown (backButton)) 
		{
			if (optionMenu.activeInHierarchy) 
			{
				ResumeGame ();
			}
		}


		if (Input.GetButtonDown (backButton)) 
		{
			for (int i = 0; i < buttons.Length; i++) 
			{
				if (buttons [i].gameObject.activeInHierarchy && buttons [i].IsInteractable ()) 
				{
					buttons [i].onClick.Invoke ();
				}
			}
		}


	}

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
    }

}
