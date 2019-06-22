using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FMODUnity;
using FMOD.Studio;

public class PauseGameScript : MonoBehaviour {

	Image image;
	private string startButton;
	private string backButton;

    public bool isPausable = true;

	[Header("Menus")]
	public GameObject optionMenu;
	public GameObject parentMenu;
	public GameObject menuBoard;
    public GameObject TutorialButton;

	public GameObject childObject;
	public Button[] buttons;
	public Sprite countdown3, countdown2, countdown1, countdown0;
    //[FMODUnity.EventRef]
    //public string PauseEventEventPath;
    //private EventInstance Pause;

    //make singleton
    public static PauseGameScript instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        //now replaces already existing gameManager instead
        else if (instance != this)
            Destroy(instance.gameObject);
    }

    void Start ()
	{
		image = childObject.GetComponent<Image> ();
		startButton = "Start Button";
		backButton = "Back Button";
		if (SceneSwitchereController.instance.Ps4) 
		{
			startButton = "PS4 Start Button";
			backButton = "Button X";
		} 
		else if(SceneSwitchereController.instance.xBox)
		{
			startButton = "Start Button";
			backButton = "Button B";
		}

        isPausable = true;
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
        //AudioController.instance.PauseSound();
		menuBoard.SetActive (true);
		optionMenu.SetActive (true);
        TutorialButton.SetActive(true);


		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(
			optionMenu.transform.GetChild(0).gameObject);

	}

    public void SetPausable(bool isWhat)
    {
        isPausable = isWhat;
    }

	public void ResumeGame()
	{
		
		pauseable = true;
		StartCoroutine (CountDown());
		optionMenu.SetActive (false);
		menuBoard.SetActive (false);
	}
		
	private IEnumerator CountDown()
	{
		this.transform.GetChild (0).gameObject.SetActive (true);
		//yield return WaitToResumeGame (0.25f);

        AudioController.instance.SetParamPauseSound(3f);
		image.sprite = countdown3;

        yield return WaitToResumeGame (1f);
        AudioController.instance.SetParamPauseSound(2f);
		image.sprite = countdown2;

		yield return WaitToResumeGame (1f);
        AudioController.instance.SetParamPauseSound(1f);
		image.sprite = countdown1;

        yield return WaitToResumeGame (1f);
        AudioController.instance.SetParamPauseSound(0f);
		image.sprite = countdown0;

        InputManager.instance.isInputsDisabled = false;
		MusicController.instance.ResumeMusic ();
        AudioController.instance.ResumeSound();
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
        if (!isPausable) return;

		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown(startButton)) 
		{
			if (!menuBoard.activeInHierarchy && !SceneSwitchereController.instance.dissableAllInputs) 
			{
				PauseGame();
			}
		}

		/*if (Input.GetButtonDown (backButton) || Input.GetKeyDown(KeyCode.JoystickButton8)) 
		{
			if (optionMenu.activeInHierarchy) 
			{
				ResumeGame ();
			}
		}*/


		if (Input.GetButtonDown (backButton) || Input.GetKeyDown(KeyCode.Backspace)) 
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
        AudioController.instance.ResumeSound();
        //MusicController.instance.ResumeMusic();
    }

}
