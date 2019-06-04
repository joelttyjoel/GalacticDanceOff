using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnSelectScript : MonoBehaviour, ISelectHandler, IDeselectHandler, IEventSystemHandler  {
    public Sprite onSelectSprite;
    public Sprite onDeselectSprite;

    private Vector3 originalscale;

    public void OnDeselect(BaseEventData eventData)
    {
        this.transform.localScale = originalscale;
        this.gameObject.GetComponent<Image>().sprite = onDeselectSprite;
    }

    public void OnSelect(BaseEventData eventData)
    {
        this.transform.localScale = 1.2f * originalscale;
        this.gameObject.GetComponent<Image>().sprite = onSelectSprite;
    }

    // Use this for initialization
    void Start () {
        originalscale = this.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
