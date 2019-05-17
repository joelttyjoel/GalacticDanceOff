using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISelectHandler : MonoBehaviour, ISelectHandler {

	private Vector3 targetRotation;
	public Animator stageAnimator;

	// Use this for initialization
	void Start () {
		targetRotation = this.transform.localScale;
		stageAnimator = GetComponent<Animator> ();
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
		if (Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			
		}
	}
}
