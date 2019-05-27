using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VolumeChanger : MonoBehaviour {
    public bool isMusic;
	public Image volumeSlider;
	public Button[] buttons;
	public GameObject[] arrows;

	private bool pressed = true;
	private bool oneFrameAxis;
	private float lerpVolume;
	private float currentVolume;
	private float value;


	// Use this for initialization
	void Start () {
		if (isMusic) 
		{
			volumeSlider.fillAmount = SceneSwitchereController.instance.volumeMusic;
			lerpVolume = volumeSlider.fillAmount;
		}
		else 
		{
			volumeSlider.fillAmount = SceneSwitchereController.instance.volumeSound;
			lerpVolume = volumeSlider.fillAmount;
		}
	}

    public void SetOnStart()
    {
        if (isMusic)
            volumeSlider.fillAmount = SceneSwitchereController.instance.volumeMusic;
        else
			volumeSlider.fillAmount = SceneSwitchereController.instance.volumeSound;
    }
		


	// Update is called once per frame
	void Update () {
        //set value on object
		if (this.transform.parent.gameObject != EventSystem.current.currentSelectedGameObject) 
		{
			return;
		}

		if (isMusic) 
		{
			volumeSlider.fillAmount = SceneSwitchereController.instance.volumeMusic;
			currentVolume = SceneSwitchereController.instance.volumeMusic;
		}
		else
		{
			volumeSlider.fillAmount = SceneSwitchereController.instance.volumeSound;
			currentVolume = SceneSwitchereController.instance.volumeSound;
		}


			

        //update value
		if (Input.GetAxisRaw ("Horizontal") != 0 && EventSystem.current.currentSelectedGameObject.transform.childCount > 0) {
			if (!oneFrameAxis && this.transform.parent.gameObject == EventSystem.current.currentSelectedGameObject) 
			{
				if(isMusic)
					currentVolume = SceneSwitchereController.instance.volumeMusic;
				else
					currentVolume = SceneSwitchereController.instance.volumeSound;
				

				if (Input.GetAxisRaw ("Horizontal") > 0 && EventSystem.current.currentSelectedGameObject == this.transform.parent.gameObject) {
					value = 0.1f;	
					EventSystem.current.currentSelectedGameObject.transform.GetChild (2).GetComponent<Animator> ().Play ("ResizeAnimation");
					//arrows [1].GetComponent<Animator> ().Play ("ResizeAnimation");
				} else if(Input.GetAxisRaw ("Horizontal") < 0 && EventSystem.current.currentSelectedGameObject == this.transform.parent.gameObject) {
					value = -0.1f;
					EventSystem.current.currentSelectedGameObject.transform.GetChild (1).GetComponent<Animator> ().Play ("ResizeAnimation");
				}

					lerpVolume = currentVolume + value;


					//pixel fixing to match the ButtonColor
				for (int i = 10; i > 0; i--) 
				{
					if (currentVolume < (float)i + 0.2f && currentVolume > (float)i - 0.8f) 
					{
						currentVolume = i / 10;
						if (currentVolume < 0.55f && currentVolume > 0.45f) 
						{
							lerpVolume = 0.49f;
						}
					}
				}
			}
			oneFrameAxis = true;
		}
			
        //ok
		if (currentVolume != lerpVolume) 
		{
            currentVolume = lerpVolume;
		}
		oneFrameAxis = false;

		if (Input.GetAxisRaw ("Horizontal") != 0) 
		{
			oneFrameAxis = true;
		}
		if(Input.GetAxisRaw ("Horizontal") == 0)
		{
			for (int i = 0; i < arrows.Length; i++) 
			{
				arrows [i].GetComponent<Animator> ().Play ("DownSizeAnimation");
			}	
		}


        //clamp
        if (currentVolume >= 1) currentVolume = 1;
        if (currentVolume <= 0) currentVolume = 0;
        //all is done for frame
		if(isMusic)
			SceneSwitchereController.instance.SetVolume(currentVolume, true);
		else
			SceneSwitchereController.instance.SetVolume(currentVolume, false);
    }
}