using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingText : MonoBehaviour {

	private bool mover;
	public GameObject introSign;
	// Use this for initialization
	void Start () {
		mover = true;
	}

	IEnumerator TextScroller()
	{
		float timer = 30f;
		while(timer > 0)
		{
			timer -= Time.deltaTime;
			Vector3 pos = this.gameObject.transform.position;
			pos.z += Time.deltaTime * 4f;
			pos.y += Time.deltaTime;
			this.gameObject.transform.position = pos;
			Quaternion target = Quaternion.Euler(0, 0, 0);
			transform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * 0.01f);


			yield return 0;
		}
		Instantiate (introSign);
	}
	
	// Update is called once per frame
	void Update () {
		if (mover) 
		{
			mover = false;

			StartCoroutine(TextScroller());
		}
			
	}
}
