using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscRotate : MonoBehaviour {

	public GameObject[] characters;
	private GameObject disc;



	// Use this for initialization
	void Start () {
		disc = this.gameObject;
		Vector3 center = this.transform.position;
		for (int i = 0; i < characters.Length; i++) 
		{
			Vector3 pos = RandomCircle(center, 3f, i);
			GameObject GO = Instantiate (characters [i], pos, Quaternion.identity);
		}
	}
	
	Vector3 RandomCircle (Vector3 center, float radius, int count)
	{
		float ang = 360/characters.Length * count;
		Vector3 pos;
		pos.z = center.z + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
		pos.x = center.x + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		pos.y = 0;
		return pos;
	}
}
