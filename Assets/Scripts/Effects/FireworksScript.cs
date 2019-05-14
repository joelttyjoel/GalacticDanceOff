using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworksScript : MonoBehaviour {

    [SerializeField]
    private float startSpeed = 5f;

    [SerializeField]
    private float lifetime = 3f;
    private float currentLifeTime = 0;

    private Rigidbody rigid;
	// Use this for initialization


    void Start()
    {
        rigid = transform.GetComponent<Rigidbody>();
        rigid.velocity = new Vector3(0, startSpeed, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        currentLifeTime += Time.deltaTime;

        if (currentLifeTime >= lifetime)
        {
            Destroy(gameObject);
        }
	}
}
