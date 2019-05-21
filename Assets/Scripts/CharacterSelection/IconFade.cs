using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconFade : MonoBehaviour {

	public Sprite info1, info2;
	private Image image;
	string horizontalController;
	string verticalController;

	void Start () {
		//resize = 1.3f;
		image = GetComponent<Image> ();
		verticalController = "Vertical";

		if (SceneSwitchereController.instance.xBox) 
		{
			horizontalController = "XboxHorizontal";
			verticalController = "XboxVertical";
		}
		if (SceneSwitchereController.instance.Ps4) 
		{
			horizontalController = "PS4Horizontal";
			verticalController = "PS4Vertical";
		}

	}

	// Update is called once per frame
	void Update () {
		if(InputManager.instance.GetAxis(verticalController) > 0 || Input.GetAxis("Vertical") > 0)
		{
				image.sprite = info1;
		}
		else if (InputManager.instance.GetAxis(verticalController) < 0 || Input.GetAxis("Vertical") < 0)
		{
			image.sprite = info2;
		}
	}

}