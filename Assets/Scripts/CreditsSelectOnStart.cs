using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsSelectOnStart : MonoBehaviour {
    Fungus.Flowchart flowChart;
	// Use this for initialization
	void Start () {
		if(SceneSwitchereController.instance.selectedCharacter == 0)
        {
            flowChart.ExecuteBlock("OruWin");
        }
        else if (SceneSwitchereController.instance.selectedCharacter == 1)
        {
            flowChart.ExecuteBlock("StickWin");
        }
        else
        {
            flowChart.ExecuteBlock("DimyWin");
        }
    }
}
