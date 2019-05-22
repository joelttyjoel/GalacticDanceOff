using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthImageOnLoad : MonoBehaviour {
    public Sprite imageIcon0;
    public Sprite imageBar0;
    public Sprite imageIcon1;
    public Sprite imageBar1;
    public Sprite imageIcon2;
    public Sprite imageBar2;
    public GameObject imageIconObject;
    public GameObject imageBarObject;

	// Use this for initialization
	void Start () {
        if(SceneSwitchereController.instance.selectedCharacter == 0)
        {
            imageIconObject.GetComponent<Image>().sprite = imageIcon0;
            imageBarObject.GetComponent<Image>().sprite = imageBar0;
        }
        else if(SceneSwitchereController.instance.selectedCharacter == 1)
        {
            imageIconObject.GetComponent<Image>().sprite = imageIcon1;
            imageBarObject.GetComponent<Image>().sprite = imageBar1;
        }
        else
        {
            imageIconObject.GetComponent<Image>().sprite = imageIcon2;
            imageBarObject.GetComponent<Image>().sprite = imageBar2;
        }
    }
}
