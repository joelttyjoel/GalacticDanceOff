﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerController : MonoBehaviour {
    [Header("Selected beatmap set?? Dunno how")]
    public int currentBeatMap = 1;
    //difficulty thing? Hm is needed or not, who know
    //Array to store all beatmaps for this level
    [SerializeField]
    public string[] beatMapNamesInOrder;

    [Header("Temp settings")]
    public bool startLevel = false;
    public bool failLevel = false;
    
    private BeatGetterFromFmodText theGetter;
    //music controller thing
    private FMOD_StudioEventEmitter eventEmitter;

    [Header("Settings Sequencing")]
    public float timeBeforeFirstRun = 5f;

    [Header("General Settings")]
    //Variables for other objects
    public float beatsSpawnToGoal_akaSpeed = 2f;

    [Header("Note settings")]
    public float beatsToSlowDownFor = 4f;
    public float beatsToSpeedUpFor = 1f;
    //settings perfect, good
    public float percentagePerfectFromCenter = 0.05f;
    //stupid name but can't short that man
    public float percentageGoodDistance = 0.1f;
    //note fade distance after has gone too far
    public float fadeDistance = 0.5f;
    //fade from grey ish to white at start, hmm
    public float startFadeDistance = 1.0f;

    //Edits EGOMeter
    [Header("EGO Bars")]
    public int damagePerMiss;
	public int maxHealth;
	public int playerHealth;
	public int AIHealth;


    //make sequence courutine
    private IEnumerator sequencer;
    private bool betweenIsDone = false;

    //make singleton
    public static GameManagerController instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        //now replaces already existing gameManager instead
        else if (instance != this)
            Destroy(instance.gameObject);

        //GET INFO ABOUT SEQUENCE ETC
        beatMapNamesInOrder = SceneSwitchereController.instance.currentSequence.beatMapNamesInOrder;
        beatsSpawnToGoal_akaSpeed = SceneSwitchereController.instance.currentSequence.beatsSpawnToGoal_akaSpeed;
    }

    void Start () {
        //get musicmanager
        GameObject musicManager = GameObject.Find("MusicManager");
        Debug.Assert(musicManager != null);
        //th thingy 
        eventEmitter = musicManager.GetComponent<FMOD_StudioEventEmitter>();
        //Get thing that reads from Fmod
        theGetter = musicManager.GetComponent<BeatGetterFromFmodText>();

        //start doing automatic sequence thing
        currentBeatMap = 1;
        sequencer = SequenceStartRunEtc();
        StartCoroutine(sequencer);
    }
	
	void Update () {
        //if(startLevel)
        //{
        //    startLevel = false;
        //    //if less than 2, aka 0 and 1 then do play current beatmap
        //    if (currentBeatMap < SceneSwitchereController.instance.currentSequence.beatMapNamesInOrder.Length)
        //    {
        //        //run current map
        //        runBeatmap();
        //        MusicController.instance.EnterLevelByInt(currentBeatMap);
        //        //increment to next
        //        currentBeatMap++;
        //    }
        //    //if 2 or above, set back to 0
        //    else
        //    {
        //        currentBeatMap = 0;
        //    }
        //}
        //if(failLevel)
        //{
        //    failLevel = false;
        //    failBeatmap();
        //}
	}

    public void takeDamage(bool isLeftP)
    {
        if (isLeftP)
        {
            AudioController.instance.PlayHpSound(0f);
            playerHealth -= damagePerMiss;
        }
        else
        {
            AIHealth -= damagePerMiss;
        }

        //if health <= 0, set to 0, also lose game
        if(playerHealth <= 0)
        {
            playerHealth = 0;
            failBeatmap();
            //dissable inputs
            InputManager.instance.isInputsDisabled = true;
        }
        if (AIHealth <= 0)
        {
            AIHealth = 0;
            //lose game
        }
    }

    private void failBeatmap()
    {
        //getter settings
        theGetter.runNextBeatmap = false;
        theGetter.currentLabelName = "Start";
        //stuff
        MusicController.instance.RestartScene();

        InputManager.instance.isInputsDisabled = true;

        //do stuff for next run, reset sequencer etc
        currentBeatMap = 1;
        //Stop sequencer
        StopCoroutine(sequencer);
        Debug.Log("Restart sequence");

        //set stuff for beatmap slowdown and removal
        StartCoroutine(failBeatmapAnimation());
    }

    private IEnumerator failBeatmapAnimation()
    {
        //slow down until stop
        float timeToSlowFor = beatsToSlowDownFor * BeatmapReader.instance.thingsPerBeat * BeatmapReader.instance.timePer16del;

        //float startTime = Time.time;
        float endTime = Time.time + timeToSlowFor;

        float percentageOfTravel = 1f;

        //continues looping until Time = endTime
        while (percentageOfTravel > 0.3f)
        {
            //find percentage of travel
            percentageOfTravel = (endTime - Time.time) / timeToSlowFor;
            Time.timeScale = 1f * percentageOfTravel;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
        //once stop, stop running beatmap and delete all notes, resete timescale to 1
        BeatmapReader.instance.StopRunningBeatmap();

        //speed up instead, see if looks good
        float timeToSpeedFor = beatsToSpeedUpFor * BeatmapReader.instance.thingsPerBeat * BeatmapReader.instance.timePer16del;
        float endTime2 = Time.time + timeToSpeedFor;
        float percentageOfTravel2 = 1f;
        //continues looping until reached end
        while (percentageOfTravel2 > 0.1f)
        {
            //find percentage of travel
            percentageOfTravel2 = (endTime2 - Time.time - 0.05f) / timeToSpeedFor;
            Time.timeScale = (1f - percentageOfTravel2);
            yield return new WaitForEndOfFrame();
        }
        Time.timeScale = 1f;

        //ONCE COMPLETE, RESTART SEQUENCE
        StartCoroutine(sequencer);

        //Debug.Log(percentageOfTravel);
        //while (noteParent.transform.childCount > 0)
        //{
        //THIS CAUSES INFINITE LOOP; FIX OR NOT DEPENDS ON WHAT LIKE
        //    //maybe add animation and shit here, not sure, if so just call function in note
        //    Destroy(noteParent.transform.GetChild(0).gameObject);
        //    Debug.Log(noteParent.trans)
        //}
    }
		
	public IEnumerator BetweenBeatMap()
	{
        InputManager.instance.isInputsDisabled = true;
        Debug.Log("Between sequences");
		yield return new WaitForSeconds (3f);
		Debug.Log ("Scoring");
		yield return new WaitForSeconds (3f);
		Debug.Log ("Animation");
		yield return new WaitForSeconds (3f);
		Debug.Log ("return to beatMap");
        betweenIsDone = true;
        //re enable inputs
        InputManager.instance.isInputsDisabled = false;
    }

    public IEnumerator FinishedSequence()
    {
        //disable inputs again
        InputManager.instance.isInputsDisabled = true;
        Debug.Log("Sequence finished");
        yield return new WaitForSeconds(3f);
        Debug.Log("Do something");
        yield return new WaitForSeconds(3f);
        Debug.Log("Done");
        //when done, just reload scene again, boom
        SceneSwitchereController.instance.LoadSceneByName(SceneSwitchereController.instance.nameCurrentScene, SceneSwitchereController.instance.nameCurrentSequence);
    }

    public IEnumerator SequenceStartRunEtc()
    {
        InputManager.instance.isInputsDisabled = true;
        Debug.Log("1 " + InputManager.instance.isInputsDisabled);
        //time until start first level
        yield return new WaitForSeconds(timeBeforeFirstRun);
        //set health to full on start
        playerHealth = 100;
        AIHealth = 100;

        //loop through all levels until reached final
        while (currentBeatMap - 1 < SceneSwitchereController.instance.currentSequence.beatMapNamesInOrder.Length)
        {
            Debug.Log("Start current beatmap");
            //enable inputs
            Debug.Log("2 " +InputManager.instance.isInputsDisabled);
            InputManager.instance.isInputsDisabled = false;
            Debug.Log("3 " + InputManager.instance.isInputsDisabled);
            //start map, start for check that next has been reached
            //run current map
            runBeatmap();
            //starts at 1 in fmod, not 0
            MusicController.instance.EnterLevelByInt(currentBeatMap);
            //increment to next
            currentBeatMap++;
            //wait for reach end of sequence
            //read tag from fmod thing, if = sequence, then sequence has passed
            //first check if isn't equal, for second time looping, will be active until moved on, once moved on, 
            //wait for end sequence again
            Debug.Log(theGetter.currentLabelName);
            yield return new WaitUntil(() => theGetter.currentLabelName != "EndSequence");
            Debug.Log(theGetter.currentLabelName);
            //only do this waiting part if not on last turn, if on last, move out of this and wait for finish instead
            if((currentBeatMap - 1) < SceneSwitchereController.instance.currentSequence.beatMapNamesInOrder.Length)
            {
                yield return new WaitUntil(() => theGetter.currentLabelName == "EndSequence");
                Debug.Log(theGetter.currentLabelName);
                //Start doing between beatmaps
                StartCoroutine(BetweenBeatMap());
                //wait for between beatmaps to be complete
                yield return new WaitUntil(() => betweenIsDone == true);
                betweenIsDone = false;
            }
        }
        //has played last beatmap
        Debug.Log("Final wait");
        yield return new WaitUntil(() => theGetter.currentLabelName == "Finish");
        StartCoroutine(FinishedSequence());
        //after reached finish, stop the thing
    }


    private void runBeatmap()
    {
        Debug.Log("Run beatmap: " + (currentBeatMap - 1));
        //ugly but shhhhh, if pretty make thse functions in musiccontrollre instead but shhh
        theGetter.runNextBeatmap = true;
        theGetter.nameOfMapToRun = beatMapNamesInOrder[currentBeatMap - 1];
        //beatMapReader.StartRunningBeatmap(beatMapNamesInOrder[currentBeatMap]);
    }
}
