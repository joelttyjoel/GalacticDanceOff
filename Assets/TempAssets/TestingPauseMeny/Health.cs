using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour 
{
	public float currentHealth;
	public float maxHealth;
	public float dmg;
	public float speed;

	void Update()
	{

		handleBar ();

		if (Input.GetKeyDown (KeyCode.P)) {
			currentHealth += dmg;

		}
		if (Input.GetKeyDown (KeyCode.O)) 
		{
			currentHealth -= dmg;

		}
	}

	private void handleBar()
	{
		if (transform.localScale.y != currentHealth / 100f) 
		{
			Vector3 targetVect = new Vector3 (transform.localScale.x, 
				currentHealth/100f, transform.localScale.z);
			transform.localScale = Vector3.Lerp (transform.localScale, 
				targetVect, Time.deltaTime* 3f);
		}
	}

}
