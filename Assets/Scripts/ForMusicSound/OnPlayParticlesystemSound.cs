using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayParticlesystemSound : MonoBehaviour {
    
    private bool lastThing;

    private void Start()
    {
        lastThing = false;
    }

    // Update is called once per frame
    void Update () {

        if (GetComponent<ParticleSystem>().IsAlive() == true && lastThing == false)
        {
            // do your jump logic
            Debug.Log("PLAY IT");
            AudioController.instance.PlayFireWork();
            lastThing = true;
        }
        else if (lastThing == true && GetComponent<ParticleSystem>().IsAlive() == false)
        {
            lastThing = false;
        }
    }
}
