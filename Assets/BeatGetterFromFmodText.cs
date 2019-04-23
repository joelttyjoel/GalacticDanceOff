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
using System.Runtime.InteropServices;
using UnityEngine;
using FMODUnity;

public class BeatGetterFromFmodText : MonoBehaviour
{
    // Variables that are modified in the callback need to be part of a seperate class.
    // This class needs to be 'blittable' otherwise it can't be pinned in memory.
    [StructLayout(LayoutKind.Sequential)]
    class TimelineInfo
    {
        public int currentMusicBar = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
        public float timeOfBeat = 0;
        public bool metronome1 = false;
        public bool metronome4 = false;
    }

    TimelineInfo timelineInfo;
    GCHandle timelineHandle;

    FMOD.Studio.EVENT_CALLBACK beatCallback;
    FMOD.Studio.EventInstance musicInstance;

    //custom variables
    public GameObject beatMapSpawnerRef;
    public int current16del = 0;
    public float current16delTime = 0f;

    private BeatmapReader beatMapReaderRef;
    private BeatmapSpawner beatMapSpawnerScriptRef;
    private float timePer16delthis = 0f;
    private float beatsPerTaktThis = 0;

    private string currentLabelName = "Start";
    public bool runNextBeatmap = false;
    public string nameOfMapToRun;

    void Start()
    {
        //custom stuff
        //set time to wait from oter thing
        beatMapReaderRef = beatMapSpawnerRef.GetComponent<BeatmapReader>();
        beatMapSpawnerScriptRef = beatMapSpawnerRef.GetComponent<BeatmapSpawner>();

        //get these two from inside the fucker instead, set in beatMapReaderRef thing plz too
        timePer16delthis = 0.166f;
        beatsPerTaktThis = 4;

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
        StartCoroutine(WaitForChanges());
    }

    void OnDestroy()
    {
        musicInstance.setUserData(IntPtr.Zero);
        musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicInstance.release();
        timelineHandle.Free();
    }

    void OnGUI()
    {
        GUILayout.Box(String.Format("Current 16del = {0}, Its time = {1}", current16del, current16delTime));
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, FMOD.Studio.EventInstance instance, IntPtr parameterPtr)
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
                        timelineInfo.currentMusicBar = parameter.beat;
                        //tik big metronome, per beat
                        timelineInfo.metronome1= !timelineInfo.metronome1;
                        //set current time
                        timelineInfo.timeOfBeat = Time.time;
                    }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.lastMarker = parameter.name;
                    }
                    break;
            }
        }
        return FMOD.RESULT.OK;
    }

    //this fukker is polling if metronome per beat has changed, if changed, start create 16delar
    private IEnumerator WaitForChanges()
    {
        float waitTimePerPoll = timePer16delthis / 16;
        bool thisMetronome = timelineInfo.metronome1;
        float timePerBeat = timePer16delthis * beatsPerTaktThis;
        while (true)
        {
            yield return new WaitForSeconds(waitTimePerPoll);
            //metronome has changed
            if(thisMetronome != timelineInfo.metronome1)
            {
                thisMetronome = timelineInfo.metronome1;
                StartCoroutine(Create16Delar());

                //ugly but shhhhhhh ok
                beatMapSpawnerScriptRef.SpawnFret(timePerBeat, timelineInfo.timeOfBeat);
            }
            //also polling if can enter next beatmap
            if(runNextBeatmap && currentLabelName != timelineInfo.lastMarker)
            {
                //if can enter, set things back, run beatmap
                currentLabelName = timelineInfo.lastMarker;
                runNextBeatmap = false;
                beatMapReaderRef.StartRunningBeatmap(nameOfMapToRun);
            }
        }
    }
    //hits beat just as started, then 3 following with time between equal to time between 16 delar, after last wait, then hopefully started again
    private IEnumerator Create16Delar()
    {
        for(int i = 0; i < beatsPerTaktThis; i++)
        {
            //add exact time to thing each time, starting at every beat
            current16delTime = timelineInfo.timeOfBeat + (timePer16delthis * i);
            //Debug.Log(current16delTime.ToString() + " at beat in beat " + (i + 1).ToString());
            //set metronome in other fucker shhhhhhhh dont say it yes is ugly but shhhhhhh
            beatMapReaderRef.metronome = !beatMapReaderRef.metronome;
            beatMapReaderRef.currentTickTime = current16delTime;
            //now wait
            yield return new WaitForSeconds(timePer16delthis);
     
        }
    }
}