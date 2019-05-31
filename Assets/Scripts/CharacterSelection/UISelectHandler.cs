using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISelectHandler : MonoBehaviour, ISelectHandler {

	private Vector3 targetRotation;
	public Animator stageAnimator;
	public GameObject[] CharInfoPage;
	public GameObject[] Arrows;
	private Vector3 targetDir;
	private string Dimy, Elüü, Oru;

	Dictionary <int, Vector3> CharSelect;
	int currentSelect = 1;
	float speed = 4f;

	void OnEnable()
	{
		CharSelect = new Dictionary<int, Vector3> ();
		CharSelect [0] = new Vector3 (35, -109, -67); //Dimy
		CharSelect [1] = new Vector3(-78, -169, -83); //Oru
		CharSelect [2] = new Vector3(-14, -114, 12);  //Elüü
	}


	// Use this for initialization
	void Start () 
	{
		Dimy = "Dimy";
		Elüü = "Elüü";
		Oru = "Oru";
		targetRotation = this.transform.localScale;
		stageAnimator = GetComponent<Animator> ();

		CorrectSelectedGameObject (currentSelect);
		CharacterPage (currentSelect);
		targetDir = CharSelect [currentSelect] - transform.position;

		Invoke ("DisableAnimator", 2.4f);
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
        
		if ((Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown(InputManager.instance.Submit)) && (EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().isActiveAndEnabled && !SceneSwitchereController.instance.dissableAllInputs))
        {
        	EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().SetTrigger("Pressed");
        }
        

        if (Input.GetKeyDown (KeyCode.Y)) 
		{
			Debug.Log (EventSystem.current.currentSelectedGameObject);
		}

		if (EventSystem.current.currentSelectedGameObject.name == Dimy) 
		{
			CorrectSelectedGameObject (0);
			targetDir = CharSelect [0] - transform.position;
			CharacterPage (0);
		}

		if (EventSystem.current.currentSelectedGameObject.name == Oru) 
		{
			CorrectSelectedGameObject (1);
			targetDir = CharSelect [1] - transform.position;
			CharacterPage (1);
		}

		if (EventSystem.current.currentSelectedGameObject.name == Elüü) 
		{
			CorrectSelectedGameObject (2);
			targetDir = CharSelect [2] - transform.position;
			CharacterPage (2);
		}

	/*	if (Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			currentSelect += 1;
			if (currentSelect == 3)
				currentSelect = 0;
			CorrectSelectedGameObject (currentSelect);
			targetDir = CharSelect [currentSelect] - transform.position;
			CharacterPage (currentSelect);
		} 
		else if (Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			currentSelect -= 1;
			if (currentSelect == -1)
				currentSelect = 2;
			CorrectSelectedGameObject (currentSelect);
			targetDir = CharSelect [currentSelect] - transform.position;
			CharacterPage (currentSelect);
		}*/
		Quaternion newDir = Quaternion.LookRotation (new Vector3(targetDir.x, 0, -targetDir.z));
		transform.rotation = Quaternion.Slerp (transform.rotation, newDir, speed * Time.deltaTime);
	}


	private void CorrectSelectedGameObject(int character)
	{
		if (character == 0)
			EventSystem.current.SetSelectedGameObject (GameObject.Find ("Dimy"));
		else if(character == 1)
			EventSystem.current.SetSelectedGameObject (GameObject.Find ("Oru"));
		else if(character == 2)
			EventSystem.current.SetSelectedGameObject (GameObject.Find ("Elüü"));
	}

}
