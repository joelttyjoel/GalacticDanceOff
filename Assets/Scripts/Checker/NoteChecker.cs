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
    private float halfPerfectPercentageDistance;

    private List<NoteController> noteQueueList = new List<NoteController>();

    [SerializeField]
    private GameObject spawnObject;
    //private float distanceFromSpawn;

    private bool note1WasHit;
	// Use this for initialization
	void Start ()
    {
        goodPercentageDistance = GameManagerController.instance.percentageGoodDistance;
        perfectPercentageDistance = GameManagerController.instance.percentagePerfectFromCenter;
        halfPerfectPercentageDistance = perfectPercentageDistance / 2;
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
        //no notes to hit
        if (noteQueueList.Count == 0)
        {
            //miss on spam when empty
            NoteMiss();
            return;
        }

        //there is atleast one note to hit
        NoteController note1 = noteQueueList[0];
        //check if is even in area
        if (CheckNoteGeneralHit(note1))
        {
            //first note is in area
            //check if correct note, if so hit, if not miss
            if (note1.noteType == noteKey)
            {
                //if perfect
                if (CheckNotePerfect(note1))
                {
                    PerfectHit(note1);
                }
                //if regular
                else
                {
                    NormalHit(note1);
                }
                noteQueueList.RemoveAt(0);
                note1.HasBeenHit();
            }
            else
            {
                note1WasHit = false;
                //note miss
                NoteMiss();
                //remove note
                noteQueueList.RemoveAt(0);
                note1.HasBeenHit();
            }
        }
        //first not wasn't in area, just outside area, if so, miss
        else
        {
            NoteMiss();
        }
        

        ////if one or more notes always check first
        //else if (noteQueueList.Count >= 1)
        //{
        //    NoteController note1 = noteQueueList[0];
        //    //check if correct note and if hit
        //    if (note1.noteType == noteKey && CheckNoteGeneralHit(note1))
        //    {
        //        //if perfect
        //        if (CheckNotePerfect(note1))
        //        {
        //            PerfectHit(note1);
        //        }
        //        //if regular
        //        else
        //        {
        //            NormalHit(note1);
        //        }
        //        note1WasHit = true;
        //        noteQueueList.RemoveAt(0);
        //        note1.HasBeenHit();
        //    }
        //    else
        //    {
        //        note1WasHit = false;
        //        NoteMiss();
        //    }
        //}
        ////if two notes or more, do one more check on second note, only if first note was not hit
        //if(!note1WasHit && noteQueueList.Count >= 2)
        //{
        //    //check second note too
        //    NoteController note2 = noteQueueList[0];
        //    //if correct key and is hit, then hit, if not, miss
        //    if (note2.noteType == noteKey && CheckNoteGeneralHit(note2))
        //    {
        //        //if perfect
        //        if (CheckNotePerfect(note2))
        //        {
        //            PerfectHit(note2);
        //        }
        //        //if regular
        //        else
        //        {
        //            NormalHit(note2);
        //        }
        //        noteQueueList.RemoveAt(0);
        //        note2.HasBeenHit();
        //    }
        //    else
        //    {
        //        NoteMiss();
        //    }
        //}

        //     //gets first note info
        //     GameObject note = noteQueue.Peek();
        //     NoteController noteCon = note.GetComponent<NoteController>();

        //     //Checks if the right key for the note was pressed and the note is in the right area
        //     if (noteCon.percentageOfTravel >= (1 - goodPercentageDistance)
        //         && noteCon.percentageOfTravel <= (1 + goodPercentageDistance)
        //&& noteCon.noteType == noteKey)
        //     {
        //         NoteGeneralHit(noteCon);
        //     }

        //     //Otherwise the note was not hit
        //     else
        //     {
        //         NoteMiss(noteCon);
        //     }
    }

    //true if hit, false if not hit
    private bool CheckNoteGeneralHit(NoteController noteToCheck)
    {
        bool wasHit = false;

        //check if was hit
        //if above half of 1 -(perfect + all of good), closest line
        //if below perfect distance from center + 1, farthest line
        if (noteToCheck.percentageOfTravel > 1 - (goodPercentageDistance + halfPerfectPercentageDistance)
            && noteToCheck.percentageOfTravel < 1 + halfPerfectPercentageDistance)
        {
            wasHit = true;
        }

        return wasHit;
    }
    //true if perfect, false if not
    private bool CheckNotePerfect(NoteController noteToCheck)
    {
        bool wasPerfect = false;

        //if withing 1 + half perfect and 1 - half perfect, good
        if(noteToCheck.percentageOfTravel < 1 + halfPerfectPercentageDistance
            && noteToCheck.percentageOfTravel > 1 - halfPerfectPercentageDistance)
        {
            wasPerfect = true;
        }

        return wasPerfect;
    }

    //The note was hit, now compare what type of hit
    //private void NoteGeneralHit(NoteController noteCon)
    //{
    //    //defines what score hit it get
    //    if (noteCon.percentageOfTravel >= (1 - perfectPercentageDistance)
    //        && noteCon.percentageOfTravel <= (1 + perfectPercentageDistance))
    //    {
    //        PerfectHit(noteCon);
    //    }

    //    else
    //    {
    //        NormalHit(noteCon);
    //    }

    //    //Both types of hit has to deque and do hasbeenhit(), makes sence
    //    DequeueNote();
    //    noteCon.HasBeenHit();
    //}

    //The note was missed
    private void NoteMiss()
    {
        //DequeueNote();
        //noteCon.HasBeenHit();
        //Effects or other things
        LosePoints();
        Debug.Log("Miss");
        //play note miss sound
        AudioController.instance.PlayNoteSound(0f);
        //take damage on miss
        GameManagerController.instance.takeDamage(true);
    }

    //The note was hit and not perfectly timed
    private void NormalHit(NoteController noteCon)
    {
        //Effects or other things
        GainPoints(false);
        Debug.Log("Hit");
        //AudioController.instance.PlayNoteSound(1f);
    }

    //Perfect timed hit
    private void PerfectHit(NoteController noteCon)
    {
        //Effects or other things
        GainPoints(true);
        Debug.Log("Perfect");
        //AudioController.instance.PlayNoteSound(2f);
    }


    //Gain points
    private void GainPoints(bool isPerfect)
    {

    }

    //Lose Points
    private void LosePoints()
    {

    }


    //takes gameobject and puts at front of list
    public void EnqueueNote(NoteController note)
    {
        noteQueueList.Add(note);
    }

    //deques /removes top item in list
    public void DequeueNote()
    {
        if(noteQueueList.Count > 0)
        {
            //remove and return top item in list
            //GameObject topItem = noteQueueList[0];
            noteQueueList.RemoveAt(0);
            //return topItem;
        }

        //else
        //{
        //    return null;
        //}
    }


    //fix this mr robin
    //private void OnDrawGizmos()
    //{
    //    float distanceFromSpawn = Mathf.Abs(transform.position.x - spawnObject.transform.position.x);
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireCube(transform.position, new Vector3(2 * distanceFromSpawn * goodPercentageDistance, 1, 0.25f));

    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireCube(transform.position, new Vector3(2 * distanceFromSpawn * perfectPercentageDistance, 1, 0.25f));
    //}
}
