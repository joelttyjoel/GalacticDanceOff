using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarScript : MonoBehaviour {
	
	public float dmg;

	private Vector3 initialScale;
	public bool takingDamage;
	private bool stopCoroutine;

	void Start()
	{
		takingDamage = false;
	}

	void Update()
	{
		if (takingDamage) 
		{
			takingDamage = false;
			stopCoroutine = true;
			StartCoroutine(lerpScale (dmg));
		}

		if (Input.GetKeyDown (KeyCode.P)) 
		{
			
			takingDamage = true;
		}
	}

	IEnumerator lerpScale(float damage)
	{
		initialScale = transform.localScale;

		Vector3 targetScale = initialScale - new Vector3 (damage / 100f, 0f, 0f);
		float time = 1f - (initialScale.x - 1f);
		float originalTime = time;
		stopCoroutine = false;

		while(time > 0f)
		{
			time -= Time.deltaTime;
			originalTime += Time.deltaTime * 2.5f;
			if (stopCoroutine) 
			{
				targetScale = transform.localScale;
				break;
			}

			if (transform.localScale.x > 0) 
			{
				transform.localScale = Vector3.Lerp (targetScale, initialScale, time / originalTime);
			}
			if(transform.localScale.x < 0)
			{
				transform.localScale = new Vector3 (0, transform.localScale.y, transform.localScale.z);
			}

			yield return 0;

		}
		if(transform.localScale.x < 0)
		{
			targetScale = new Vector3 (0, transform.localScale.y, transform.localScale.z);
		}
		transform.localScale = targetScale;
	}



}
