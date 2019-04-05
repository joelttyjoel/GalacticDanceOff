using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempSpawnerScript : MonoBehaviour {

    [SerializeField]
    private GameObject beatObjectPrefab;

    [SerializeField]
    private float bpm = 20;

    private float currentTime = 0.0f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= 60/bpm
            && beatObjectPrefab)
        {
            currentTime -= 60 / bpm;
            GameObject beat = Instantiate(beatObjectPrefab, transform.position, transform.rotation);
            Destroy(beat, 10.0f);
        }
	}
}
