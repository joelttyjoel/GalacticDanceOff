using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class OldBeatMapReader : MonoBehaviour {

    //public
    [Header("Beatmap")]
    public TextAsset beatMap;
    //ev hide so noone changes, gets confused
    [Header("Beatmap settings")]
    public float bpm;
    public float howLongBeats = 0;
    public float beatsPerTakt = 0;
    public float thingsPerBeat = 0;
    public int currentLineNumber = 0;

    [Header("Run")]
    public bool runBeatMap = false;
    [HideInInspector]
    public bool metronome = false;

    //private
    private string beatMapPath;
    private string[] beatMapLines;

    private float timeToWait;

    private bool beatMapIsRunning = false;

    private bool hasHadCooldown = true;

    void Start()
    {
        //setup text document
        beatMapPath = (AssetDatabase.GetAssetPath(beatMap));
        //get all lines as string array
        beatMapLines = File.ReadAllLines(beatMapPath);
        //get setup for start
        GoToStartOfBeats();
        //set things once bpm has been recieved
        timeToWait = ((60 / bpm) / beatsPerTakt);
        //set fixed update metronome to thing
        Time.fixedDeltaTime = timeToWait;
        //StartCoroutine(Metronome());
        //things per beat deciding
        if (beatsPerTakt == 8) thingsPerBeat = 2;
        else thingsPerBeat = 4;
    }

    void Update()
    {
        //replace with public function instead for next stuff
        //starts runbeatmap() and disables bool button to start again, meh
        if (runBeatMap == true && beatMapIsRunning == false)
        {
            runBeatMap = false;
            StartCoroutine(RunBeatmap());
        }
        if (runBeatMap == true) runBeatMap = false;
    }

    public void StartRunningBeatmap()
    {
        if (!beatMapIsRunning) StartCoroutine(RunBeatmap());
    }

    IEnumerator RunBeatmap()
    {
        //set things here again for safety
        beatMapIsRunning = true;
        //get thing
        OldBeatMapSpawner beatmapSpawner = GetComponent<OldBeatMapSpawner>();
        //thing for cooldown check
        hasHadCooldown = true;

        //read through beatmap, send info at current to function in spawner
        while (beatMapIsRunning)
        {
            //computations wont take too long, if needed, shift due to time
            string currentLine = beatMapLines[currentLineNumber];
            char[] currentLineArray = currentLine.ToCharArray();
            //beat thing, most commonly just does this then last parts
            if (currentLineArray[1] == '/')
            {
                //check if has note, if not do nothing
                if (currentLineArray.Length > 6)
                {
                    beatmapSpawner.SpawnItem(currentLineArray[6], (timeToWait * thingsPerBeat));
                }
            }
            //if stop thing
            else if (currentLineArray[0].ToString() + currentLineArray[1].ToString() == "--")
            {
                if (currentLineArray[2] == 's')
                {
                    //dirty, but basicly move outside of while loop and have beatMap start at next line
                    goto atStop;
                }
            }
            //else is comment or something else, do nothing just move on without pause
            else
            {
                goto noCooldown;
            }
            //in almost all cases, do a wait then move to next
            //start cooldown thing to wait on instead of waiting here
            yield return new WaitUntil(() => hasHadCooldown != metronome);
            hasHadCooldown = metronome;

        noCooldown:;
            currentLineNumber++;
        }

    //wen done, is here
    atStop:;
        GoToStartOfBeats();
        beatMapIsRunning = false;
        yield return new WaitForEndOfFrame();
    }

    IEnumerator WaitForXThenFlip(float waitFor, bool inputBool)
    {
        yield return new WaitForSeconds(waitFor);
        inputBool = !inputBool;
    }

    //IEnumerator Metronome()
    //{
    //    float thisTimeToWait = timeToWait + metronomeOffset;
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(thisTimeToWait);
    //        metronome = !metronome;
    //    }
    //}

    //double timeDifference = 0;
    //double lastTime = 0;
    //double nowTime = 0;
    //METRONOME
    void FixedUpdate()
    {
        //nowTime = Time.timeSinceLevelLoad;
        ////now - last
        //timeDifference = nowTime - lastTime;
        //Debug.Log("TimeDifference: " + timeDifference);
        ////set last to now, which is last for next loop
        //lastTime = nowTime;
        metronome = !metronome;
    }

    private void GoToStartOfBeats()
    {
        //skips initial comments, reads data at start etc
        currentLineNumber = 0;
        //do until break
        while (currentLineNumber < 1000)
        {
            string currentLine = beatMapLines[currentLineNumber];
            char[] currentLineArray = currentLine.ToCharArray();
            //big juicy if
            //is -- command
            if (currentLineArray[0].ToString() + currentLineArray[1].ToString() == "--")
            {
                //if setBpm
                if (currentLineArray[2] == 'b')
                {
                    //read line to find bpm, first char of number is at index 5
                    string fullValString = "";

                    int currentIndex = 5;
                    char currentChar = currentLineArray[currentIndex];
                    //is false when reached end of numbers
                    while (currentChar != '-')
                    {
                        //add current number to string
                        fullValString += currentChar.ToString();
                        currentIndex++;
                        currentChar = currentLineArray[currentIndex];
                    }

                    bpm = int.Parse(fullValString);
                }
                //if set timesignature
                else if (currentLineArray[2] == 't')
                {
                    howLongBeats = float.Parse(currentLineArray[4].ToString());
                    beatsPerTakt = float.Parse(currentLineArray[6].ToString());
                }
                //if start
                else if (currentLineArray[2] == 's')
                {
                    //dirty, but basicly move outside of while loop and have beatMap start at next line
                    currentLineNumber++;
                    goto atStart;
                }
            }
            //comments are skipped, move to next
            currentLineNumber++;
        }
    //when done, end here
    atStart:;
    }
}
