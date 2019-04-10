using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour {
    [SerializeField]
    [Header("Sprites for input1")]
    List<Sprite> sprites1;

    [HideInInspector]
    public int noteType;
    [HideInInspector]
    public float timeAtBirth;
    [HideInInspector]
    public float timeSinceBirth;
    //[HideInInspector]
    public float distanceSpawnDestroyer;
    [HideInInspector]
    public float timePerBeat;
    [HideInInspector]
    public GameObject noteChecker;

    public float percentageOfTravel = 0f;


    private float timeUntilGoal;
    private Vector3 originalPos;

    //GET THIS FROM BIG THING SOMEWHERE ELSE INSTEAD, PROBABLY JUST ONCE IN SPAWNER
    private float beatsUntilGoal = 4f;

    void Start () {
        //choose sprite dending on input method
        GetComponent<SpriteRenderer>().sprite = sprites1[noteType];
        //set birth time
        timeAtBirth = Time.time;
        //set time until goal
        timeUntilGoal = beatsUntilGoal * timePerBeat;

        originalPos = transform.position;
    }
	
	void Update () {
        //compare current time to birth time
        timeSinceBirth = Time.time - timeAtBirth;
        //depending on how far compared to full time, do the do, percentage of completed
        percentageOfTravel = timeSinceBirth / timeUntilGoal;
        Vector3 currentPos = transform.position;
        transform.position = new Vector3(originalPos.x - (distanceSpawnDestroyer * percentageOfTravel), currentPos.y, currentPos.z);

        if (percentageOfTravel > 1.02f) GoneTooFar();
    }

    private void GoneTooFar()
    {
        //will deque
        Destroy(this.gameObject);
        //noteChecker.GetComponent<checker>().deque();
        //StartCoroutine.HasBeenHit();
    }

    //public IEnumerator()
    //{
        //slow fade until destroy
    //}
}
