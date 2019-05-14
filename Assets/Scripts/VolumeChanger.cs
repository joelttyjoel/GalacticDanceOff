using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeChanger : MonoBehaviour {

	public Image volumeSlider;
	public Button[] buttons;
	private bool pressed = true;
	private bool oneFrameAxis;
	private float lerpVolume;
	// Use this for initialization
	void Start () {
		lerpVolume = volumeSlider.fillAmount;
	}
		
	// Update is called once per frame
	void Update () {
		if (this.gameObject.activeInHierarchy && pressed) 
		{
			for (int i = 0; i < buttons.Length; i++) 
			{
				buttons [i].interactable = false;
			}
		}
		if (Input.GetAxisRaw ("Horizontal") != 0) 
		{
			if (!oneFrameAxis) 
			{
				float value;
				if (Input.GetAxisRaw ("Horizontal") > 0) {
					value = 0.1f;	
				} else 
				{
					value = -0.1f;
				}
				lerpVolume = volumeSlider.fillAmount + value;

				for (int i = 10; i > 0; i--) 
				{
					if (volumeSlider.fillAmount < (float)i + 0.2f && volumeSlider.fillAmount > (float)i - 0.8f) 
					{
						volumeSlider.fillAmount = i / 10;
						if (volumeSlider.fillAmount < 0.55f && volumeSlider.fillAmount > 0.45f) 
						{
							lerpVolume = 0.49f;
						}
					}
				}
				oneFrameAxis = true;

			}
		}
		if (volumeSlider.fillAmount != lerpVolume) 
		{
			volumeSlider.fillAmount = lerpVolume;
		}
		oneFrameAxis = false;

		if (Input.GetAxisRaw ("Horizontal") != 0) 
		{
			oneFrameAxis = true;
		}

		if (Input.GetButtonDown ("Button B") || Input.GetButtonDown("BackKeyboard")) 
		{
			for (int i = 0; i < buttons.Length; i++) 
			{
				buttons [i].interactable = true;
			}
			buttons [0].Select ();
			this.gameObject.SetActive (false);
		}
	}
}