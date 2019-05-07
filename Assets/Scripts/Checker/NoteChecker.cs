using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteChecker : MonoBehaviour {

	public bool isLeftBoard;
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
		if (isLeftBoard) 
		{
			if (SceneSwitchereController.instance.keyBoard)
			{
				inputSet [0].stringInputManager = "W";
				inputSet [1].stringInputManager = "A";
				inputSet [2].stringInputManager = "S";
				inputSet [3].stringInputManager = "D";
			}
			//keypad xbox


			//keypad ps4
		}
		if (!isLeftBoard) 
		{
			if (SceneSwitchereController.instance.keyBoard) 
			{
				inputSet [0].stringInputManager = "Up";
				inputSet [1].stringInputManager = "Left";
				inputSet [2].stringInputManager = "Down";
				inputSet [3].stringInputManager = "Right";
			}
			if (SceneSwitchereController.instance.xBox) {
				inputSet [0].stringInputManager = "Y";
				inputSet [1].stringInputManager = "X";
				inputSet [2].stringInputManager = "A";
				inputSet [3].stringInputManager = "B";
			}
			if (SceneSwitchereController.instance.Ps4) {
				inputSet [0].stringInputManager = "Triangle";
				inputSet [1].stringInputManager = "Square";
				inputSet [2].stringInputManager = "Cross";
				inputSet [3].stringInputManager = "Circle";
			}
		}

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

		if (isLeftBoard && (SceneSwitchereController.instance.Ps4 || SceneSwitchereController.instance.xBox) )
		{
			if (InputManager.instance.GetAxis ("XboxVertical") > 0 ||
				InputManager.instance.GetAxis("PS4Vertical") > 0) 
			{
				NoteKeyDown(inputSet[0].valueOfNoteCheck);
			}

			if (InputManager.instance.GetAxis ("XboxHorizontal") < 0 ||
				InputManager.instance.GetAxis("PS4Horizontal") < 0) 
			{
				NoteKeyDown(inputSet[1].valueOfNoteCheck);
			}
				
			if (InputManager.instance.GetAxis ("XboxVertical") < 0 ||
				InputManager.instance.GetAxis("PS4Vertical") < 0) 
			{
				NoteKeyDown(inputSet[2].valueOfNoteCheck);
			}

			if (InputManager.instance.GetAxis ("XboxHorizontal") > 0 ||
				InputManager.instance.GetAxis("PS4Horizontal") > 0) 
			{
				NoteKeyDown(inputSet[3].valueOfNoteCheck);
			}
		}

		if (!isLeftBoard || SceneSwitchereController.instance.keyBoard) {
			//can click multiple buttons at once, grrrrr, are however checked in order which is suck, should be checked all at once
			if (InputManager.instance.GetButtonDown (inputSet [0].stringInputManager)) {
				NoteKeyDown (inputSet [0].valueOfNoteCheck);
			}
			if (InputManager.instance.GetButtonDown (inputSet [1].stringInputManager)) {
				NoteKeyDown (inputSet [1].valueOfNoteCheck);
			}
			if (InputManager.instance.GetButtonDown (inputSet [2].stringInputManager)) {
				NoteKeyDown (inputSet [2].valueOfNoteCheck);
			}
			if (InputManager.instance.GetButtonDown (inputSet [3].stringInputManager)) {
				NoteKeyDown (inputSet [3].valueOfNoteCheck);
			}

		}

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
            //dont take hit on miss sound if nothing is in area yes
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
                NoteMiss();
                //remove note
                noteQueueList.RemoveAt(0);
                note1.HasBeenHit();
            }
        }
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
    }
}
