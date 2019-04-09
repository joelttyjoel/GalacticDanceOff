using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatmapSpawner : MonoBehaviour {
    //The notes select sprite for themselves depending on settings, this just instantiates them in scene
    [SerializeField]
    [Header("List of all prefabs, 1-8")]
    List<GameObject> prefabsNotes;

    //for testing
    [SerializeField]
    public GameObject thing;
    Transform things;
    bool flip = true;

	void Start () {
        things = thing.GetComponent<Transform>();
	}
	
	void Update () {
		
	}

    public void SpawnItem(char itemValue)
    {
        //use this value and a switch case to spawn shit
        Debug.Log("Spawn this: " + itemValue.ToString());

        //just to visualize ticks
        flip = !flip;
        if(flip)
        {
            things.position = new Vector3(0, 1, 0);
        }
        else
        {
            things.position = new Vector3(0, 0.5f, 0);
        }
    }
}
