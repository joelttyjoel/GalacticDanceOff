using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteChecker : MonoBehaviour {

    [SerializeField]
    public List<GameObject> goodPerfMiss;
    private List<SpriteRenderer> goodPerfMissSprites = new List<SpriteRenderer>();
    private float timeToShowFor = 0.3f;

	public bool isLeftBoard;
	private bool oneFrameAxis = false;
	private string horizontalController, verticalController;
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
        //read in images for hit
        goodPerfMissSprites.Add(goodPerfMiss[0].GetComponent<SpriteRenderer>());
        goodPerfMissSprites.Add(goodPerfMiss[1].GetComponent<SpriteRenderer>());
        goodPerfMissSprites.Add(goodPerfMiss[2].GetComponent<SpriteRenderer>());
        timeToShowFor = GameManagerController.instance.timeShowOnHitFor;
        
        if (isLeftBoard) 
		{
			if (SceneSwitchereController.instance.keyBoard)
			{
				inputSet [0].stringInputManager = "W";
				inputSet [1].stringInputManager = "A";
				inputSet [2].stringInputManager = "S";
				inputSet [3].stringInputManager = "D";
			}

			if (SceneSwitchereController.instance.xBox) 
			{
				horizontalController = "XboxHorizontal";
				verticalController = "XboxVertical";
			}
			//keypad xbox

			if (SceneSwitchereController.instance.Ps4) 
			{
				horizontalController = "PS4Horizontal";
				verticalController = "PS4Vertical";
			}
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
			if (SceneSwitchereController.instance.xBox) 
			{
				inputSet [0].stringInputManager = "Y";
				inputSet [1].stringInputManager = "X";
				inputSet [2].stringInputManager = "A";
				inputSet [3].stringInputManager = "B";
			}
			if (SceneSwitchereController.instance.Ps4) 
			{
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
		if (InputManager.instance.isInputsDisabled) {
			return;
		}

		if (isLeftBoard && (SceneSwitchereController.instance.Ps4 || SceneSwitchereController.instance.xBox)) 
		{
			if (InputManager.instance.GetAxis (verticalController) > 0) 
			{
				if (!oneFrameAxis) {
					
					NoteKeyDown (inputSet [0].valueOfNoteCheck);
					oneFrameAxis = true;
				}
			}

			if (InputManager.instance.GetAxis (horizontalController) < 0) 
			{
				if (!oneFrameAxis) {
					NoteKeyDown (inputSet [1].valueOfNoteCheck);
					oneFrameAxis = true;
				}
			}
				
			if (InputManager.instance.GetAxis (verticalController) < 0) 
			{
				if (!oneFrameAxis) {
					NoteKeyDown (inputSet [2].valueOfNoteCheck);
					oneFrameAxis = true;
				}
			}

			if (InputManager.instance.GetAxis (horizontalController) > 0) 
			{
				if (!oneFrameAxis) {
					NoteKeyDown (inputSet [3].valueOfNoteCheck);
					oneFrameAxis = true;
				}
			}

			oneFrameAxis = false;
			if (InputManager.instance.GetAxis (verticalController) != 0 
				||	InputManager.instance.GetAxis (horizontalController) != 0) 
			{
				oneFrameAxis = true;
			}
			return;
		}


		if(!isLeftBoard || SceneSwitchereController.instance.keyBoard)
		{
			if (InputManager.instance.GetButtonDown (inputSet [0].stringInputManager)) 
			{
				NoteKeyDown (inputSet [0].valueOfNoteCheck);
				//Debug.Log ("1");
			}
			if (InputManager.instance.GetButtonDown (inputSet [1].stringInputManager)) 
			{
				NoteKeyDown (inputSet [1].valueOfNoteCheck);
				//Debug.Log ("2");
			}
			if (InputManager.instance.GetButtonDown (inputSet [2].stringInputManager))
			{
				NoteKeyDown (inputSet [2].valueOfNoteCheck);
				//Debug.Log ("3");
			}
			if (InputManager.instance.GetButtonDown (inputSet [3].stringInputManager)) {
				NoteKeyDown (inputSet [3].valueOfNoteCheck);
				//Debug.Log ("4");
			}
		}
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
                //is hit
                GetComponent<ParticleSystem>().Stop();
                GetComponent<ParticleSystem>().Play();
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


    //The note was missed
    private void NoteMiss()
    {
        //DequeueNote();
        //noteCon.HasBeenHit();
        //Effects or other things
        Debug.Log("Miss");
        //play note miss sound
        AudioController.instance.PlayNoteSound(0f);
        //take damage on miss
        GameManagerController.instance.takeDamage(true);
        StartCoroutine(FadeOnHitImage(goodPerfMissSprites[2]));
    }

    //The note was hit and not perfectly timed
    private void NormalHit(NoteController noteCon)
    {
        //Effects or other things
        Debug.Log("Hit");
        StartCoroutine(FadeOnHitImage(goodPerfMissSprites[0]));
        GameManagerController.instance.addScore(true, false);
        AudioController.instance.PlayNoteSound(1f);
    }

    //Perfect timed hit
    private void PerfectHit(NoteController noteCon)
    {
        //Effects or other things
        Debug.Log("Perfect");
        StartCoroutine(FadeOnHitImage(goodPerfMissSprites[1]));
        GameManagerController.instance.addScore(true, true);
        AudioController.instance.PlayNoteSound(2f);
    }

    private IEnumerator FadeOnHitImage(SpriteRenderer spriteToDo)
    {
        float timePerStep = timeToShowFor / 3;
        float elapsedTime = 0f;
        float currentOpacity = 0f;
        //fade in
        while (elapsedTime < timePerStep)
        {
            //part of whole
            currentOpacity = elapsedTime / timePerStep;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            //set the do
            spriteToDo.color = new Vector4(1, 1, 1, currentOpacity);
        }
        //stay
        //set the do
        spriteToDo.color = new Vector4(1, 1, 1, currentOpacity);
        yield return new WaitForSeconds(timePerStep);
        elapsedTime += timePerStep;
        //fade out
        while (elapsedTime < timeToShowFor)
        {
            //0.66>1 < 1, 0.0>0.33, 0.33
            currentOpacity = 1 - ((elapsedTime - (2 * timePerStep)) / timePerStep);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            //set the do
            spriteToDo.color = new Vector4(1, 1, 1, currentOpacity);
        }
        //make hidey again
        spriteToDo.color = new Vector4(1, 1, 1, 0);
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

    //fix this mr robin
    private void OnDrawGizmos()
    {
        float distanceFromSpawn = Vector2.Distance(new Vector2(transform.position.y, transform.position.z), new Vector2(spawnObject.transform.position.y, spawnObject.transform.position.z));//Mathf.Abs(transform.position.x - spawnObject.transform.position.x);

        Vector3 baseOffset = new Vector3(0, (goodPercentageDistance * distanceFromSpawn / 2) + halfPerfectPercentageDistance * distanceFromSpawn, 0);
        Gizmos.color = Color.green;
        //Gizmos.DrawWireCube(transform.position, new Vector3(1 /*2 * distanceFromSpawn * goodPercentageDistance*/, 1, 0.25f));
        Gizmos.DrawWireCube(transform.position + baseOffset, new Vector3(1, goodPercentageDistance * distanceFromSpawn, 0.5f));


        Gizmos.color = Color.yellow;
        //Gizmos.DrawWireCube(transform.position, new Vector3(1 /*2 * distanceFromSpawn * perfectPercentageDistance*/, 0.5f, 0.25f));
        Gizmos.DrawWireCube(transform.position, new Vector3(1.5f, 2 * halfPerfectPercentageDistance * distanceFromSpawn, 0.5f));
    }
}
