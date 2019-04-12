using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarScript : MonoBehaviour {

	private float maxHealth;
	private float currentHealth;
	public float damage;
	private float tmpHealth;
	private Vector3 originalScale;
	private bool takingDamage;

	void Start()
	{
		currentHealth = 100f;
		maxHealth = 100f;
		takingDamage = false;
	}
	void Update()
	{
		if (Input.GetKeyUp (KeyCode.O)) 
		{
			damage = -damage;
		}
		if (Input.GetKeyUp (KeyCode.P) && !takingDamage) 
		{
			takingDamage = true;
			tmpHealth = currentHealth;

			if (currentHealth >= 0) 
			{
				currentHealth -= damage;
				StartCoroutine ("lerpScale");

			}

		}
	}
	IEnumerator lerpScale()
	{
		float time = 0.5f;
		originalScale = transform.localScale;
		Vector3 targetScale = originalScale - new Vector3 (damage/100f,
			0f, 0f);
		float originalTime = time;
		while (time > 0f) 
		{
			time -= Time.deltaTime;
			//originalTime += Time.deltaTime * 5f;
			if (transform.localScale.x > 0f) 
			{
				transform.localScale = Vector3.Lerp (targetScale, originalScale, time / originalTime);
			} 
			yield return 0;
		}
		transform.localScale = targetScale;
		if (transform.localScale.x < 0f) 
		{
			transform.localScale = new Vector3 (0, transform.localScale.y, transform.localScale.z);
		} 
		takingDamage = false;
	}

}
