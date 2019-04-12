using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	public float maxHealth;
	public float currentHealth;
	public float damage;
	public Vector3 tmpVect;
	public Vector3 finalVect;
	// Use this for initialization
	void Start () {
		tmpVect = transform.localScale;
		finalVect = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.O)) 
		{
			//take damage
			currentHealth -= damage;
			tmpVect = transform.localScale;
			finalVect = new Vector3(currentHealth/maxHealth,
				transform.localScale.y, transform.localScale.z);
			}
		if (Input.GetKeyUp (KeyCode.P)) 
		{
			//gain hp
			currentHealth += damage;
			tmpVect = transform.localScale;
			finalVect = new Vector3(currentHealth/maxHealth,
				transform.localScale.y, transform.localScale.z);
		}
		transform.localScale = Vector3.Lerp (tmpVect, finalVect, currentHealth/maxHealth);
	}
}
