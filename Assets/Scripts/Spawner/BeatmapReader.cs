using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BeatmapReader : MonoBehaviour {
    //public
    //[Header("Beatmap")]
    //public TextAsset beatMap;
    //ev hide so noone changes, gets confused
    [Header("Beatmap settings")]
    public float bpm;
    public float howLongBeats = 0;
    public float beatsPerTakt = 0;
    public float thingsPerBeat = 0;
    public int currentLineNumber = 0;
    private float currentTickTime = 0f;

    //1-8 = notes, 0 = nothing, 9 = stop
    public List<int> easyToReadBeats;

    //private
    [HideInInspector]
    public bool metronome = false;
    private string beatMapPath;
    private string[] beatMapLines;

    private float timeToWait;

    private bool beatMapIsRunning = false;
    private bool hasHadCooldown = true;

    private int thingsPerBeatCounter = 0;
    
    public void StartRunningBeatmap(TextAsset beatMap)
    {
        if (beatMapIsRunning) return;
        //get all lines as string array
        //Debug.Log("THIS IS PATH: " + Application.dataPath);
        beatMapLines = File.ReadAllLines(Application.dataPath + "/StreamingAssets/Beatmaps/" + beatMap.name + ".txt");
        //get setup for start
        GoToStartOfBeats();
        //create easy to read beatlist for computer
        CreateEasyToReadBeats();
        //set things once bpm has been recieved
        timeToWait = ((60 / bpm) / beatsPerTakt);
        //set fixed update metronome to thing
        Time.fixedDeltaTime = timeToWait;
        //StartCoroutine(Metronome());
        //things per beat deciding
        if (beatsPerTakt == 8) thingsPerBeat = 2;
        else thingsPerBeat = 4;

        //actually start courutine
        StartCoroutine(RunBeatmap());
    }

    IEnumerator RunBeatmap()
    {
        //set things here again for safety
        beatMapIsRunning = true;
        //get thing
        BeatmapSpawner beatmapSpawner = GetComponent<BeatmapSpawner>();
        //thing for cooldown check
        hasHadCooldown = metronome;

        float totalWaitTime = timeToWait * thingsPerBeat;

        int currentNoteIndex = 0;
        //read through beatmap, if note send info to spawner, if end, go out of
        while (beatMapIsRunning)
        {
            //wait at start of thing, starts as soon as metronome changes from what was at start
            yield return new WaitUntil(() => hasHadCooldown != metronome);
            hasHadCooldown = metronome;

            //read what is in list, do that
            int currentValue = easyToReadBeats[currentNoteIndex];
            //NOTES
            if (currentValue > 0 && currentValue < 9)
            {
                beatmapSpawner.SpawnNote(currentValue, totalWaitTime, currentTickTime);
            }
            //NOTHING
            else if (currentValue == 0)
            {
                //do tiny thing to equal note spawning
                int hi;
                hi = 1 + 1;
            }
            //IF NOTHING, JUST MOVE TO NEXT AND WAIT
            else
            {
                goto atStop;
            }

            //count thingsPerBeat to get full metronome switch
            //0, 1, 2, 3, 4
            if (thingsPerBeatCounter == thingsPerBeat) thingsPerBeatCounter = 0;
            if (thingsPerBeatCounter == 0) beatmapSpawner.SpawnFret(totalWaitTime, currentTickTime);

            thingsPerBeatCounter++;
            currentNoteIndex++;
        }

        //wen done, is here
        atStop:;
        beatMapIsRunning = false;
        yield return new WaitForEndOfFrame();
    }

    //METRONOME
    void FixedUpdate()
    {
        currentTickTime = Time.time;
        metronome = !metronome;
    }

    private void GoToStartOfBeats()
    {
        //skips initial comments, reads data at start etc
        currentLineNumber = 0;
        //do until break
        while(currentLineNumber < 2000)
        {
            string currentLine = beatMapLines[currentLineNumber];
            char[] currentLineArray = currentLine.ToCharArray();
            //big juicy if
            //is -- command
            if(currentLineArray[0].ToString() + currentLineArray[1].ToString() == "--")
            {
                //if setBpm
                if(currentLineArray[2] == 'b')
                {
                    //read line to find bpm, first char of number is at index 5
                    string fullValString = "";

                    int currentIndex = 5;
                    char currentChar = currentLineArray[currentIndex];
                    //is false when reached end of numbers
                    while(currentChar != '-')
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

    private void CreateEasyToReadBeats()
    {
        bool hasReachedEnd = false;
        //while last read isn't equal to stop
        while (!hasReachedEnd)
        {
            //computations wont take too long, if needed, shift due to time
            string currentLine = beatMapLines[currentLineNumber];
            char[] currentLineArray = currentLine.ToCharArray();

            //beat thing, most commonly just does this then last parts
            if (currentLineArray[1] == '/')
            {
                //check if has note, if so add thing to list
                if (currentLineArray.Length > 6)
                {
                    //thing
                    //beatmapSpawner.SpawnItem(currentLineArray[6], (timeToWait * thingsPerBeat));
                    easyToReadBeats.Add(int.Parse(currentLineArray[6].ToString()));
                }
                //else add 0 to list
                else
                {
                    easyToReadBeats.Add(0);
                }
            }
            else if(currentLineArray[0] == '#')
            {
                //litrally nothing
            }
            //if command thing, can only be stop for now
            else if (currentLineArray[0] == '-')
            {
                if (currentLineArray[2] == 's')
                {
                    easyToReadBeats.Add(9);
                    hasReachedEnd = true;
                }
            }

            //move onto next line in textDocumentArray
            currentLineNumber++;
        }
    }
}
