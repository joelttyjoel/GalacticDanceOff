using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerController : MonoBehaviour {
    [Header("Selected beatmap set?? Dunno how")]
    public int currentBeatMap = 0;
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

    [Header("Scores: ")]
    //Scores
    public int egoMeterLeft = 0;
    public int egoMeterRight = 0;

    [Header("General Settings")]
    //Variables for other objects
    public float beatsSpawnToGoal_akaSpeed = 2f;

    [Header("Note settings")]
    public float beatsToSlowDownFor = 4f;
    public float beatsToSpeedUpFor = 1f;
    public GameObject noteParent;
    //settings perfect, good
    public float percentagePerfectFromCenter = 0.05f;
    //stupid name but can't short that man
    public float percentageGoodFromCenter = 0.1f;
    //note fade distance after has gone too far
    public float fadeDistance = 0.5f;
    //fade from grey ish to white at start, hmm
    public float startFadeDistance = 1.0f;

    //make singleton
    public static GameManagerController instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        //now replaces already existing gameManager instead
        else if (instance != this)
            Destroy(instance.gameObject);
    }

    void Start () {
        //get musicmanager
        GameObject musicManager = GameObject.Find("MusicManager");
        Debug.Assert(musicManager != null);
        //th thingy 
        eventEmitter = musicManager.GetComponent<FMOD_StudioEventEmitter>();
        //Get thing that reads from Fmod
        theGetter = musicManager.GetComponent<BeatGetterFromFmodText>();
    }
	
	void Update () {
        if(startLevel)
        {
            startLevel = false;
            //if less than 2, aka 0 and 1 then do play current beatmap
            if (currentBeatMap < 2)
            {
                //run current map
                runBeatmap();
                MusicController.instance.EnterLevelByInt(currentBeatMap);
                //increment to next
                currentBeatMap++;
            }
            //if 2 or above, set back to 0
            else
            {
                currentBeatMap = 0;
            }
        }
        if(failLevel)
        {
            failBeatmap();
        }
	}

    private void failBeatmap()
    {
        //reset stuff for restart
        currentBeatMap = 0;
        //getter settings
        theGetter.runNextBeatmap = false;
        theGetter.currentLabelName = "Start";
        //stuff
        MusicController.instance.RestartScene();
        failLevel = false;

        //set stuff for beatmap slowdown and removal
        StartCoroutine(failNoteAnimation());
    }

    private IEnumerator failNoteAnimation()
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
            Debug.Log(Time.timeScale);
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

        //Debug.Log(percentageOfTravel);
        //while (noteParent.transform.childCount > 0)
        //{
        //THIS CAUSES INFINITE LOOP; FIX OR NOT DEPENDS ON WHAT LIKE
        //    //maybe add animation and shit here, not sure, if so just call function in note
        //    Destroy(noteParent.transform.GetChild(0).gameObject);
        //    Debug.Log(noteParent.trans)
        //}
    }

    private void runBeatmap()
    {
        Debug.Log("Run beatmap: " + currentBeatMap);
        //ugly but shhhhh, if pretty make thse functions in musiccontrollre instead but shhh
        theGetter.runNextBeatmap = true;
        theGetter.nameOfMapToRun = beatMapNamesInOrder[currentBeatMap];
        //beatMapReader.StartRunningBeatmap(beatMapNamesInOrder[currentBeatMap]);
    }
}
