using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthImageOnLoad : MonoBehaviour {
    public Sprite imageIcon00;
    public Sprite imageIcon01;
    public Sprite imageBar0;
    public Sprite imageIcon10;
    public Sprite imageIcon11;
    public Sprite imageBar1;
    public Sprite imageIcon20;
    public Sprite imageIcon21;
    public Sprite imageBar2;
    public GameObject imageIconObject;
    public GameObject imageBarObject;

    public float healthDamageThreshHoldImage = 0.30f;

    private GameManagerController gameManager;

	// Use this for initialization
	void Start () {
        gameManager = GameManagerController.instance;

        if(SceneSwitchereController.instance.selectedCharacter == 0)
        {
            imageIconObject.GetComponent<Image>().sprite = imageIcon00;
            imageBarObject.GetComponent<Image>().sprite = imageBar0;
        }
        else if(SceneSwitchereController.instance.selectedCharacter == 1)
        {
            imageIconObject.GetComponent<Image>().sprite = imageIcon10;
            imageBarObject.GetComponent<Image>().sprite = imageBar1;
        }
        else
        {
            imageIconObject.GetComponent<Image>().sprite = imageIcon20;
            imageBarObject.GetComponent<Image>().sprite = imageBar2;
        }
    }

    void LateUpdate()
    {
        if (SceneSwitchereController.instance.selectedCharacter == 0)
        {
            if ((float)gameManager.playerHealth / (float)gameManager.maxHealth > healthDamageThreshHoldImage)
            {
                imageIconObject.GetComponent<Image>().sprite = imageIcon00;
            }

            else
            {
                imageIconObject.GetComponent<Image>().sprite = imageIcon01;
            }
        }
        else if (SceneSwitchereController.instance.selectedCharacter == 1)
        {
            if ((float)gameManager.playerHealth / (float)gameManager.maxHealth > healthDamageThreshHoldImage)
            {
                imageIconObject.GetComponent<Image>().sprite = imageIcon10;
                return;
            }

            else
            {
                imageIconObject.GetComponent<Image>().sprite = imageIcon11;
            }
        }
        else
        {
            if ((float)gameManager.playerHealth / (float)gameManager.maxHealth > healthDamageThreshHoldImage)
            {
                imageIconObject.GetComponent<Image>().sprite = imageIcon20;
                return;
            }

            else
            {
                imageIconObject.GetComponent<Image>().sprite = imageIcon21;
            }
        }
    }
}
