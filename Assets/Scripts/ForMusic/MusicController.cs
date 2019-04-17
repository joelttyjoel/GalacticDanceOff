using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

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
        //if (Input.GetKeyUp(KeyCode.U))
        //{
        //    myEmitter.EventInstance.triggerCue();
        //}

        //if (Input.GetKeyUp(KeyCode.I))
        //{
        //    myEmitter.SetParameter("Restart", 1f);

        //}
    }

    public void RestartScene()
    {
        //set to restart, does slowdown goes to begining
        myEmitter.SetParameter("Restart", 1f);
        //set all other start values to 0
        for(int i = 0; i < numberOfLevels; i++)
        {
            myEmitter.SetParameter("StartLvl" + (i + 1).ToString(), 0f);
        }
        //now set restart back to 0 so it doesen't fuck up rest of shit
        myEmitter.SetParameter("Restart", 0f);
    }
}
