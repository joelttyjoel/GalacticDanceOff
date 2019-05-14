using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour {
    [SerializeField]
    [Header("Sprites for WASD")]
    List<Sprite> spritesWASD;
    [SerializeField]
    [Header("Sprites for XBOX")]
    List<Sprite> spritesXBOX;
    [SerializeField]
    [Header("Sprites for PS")]
    List<Sprite> spritesPS;

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

    //new shit for going along board instead of just along axis
    [HideInInspector]
    public Vector3 vectorStartToGoal;

    //nd fade
    private float fadeDistance = 0.5f;
    private float percentageAboveFinal = 0.1f;
    private float totalPercentageFinal = 1.0f;

    //start fade
    private float startFadeDistance = 0.5f;

    public float percentageOfTravel = 0f;
    private float timeUntilGoal;
    private Vector3 originalPos;
    private bool hasgoneTooFar = false;

    void Start()
    {
        //distance after good when item should be dequed and fade away
        percentageAboveFinal = GameManagerController.instance.percentagePerfectFromCenter / 2;
        //fade distance
        fadeDistance = GameManagerController.instance.fadeDistance;
        //start fade distance
        startFadeDistance = GameManagerController.instance.startFadeDistance;
        //choose sprite dending on input method
        if(SceneSwitchereController.instance.keyBoard)
            GetComponent<SpriteRenderer>().sprite = spritesWASD[noteType];
        if (SceneSwitchereController.instance.xBox)
            GetComponent<SpriteRenderer>().sprite = spritesXBOX[noteType];
        if (SceneSwitchereController.instance.Ps4)
            GetComponent<SpriteRenderer>().sprite = spritesPS[noteType];
        //set time until goal
        timeUntilGoal = GameManagerController.instance.beatsSpawnToGoal * timePerBeat;
        //set original position, moves from there X wise
        originalPos = transform.position;
        //set total final
        totalPercentageFinal = totalPercentageFinal + percentageAboveFinal;

        //start fade in 
        StartCoroutine(OnSpawnFade());
    }

    void Update () {
        //compare current time to birth time
        timeSinceBirth = Time.time - timeAtBirth;
        //depending on how far compared to full time, do the do, percentage of completed
        percentageOfTravel = timeSinceBirth / timeUntilGoal;
        Vector3 currentPos = transform.position;

        //add vector * percentage (all way) to original position, boom
        transform.position = originalPos + (vectorStartToGoal * percentageOfTravel);

        if (percentageOfTravel > totalPercentageFinal && !hasgoneTooFar)
        {
            hasgoneTooFar = true;
            GoneTooFar();
        }
    }

    private void GoneTooFar()
    {
        //take damage to left player, player, if isn't restarting
        if (!GameManagerController.instance.isRestarting)
        {
            GameManagerController.instance.takeDamage(true);

            //note miss sounds
            AudioController.instance.PlayNoteSound(0f);
        }
        //will deque
        noteChecker.GetComponent<NoteChecker>().DequeueNote();
        //do fadeout once too far
        StartCoroutine(HasGoneTooFarFade());
    }

    public void HasBeenHit()
    {
        Destroy(this.gameObject);
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

    public IEnumerator OnSpawnFade()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float startColor = 0.5f;
        //set start
        sr.color = new Color(startColor, startColor, startColor, 1f);
        while (true)
        {
            //wait for 1 frame
            yield return new WaitForEndOfFrame();
            //set color to this, wont be set above 1 due to chck at end
            sr.color = new Color(startColor, startColor, startColor, 1f);
            //fade depending on how far of distance is made
            startColor = 0.5f + (percentageOfTravel / startFadeDistance);
            //if startcolor >= 1f, set all to full white then end
            if (startColor >= 1f)
            {
                sr.color = new Color(1f, 1f, 1f, 1f);
                break;
            }
        }
    }
}
