using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatmapSpawner : MonoBehaviour {
    //The notes select sprite for themselves depending on settings, this just instantiates them in scene
    [SerializeField]
    [Header("Note prefab, selects sprite on spawn")]
    GameObject note;

    [SerializeField]
    [Header("Object in scene references")]
    GameObject noteDestroyer;
    [SerializeField]
    GameObject noteParent;

    public float distanceThisToDestroyer;

    void Start () {
        distanceThisToDestroyer = Vector3.Distance(transform.position, noteDestroyer.transform.position);
    }
	
	void Update () {
		
	}

    public void SpawnItem(char itemValue, float timeToWait)
    {
        //turn char into int, -1 to be position 3 in list
        int itemValueInt = (int.Parse(itemValue.ToString())) - 1;
        //spawn gameobject depending on itemValueInt
        GameObject currentNote = GameObject.Instantiate(note);
        //set variables in gameobject
        //get notecontroller here, but later
        //set item type in gameobject to select features
        NoteController controllerNote = currentNote.GetComponent<NoteController>();
        controllerNote.noteType = itemValueInt;
        //hand distance to child
        controllerNote.distanceSpawnDestroyer = distanceThisToDestroyer;
        //hand timeWait
        controllerNote.timePerBeat = timeToWait;
        //set checker
        controllerNote.noteChecker = noteDestroyer;
        //set position to position of the spawner
        currentNote.transform.position = this.transform.position;
        //set parent to notes, this keeps things sorted in scene
        currentNote.transform.SetParent(noteParent.transform);

        //push to list in note destroyer
        //DO THIS HERE

        //use this value and a switch case to spawn shit
        //Debug.Log("Spawn this: " + itemValue.ToString());
    }
}
