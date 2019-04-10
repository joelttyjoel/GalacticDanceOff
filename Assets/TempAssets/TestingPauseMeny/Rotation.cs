using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

	float smooth = 5.0f;
	float tiltAngle = 60.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		gameObject.transform.Rotate (Vector3.up * (50 * Time.deltaTime));

	}
}
