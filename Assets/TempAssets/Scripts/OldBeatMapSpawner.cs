using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldBeatMapSpawner : MonoBehaviour {

    //The notes select sprite for themselves depending on settings, this just instantiates them in scene
    [SerializeField]
    [Header("Note prefab, selects sprite on spawn")]
    GameObject note;

    [SerializeField]
    [Header("Object in scene references")]
    GameObject noteCheckerGameobject;
    private NoteChecker noteCheck;
    [SerializeField]
    GameObject noteParent;

    public float distanceThisToDestroyer;

    void Start()
    {
        distanceThisToDestroyer = Vector3.Distance(transform.position, noteCheckerGameobject.transform.position);
        noteCheck = noteCheckerGameobject.GetComponent<NoteChecker>();
    }

    void Update()
    {

    }

    public void SpawnItem(char itemValue, float timeToWait)
    {
        //turn char into int, -1 to be position 3 in list
        int itemValueInt = (int.Parse(itemValue.ToString())) - 1;
        //spawn gameobject depending on itemValueInt
        GameObject currentNote = GameObject.Instantiate(note);
        noteCheck.EnqueueNote(currentNote);
        //set variables in gameobject
        //get notecontroller here, but later
        //set item type in gameobject to select features
        OldNoteController controllerNote = currentNote.GetComponent<OldNoteController>();
        controllerNote.noteType = itemValueInt;
        //hand distance to child
        controllerNote.distanceSpawnDestroyer = distanceThisToDestroyer;
        //hand timeWait
        controllerNote.timePerBeat = timeToWait;
        //set checker
        controllerNote.noteChecker = noteCheckerGameobject;
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
