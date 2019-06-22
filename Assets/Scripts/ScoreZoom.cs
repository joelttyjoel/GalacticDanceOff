using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreZoom : MonoBehaviour {

	public bool isPlayers;
    public Vector3 positionLose;
    public Vector3 positionWin;

    // Update is called once per frame
    void Update () {
		if (isPlayers) 
		{
			if (GameManagerController.instance.playerScore > GameManagerController.instance.AIScore) 
			{
                transform.localPosition = positionWin;
			}
			else
			{
                transform.localPosition = positionLose;
            }
		}
		else
		{
			if (GameManagerController.instance.playerScore < GameManagerController.instance.AIScore) 
			{
                transform.localPosition = positionWin;
            }
			else
			{
                transform.localPosition = positionLose;
            }
		}
	}
}
