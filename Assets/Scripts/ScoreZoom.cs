using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreZoom : MonoBehaviour {

	public bool isPlayers;
	// Use this for initialization
	void Start () {
	}


	
	// Update is called once per frame
	void Update () {
		if (isPlayers) 
		{
			if (GameManagerController.instance.playerScore > GameManagerController.instance.AIScore) 
			{
				
			}
			else
			{
				
			}
		}
		else
		{
			if (GameManagerController.instance.playerScore < GameManagerController.instance.AIScore) 
			{
				
			}
			else
			{
				
			}
		}
	}
}
