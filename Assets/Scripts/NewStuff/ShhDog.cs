using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShhDog : MonoBehaviour {
    

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.J))
        {
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
        }
	}
}
