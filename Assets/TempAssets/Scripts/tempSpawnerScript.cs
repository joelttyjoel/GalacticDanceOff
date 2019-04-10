using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempSpawnerScript : MonoBehaviour {

    [SerializeField]
    private GameObject beatObjectPrefab;

    [SerializeField]
    private GameObject beatChecker;

    [SerializeField]
    private float bpm = 20;

    private float currentTime = 0.0f;

    private Queue<GameObject> beatQueue = new Queue<GameObject>();

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= 60/bpm
            && beatObjectPrefab)
        {
            currentTime -= 60 / bpm;
            GameObject beat = Instantiate(beatObjectPrefab, transform.position, transform.rotation);
            beatQueue.Enqueue(beat.gameObject);

            Rigidbody rig = beat.GetComponent<Rigidbody>();
            rig.velocity = new Vector3(Mathf.Abs(transform.position.x - beatChecker.transform.position.x) * 60 / bpm / (2), 0, 0);   //divided by 2 temporary

            //Destroy(beat, 10.0f);
        }
	}


    public GameObject GetFirstInQueue()
    {
        if (beatQueue.Count > 0)
        {
            return beatQueue.Peek();
        }

        else
        {
            return null;
        }
    }

    
    public void DeleteFirstInQueue()
    {
        if (beatQueue.Count > 0)
        {
            Destroy(beatQueue.Dequeue());
        }
    }
}
