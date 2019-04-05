using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempBeatChecker : MonoBehaviour {

    //https://answers.unity.com/questions/821530/call-function-on-all-objects-in-trigger.html


    [SerializeField]
    private float perfectDistance = 0.1f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameObject.GetComponent<Collider>().)
        }
        */
	}

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Mathf.Abs(transform.position.x - other.transform.position.x) <= perfectDistance)
            {
                Debug.Log("Perfect");
            }

            else
            {
                Debug.Log("Beat hit");
            }

            Destroy(other.gameObject);
        }
    }
}
