using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSelectionChangePlaySound : MonoBehaviour {

    public bool playSelectedSoundOnThis;

    public void WasSelected(string lastSelectedName)
    {
        Debug.Log("This was selected");
        if(playSelectedSoundOnThis && this.isActiveAndEnabled)
        {
            AudioController.instance.PlayMainMenueSound(0f);
        }
        else
        {
            // in song select
            if(lastSelectedName.Length >= 8)
            AudioController.instance.PlaySongSelectSounds(0f);
        }
    }
}
