using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceVolume : MonoBehaviour {

    private SceneSwitchereController ssc;
    private AudioSource audSource;

	// Use this for initialization
	void Start ()
    {
        ssc = SceneSwitchereController.instance;
        audSource = transform.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void LateUpdate ()
    {
        audSource.volume = ssc.volumeSound;
	}
}
