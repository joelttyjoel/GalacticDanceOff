using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MakeThingsEnabledWhenThisSelected : MonoBehaviour {
    public List<GameObject> thingsToEnableWhenThisEnabled;

	// Use this for initialization
	void Start () {
		
	}
    private bool isSelected = false;
	// Update is called once per frame
	void Update () {
        //on selected
        if (isSelected == false && EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            foreach(GameObject a in thingsToEnableWhenThisEnabled)
            {
                a.SetActive(true);
            }

            isSelected = true;
        }

        //was just deselected
        if(isSelected == true && EventSystem.current.currentSelectedGameObject != this.gameObject)
        {
            //if deselected towards things in list, do nothing, if deselected for something else, dissable objects in list
            bool wentToInList = false;
            foreach(GameObject a in thingsToEnableWhenThisEnabled)
            {
                if (a == EventSystem.current.currentSelectedGameObject) wentToInList = true;
            }

            if(!wentToInList)
            {
                foreach (GameObject a in thingsToEnableWhenThisEnabled)
                {
                    a.SetActive(false);
                }
            }

            isSelected = false;
        }
    }
}
