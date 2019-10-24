using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVolumeOnStart : MonoBehaviour {
    public VolumeChanger thisOnes;

	// Use this for initialization
	void OnEnable () {
        thisOnes.SetOnStart();
	}
}
