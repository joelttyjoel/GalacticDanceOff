using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickRemapper : MonoBehaviour {
    SceneSwitchereController sceneSwitcher;
	// Use this for initialization
	void Start () {
        sceneSwitcher = SceneSwitchereController.instance;
	}
	
    public void LoadSceneByName(string nameScene, string nameSequence)
    {
        sceneSwitcher.LoadSceneByName(nameScene, nameSequence);
    }
    public void GotoScene_SetName(string sceneName)
    {
        sceneSwitcher.GotoScene_SetName(sceneName);
    }
    public void GotoScene_SetSequence(string sequenceName)
    {
        sceneSwitcher.GotoScene_SetSequence(sequenceName);
    }
    public void SetSpOrMp_TrueIsSp(bool isSpin)
    {
        sceneSwitcher.SetSpOrMp_TrueIsSp(isSpin);
    }
}
