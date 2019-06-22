﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickRemapper : MonoBehaviour {
    SceneSwitchereController sceneSwitcher;
	// Use this for initialization
	void Start () {
        sceneSwitcher = SceneSwitchereController.instance;
	}
	
    //public void LoadSceneByName(string nameScene, string nameSequence)
    //{
    //    sceneSwitcher.LoadSceneByName(nameScene, nameSequence);
    //}
    public void GotoScene_SetName(string sceneName)
    {
        sceneSwitcher.GotoScene_SetName(sceneName);
    }
    public void GotoScene_SetSequence(string sequenceName)
    {
        sceneSwitcher.GotoScene_SetSequence(sequenceName);
    }
    public void SetButtonSongSelect(GameObject sender)
    {
        SceneSwitchereController.instance.SetButtonSongSelect(sender);
    }

    public void SetOwnCharacter(int selected)
    {
        SceneSwitchereController.instance.selectedCharacter = selected;
    }
    public void SetOponentCharacter(int selected)
    {
        SceneSwitchereController.instance.selectedOponent = selected;
    }

    public void SetPausable(bool thing)
    {
        PauseGameScript.instance.isPausable = thing;
    }

    public void SetCampaignMode(bool mode)
    {
        SceneSwitchereController.instance.SetGameMode(mode);
    }

    public void ResetVariables()
    {
        SceneSwitchereController.instance.ResetVariables();
    }
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("ClEARING PLAYERPREFS");
    }
}
