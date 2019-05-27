using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicManagerScript : MonoBehaviour {
    public StudioEventEmitter menuEmitter;

    [FMODUnity.EventRef]
    private EventInstance menuEvent;

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

    private void Start()
    {
        menuEvent = menuEmitter.EventInstance;
        menuEvent.setVolume(0f);
    }

    public void ResetSelected()
    {
        menuEmitter.SetParameter("Selected Song", 0);
    }

    public void SetVolumeMusic(float volume)
    {
        menuEvent.setVolume(volume);
    }
}
