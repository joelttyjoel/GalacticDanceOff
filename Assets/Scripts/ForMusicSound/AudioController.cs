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

    [FMODUnity.EventRef]
    public string loseStingerEventPath;
    private EventInstance loseStingerSounds;

    [FMODUnity.EventRef]
    public string spotlightEventPath;
    private EventInstance spotlightSounds;

    [FMODUnity.EventRef]
    public string crowdCheerEventPath;
    private EventInstance crowdCheerSounds;

    [FMODUnity.EventRef]
    public string fireworkEventPath;
    private EventInstance fireWorkSounds;

    [FMODUnity.EventRef]
    public string songSelectSoundsPath;
    private EventInstance songSelectSounds;

    [FMODUnity.EventRef]
    public string planetSoundsPath;
    private EventInstance planetSounds;

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
        loseStingerSounds = FMODUnity.RuntimeManager.CreateInstance(loseStingerEventPath);
        spotlightSounds = FMODUnity.RuntimeManager.CreateInstance(spotlightEventPath);
        crowdCheerSounds = FMODUnity.RuntimeManager.CreateInstance(crowdCheerEventPath);
        fireWorkSounds = FMODUnity.RuntimeManager.CreateInstance(fireworkEventPath);
        songSelectSounds = FMODUnity.RuntimeManager.CreateInstance(songSelectSoundsPath);
        planetSounds = FMODUnity.RuntimeManager.CreateInstance(planetSoundsPath);

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

    public void PlaySongSelectSounds(float selectSound)
    {
        if (!audioIsEnabled) return;
        songSelectSounds.setParameterValue("StageSelectUI", selectSound);
        songSelectSounds.start();
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

    public void PlayLoseStinger()
    {
        if (!audioIsEnabled) return;
        loseStingerSounds.start();
    }

    public void PlaySpotlight()
    {
        if (!audioIsEnabled) return;
        spotlightSounds.start();
    }

    public void PlayCrowdCheer()
    {
        if (!audioIsEnabled) return;
        crowdCheerSounds.start();
    }

    public void PlayFireWork()
    {
        if (!audioIsEnabled) return;
        fireWorkSounds.start();
    }

    public void PlayPlanetSound()
    {
        if (!audioIsEnabled) return;
        planetSounds.start();
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
    
    //pause all but crowd sound
    public void PauseSound()
    {
        Debug.Log("Has been paused");
        buttonPress.setPaused(true);
        hpSounds.setPaused(true);
        pauseSounds.setPaused(true);
        mainMenuSounds.setPaused(true);
        scoreSounds.setPaused(true);
        winStingerSounds.setPaused(true);
        spotlightSounds.setPaused(true);
        crowdCheerSounds.setPaused(true);
    }
    public void ResumeSound()
    {
        Debug.Log("Resumed");
        buttonPress.setPaused(false);
        hpSounds.setPaused(false);
        pauseSounds.setPaused(false);
        mainMenuSounds.setPaused(false);
        scoreSounds.setPaused(false);
        winStingerSounds.setPaused(false);
        spotlightSounds.setPaused(false);
        crowdCheerSounds.setPaused(false);
    }
}
