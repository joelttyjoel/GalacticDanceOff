using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temoBeatDeleter : MonoBehaviour {


    [SerializeField]
    private GameObject beatSpawner;
    private tempSpawnerScript tss;

    // Use this for initialization
    void Start ()
    {
        tss = beatSpawner.GetComponent<tempSpawnerScript>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        tss.GetComponent<tempSpawnerScript>().DeleteFirstInQueue();
        //Destroy(other.gameObject, 0.1f);
        Debug.Log("Missed beat");
    }
}
