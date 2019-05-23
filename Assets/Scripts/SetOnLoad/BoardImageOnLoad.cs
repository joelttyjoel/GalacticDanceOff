using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardImageOnLoad : MonoBehaviour {
    public Material imageBoard0;
    public Material imageBoard1;
    public Material imageBoard2;
    public GameObject imageBoardLeft;
    public GameObject imageBoardRight;

	// Use this for initialization
	void Start () {
        if(SceneSwitchereController.instance.selectedCharacter == 0)
        {
            imageBoardLeft.GetComponent<MeshRenderer>().material = imageBoard0;
            imageBoardRight.GetComponent<MeshRenderer>().material = imageBoard0;
        }
        else if(SceneSwitchereController.instance.selectedCharacter == 1)
        {
            imageBoardLeft.GetComponent<MeshRenderer>().material = imageBoard1;
            imageBoardRight.GetComponent<MeshRenderer>().material = imageBoard1;
        }
        else
        {
            imageBoardLeft.GetComponent<MeshRenderer>().material = imageBoard2;
            imageBoardRight.GetComponent<MeshRenderer>().material = imageBoard2;
        }
    }
}
