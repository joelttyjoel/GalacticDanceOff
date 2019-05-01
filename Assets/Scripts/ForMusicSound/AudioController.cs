using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour {
    [FMODUnity.EventRef]
    public string buttonPressEventPath;
    private EventInstance buttonPress;

    [FMODUnity.EventRef]
    public string hpSoundsEventPath;
    private EventInstance hpSounds;

    [FMODUnity.EventRef]
    public string pauseSoundsEventPath;
    private EventInstance pauseSounds;

    public bool audioIsEnabled = true;

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
        hpSounds = FMODUnity.RuntimeManager.CreateInstance(hpSoundsEventPath);
        pauseSounds = FMODUnity.RuntimeManager.CreateInstance(pauseSoundsEventPath);
    }

    public void PlayNoteSound(float selectSound)
    {
        if (!audioIsEnabled) return;
        buttonPress.setParameterValue("Tightness" , selectSound);
        buttonPress.start();
    }

    public void PlayHpSound(float selectSound)
    {
        if (!audioIsEnabled) return;
        hpSounds.setParameterValue("EgoMeter", selectSound);
        hpSounds.start();
    }

    public void PlayPauseSound(float selectSound)
    {
        if (!audioIsEnabled) return;
        pauseSounds.setParameterValue("PauseCountdown", selectSound);
        pauseSounds.start();
    }

    public void SetVolumeBySlider(Slider sliderIn)
    {
        buttonPress.setVolume(sliderIn.value);
        hpSounds.setVolume(sliderIn.value);
        pauseSounds.setVolume(sliderIn.value);
    }
}
