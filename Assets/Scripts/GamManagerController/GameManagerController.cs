using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerController : MonoBehaviour {
    [Header("Selected beatmap set settings")]
    public int currentBeatMap = 1;
    //difficulty thing? Hm is needed or not, who know
    //Array to store all beatmaps for this level
    [SerializeField]
    public string[] beatMapNamesInOrder;
    
    private BeatGetterFromFmodText theGetter;
    //music controller thing
    private FMOD_StudioEventEmitter eventEmitter;

    [Header("Settings Sequencing")]
    public GameObject spotlightLeft;
    public GameObject spotlightRight;
    public List<GameObject> fireworks;
    public List<GameObject> confetties;
    public GameObject fungusFlowChartObject;
    [System.NonSerialized]
    public Fungus.Flowchart fungusFlowChart;
    public float timeBeforeFirstRun = 5f;
    public int numberClearedToWin = 1;

    [Header("Animation settings")]
    public GameObject leftCharacterHolder;
    public GameObject rightCharacterHolder;
    public List<GameObject> allCharacterPrefabs012;
    public Animator[] audienceAnimations;
    private Animator leftAnimator;
    private Animator rightAnimator;

    [Header("General Settings")]
    //Variables for other objects
    public float beatsSpawnToGoal = 2f;
    public float speedMultiplier = 1f;
    public float timeShowOnHitFor = 1f;

    [Header("Note settings")]
    public float beatsToSlowDownFor = 4f;
    public float beatsToSpeedUpFor = 1f;
    //settings perfect, good
    public float percentagePerfectFromCenter = 0.05f;
    //stupid name but can't short that man
    public float percentageGoodDistance = 0.1f;
    //note fade distance after has gone too far
    public float fadeDistance = 0.5f;

    //Edits EGOMeter
    [Header("EGO Bars")]
    public int damagePerMiss;
    public int healingPerPoint10Sec;
	public int maxHealth;
	public int playerHealth;

    [Header("Scores")]
    public GameObject playerScoreObject;
    public GameObject aiScoreObject;
    public float timeToDoFor = 3f;
    public int scoreForNormalHit = 50;
    public int scoreForPerfectHit = 100;
    public int playerScore;
    public int AIScore;

    [HideInInspector]
    public bool isRestarting = false;


    //make sequence courutine
    //private Coroutine sequencer;
    private bool betweenIsDone = true;

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
        beatsSpawnToGoal = SceneSwitchereController.instance.currentSequence.beatsSpawnToGoal;
        speedMultiplier = SceneSwitchereController.instance.currentSequence.speedMultiplier;
    }

    void Start () {
        //set characters to do the dancin oof oof
        GameObject charLeft = Instantiate(allCharacterPrefabs012[SceneSwitchereController.instance.selectedCharacter], leftCharacterHolder.transform);
        charLeft.transform.SetParent(leftCharacterHolder.transform);
        GameObject charRight = Instantiate(allCharacterPrefabs012[SceneSwitchereController.instance.selectedOponent], rightCharacterHolder.transform);
        charRight.transform.SetParent(rightCharacterHolder.transform);
        //get flowchar
        fungusFlowChart = fungusFlowChartObject.GetComponent<Fungus.Flowchart>();
        //for animation, 0 to 5
        leftAnimator = leftCharacterHolder.transform.GetChild(0).GetComponent<Animator>();
        rightAnimator = rightCharacterHolder.transform.GetChild(0).GetComponent<Animator>();
        //set watching
        leftAnimator.SetInteger("SelectState", 1);
        rightAnimator.SetInteger("SelectState", 1);
        //get musicmanager
        GameObject musicManager = GameObject.Find("MusicControllerGame");
        Debug.Assert(musicManager != null);
        //th thingy 
        eventEmitter = musicManager.GetComponent<FMOD_StudioEventEmitter>();
        //Get thing that reads from Fmod
        theGetter = musicManager.GetComponent<BeatGetterFromFmodText>();

        //reset last won, is for setting button image
        SceneSwitchereController.instance.wonLast = false;

        //set between
        betweenIsDone = true;
        //start doing automatic sequence thing
        currentBeatMap = 1;
        StartCoroutine("SequenceStartRunEtc");

        //score
        playerScore = 0;
        AIScore = 0;

        //start healing each 10th
        StartCoroutine(healEachPoint10());
    }
    //health
    public void takeDamage(bool isLeftP)
    {
        if (isLeftP)
        {
            AudioController.instance.PlayHpSound(0f);
            playerHealth -= damagePerMiss;
            //shake camera
            //SCREENSHAKE MMM BIG
            //fungusFlowChart.ExecuteBlock("ShakeCameraSoft");
            //set feel ouch
            int currentState = leftAnimator.GetInteger("SelectState");
            //set to one or the other
            if(currentState == 4) leftAnimator.SetInteger("SelectState", 6);
            else leftAnimator.SetInteger("SelectState", 4);
            //start new timer to reset back to idle
            StopCoroutine("returnToIdleLeft");
            StartCoroutine("returnToIdleLeft");
        }
        else
        {
            int currentState = rightAnimator.GetInteger("SelectState");
            //set to one or the other
            if (currentState == 4) rightAnimator.SetInteger("SelectState", 6);
            else rightAnimator.SetInteger("SelectState", 4);
            //start new timer to reset back to idle
            StopCoroutine("returnToIdleRight");
            StartCoroutine("returnToIdleRight");
        }

        //if health <= 0, set to 0, also lose game
        if(playerHealth <= 0)
        {
            playerHealth = 0;
            failBeatmap();
            //dissable inputs
            InputManager.instance.isInputsDisabled = true;
        }
    }

    private IEnumerator returnToIdleLeft()
    {
        yield return new WaitForSeconds(0.60f);
        if(betweenIsDone)leftAnimator.SetInteger("SelectState", 1);
        else leftAnimator.SetInteger("SelectState", 0);
    }
    private IEnumerator returnToIdleRight()
    {
        yield return new WaitForSeconds(0.60f);
        if(betweenIsDone) rightAnimator.SetInteger("SelectState", 1);
        else rightAnimator.SetInteger("SelectState", 0);
    }

    public void healDamage(bool isLeftP)
    {
        if (isLeftP)
        {
            playerHealth += healingPerPoint10Sec;
        }
        //else
        //{
        //    AIHealth -= damagePerMiss;
        //}

        //if health <= 0, set to 0, also lose game
        if (playerHealth >= maxHealth)
        {
            playerHealth = maxHealth;
        }
        //if (AIHealth >= maxHealth)
        //{
        //    AIHealth = maxHealth;
        //}
    }

    private IEnumerator healEachPoint10()
    {
        while(true)
        {
            if(!isRestarting) healDamage(true);
            yield return new WaitForSeconds(0.1f);
        }
    }
    //score
    public void addScore(bool isLeftP, bool isPerfect)
    {
        int multiplier = MusicController.instance.starCountBig + 1;

        if (isLeftP)
        {
            if (isPerfect) playerScore += scoreForPerfectHit * multiplier;
            else playerScore += scoreForNormalHit * multiplier;
            //now set hp thing
            SetScore(playerScore, playerScoreObject);
        }

        else
        {
            if (isPerfect) AIScore += scoreForPerfectHit * (2 + Random.Range(0,2));
            else AIScore += scoreForNormalHit * (2 + Random.Range(0, 2));
            //now set hp thing
            SetScore(AIScore, aiScoreObject);
        }
    }

    private void SetScore(int score, GameObject scoreObject)
    {
        string scoreString = GetScoreString(score);
        scoreObject.GetComponent<TextMesh>().text = scoreString;
    }

    private string GetScoreString(int inputScore)
    {
        string outputString = "";
        int totalNumLength = 5;
        int inputLength = inputScore.ToString().Length;

        outputString += "Score: ";

        //add more shits at end
        for(int i = 0; i < totalNumLength - inputLength; i++)
        {
            outputString += " ";
        }

        outputString += inputScore.ToString();

        return outputString;
    }

    private void failBeatmap()
    {
        isRestarting = true;
        //getter settings
        theGetter.runNextBeatmap = false;
        theGetter.currentLabelName = "Start";
        //stuff
        MusicController.instance.RestartScene();

        InputManager.instance.isInputsDisabled = true;

        //say text if early lose
        fungusFlowChart.ExecuteBlock("CommentaryForEarlyLost");

        //SCREENSHAKE MMM BIG
        fungusFlowChart.ExecuteBlock("ShakeCameraRough");

        //reset values, with animation
        //PLAY NOTE FALL SOUDN, only if either has score
        if(playerScore > 50 && AIScore > 50) AudioController.instance.PlayScoreSound();
        //then show nimations
        StartCoroutine(FailBeatmapScoreFall(playerScore, playerScoreObject));
        StartCoroutine(FailBeatmapScoreFall(AIScore, aiScoreObject));
        playerScore = 0;
        AIScore = 0;

        //do stuff for next run, reset sequencer etc
        currentBeatMap = 1;
        //Stop sequencer
        StopCoroutine("SequenceStartRunEtc");
        Debug.Log("Stop sequence");

        //set stuff for beatmap slowdown and removal
        StartCoroutine(failBeatmapAnimation());
    }

    private IEnumerator FailBeatmapScoreFall(int startScore, GameObject scoreObject)
    {
        float timeUntilDone = timeToDoFor;

        int currentScore = startScore;
        float percentageOfTimeLeft = 1f;

        while(timeUntilDone >= 0f)
        {
            yield return new WaitForEndOfFrame();
            timeUntilDone -= Time.deltaTime;

            //does every frame for 5 seconds
            //get percentage of time done
            percentageOfTimeLeft = timeUntilDone / timeToDoFor;

            //set currentScore as int
            currentScore = (int)((float)startScore * percentageOfTimeLeft);

            //now set score
            SetScore(currentScore, scoreObject);
        }
        currentScore = 0;
        SetScore(currentScore, scoreObject);
    }

    private IEnumerator failBeatmapAnimation()
    {
        //slow down until stop
        float timeToSlowFor = beatsToSlowDownFor * BeatmapReader.instance.sixteenthsPerBeat * BeatmapReader.instance.timePer16del;

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
        float timeToSpeedFor = beatsToSpeedUpFor * BeatmapReader.instance.sixteenthsPerBeat * BeatmapReader.instance.timePer16del;
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

        //set hp back to max for both players
        playerHealth = maxHealth;

        //wait for some time to make sure resetting has occured
        yield return new WaitForSeconds(5f);
        isRestarting = false;
        Debug.Log("Start sequence");
        StartCoroutine("SequenceStartRunEtc");

        //Debug.Log(percentageOfTravel);
        //while (noteParent.transform.childCount > 0)
        //{
        //THIS CAUSES INFINITE LOOP; FIX OR NOT DEPENDS ON WHAT LIKE
        //    //maybe add animation and shit here, not sure, if so just call function in note
        //    Destroy(noteParent.transform.GetChild(0).gameObject);
        //    Debug.Log(noteParent.trans)
        //}
    }
		
    //between beatmaps, between sequences
	public IEnumerator BetweenBeatMap()
	{
        InputManager.instance.isInputsDisabled = true;
        Debug.Log("Between sequences");
        //set animations
        leftAnimator.SetInteger("SelectState", 0);
        rightAnimator.SetInteger("SelectState", 0);

        //increase crowd
        MusicController.instance.crowdEmitter.SetParameter("CrowdScore", 100f);

        yield return new WaitForSeconds (3f);

		Debug.Log ("Scoring");

        //start fireworks
        foreach(GameObject a in fireworks)
        {
            a.GetComponent<ParticleSystem>().Play();
        }

        yield return new WaitForSeconds(2.5f);
        //start cheer a bit earlier
        AudioController.instance.PlayCrowdCheer();
        yield return new WaitForSeconds(0.3f);
        //start confetties
        foreach (GameObject a in confetties)
        {
            a.GetComponent<ParticleSystem>().Play();
        }
        yield return new WaitForSeconds (0.2f);
		Debug.Log ("Animation");

        //commntator
        if (playerScore > AIScore) fungusFlowChart.ExecuteBlock("CommentaryBetweenDoingGood");
        else fungusFlowChart.ExecuteBlock("CommentaryBetweenDoingBad");

        //spotlight sound turn on
        AudioController.instance.PlaySpotlight();

        if (playerScore > AIScore)
        {
            //left win
            for (int i = 0; i < audienceAnimations.Length; i++)
            {
                audienceAnimations[i].SetInteger("SelectState", 2);
            }
            spotlightLeft.SetActive(true);
            leftAnimator.SetInteger("SelectState", 2);
            rightAnimator.SetInteger("SelectState", 5);
        }
        else
        {
            //set animations
            for (int i = 0; i < audienceAnimations.Length; i++)
            {
                audienceAnimations[i].SetInteger("SelectState", 1);
            }
            spotlightRight.SetActive(true);
            leftAnimator.SetInteger("SelectState", 5);
            rightAnimator.SetInteger("SelectState", 2);
        }

        yield return new WaitForSeconds (3.5f);
        //stop fireworks
        foreach (GameObject a in fireworks)
        {
            a.GetComponent<ParticleSystem>().Stop();
        }

        //set back spotlight
        spotlightLeft.SetActive(false);
        spotlightRight.SetActive(false);
        //set animations back to dancing once done, thanks
        leftAnimator.SetInteger("SelectState", 1);
        rightAnimator.SetInteger("SelectState", 1);

        //Audience to idle animation
        for (int i = 0; i < audienceAnimations.Length; i++)
        {
            audienceAnimations[i].SetInteger("SelectState", 0);
        }

        //increase crowd
        MusicController.instance.crowdEmitter.SetParameter("CrowdScore", 0f);

        Debug.Log ("return to beatMap");
        betweenIsDone = true;
        //re enable inputs
        InputManager.instance.isInputsDisabled = false;
    }

    //on won
    public IEnumerator FinishedSequence()
    {
        //disable inputs again
        InputManager.instance.isInputsDisabled = true;
        Debug.Log("Sequence finished");
        //set animations
        leftAnimator.SetInteger("SelectState", 0);
        rightAnimator.SetInteger("SelectState", 0);

        //increase crowd
        MusicController.instance.crowdEmitter.SetParameter("CrowdScore", 100f);

        yield return new WaitForSeconds(3f);
        Debug.Log("Scoring final");

        AudioController.instance.PlayWinStinger();

        yield return new WaitForSeconds(3f);
        Debug.Log("Win animations");
        //play conffeties
        foreach (GameObject a in confetties)
        {
            a.GetComponent<ParticleSystem>().Play();
        }
        //spotlight sound turn on
        AudioController.instance.PlaySpotlight();
        bool didWin = false;
        if (playerScore > AIScore)
		{
            for (int i = 0; i < audienceAnimations.Length; i++)
            {
                audienceAnimations[i].SetInteger("SelectState", 2);
            }
            //set animations
            leftAnimator.SetInteger("SelectState", 3);
			rightAnimator.SetInteger("SelectState", 5);
            spotlightLeft.SetActive(true);
            //add to finished win
            didWin = true;
        }
		else
		{
            for (int i = 0; i < audienceAnimations.Length; i++)
            {
                audienceAnimations[i].SetInteger("SelectState", 1);
            }
            //set animations
            spotlightRight.SetActive(true);
            leftAnimator.SetInteger("SelectState", 5);
			rightAnimator.SetInteger("SelectState", 3);
		}
        yield return new WaitForSeconds(1f);
        Debug.Log("Done");
        if(didWin)
        {
            bool hasBeenClearedBefore = false;
            foreach (string a in SceneSwitchereController.instance.buttonsAllCleared)
            {
                if (a == SceneSwitchereController.instance.lastBattleButtonName)
                {
                    hasBeenClearedBefore = true;
                }
            }
            //only add one to score if hasn't been clread before
            if (!hasBeenClearedBefore) SceneSwitchereController.instance.numberClearedLevels++;

            SceneSwitchereController.instance.wonLast = true;
            //if big win
            if (SceneSwitchereController.instance.numberClearedLevels >= numberClearedToWin)
            {
                fungusFlowChart.SetIntegerVariable("CommentaryRan", SceneSwitchereController.instance.selectedCharacter);
                fungusFlowChart.ExecuteBlock("EndOfCompetition");
                yield return new WaitForSeconds(2f);
                leftAnimator.SetInteger("SelectState", 1);
                rightAnimator.SetInteger("SelectState", 1);
                SceneSwitchereController.instance.ResetVariables();
                yield return new WaitForSeconds(3f);
                //SceneSwitchereController.instance.GotoScene_SetName("Credits");
                //SceneSwitchereController.instance.GotoScene_SetSequence("null");
            }
            //small win
            else
            {
                fungusFlowChart.ExecuteBlock("EndOfSong");
                yield return new WaitForSeconds(2f);
                leftAnimator.SetInteger("SelectState", 1);
                rightAnimator.SetInteger("SelectState", 1);
                yield return new WaitForSeconds(1f);
                SceneSwitchereController.instance.GotoScene_SetName("SongSelect");
                SceneSwitchereController.instance.GotoScene_SetSequence("null");
            }
        }
        //lose
        else
        {
            fungusFlowChart.ExecuteBlock("EndOfSong");
            yield return new WaitForSeconds(2f);
            leftAnimator.SetInteger("SelectState", 1);
            rightAnimator.SetInteger("SelectState", 1);
            yield return new WaitForSeconds(1f);
            SceneSwitchereController.instance.GotoScene_SetName("SongSelect");
            SceneSwitchereController.instance.GotoScene_SetSequence("null");
        }
    }

    public IEnumerator SequenceStartRunEtc()
    {
        //set start settings sequence
        InputManager.instance.isInputsDisabled = true;
        //time until start first level
        yield return new WaitForSeconds(timeBeforeFirstRun);
        //set health to full on start
        playerHealth = maxHealth;

        //loop through all levels until reached final
        while (currentBeatMap - 1 < SceneSwitchereController.instance.currentSequence.beatMapNamesInOrder.Length)
        {
            //enable inputs
            InputManager.instance.isInputsDisabled = false;
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
            yield return new WaitUntil(() => theGetter.currentLabelName != "End Sequnce");
            //only do this waiting part if not on last turn, if on last, move out of this and wait for finish instead
            if((currentBeatMap - 1) < SceneSwitchereController.instance.currentSequence.beatMapNamesInOrder.Length)
            {
                Debug.Log("Has passed into next, curreent labl is: " + theGetter.currentLabelName);
                //missspelled string but meh
                yield return new WaitUntil(() => theGetter.currentLabelName == "End Sequnce");
                //Start doing between beatmaps
                StartCoroutine(BetweenBeatMap());
                //wait for between beatmaps to be complete
                betweenIsDone = false;
                yield return new WaitUntil(() => betweenIsDone == true);
            }
        }
        //has played last beatmap
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
