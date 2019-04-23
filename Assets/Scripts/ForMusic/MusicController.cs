using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicController : MonoBehaviour
{
    [Header("The thing above reference")]
    public StudioEventEmitter myEmitter;

    private int numberOfLevels = 1;

    //hardcode names of parameters, dont change around 

    void Start()
    {
        //get number of levels, used to reset etc
        numberOfLevels = GameManagerController.instance.beatMapNamesInOrder.Length;
    }

    // Update is called once per frame
    void Update()
    {

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
        myEmitter.SetParameter("StartLvl" + levelNumber.ToString(), 1f);
        Debug.Log("StartLvl" + levelNumber.ToString());
    }

    public void RestartScene()
    {
        StartCoroutine(DoRestartScene());
    }
    private IEnumerator DoRestartScene()
    {
        Debug.Log("Restarting");
        //set to restart, does slowdown goes to begining
        myEmitter.SetParameter("Restart", 1f);
        //set all other start values to 0
        for (int i = 0; i < numberOfLevels; i++)
        {
            //get values 0, and 1 if count = 2
            myEmitter.SetParameter("StartLvl" + (i).ToString(), 0f);
        }
        //now set restart back to 0 so it doesen't fuck up rest of shit
        yield return new WaitForEndOfFrame();
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
