using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiController: MonoBehaviour {

	public GameObject pausePanel;
	private float countdown;

	public void Start()
	{
		countdown = 3f;
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
		float pauseTime = Time.realtimeSinceStartup + countdown;
		while (Time.realtimeSinceStartup < pauseTime) 
		{
			Debug.Log ("working=");
			yield return 0;
		}
		Time.timeScale = 1f;
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
