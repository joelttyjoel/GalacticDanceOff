using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class ExecuteBlockStartOnce : MonoBehaviour {
    public string nameOfBlock;
    public bool isCharSelectNotSongSel;
	// Use this for initialization
	void Start () {
        if(isCharSelectNotSongSel)
        {
            if (!SceneSwitchereController.instance.hasDoneCharacterSelect)
            {
                SceneSwitchereController.instance.hasDoneCharacterSelect = true;
                GetComponent<Flowchart>().ExecuteBlock(nameOfBlock);
            }
        }
        else
        {
            if (!SceneSwitchereController.instance.hasDoneSongSelect)
            {
                SceneSwitchereController.instance.hasDoneSongSelect = true;
                GetComponent<Flowchart>().ExecuteBlock(nameOfBlock);
            }
        }
	}
}
