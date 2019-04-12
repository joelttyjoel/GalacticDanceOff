using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatmapSpawner : MonoBehaviour {
    //The notes select sprite for themselves depending on settings, this just instantiates them in scene
    [SerializeField]
    [Header("Note prefab, selects sprite on spawn")]
    GameObject note;
    [SerializeField]
    GameObject fret;

    [SerializeField]
    [Header("Object in scene references")]
    GameObject noteCheckerGameobject;
    NoteChecker noteCheck;

    [SerializeField]
    GameObject noteParent;
    [SerializeField]
    private GameObject fretParent;

    public float distanceThisToDestroyer;

    private float distanceBehindNotes = 0.01f;

    void Start () {
        distanceThisToDestroyer = Vector3.Distance(transform.position, noteCheckerGameobject.transform.position);
        noteCheck = noteCheckerGameobject.GetComponent<NoteChecker>();
    }
	
	void Update () {
		
	}

    public void SpawnNote(int itemValue, float timeToWait, float currentTickTime)
    {
        //spawn gameobject depending on itemValueInt
        GameObject currentNote = GameObject.Instantiate(note);
        //set variables in gameobject
        //set item type in gameobject to select features
        NoteController controllerNote = currentNote.GetComponent<NoteController>();
        //set birth time
        controllerNote.timeAtBirth = currentTickTime;
        //-1 due to thing
        controllerNote.noteType = itemValue - 1;
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
        noteCheck.EnqueueNote(currentNote);
    }

    public void SpawnFret(float timeToWait, float currentTickTime)
    {
        //spawn gameobject depending on itemValueInt
        GameObject currentFret = GameObject.Instantiate(fret);
        //set variables in gameobject
        //set item type in gameobject to select features
        FretController controllerFret = currentFret.GetComponent<FretController>();
        //set birth time
        controllerFret.timeAtBirth = currentTickTime;
        //hand distance to child
        controllerFret.distanceSpawnDestroyer = distanceThisToDestroyer;
        //hand timeWait
        controllerFret.timePerBeat = timeToWait;
        //set position to position of the spawner
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + distanceBehindNotes);
        currentFret.transform.position = newPosition;
        //set parent to notes, this keeps things sorted in scene
        currentFret.transform.SetParent(fretParent.transform);
    }
}
