using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitchereController : MonoBehaviour {
    public GameObject blackScreen;
    public bool dissableAllInputs = false;
    //List of all scenes
    public List<Object> all_Scenes;
    //List of all sequencs for battle scene
    public List<Info_Sequence> all_Sequences_Sp;
    //Namme current scene
    public string nameCurrentScene;
    //Name current sequence in battle
    public string nameCurrentSequence;
    public Info_Sequence currentSequence;
    //Single or multiplayer
	public bool keyBoard, xBox, Ps4;
    //settings for character, oponent, cleared stages etc.
    public int numberClearedLevels = 0;
    //purple = 0, stick = 1, birb = 2
    public int selectedCharacter = 0;
    public int selectedOponent = 0;
    //volumes
    public float volumeMusic = 1;
    public float volumeSound = 1;

    public static SceneSwitchereController instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        //now replaces already existing gameManager instead
        else if (instance != this)
            Destroy(instance.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(blackScreen.gameObject);
    }

    void Update()
    {

    }

    public void LoadSceneByName(string nameOfScene, string nameOfSequence)
    {
        //load scene no matter what
        SceneManager.LoadSceneAsync(nameOfScene);
        nameCurrentScene = nameOfScene;
        //Set current sequence
        nameCurrentSequence = nameOfSequence;

        //select correct sequence list to search
        List<Info_Sequence> listOfSeqToSearch;
        listOfSeqToSearch = all_Sequences_Sp;
        //search in list for correct name one
        foreach(Info_Sequence a in listOfSeqToSearch)
        {
            if(a.name == nameOfSequence)
            {
                currentSequence = a;
                break;
            }
        }
        //if not found, shit will break, but for now, just assume stuff with work

        Debug.Log("Changing to Scene: " + nameOfScene + " and playing sequence: " + nameOfSequence);
    }

	public void KeyBoard()
	{
		keyBoard = true;
		xBox = false;
		Ps4 = false;

	}

	public void XBOX()
	{
		keyBoard = false;
		xBox = true;
		Ps4 = false;

	}

	public void PS4()
	{
		keyBoard = false;
		xBox = false;
		Ps4 = true;

	}

    public void SetOwnCharacter(int selected)
    {
        selectedCharacter = selected;
    }

    public void SetOponentCharacter(int selected)
    {
        selectedOponent = selected;
    }

    public void SetVolume(float value, bool isMusic)
    {
        volumeMusic = value;
        if (isMusic)
        {
            volumeMusic = value;
            AudioController.instance.SetVolumeByFloat(volumeMusic, true);
        }
        else
        {
            volumeSound = value;
            AudioController.instance.SetVolumeByFloat(volumeSound, false);
        }
    }

    //To enable on click, shhhhh is super effecient dont worry, is great
    private bool hasGotName = false;
    private bool hasGotSequence = false;
    private string nameVar;
    private string sequenceVar;
    public void GotoScene_SetName(string name)
    {
        nameVar = name;
        hasGotName = true;
    }
    public void GotoScene_SetSequence(string sequence)
    {
        sequenceVar = sequence;
        hasGotSequence = true;
        StartCoroutine(WaitForBothInputs());
    }

    private IEnumerator WaitForBothInputs()
    {
        yield return new WaitUntil(() => hasGotName == true);
        yield return new WaitUntil(() => hasGotSequence == true);
        hasGotName = false;
        hasGotSequence = false;
        //SWITCHES IN HERE
        StartCoroutine(DoFadeBlack());
    }

    public IEnumerator DoFadeBlack()
    {
        //dissalble inpts
        dissableAllInputs = true;
        Image sr = blackScreen.GetComponent<Image>();
        //MAKE BLACK
        float startTime2 = 0f;
        while (startTime2 <= 1)
        {
            sr.color = new Color(0, 0, 0, startTime2);
            startTime2 += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        //opacity = 1.0f;
        sr.color = new Color(0, 0, 0, 1);
   
        //SWITCH SCENE BETWEEN
        LoadSceneByName(nameVar, sequenceVar);
        yield return new WaitForSeconds(0.5f);
        //FADE BACK IN
        float startTime1 = 1f;
        while (startTime1 >= 0)
        {
            sr.color = new Color(0, 0, 0, startTime1);
            startTime1 -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        sr.color = new Color(0, 0, 0, 0);
        //reenable inputs
        dissableAllInputs = false;
    }
}
