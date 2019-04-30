using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class SceneSwitchereController : MonoBehaviour {
    //List of all scenes
    public List<Object> all_Scenes;
    //List of all sequencs for battle scene
    public List<Info_Sequence> all_Sequences_Sp;
    public List<Info_Sequence> all_Sequences_Mp;
    //Namme current scene
    public string nameCurrentScene;
    //Name current sequence in battle
    public string nameCurrentSequence;
    public Info_Sequence currentSequence;
    //Single or multiplayer
    public bool isSp = true;


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

    public void LoadSceneByName(string nameOfScene, string nameOfSequence)
    {
        //load scene no matter what
        EditorSceneManager.LoadSceneAsync(nameOfScene);
        nameCurrentScene = nameOfScene;
        //Set current sequence
        nameCurrentSequence = nameOfSequence;

        //select correct sequence list to search
        List<Info_Sequence> listOfSeqToSearch;
        if (isSp) listOfSeqToSearch = all_Sequences_Sp;
        else listOfSeqToSearch = all_Sequences_Mp;
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
    
    void Start () {
        
	}
	
	void Update () {
		
	}

    //set Sp or Mp
    public void SetSpOrMp_TrueIsSp(bool isSpin)
    {
        isSp = isSpin;
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
        LoadSceneByName(nameVar, sequenceVar);
    }
}
