using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempBeatMover : MonoBehaviour {

    [SerializeField]
    private float speed = 2.0f;

    private Rigidbody rig;

	// Use this for initialization
	void Start ()
    {
        rig = gameObject.GetComponent<Rigidbody>();

        rig.velocity = new Vector3(speed, 0, 0);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
