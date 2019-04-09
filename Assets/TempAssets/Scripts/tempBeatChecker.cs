using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempBeatChecker : MonoBehaviour {

    //https://answers.unity.com/questions/821530/call-function-on-all-objects-in-trigger.html


    [SerializeField]
    private float hitDistance = 0.5f;
    [SerializeField]
    private float perfectDistance = 0.1f;

    [SerializeField]
    private GameObject beatSpawner;
    private tempSpawnerScript tss;

	// Use this for initialization
	void Start ()
    {
        tss = beatSpawner.GetComponent<tempSpawnerScript>();
	}
	
    //Change and use time instead of distance between the checker and beat!
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject beat = tss.GetFirstInQueue();
            if (beat)   //beat != null
            {
                if (Mathf.Abs(transform.position.x - beat.transform.position.x) <= hitDistance)
                {
                    if (Mathf.Abs(transform.position.x - beat.transform.position.x) <= perfectDistance)
                    {
                        Debug.Log("Perfect");
                    }

                    else
                    {
                        Debug.Log("Hit");
                    }

                    tss.DeleteFirstInQueue();
                }
            }
        }
	}


    //Draw Gismo to view distance in inspector




    //private void OnTriggerStay(Collider other)
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        if (Mathf.Abs(transform.position.x - other.transform.position.x) <= perfectDistance)
    //        {
    //            Debug.Log("Perfect");
    //        }

    //        else
    //        {
    //            Debug.Log("Beat hit");
    //        }

    //        Destroy(other.gameObject);
    //    }
    //}
}
