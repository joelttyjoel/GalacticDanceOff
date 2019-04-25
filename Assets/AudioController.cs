using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioController : MonoBehaviour {
    [FMODUnity.EventRef]
    public string buttonPressEventPath;
    private EventInstance buttonPress;

    public static AudioController instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        //now replaces already existing gameManager instead
        else if (instance != this)
            Destroy(instance.gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        buttonPress = FMODUnity.RuntimeManager.CreateInstance(buttonPressEventPath);
	}

    public void PlayNoteSound(float selectSound)
    {
        buttonPress.setParameterValue("Tightness" , selectSound);
        buttonPress.start();
    }
}
