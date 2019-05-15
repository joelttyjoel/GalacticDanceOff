using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeSpriteIfInteracted : MonoBehaviour {

	Image image;
	public Sprite changeSprite1;
	public Sprite changeSprite2;
	private Vector3 originalScale;
	public float resize;
	// Use this for initialization
	void Start () {
		//resize = 1.3f;
		image = GetComponent<Image> ();
		originalScale = this.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if (EventSystem.current.currentSelectedGameObject == this.gameObject) 
		{
			if (image.sprite != changeSprite2) 
			{
                image.sprite = changeSprite2;
				this.transform.localScale = originalScale * resize;
			}
		}
		else if (image.sprite != changeSprite1 ) 
		{
			Debug.Log("Switch sound PART 2");

			image.sprite = changeSprite1;
			this.transform.localScale = originalScale;
		}



	}
}
