using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MusicManagerScript : MonoBehaviour {
    public StudioEventEmitter topEmitter;
    public StudioEventEmitter menuEmitter;

    public static MusicManagerScript instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        //now replaces already existing gameManager instead
        else if (instance != this)
            Destroy(instance.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    public void FadeInMusic(bool isMenuMusic)
    {

    }

    public void FadeOutMusic(bool isMenuMusic)
    {

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
