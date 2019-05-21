using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISelectHandler : MonoBehaviour, ISelectHandler {

	private Vector3 targetRotation;
	public Animator stageAnimator;
	public GameObject[] CharInfoPage;

	private Vector3 targetDir;

	Dictionary <int, Vector3> CharSelect;
	int currentSelect = 1;
	float speed = 3f;

	void OnEnable()
	{
		CharSelect = new Dictionary<int, Vector3> ();
		CharSelect [0] = new Vector3 (35, -109, -67); //birb
		CharSelect [1] = new Vector3(-78, -169, -83); //square
		CharSelect [2] = new Vector3(-14, -114, 12); //napkin
	}


	// Use this for initialization
	void Start () 
	{
		targetRotation = this.transform.localScale;
		stageAnimator = GetComponent<Animator> ();
		Invoke ("DisableAnimator", 2.5f);

		targetDir = CharSelect [currentSelect] - transform.position;
	}

	private void CharacterPage(int character)
	{
		for (int i = 0; i < CharInfoPage.Length; i++) 
		{
			
			if (i == character) 
			{
				CharInfoPage [i].SetActive (true);
			} else 
			{
				CharInfoPage [i].SetActive (false);
			}
		}


	}
		
	private void DisableAnimator()
	{
		stageAnimator.enabled = false;
	}

	public void OnSelect(BaseEventData eventData)
	{
		Debug.Log ( this.gameObject.name + " was selected");
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
		}

		/* if (Input.GetKeyDown (KeyCode.Return)) 
		{
			Debug.Log (EventSystem.current.currentSelectedGameObject);
		} */

		if (Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			currentSelect += 1;
			if (currentSelect == 3)
				currentSelect = 0;
			targetDir = CharSelect [currentSelect] - transform.position;
			CharacterPage (currentSelect);
		} 
		else if (Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			currentSelect -= 1;
			if (currentSelect == -1)
				currentSelect = 2;
			targetDir = CharSelect [currentSelect] - transform.position;
			CharacterPage (currentSelect);
		}

		Quaternion newDir = Quaternion.LookRotation (new Vector3(targetDir.x, 0, -targetDir.z));
		transform.rotation = Quaternion.Slerp (transform.rotation, newDir, speed * Time.deltaTime);
	}
}
