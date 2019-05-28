using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DelayInputs : MonoBehaviour {

	private GameObject currentButton;
	private AxisEventData currentAxis;

	//timer
	private float timeBetweenInputs = 1f; //in seconds
	private float timer = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (timer == 0) 
		{
			currentAxis = new AxisEventData (EventSystem.current);
			currentButton = EventSystem.current.currentSelectedGameObject;

			if (Input.GetAxis ("Horizontal2") > 0) 
			{
				currentAxis.moveDir = MoveDirection.Right;
				ExecuteEvents.Execute (currentButton, currentAxis, ExecuteEvents.moveHandler);
				timer = timeBetweenInputs;
			}
			else if (Input.GetAxis ("Horizontal2") < 0) 
			{
				currentAxis.moveDir = MoveDirection.Left;
				ExecuteEvents.Execute (currentButton, currentAxis, ExecuteEvents.moveHandler);
				timer = timeBetweenInputs;
			}

		}
		if (timer > 0) 
		{
			timer -= Time.deltaTime;
		} else 
		{
			timer = 0;
		}
	}
}
