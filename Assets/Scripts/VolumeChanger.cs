using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeChanger : MonoBehaviour {
    public bool isMusic;
	public Image volumeSlider;
	public Button[] buttons;
	public GameObject[] arrows;



	private bool pressed = true;
	private bool oneFrameAxis;
	private float lerpVolume;
	private Vector3 arrowSize;
	// Use this for initialization
	void Start () {
        if (isMusic)
            volumeSlider.fillAmount = SceneSwitchereController.instance.volumeMusic;
        else
            volumeSlider.fillAmount = SceneSwitchereController.instance.volumeSound;

        lerpVolume = volumeSlider.fillAmount;
		arrowSize = arrows [0].transform.localScale;
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
		if (this.gameObject.activeInHierarchy && pressed) 
		{
			for (int i = 0; i < buttons.Length; i++) 
			{
				buttons [i].interactable = false;
			}
		}

        //set value on object
        if (isMusic)
            volumeSlider.fillAmount = SceneSwitchereController.instance.volumeMusic;
        else
            volumeSlider.fillAmount = SceneSwitchereController.instance.volumeSound;

        float currentVolume;
        if (isMusic)
            currentVolume = SceneSwitchereController.instance.volumeMusic;
        else
            currentVolume = SceneSwitchereController.instance.volumeSound;

        //update value
        if (Input.GetAxisRaw ("Horizontal") != 0) 
		{
			if (!oneFrameAxis) 
			{
                if (isMusic)
                    currentVolume = SceneSwitchereController.instance.volumeMusic;
                else
                    currentVolume = SceneSwitchereController.instance.volumeSound;

                float value;
				if (Input.GetAxisRaw ("Horizontal") > 0) {
					value = 0.1f;	
					arrows [1].GetComponent<Animator> ().Play ("ResizeAnimation");
				} else 
				{
					value = -0.1f;
					arrows [0].GetComponent<Animator> ().Play ("ResizeAnimation");
				}
				lerpVolume = currentVolume + value;

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
				oneFrameAxis = true;

			}
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

		if (Input.GetButtonDown ("Button B") || Input.GetButtonDown("BackKeyboard")) 
		{
			for (int i = 0; i < buttons.Length; i++) 
			{
				buttons [i].interactable = true;
			}
			buttons [0].Select ();
			for (int i = 0; i < arrows.Length; i++) 
			{
				arrows [i].gameObject.SetActive (false);
			}
			this.gameObject.SetActive (false);
		}
        //clamp
        if (currentVolume >= 1) currentVolume = 1;
        if (currentVolume <= 0) currentVolume = 0;
        //all is done for frame
        if (isMusic)
            SceneSwitchereController.instance.SetVolume(currentVolume, true);
        else
            SceneSwitchereController.instance.SetVolume(currentVolume, false);

    }
}