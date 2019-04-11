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
    
    private float fadeDistance = 0.5f;
    private float percentageOfTravel = 0f;
    private float timeUntilGoal;
    private Vector3 originalPos;
    public float beatsUntilGoal = 1f;
    public float percentageAboveFinal = 0.1f;
    private float totalPercentageFinal = 1.0f;
    private bool hasgoneTooFar = false;

    void Start()
    {
        //set things from GameManager
        beatsUntilGoal = GameManagerController.instance.beatsSpawnToGoal_akaSpeed;
        //distance after good when item should be dequed and fade away
        percentageAboveFinal = GameManagerController.instance.percentageGoodFromCenter;
        //fade distance
        fadeDistance = GameManagerController.instance.fadeDistance;
        //choose sprite dending on input method
        GetComponent<SpriteRenderer>().sprite = sprites1[noteType];
        //set time until goal
        timeUntilGoal = beatsUntilGoal * timePerBeat;
        //set original position, moves from there X wise
        originalPos = transform.position;
        //set total final
        totalPercentageFinal = totalPercentageFinal + percentageAboveFinal;
    }

    void Update () {
        //compare current time to birth time
        timeSinceBirth = Time.time - timeAtBirth;
        //depending on how far compared to full time, do the do, percentage of completed
        percentageOfTravel = timeSinceBirth / timeUntilGoal;
        Vector3 currentPos = transform.position;
        transform.position = new Vector3(originalPos.x - (distanceSpawnDestroyer * percentageOfTravel), currentPos.y, currentPos.z);

        if (percentageOfTravel > totalPercentageFinal && !hasgoneTooFar)
        {
            hasgoneTooFar = true;
            GoneTooFar();
        }
    }

    private void GoneTooFar()
    {
        //will deque

        //do fadeout once too far
        StartCoroutine(HasGoneTooFarFade());
    }

    public IEnumerator HasGoneTooFarFade()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float opacity = 1.0f;
        while(true)
        {
            //Should be adjustd to something irrelevant of bpm
            yield return new WaitForEndOfFrame();
            sr.color = new Color(1, 1, 1, opacity);
            //fade depending on how far of distance is made
            opacity = (1 - ((percentageOfTravel - totalPercentageFinal) / fadeDistance));
            //Debug.Log((1 - ((percentageOfTravel - totalPercentageFinal) / fadeDistance)));
            if (opacity <= 0.1f) Destroy(this.gameObject);
        }
    }
}
