using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongIconChecker : MonoBehaviour {
    public Sprite thisSpriteIfCleared;

	// Use this for initialization
	void Start () {
        //if this button and won last, this is cleared
        //Debug.Log(this.gameObject.name + " " + SceneSwitchereController.instance.wonLast + SceneSwitchereController.instance.lastBattleButton);
        if (SceneSwitchereController.instance.wonLast && SceneSwitchereController.instance.lastBattleButtonName == this.gameObject.name)
        {
            //Debug.Log("tHis has achangged: " + this.gameObject.name);
            SceneSwitchereController.instance.buttonsAllCleared.Add(this.gameObject.name);
        }

        //wait tiny amount so all can finish adding then go through all and check if fthis is in it to see if should dissable
        Invoke("checkAllIfThisIs", 0.05f);
	}

    void checkAllIfThisIs()
    {
        foreach(string a in SceneSwitchereController.instance.buttonsAllCleared)
        {
            if(a == this.gameObject.name)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = thisSpriteIfCleared;
            }
        }
    }
}
