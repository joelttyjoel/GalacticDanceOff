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
    //for inputing what strings to check for in input manager
    [System.Serializable]
    public class InputStringsAndTheirValues
    {
        public string stringInputManager;
        public int valueOfNoteCheck;
    }
    //ex, w 0, a 1, s 2, d 3. 
    [SerializeField]
    public InputStringsAndTheirValues[] inputSet = new InputStringsAndTheirValues[4];
        
    private float goodPercentageDistance;
    private float perfectPercentageDistance;

    private Queue<GameObject> noteQueue = new Queue<GameObject>();

    [SerializeField]
    private GameObject spawnObject;
    //private float distanceFromSpawn;

	// Use this for initialization
	void Start ()
    {
        goodPercentageDistance = GameManagerController.instance.percentageGoodFromCenter;
        perfectPercentageDistance = GameManagerController.instance.percentagePerfectFromCenter;
    }
	
	void FixedUpdate()
	{

	}
	// Update is called once per frame
	void LateUpdate ()
    {
		if (InputManager.instance.isInputsDisabled) 
		{
			return;
		}
        //can click multiple buttons at once, grrrrr, are however checked in order which is suck, should be checked all at once
        if(InputManager.instance.GetButtonDown(inputSet[0].stringInputManager))
        {
            NoteKeyDown(inputSet[0].valueOfNoteCheck);
        }
        if (InputManager.instance.GetButtonDown(inputSet[1].stringInputManager))
        {
            NoteKeyDown(inputSet[1].valueOfNoteCheck);
        }
        if (InputManager.instance.GetButtonDown(inputSet[2].stringInputManager))
        {
            NoteKeyDown(inputSet[2].valueOfNoteCheck);
        }
        if (InputManager.instance.GetButtonDown(inputSet[3].stringInputManager))
        {
            NoteKeyDown(inputSet[3].valueOfNoteCheck);
        }

        //if (Input.GetButtonDown ("Button A")) 
        //{
        //	NoteKeyDown(0);
        //}
        //else if(Input.GetButtonDown ("Button B")) 
        //{
        //	NoteKeyDown(1);
        //}
        //else if (Input.GetButtonDown ("Button X")) 
        //{
        //	NoteKeyDown(2);
        //}
        //else if (Input.GetButtonDown ("Button Y")) 
        //{
        //	NoteKeyDown(3);
        //}


        //if (Input.GetAxisRaw ("Vertical") > 0) 
        //{
        //	NoteKeyDown (4);
        //}
        //else if (Input.GetAxisRaw ("Horizontal") > 0) 
        //{
        //	NoteKeyDown (7);
        //}

        //if (Input.GetAxisRaw ("Vertical") < 0) 
        //{
        //	NoteKeyDown (6);
        //}
        //else if (Input.GetAxisRaw ("Horizontal") < 0) 
        //{
        //	NoteKeyDown (5);
        //}

    }


    private void NoteKeyDown(int noteKey)
    {
        if(noteQueue.Count == 0)
        {
            QueEmptyHit();
            return;
        }

        GameObject note = noteQueue.Peek();
        NoteController noteCon = note.GetComponent<NoteController>();

        //Checks if the right key for the note was pressed and the note is in the right area
        if (noteCon.percentageOfTravel >= (1 - goodPercentageDistance)
            && noteCon.percentageOfTravel <= (1 + goodPercentageDistance)
			&& noteCon.noteType == noteKey)
        {
            NoteGeneralHit(noteCon);
        }

        //Otherwise the note was not hit
        else
        {
            NoteMiss(noteCon);
        }
    }

    private void QueEmptyHit()
    {
        Debug.Log("Que empty");
    }

    //The note was hit, now compare what type of hit
    private void NoteGeneralHit(NoteController noteCon)
    {
        //defines what score hit it get
        if (noteCon.percentageOfTravel >= (1 - perfectPercentageDistance)
            && noteCon.percentageOfTravel <= (1 + perfectPercentageDistance))
        {
            PerfectHit(noteCon);
        }

        else
        {
            NormalHit(noteCon);
        }

        //Both types of hit has to deque and do hasbeenhit(), makes sence
        DequeueNote();
        noteCon.HasBeenHit();
    }

    //The note was missed
    private void NoteMiss(NoteController noteCon)
    {
        //DequeueNote();
        //noteCon.HasBeenHit();
        //Effects or other things
        LosePoints();
        Debug.Log("Miss");
    }

    //The note was hit and not perfectly timed
    private void NormalHit(NoteController noteCon)
    {
        //Effects or other things
        GainPoints(false);
        Debug.Log("Hit");
    }

    //Perfect timed hit
    private void PerfectHit(NoteController noteCon)
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
        Gizmos.DrawWireCube(transform.position, new Vector3(2 * distanceFromSpawn * goodPercentageDistance, 1, 0.25f));

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(2 * distanceFromSpawn * perfectPercentageDistance, 1, 0.25f));
    }
}
