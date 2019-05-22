using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeOneDissabled : MonoBehaviour {
    public List<GameObject> allPurpleIcons;
    public List<GameObject> allStickIcons;
    public List<GameObject> allBirbIcons;
    public List<GameObject> allPathsInOrder;
    public GameObject hubBoi;

    // Use this for initialization
    void Start () {
        //all transparent and stop from navigation
        allPathsInOrder[SceneSwitchereController.instance.selectedCharacter].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        //for all
        Navigation noSelect = new Navigation();
        //purple
        if (SceneSwitchereController.instance.selectedCharacter == 0)
        {
            noSelect = hubBoi.GetComponent<Button>().navigation;
            noSelect.selectOnDown = null;
            hubBoi.GetComponent<Button>().navigation = noSelect;

            foreach (GameObject a in allPurpleIcons)
            {
                a.GetComponent<Button>().interactable = false;
            }
        }
        //stick
        else if (SceneSwitchereController.instance.selectedCharacter == 1)
        {
            noSelect = hubBoi.GetComponent<Button>().navigation;
            noSelect.selectOnRight = null;
            hubBoi.GetComponent<Button>().navigation = noSelect;

            foreach (GameObject a in allStickIcons)
            {
                a.GetComponent<Button>().interactable = false;
            }
        }
        //birb
        else
        {
            noSelect = hubBoi.GetComponent<Button>().navigation;
            noSelect.selectOnLeft = null;
            hubBoi.GetComponent<Button>().navigation = noSelect;

            foreach (GameObject a in allBirbIcons)
            {
                a.GetComponent<Button>().interactable = false;
            }

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
