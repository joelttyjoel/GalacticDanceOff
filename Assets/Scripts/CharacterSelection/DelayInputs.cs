using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DelayInputs : MonoBehaviour {

	private GameObject currentButton;
	private AxisEventData currentAxis;
	private string currentHorizontal;
	private string PS4_controller = "PHorizontal"; 
	private string XBOX_controller = "XHorizontal";

	//timer
	private float timeBetweenInputs = 1f; //in seconds
	private float timer = 0;

	// Use this for initialization
	void Start () {
		currentHorizontal = "Horizontal";
		if (SceneSwitchereController.instance.Ps4)
			currentHorizontal = PS4_controller;
		if (SceneSwitchereController.instance.xBox)
			currentHorizontal = XBOX_controller;
	}
	
	// Update is called once per frame
	void Update () {
		if (timer == 0) 
		{
			currentAxis = new AxisEventData (EventSystem.current);
			currentButton = EventSystem.current.currentSelectedGameObject;

			if ((Input.GetAxis (currentHorizontal) > 0 || Input.GetAxis("Horizontal") > 0) && !SceneSwitchereController.instance.dissableAllInputs) 
			{
				currentAxis.moveDir = MoveDirection.Right;
				ExecuteEvents.Execute (currentButton, currentAxis, ExecuteEvents.moveHandler);
				timer = timeBetweenInputs;
			}
			else if ((Input.GetAxis (currentHorizontal) < 0 || Input.GetAxis("Horizontal") < 0) && !SceneSwitchereController.instance.dissableAllInputs) 
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
