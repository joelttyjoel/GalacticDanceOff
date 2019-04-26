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
        //Set name of current Sequence so other objects can read from correct one on launch
        if (nameOfSequence != "") nameCurrentSequence = nameOfSequence; 
    }
    
    void Start () {
        
	}
	
	void Update () {
		
	}
}
