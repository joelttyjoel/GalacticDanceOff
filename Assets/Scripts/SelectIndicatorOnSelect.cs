using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectIndicatorOnSelect : MonoBehaviour {

    public List<Sprite> imagesInOrder;

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().sprite = imagesInOrder[SceneSwitchereController.instance.selectedCharacter];
	}
}
