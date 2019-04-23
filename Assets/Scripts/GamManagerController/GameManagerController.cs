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

    public GameObject beatGetterThingGameObject;
    private BeatGetterFromFmodText theGetter;

    [Header("Scores: ")]
    //Scores
    public int egoMeterLeft = 0;
    public int egoMeterRight = 0;

    [Header("General Settings")]
    //Variables for other objects
    public float beatsSpawnToGoal_akaSpeed = 2f;

    [Header("Note settings")]
    //settings perfect, good
    public float percentagePerfectFromCenter = 0.05f;
    //stupid name but can't short that man
    public float percentageGoodFromCenter = 0.1f;
    //note fade distance after has gone too far
    public float fadeDistance = 0.5f;
    //fade from grey ish to white at start, hmm
    public float startFadeDistance = 1.0f;

    //beatmap reader reference
    private BeatmapReader beatMapReader;
    //music controller thing
    private FMOD_StudioEventEmitter eventEmitter;
    private MusicController musicController;
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
        //get temp thing
        theGetter = beatGetterThingGameObject.GetComponent<BeatGetterFromFmodText>();

        //get beatmapreader
        beatMapReader = GameObject.Find("BeatMapSpawner").GetComponent<BeatmapReader>();
        Debug.Assert(beatMapReader != null);
        //get musicmanager
        GameObject musicManager = GameObject.Find("MusicManager");
        Debug.Assert(musicManager != null);
        musicController = musicManager.GetComponent<MusicController>();
        eventEmitter = musicManager.GetComponent<FMOD_StudioEventEmitter>();
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
                musicController.EnterLevelByInt(currentBeatMap);
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
            musicController.RestartScene();
            failLevel = false;
        }
	}

    private void runBeatmap()
    {
        Debug.Log("Run beatmap: " + currentBeatMap);
        theGetter.runNextBeatmap = true;
        theGetter.nameOfMapToRun = beatMapNamesInOrder[currentBeatMap];
        //beatMapReader.StartRunningBeatmap(beatMapNamesInOrder[currentBeatMap]);
    }
}
