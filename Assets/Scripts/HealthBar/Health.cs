using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour 
{
	private float currentHealth;
	private float maxHealth;


	void Start()
	{
		if (gameObject.CompareTag ("Player 1")) {
			currentHealth = GameManagerController.instance.playerHealth;
		}
		if (gameObject.CompareTag ("Player 2")) 
		{
			currentHealth = GameManagerController.instance.AIHealth;
		}
		maxHealth = GameManagerController.instance.maxHealth;
	}

	void Update()
	{
		if (gameObject.CompareTag ("Player 1")) {
			currentHealth = GameManagerController.instance.playerHealth;
		}
		if (gameObject.CompareTag ("Player 2")) 
		{
			currentHealth = GameManagerController.instance.AIHealth;
		}

		handleBar (currentHealth);
	}

	//makes EGOBar always move towards its current EGO
	private void handleBar(float currentHealth)
	{
		if (transform.localScale.y != currentHealth / maxHealth) 
		{
			Vector3 targetVect = new Vector3 (transform.localScale.x, 
				Mathf.Clamp(currentHealth / maxHealth, 0, maxHealth/maxHealth), transform.localScale.z);
	
			transform.localScale = Vector3.Lerp (transform.localScale, 
					targetVect, Time.deltaTime* 3f);
			if (transform.localScale.y <= 0.01f) 
			{
				targetVect.y = 0;
				transform.localScale = targetVect;
			}
		}
	}

}
