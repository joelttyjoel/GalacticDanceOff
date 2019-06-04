//--------------------------------------------------------------------
//
// This is a Unity behaviour script that demonstrates how to use
// timeline markers in your game code. 
//
// Timeline markers can be implicit - such as beats and bars. Or they 
// can be explicity placed by sound designers, in which case they have 
// a sound designer specified name attached to them.
//
// Timeline markers can be useful for syncing game events to sound
// events.
//
// The script starts a piece of music and then displays on the screen
// the current bar and the last marker encountered.
//
// This document assumes familiarity with Unity scripting. See
// https://unity3d.com/learn/tutorials/topics/scripting for resources
// on learning Unity scripting. 
//
//--------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class BeatGetterFromFmodText : MonoBehaviour
{
    public List<Animator> omfers;
    // Variables that are modified in the callback need to be part of a seperate class.
    // This class needs to be 'blittable' otherwise it can't be pinned in memory.
    [StructLayout(LayoutKind.Sequential)]
    public class TimelineInfo
    {
        //label info for knowing when started
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
        //time of beat, to set time of spawn etc for bois
        public float timeOfBeat = 0;
        //metronome for full beats
        public bool metronome1 = false;
    }

    //[StructLayout(LayoutKind.Sequential)]
    //public class ForFunction
    //{
    //    //function to call
    //    private void BeatHasHap()
    //    {
    //        //allow time per beat to change, not needed but oof
    //        float timePerBeat = BeatmapReader.instance.timePer16del * BeatmapReader.instance.sixteenthsPerBeat;
    //        //start create x parts per beat
    //        StartCoroutine(Create16Delar());
    //        //ugly but shhhhhhh ok, spawn fret on each beat
    //        beatSpawnerTop.GetComponent<BeatmapSpawner>().SpawnFret(timePerBeat, timelineInfo.timeOfBeat);
    //        beatSpawnerBot.GetComponent<BeatmapSpawner>().SpawnFret(timePerBeat, timelineInfo.timeOfBeat);

    //        //also polling if can enter next beatmap, can only happen on beat
    //        //if marker has changed during this turn, will change, yummy solutions
    //        if (runNextBeatmap && currentLabelName != timelineInfo.lastMarker)
    //        {
    //            runNextBeatmap = false;
    //            BeatmapReader.instance.StartRunningBeatmap(nameOfMapToRun);
    //        }

    //        //update marker value thing
    //        currentLabelName = timelineInfo.lastMarker;
    //    }
    //}

    public TimelineInfo timelineInfo;
    public GCHandle timelineHandle;

    FMOD.Studio.EVENT_CALLBACK beatCallback;
    FMOD.Studio.EventInstance musicInstance;

    //custom variables
    public float current16delTime = 0f;
    
    public string currentLabelName = "Start";
    [NonSerialized]
    public bool runNextBeatmap = false;
    [NonSerialized]
    public string nameOfMapToRun;

    public GameObject beatSpawnerTop;
    public GameObject beatSpawnerBot;

    //make singleton
    public static BeatGetterFromFmodText instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        //now replaces already existing gameManager instead
        else if (instance != this)
            Destroy(instance.gameObject);
    }

    void Start()
    {
        //custom stuff

        

        //---
        timelineInfo = new TimelineInfo();

        // Explicitly create the delegate object and assign it to a member so it doesn't get freed
        // by the garbage collected while it's being used
        beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);

        //XXX set this to thing above
        musicInstance = GetComponent<StudioEventEmitter>().EventInstance;

        // Pin the class that will store the data modified during the callback
        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        // Pass the object through the userdata of the instance
        musicInstance.setUserData(GCHandle.ToIntPtr(timelineHandle));

        musicInstance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        //musicInstance.start();
        //finally start polling the fukker
        //StartCoroutine(WaitForChanges());
    }

    void OnDestroy()
    {
        musicInstance.setUserData(IntPtr.Zero);
        musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicInstance.release();
        timelineHandle.Free();
    }

    //void OnGUI()
    //{
    //    GUILayout.Box(String.Format("Current metronome in beatmapReader = {0}, Its time = {1}", 69, current16delTime));
    //}

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, FMOD.Studio.EventInstance instance, IntPtr parameterPtr)
    {
        // Retrieve the user data
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero)
        {
            // Get the object to store beat and marker details
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                        //set current time
                        timelineInfo.timeOfBeat = Time.time;
                        //set time per 16del this
                        //currently hardcoded how many thingsPerBeat there is, not sure how to fix
                        BeatmapReader.instance.timePer16del = ((60 / parameter.tempo) / BeatmapReader.instance.sixteenthsPerBeat);
                        //new
                        BeatHasHap();

                        //old
                        //tik big metronome, per beat, tick after settings
                        //timelineInfo.metronome1= !timelineInfo.metronome1;
                    }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.lastMarker = parameter.name;

                        //set start beaetmap here instead stupid, basicly if label changes, bruh
                        CheckStartBeatmap();
                    }
                    break;
            }
        }
        return FMOD.RESULT.OK;
    }

    private void CheckStartBeatmap()
    {
        if (runNextBeatmap && currentLabelName != timelineInfo.lastMarker)
        {
            runNextBeatmap = false;
            BeatmapReader.instance.StartRunningBeatmap(nameOfMapToRun);
        }
        currentLabelName = timelineInfo.lastMarker;
    }
    
    private void BeatHasHap()
    {
        //Debug.Log("Beat");
        //instantly start doing 16delar to decrease delayed notes
        StartCoroutine(Create16Delar());
        //allow time per beat to change, not needed but oof
        float timePerBeat = BeatmapReader.instance.timePer16del * BeatmapReader.instance.sixteenthsPerBeat;
        //ugly but shhhhhhh ok, spawn fret on each beat
        beatSpawnerTop.GetComponent<BeatmapSpawner>().SpawnFret(timePerBeat, timelineInfo.timeOfBeat);
        beatSpawnerBot.GetComponent<BeatmapSpawner>().SpawnFret(timePerBeat, timelineInfo.timeOfBeat);
        
        //for oomf animation thing
        if(MusicController.instance.starCountBig >= 2)
        {
            if (!GameManagerController.instance.isRestarting)
            {
                foreach (Animator a in omfers)
                {
                    a.Play("thing");
                }
            }
        }

        //also polling if can enter next beatmap, can only happen on beat
        //if marker has changed during this turn, will change, yummy solutions
        //Debug.Log("Current: " + currentLabelName + " Last: " + timelineInfo.lastMarker);
        //if (runNextBeatmap && currentLabelName != timelineInfo.lastMarker)
        //{
        //    Debug.Log("Run beatmap");
        //    runNextBeatmap = false;
        //    BeatmapReader.instance.StartRunningBeatmap(nameOfMapToRun);
        //}

        //update marker value thing
        //currentLabelName = timelineInfo.lastMarker;
    }

    //this fukker is polling if metronome per beat has changed, if changed, start create 16delar
    //private IEnumerator WaitForChanges()
    //{
    //    float waitTimePerPoll = BeatmapReader.instance.timePer16del / 16;
    //    bool thisMetronome = timelineInfo.metronome1;
    //    float timePerBeat = BeatmapReader.instance.timePer16del * BeatmapReader.instance.sixteenthsPerBeat;

    //    //just sit and poll this badboi fast as fuk boi
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(waitTimePerPoll);
    //        //metronome has changed do create 16delar, 
    //        if (thisMetronome != timelineInfo.metronome1)
    //        {
    //            //set to current to tell if changed next time
    //            thisMetronome = timelineInfo.metronome1;
    //            //start create x parts per beat
    //            StartCoroutine(Create16Delar());
    //            //ugly but shhhhhhh ok, spawn fret on each beat
    //            beatSpawnerTop.GetComponent<BeatmapSpawner>().SpawnFret(timePerBeat, timelineInfo.timeOfBeat);
    //            beatSpawnerBot.GetComponent<BeatmapSpawner>().SpawnFret(timePerBeat, timelineInfo.timeOfBeat);

    //            //also polling if can enter next beatmap, can only happen on beat
    //            //if marker has changed during this turn, will change, yummy solutions
    //            if (runNextBeatmap && currentLabelName != timelineInfo.lastMarker)
    //            {
    //                runNextBeatmap = false;
    //                BeatmapReader.instance.StartRunningBeatmap(nameOfMapToRun);
    //            }

    //            //update marker value thing
    //            currentLabelName = timelineInfo.lastMarker;
    //        }
    //    }
    //}
    //hits beat just as started, then 3 following with time between equal to time between 16 delar, after last wait, then hopefully started again

    private IEnumerator Create16Delar()
    {
        for(int i = 0; i < BeatmapReader.instance.sixteenthsPerBeat; i++)
        {
            //add exact time to thing each time, starting at every beat
            current16delTime = timelineInfo.timeOfBeat + (BeatmapReader.instance.timePer16del * i);
            //Debug.Log(current16delTime.ToString() + " at beat in beat " + (i + 1).ToString());
            //set metronome in other fucker shhhhhhhh dont say it yes is ugly but shhhhhhh
            BeatmapReader.instance.metronome = !BeatmapReader.instance.metronome;
            BeatmapReader.instance.currentTickTime = current16delTime;
            //now wait
            yield return new WaitForSeconds(BeatmapReader.instance.timePer16del);
     
        }

    }
}