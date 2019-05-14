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
				float value = Input.GetAxisRaw ("Horizontal") / 10f;
				lerpVolume = volumeSlider.fillAmount + value;
				oneFrameAxis = true;
				//Input.ResetInputAxes ();
			}
		}
		if (volumeSlider.fillAmount != lerpVolume) 
		{
			volumeSlider.fillAmount = Mathf.Lerp (volumeSlider.fillAmount, lerpVolume, 1f);
		}
		oneFrameAxis = false;

		if (Input.GetAxisRaw ("Horizontal") != 0) 
		{
			oneFrameAxis = true;
		}

		if (Input.GetButtonDown ("Button B")) 
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