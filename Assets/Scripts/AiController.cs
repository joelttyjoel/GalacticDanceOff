using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour {

    public float chanceToHitAll = 0.5f;

    public static AiController instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        //now replaces already existing gameManager instead
        else if (instance != this)
            Destroy(instance.gameObject);
    }

    public void NoteForAi()
    {
        StartCoroutine(NoteTest());
    }

    private IEnumerator NoteTest()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Score AI");
        if (Random.Range(0, 1) > chanceToHitAll)
        {
            if (Random.Range(0, 1) > 0.8f)
            {
                GameManagerController.instance.addScore(false, true);
            }
            else
            {
                GameManagerController.instance.addScore(false, false);
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
