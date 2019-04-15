using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerController : MonoBehaviour {
    [Header("Selected beatmap set?? Dunno how")]
    public int currentBeatMap = 0;
    //difficulty thing? Hm is needed or not, who know
    //Array to store all beatmaps for this level
    [SerializeField]
    public TextAsset[] beatMaps;

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
        //get beatmapreader
        beatMapReader = GameObject.Find("BeatMapSpawner").GetComponent<BeatmapReader>();
        Debug.Assert(beatMapReader != null);
	}
	
	void Update () {
        runBeatmap();
	}

    private void runBeatmap()
    {
        beatMapReader.StartRunningBeatmap(beatMaps[currentBeatMap]);
    }
}
