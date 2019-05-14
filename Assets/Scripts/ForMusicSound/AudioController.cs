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

    [FMODUnity.EventRef]
    public string mainMenuEventPath;
    private EventInstance mainMenuSounds;

    public bool audioIsEnabled = true;

    public static AudioController instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        //now replaces already existing gameManager instead
        else if (instance != this)
            Destroy(instance.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        buttonPress = FMODUnity.RuntimeManager.CreateInstance(buttonPressEventPath);
        hpSounds = FMODUnity.RuntimeManager.CreateInstance(hpSoundsEventPath);
        pauseSounds = FMODUnity.RuntimeManager.CreateInstance(pauseSoundsEventPath);
        mainMenuSounds = FMODUnity.RuntimeManager.CreateInstance(mainMenuEventPath);
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

    public void SetParamPauseSound(float selectSound)
    {
        if (!audioIsEnabled) return;
        pauseSounds.setParameterValue("PauseCountdown", selectSound);
    }
    public void PlayPauseSound()
    {
        if (!audioIsEnabled) return;
        pauseSounds.start();
    }

    public void PlayMainMenueSound(float selectSound)
    {
        if (!audioIsEnabled) return;
        mainMenuSounds.setParameterValue("MenuSelect", selectSound);
        mainMenuSounds.start();
    }

	public void SetVolumeBySlider(Image sliderIn)
    {
		buttonPress.setVolume(sliderIn.fillAmount);
		hpSounds.setVolume(sliderIn.fillAmount);
		pauseSounds.setVolume(sliderIn.fillAmount);
		mainMenuSounds.setVolume(sliderIn.fillAmount);
    }
}
