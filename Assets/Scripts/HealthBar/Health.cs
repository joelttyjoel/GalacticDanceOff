using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour 
{
	private float currentHealth;
	private float maxHealth;

	public Image healthBar;

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
			currentHealth = Mathf.Clamp(GameManagerController.instance.playerHealth, 0f, GameManagerController.instance.maxHealth);
		}
		if (gameObject.CompareTag ("Player 2")) 
		{
			currentHealth = Mathf.Clamp (GameManagerController.instance.playerHealth, 0f, GameManagerController.instance.maxHealth);
		}

		//handleBar (currentHealth);
		FillBar(currentHealth);
	}


	private void FillBar(float currentHealth)
	{
		if (healthBar.fillAmount != currentHealth / maxHealth) 
		{
			healthBar.fillAmount = Mathf.Lerp (healthBar.fillAmount, currentHealth/maxHealth, Time.deltaTime*3f);

		}
	}
}
