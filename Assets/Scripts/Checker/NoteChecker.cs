using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteChecker : MonoBehaviour {

    //We have the point system within another script???
    //Gain an increasing amount of points to a max?
    //basePoints * currentMultiplier;
    //currentMultiplier increase to a max
    //Multiplier reset if last note was a hit and you miss or the last note was a miss and you hit
    //You gain/lose more points the more notes you hit/miss in a row, reset when you don't
    /*
    [SerializeField]
    private int pointGainedPerNote = 20;
    [SerializeField]
    private int pointGainedPerPerfectNote = 100;    //Multiplier instead?
    [SerializeField]
    private int pointLostPerMissedNote = 20;
    */


    [SerializeField]
    private float minDistancePercentageFromCheckArea = 0.2f;
    [SerializeField]
    private float perfectPercentageDistance = 0.05f;

    private Queue<GameObject> noteQueue = new Queue<GameObject>();

    [SerializeField]
    private GameObject spawnObject;
    //private float distanceFromSpawn;

	// Use this for initialization
	void Start ()
    {

	}
	

	// Update is called once per frame
	void LateUpdate ()
    {
        //Get button down
        /*
        //Note1
		if (Input.GetButtonDown("Note1"))
        {
            NoteKeyDown(1);
        }
        //Note2...
        //etc
        */

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NoteKeyDown(1);
        }
	}


    private void NoteKeyDown(int noteKey)
    {
        //If the queue contains more than one object
        if (noteQueue.Count > 0)
            /*&& noteQueue.Peek() != null)*/
        {
            GameObject note = noteQueue.Peek();
            OldNoteController noteCon = note.GetComponent<OldNoteController>();

            //Checks if the right key for the note was pressed and the note is in the right area
            if (noteCon.percentageOfTravel >= (1 - minDistancePercentageFromCheckArea)
                /*&& noteCon.NoteValue() == noteKey*/)
            {
                NoteHit(noteCon);
            }

            //Otherwise the note was not hit
            else
            {
                NoteMiss();
            }
        }

        else
        {
            Debug.Log("No notes");
            NoteMiss();
        }
    }


    //The note was missed
    private void NoteMiss()
    {
        //Effects or other things
        LosePoints();
    }


    //The note was hit
    private void NoteHit(OldNoteController noteCon)
    {
        if (noteCon.percentageOfTravel >= (1 - perfectPercentageDistance))
        {
            PerfectHit(noteCon);
        }

        else
        {
            NormalHit(noteCon);
        }


        //Do something else that both perfect and the normal hit does?

        //Destroy note for now
        Destroy(DequeueNote());
    }

    //The note was hit and not perfectly timed
    private void NormalHit(OldNoteController noteCon)
    {
        //Effects or other things
        GainPoints(false);
        Debug.Log("Hit");
    }

    //Perfect timed hit
    private void PerfectHit(OldNoteController noteCon)
    {
        //Effects or other things
        GainPoints(true);
        Debug.Log("Perfect");
    }


    //Gain points
    private void GainPoints(bool perfect)
    {

    }

    //Lose Points
    private void LosePoints()
    {

    }



    public void EnqueueNote(GameObject note)
    {
        noteQueue.Enqueue(note);
    }


    public GameObject DequeueNote()
    {
        if(noteQueue.Count > 0)
        {
            return noteQueue.Dequeue();
        }

        else
        {
            return null;
        }
    }



    private void OnDrawGizmos()
    {
        float distanceFromSpawn = Mathf.Abs(transform.position.x - spawnObject.transform.position.x);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(distanceFromSpawn * minDistancePercentageFromCheckArea, 1, 0.25f));

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(distanceFromSpawn * perfectPercentageDistance, 1, 0.25f));
    }
}
