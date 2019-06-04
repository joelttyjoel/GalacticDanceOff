using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TutorialSwitcher : MonoBehaviour {
    public List<Sprite> tutorialPages;
    public Button backButton;
    private int count = 0;
    private bool coolDown;
    private float timer;

    private bool hasBeenClicked = false;

	// Use this for initialization
	void Start () {
        coolDown = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetAxisRaw(EventSystem.current.GetComponent<MyInputModule>().horizontalAxis) > 0 && !hasBeenClicked)
        {
            AudioController.instance.PlayMainMenueSound(0);
            // coolDown = false;
            count++;
            hasBeenClicked = true;
            //Invoke("CoolDown", 0.1f);
        }
        else if(Input.GetAxisRaw(EventSystem.current.GetComponent<MyInputModule>().horizontalAxis) < 0 && !hasBeenClicked)
        {
            if(count != 0)AudioController.instance.PlayMainMenueSound(0);
            //coolDown = false;
            count--;
            hasBeenClicked = true;
            //Invoke("CoolDown", 0.1f);
        }
        if (count <= 0) count = 0;
        if (count >= tutorialPages.Count)
        {
            count = 0;
            backButton.GetComponent<Button>().onClick.Invoke();
        }

        if (Input.GetAxisRaw(EventSystem.current.GetComponent<MyInputModule>().horizontalAxis) == 0)
        {
            hasBeenClicked = false;
        }

        GetComponent<Image>().sprite = tutorialPages[count];
    }
}
