using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    [Header("The thing above reference")]
    public StudioEventEmitter myEmitter;

    public StudioEventEmitter crowdEmitter;

    private int numberOfLevels = 1;

    public int numberOfNotesTracked = 12;
    private List<bool> noteHits = new List<bool>();

    //for creating singleton, love easy referencing
    public static MusicController instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        //now replaces already existing gameManager instead
        else if (instance != this)
            Destroy(instance.gameObject);

        //GET INFO ABOUT MUSIC EVENT FOR THIS SCENE GRRRRRR
        myEmitter.Event = SceneSwitchereController.instance.currentSequence.pathToFmodEvent;
    }

    void Start()
    {
        //get number of levels, used to reset etc
        numberOfLevels = GameManagerController.instance.beatMapNamesInOrder.Length;
        //fill up bar of things
        noteHits.Add(true);
        noteHits.Add(true);
        noteHits.Add(true);
        noteHits.Add(true);
        noteHits.Add(true);
        noteHits.Add(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddNoteHitMiss(bool wasHit)
    {
        //add notevalue to end of list, if false add more
        if(wasHit)
        {
            noteHits.Add(wasHit);
        }
        else
        {
            noteHits.Add(wasHit);
            noteHits.Add(wasHit);
            noteHits.Add(wasHit);
            noteHits.Add(wasHit);
        }
        //remove from back until isn't full anymore
        while(noteHits.Count >= numberOfNotesTracked)
        {
            noteHits.RemoveAt(0);
        }
        //if not, compare how big percentage of notes are hit vs miss
        float trueCount = 0;
        float falseCount = 0;
        foreach (bool a in noteHits)
        {
            if (a) trueCount++;
            else falseCount++;
        }

        float percentageOfTrue = trueCount / (trueCount + falseCount);
        //send this to musicEvent
        SetHitPercentage(percentageOfTrue);
        Debug.Log("percentage of true: "+percentageOfTrue);
    }

    private void SetHitPercentage(float percentage)
    {
        float hunnerdPercentage = percentage * 100;
        myEmitter.SetParameter("Score", hunnerdPercentage);
    }

    public void PauseMusic()
    {
        //something with event, hmm
        myEmitter.EventInstance.setPaused(true);
        crowdEmitter.EventInstance.setPaused(true);
    }

    public void ResumeMusic()
    {
        //same as pause but oposite
        myEmitter.EventInstance.setPaused(false);
        crowdEmitter.EventInstance.setPaused(false);
    }

    public void EnterLevelByInt(int levelNumber)
    {
        myEmitter.SetParameter("StartLvl", levelNumber);
        Debug.Log("StartLvl" + levelNumber.ToString());
    }

    public void RestartScene()
    {
        StartCoroutine(DoRestartScene());
    }
    private IEnumerator DoRestartScene()
    {
        Debug.Log("Restarting Music");
        //set to restart, does slowdown goes to begining
        myEmitter.SetParameter("Restart", 1f);
        myEmitter.SetParameter("StartLvl", 0f);
        //now set restart back to 0 so it doesen't fuck up rest of shit
        yield return new WaitForSeconds(2.0f);
        myEmitter.SetParameter("Restart", 0f);
        //do slow down time at same rate as thing
    }

    //how to get parameter value, probably smilar query beat time thing
    //ParameterInstance instance;
    //float theValue;
    ////im guessing a RESULt = a querry to the fmod thing
    //FMOD.RESULT thisResult = myEmitter.EventInstance.getParameter("StartLvl" + (i).ToString(), out instance);
    //FMOD.RESULT secondResult = instance.getValue(out theValue);
}
