using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioRemapper : MonoBehaviour {
    AudioController audioController;

	// Use this for initialization
	void Start () {
        audioController = AudioController.instance;
	}

    public void PlayNoteSound(float selectSound)
    {
        audioController.PlayNoteSound(selectSound);
    }

    public void PlayHpSound(float selectSound)
    {
        audioController.PlayHpSound(selectSound);
    }

    //public void SetParamPauseSound(float selectSound)
    //{
    //    if (!audioIsEnabled) return;
    //    pauseSounds.setParameterValue("PauseCountdown", selectSound);
    //}
    //public void PlayPauseSound()
    //{
    //    if (!audioIsEnabled) return;
    //    pauseSounds.start();
    //}

    public void PlayMainMenueSound(float selectSound)
    {
        audioController.PlayMainMenueSound(selectSound);
    }

    public void PlaySongSelectSounds(float selectSound)
    {
        audioController.PlaySongSelectSounds(selectSound);
    }
}
