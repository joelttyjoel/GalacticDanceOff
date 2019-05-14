using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISelectHandler : MonoBehaviour, ISelectHandler {

	// Use this for initialization
	void Start () {
		
	}

	public void OnSelect(BaseEventData eventData)
	{
		Debug.Log ( this.gameObject.name + " was selected");
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) 
		{
			Debug.Log (EventSystem.current.currentSelectedGameObject);
		}

		//
	}
}
