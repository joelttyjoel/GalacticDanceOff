﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    [Header("The thing above reference")]
    public StudioEventEmitter myEmitter;

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddNoteHitMiss(bool wasHit)
    {
        //add notevalue to end of list
        noteHits.Add(wasHit);
        //if is full, remove last element
        if(noteHits.Count >= numberOfNotesTracked)
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
        myEmitter.SetParameter("Score", percentage * 100);
    }

    public void PauseMusic()
    {
        //something with event, hmm
        myEmitter.EventInstance.setPaused(true);
    }

    public void ResumeMusic()
    {
        //same as pause but oposite
        myEmitter.EventInstance.setPaused(false);
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
