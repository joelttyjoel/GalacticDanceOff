using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MusicController : MonoBehaviour
{
    [Header("The thing above reference")]
    public StudioEventEmitter myEmitter;

    public StudioEventEmitter crowdEmitter;
    
    public List<SpriteRenderer> stars;
    public List<Animator> animations;
    public Sprite spriteGood;
    public Sprite spriteBad;

    private int lastCount = 0;

    [System.NonSerialized]
    public int starCountBig = 0;
    private int numberOfLevels = 1;

    public int streak = 0;

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
        lastCount = 0;
        starCountBig = 0;
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
            streak++;
        }
        else
        {
            streak = 0;
        }
        //calculate what percentage to be
        int starCount = 0;
        if (streak > 10) starCount++;
        if (streak > 25) starCount++;
        if (streak > 40) starCount++;
        if (streak > 55) starCount++;
        starCountBig = starCount;
        //do check for once execution
        if (lastCount != starCountBig)
        {
            //Debug.Log("Play it: " + starCountBig);
            lastCount = starCountBig;
            if (starCountBig != 0)
                animations[starCountBig - 1].Play("thing");
            if (starCountBig == 0)
                GameManagerController.instance.fungusFlowChart.ExecuteBlock("CommentaryDuringDoingBad");
            if (starCountBig == 2)
                GameManagerController.instance.fungusFlowChart.ExecuteBlock("CommentaryDuringDoingGood");
        }
        //now do others
        float percentageToFull = ((float)starCount / (float)stars.Count);
        //send this to musicEvent
        SetHitPercentage(percentageToFull * 100f);
        SetStars(percentageToFull);
    }

    private void SetHitPercentage(float percentageOfFull)
    {
        myEmitter.SetParameter("Score", percentageOfFull);
    }

    private void SetStars(float percentageOfFull)
    {
        float remainingOfStart = percentageOfFull;
        for (int i = 0; i <= stars.Count - 1; i++)
        {
            if(remainingOfStart >= 0.25f)
            {
                stars[i].sprite = spriteGood;
            }
            else
            {
                stars[i].sprite = spriteBad;
            }

            remainingOfStart -= 0.25f;
        }
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
