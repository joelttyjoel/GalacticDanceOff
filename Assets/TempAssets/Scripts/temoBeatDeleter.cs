using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temoBeatDeleter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject, 0.1f);
        Debug.Log("Missed beat");
    }
}
