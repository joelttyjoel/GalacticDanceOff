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
    public GameObject beatSpawnerTop;
    public GameObject beatSpawnerBot;
    private BeatmapSpawner beatSpawnerTopScript;
    private BeatmapSpawner beatSpawnerBotScript;
    public float thingsPerBeat = 4;
    public float currentTickTime = 0f;

    //1-8 = notes, 0 = nothing, 9 = stop
    public int currentLineNumber = 0;
    //top = wasd, bot = arrowkeys
    public List<int> easyToReadBeatsTop;
    public List<int> easyToReadBeatsBot;

    //timing stuff
    [HideInInspector]
    public bool metronome = false;
    [HideInInspector]
    public float timePer16del = 0.1666f;
    private bool beatMapIsRunning = false;
    private bool hasHadCooldown = true;

    //beatmap stuff
    private string beatMapPath;
    private string[] beatMapLines;

    //for creating singleton, love easy referencing
    public static BeatmapReader instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        //now replaces already existing gameManager instead
        else if (instance != this)
            Destroy(instance.gameObject);

        //set shit, dunno
        thingsPerBeat = 4f;
        timePer16del = 0.1666f;
    }

    private void Start()
    {
        beatSpawnerTopScript = beatSpawnerTop.GetComponent<BeatmapSpawner>();
        beatSpawnerBotScript = beatSpawnerBot.GetComponent<BeatmapSpawner>();
    }

    public void StartRunningBeatmap(string beatMapName)
    {
        if (beatMapIsRunning) return;
        //get all lines as string array
        //Debug.Log("THIS IS PATH: " + Application.dataPath);
        beatMapLines = File.ReadAllLines(Application.dataPath + "/StreamingAssets/Beatmaps/2laneBeatmaps/" + beatMapName + ".txt");
        //get setup for start
        GoToStartOfBeats();
        //create easy to read beatlist for computer
        easyToReadBeatsTop.Clear();
        easyToReadBeatsBot.Clear();
        CreateEasyToReadBeats();
        //actually start courutine that places notes
        StartCoroutine(RunBeatmap());
    }
    
    IEnumerator RunBeatmap()
    {
        //set things here again for safety
        beatMapIsRunning = true;
        //thing for cooldown check
        hasHadCooldown = metronome;
        
        //fix that boi too yes
        float totalWaitTime = timePer16del * thingsPerBeat;

        int currentNoteIndex = 0;
        //read through beatmap, if note send info to spawner, if end, go out of
        while (beatMapIsRunning)
        {

            //wait at start of thing, starts as soon as metronome changes from what was at start
            yield return new WaitUntil(() => hasHadCooldown != metronome);
            hasHadCooldown = metronome;

            //read what is in list, do that
            int currentValueTop = easyToReadBeatsTop[currentNoteIndex];
            int currentValueBot = easyToReadBeatsBot[currentNoteIndex];
            //TOP-------------------------
            //NOTES
            if (currentValueTop > 0 && currentValueTop < 9)
            {
                beatSpawnerTopScript.SpawnNote(currentValueTop, totalWaitTime, currentTickTime);
            }
            //NOTHING
            else if (currentValueTop == 0)
            {
                //nothing, empty space
            }
            //IF NOTHING, JUST MOVE TO NEXT AND WAIT
            //If is 9, stop
            else
            {
                goto atStop;
            }
            //BOT-------------------------
            if (currentValueBot > 0 && currentValueBot < 9)
            {
                beatSpawnerBotScript.SpawnNote(currentValueBot, totalWaitTime, currentTickTime);
            }
            //NOTHING
            else if (currentValueBot == 0)
            {
                //nothing, empty space
            }
            //IF NOTHING, JUST MOVE TO NEXT AND WAIT
            //If is 9, stop
            else
            {
                goto atStop;
            }

            currentNoteIndex++;
        }

        //wen done, is here
        atStop:;

		// -----------
		//call gamemanager function
		// -----------

        
		beatMapIsRunning = false;
        yield return new WaitForEndOfFrame();
    }

    public void StopRunningBeatmap()
    {
        beatMapIsRunning = false;
    }

    //METRONOME, OLD BADBOY, GRRRR
    //void FixedUpdate()
    //{
    //    currentTickTime = Time.time;
    //    metronome = !metronome;
    //}

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
                //if(currentLineArray[2] == 'b')
                //{
                //    //read line to find bpm, first char of number is at index 5
                //    //string fullValString = "";

                //    //int currentIndex = 5;
                //    //char currentChar = currentLineArray[currentIndex];
                //    ////is false when reached end of numbers
                //    //while(currentChar != '-')
                //    //{
                //    //    //add current number to string
                //    //    fullValString += currentChar.ToString();
                //    //    currentIndex++;
                //    //    currentChar = currentLineArray[currentIndex];
                //    //}

                //    //bpm = int.Parse(fullValString);
                //}
                ////if set timesignature
                //else if (currentLineArray[2] == 't')
                //{
                //    //howLongBeats = float.Parse(currentLineArray[4].ToString());
                //    //beatsPerTakt = float.Parse(currentLineArray[6].ToString());
                //}
                //if start
                if (currentLineArray[2] == 's')
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
                //add thing 6 to top, add thing 7 to bot, if 0, adds 0
                easyToReadBeatsTop.Add(int.Parse(currentLineArray[6].ToString()));
                easyToReadBeatsBot.Add(int.Parse(currentLineArray[7].ToString()));
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
                    //add 9 to ends of both, tis is end
                    easyToReadBeatsTop.Add(9);
                    easyToReadBeatsBot.Add(9);
                    hasReachedEnd = true;
                }
            }

            //move onto next line in textDocumentArray
            currentLineNumber++;
        }
    }
    
}
