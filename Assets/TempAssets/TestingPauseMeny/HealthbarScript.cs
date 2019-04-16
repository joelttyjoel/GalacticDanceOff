using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarScript : MonoBehaviour {

	private float maxHealth;
	private float currentHealth;
	public float dmg;
	private Vector3 initialScale;
	private bool takingDamage;
	[Range (0, 10)]
	public float decreaseSpeed;

	void Start()
	{
		currentHealth = 100f;
		maxHealth = 100f;
		takingDamage = false;
	}
	void Update()
	{
		//taking dmg
		if (Input.GetKeyUp (KeyCode.P) && !takingDamage) 
		{
			takingDamage = true;

			if (currentHealth >= 0) 
			{
				
				StartCoroutine (lerpScale(decreaseSpeed, dmg));
			}

		}
	}


	IEnumerator lerpScale(float time, float damage)
	{
		currentHealth -= damage;

		//slower if taking lethal damage. time = time for health to go down
		if (currentHealth <= 0) 
		{
			time *= 1.75f;
		}

		initialScale = transform.localScale;
		//transforming damage to decimal
		Vector3 targetScale = initialScale - new Vector3 (damage/100f,
			0f, 0f);
		float originalTime = time;

		//smooth healthloss
		while (time > 0f) 
		{
			time -= Time.deltaTime;
			originalTime += Time.deltaTime * 2.5f;
			//setting scale
			if (transform.localScale.x > 0f) 
			{
				transform.localScale = Vector3.Lerp (targetScale, initialScale, time / originalTime);
			}
			if (transform.localScale.x < 0f) 
			{
				transform.localScale = new Vector3 (0, transform.localScale.y, transform.localScale.z);
			} 
			yield return 0;
		}

		transform.localScale = targetScale;
		takingDamage = false;
	}

}
