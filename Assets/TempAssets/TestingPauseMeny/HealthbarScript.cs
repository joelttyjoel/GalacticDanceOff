using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarScript : MonoBehaviour {

	private float maxHealth;
	private float currentHealth;
	private float damage;
	private Vector3 initialScale;
	private Vector3 finalScale;
	private float timeScale = 0.5f;
	void Start()
	{
		maxHealth = 100f;
		currentHealth = 100f;
		initialScale = gameObject.transform.localScale;
		damage = 5;
		finalScale = gameObject.transform.localScale;

	}
		
	IEnumerator LerpHealth()
	{
		float progress = 0f;

		while (progress <= 5f) 
		{
			transform.localScale = Vector3.Lerp (initialScale, finalScale, Time.deltaTime);
			progress += Time.deltaTime;
			yield return null;
		}
		transform.localScale = finalScale;
	}
	//progress = mathf.lerp prograessbar,
	void Update()
	{
		if (Input.GetKeyUp (KeyCode.Space)) 
		{
			currentHealth -= damage;
			finalScale.x = 0.5f;
			StartCoroutine ("LerpHealth");
		}

	}
}
