using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconFade : MonoBehaviour {

	public Sprite info1, info2;
	public bool Arrows;
	public bool Right;

	private bool cooldown;
	private Image image;
	string horizontalController;
	string verticalController;

	void Start () {
		//resize = 1.3f;
		cooldown = true;
		image = GetComponent<Image> ();
		verticalController = "Vertical";
		horizontalController = "Horizontal";

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
		
		InfoPage (Arrows);
		ArrowPage (Arrows);

	}

	private void InfoPage(bool info)
	{
		if((InputManager.instance.GetAxis(verticalController) > 0 || Input.GetAxis("Vertical") > 0) && !info)
		{
			image.sprite = info1;
		}
		else if ((InputManager.instance.GetAxis(verticalController) < 0 || Input.GetAxis("Vertical") < 0) && !info)
		{
			image.sprite = info2;
		}
	}

	private void ArrowPage(bool arrow)
	{
		if ((InputManager.instance.GetAxis (horizontalController) > 0 || Input.GetAxis ("Horizontal") > 0) && arrow && Right && cooldown) 
		{
			cooldown = false;
			this.gameObject.GetComponent<Animator> ().Play ("ResizeAnimation");
			Invoke ("CoolDown", 1f);
			image.sprite = info2;
		} 
		else if ((InputManager.instance.GetAxis (horizontalController) < 0 || Input.GetAxis ("Horizontal") < 0) && arrow && !Right && cooldown) 
		{
			cooldown = false;
			this.gameObject.GetComponent<Animator> ().Play ("ResizeAnimation");
			Invoke ("CoolDown", 1f);
			image.sprite = info2;
		}
	}
	private void CoolDown()
	{
		Input.ResetInputAxes ();
		image.sprite = info1;
		cooldown = true;
	}

}