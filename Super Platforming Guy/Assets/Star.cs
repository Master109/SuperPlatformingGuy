using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour
{
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.name == "Player")
		{
			GameObject.Find("Player").GetComponent<Player>().starCollected = true;
			Destroy(gameObject);
		}
	}
}
