using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Vector3 teleport;
	public Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();

		inputManager = GameObject.FindObjectOfType<InputManager> ();

	}

	InputManager inputManager;


	// Update is called once per frame
	void Update () {
		if (inputManager.GetButtonDown("W")) 
		{
			GetComponent<Rigidbody> ().velocity = new Vector2 (0, 1);
		}
		if (inputManager.GetButtonDown("S")) 
		{
			GetComponent<Rigidbody> ().velocity = new Vector2 (0, -1);
		}
		if (inputManager.GetButtonDown("A"))  
		{
			GetComponent<Rigidbody> ().velocity = new Vector2 (-1, 0);
		}
		if (inputManager.GetButtonDown("D")) 
		{
			GetComponent<Rigidbody> ().velocity = new Vector2 (1, 0);
		}


	}
}
