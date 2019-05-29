using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour {
    public string musicVolPath;
    public string soundVolPath;

    FMOD.Studio.VCA musicVca;
    FMOD.Studio.VCA soundVca;

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

    [FMODUnity.EventRef]
    public string scoreEventPath;
    private EventInstance scoreSounds;

    [FMODUnity.EventRef]
    public string winStingerEventPath;
    private EventInstance winStingerSounds;

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
        scoreSounds = FMODUnity.RuntimeManager.CreateInstance(scoreEventPath);
        winStingerSounds = FMODUnity.RuntimeManager.CreateInstance(winStingerEventPath);

        //get vcas
        musicVca = FMODUnity.RuntimeManager.GetVCA(musicVolPath);
        soundVca = FMODUnity.RuntimeManager.GetVCA(soundVolPath);
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

    public void PlayScoreSound()
    {
        
        if (!audioIsEnabled) return;
        //scoreSounds.setParameterValue("MenuSelect", selectSound);
        scoreSounds.start();
    }

    public void PlayWinStinger()
    {
        if (!audioIsEnabled) return;
        winStingerSounds.start();
    }

    public void SetVolumeByFloat(float value, bool isMusic)
    {
        if(isMusic)
        {
            musicVca.setVolume(value);
        }
        else
        {
            soundVca.setVolume(value);
        }
    }
}
