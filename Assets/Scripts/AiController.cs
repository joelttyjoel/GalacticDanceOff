using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour {

    public float chanceToHitAll = 0.5f;
    public float chanceForPerfect = 1f;
       

    public static AiController instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        //now replaces already existing gameManager instead
        else if (instance != this)
            Destroy(instance.gameObject);
    }

    public void NoteForAi(float timeUntilGoal)
    {
        StartCoroutine(NoteTest(timeUntilGoal));
    }

    private IEnumerator NoteTest(float timeUntilGoal)
    {
        //wait for time it takes for note to reach end before doing calculations
        yield return new WaitForSeconds(timeUntilGoal);

        //only add score if isn't restarting
        if (Random.Range(0f, 1f) < chanceToHitAll && !GameManagerController.instance.isRestarting)
        {
            if (Random.Range(0f, 1f) < chanceForPerfect)
            {
                GameManagerController.instance.addScore(false, true);
            }
            else
            {
                GameManagerController.instance.addScore(false, false);
            }
        }
        //if miss and isn't restarting
        else if(!GameManagerController.instance.isRestarting)
        {
            GameManagerController.instance.takeDamage(false);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
