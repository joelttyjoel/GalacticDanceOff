using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiController: MonoBehaviour {

	public GameObject pausePanel;
	private float countdown;
	private float pauseTime;
	private bool pausable;

	public void Start()
	{
		pauseTime = 0;
		countdown = 2f;
		pausable = true;
		pausePanel.SetActive (false);
	}

	//pauses the game with time.timescale
	public void PauseGame() 
	{
		Time.timeScale = 0f;
		pausePanel.SetActive (true);
	}

	//resumes the game
	public void ResumeGame()
	{
		pausePanel.SetActive (false);
		StartCoroutine ("StartDelay");
	}

	//resumes the game with a delay
	IEnumerator StartDelay()
	{
		pauseTime = Time.realtimeSinceStartup + countdown;
		while (Time.realtimeSinceStartup < pauseTime) 
		{
			Debug.Log ("working=");

			yield return 0;
		}
		//makes it possible to pause the game.
		if (pausePanel.activeInHierarchy) 
		{
			pausable = false;
		}
		if (pausable) 
		{
			Time.timeScale = 1f;
		}
		pausable = true;
	}

	public void Update()
	{
		//pressing ESC or Start button activates the pause menu
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButton("Start Button")) 
		{
			if (!pausePanel.activeInHierarchy) {
				PauseGame ();
			} 
			else 
			{
				ResumeGame ();
			}
		}
	}
}
