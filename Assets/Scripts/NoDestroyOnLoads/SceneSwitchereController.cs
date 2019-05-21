using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitchereController : MonoBehaviour {
    public GameObject blackScreen;
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
        Image sr = blackScreen.GetComponent<Image>();
        //MAKE BLACK
        float opacity = 1.0f;
        sr.color = new Color(0, 0, 0, opacity);
        //SWITCH SCENE BETWEEN
        yield return new WaitForSeconds(1.0f);
        LoadSceneByName(nameVar, sequenceVar);
        yield return new WaitForSeconds(1.0f);
        //FADE BACK IN
        float opacity2 = 0.0f;
        sr.color = new Color(0, 0, 0, opacity2);
        yield return new WaitForEndOfFrame();
    }
}
