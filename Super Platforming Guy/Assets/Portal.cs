using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.name == "Player")
		{
			if (PlayerPrefs.GetFloat("Level " + (Application.loadedLevel) + " Time", Mathf.Infinity) == Mathf.Infinity)
				PlayerPrefs.SetInt("Last Level Unlocked", PlayerPrefs.GetInt("Last Level Unlocked", GameObject.Find("Player").GetComponent<Player>().lastLevelUnlockedInit) + 2);
			if (other.gameObject.GetComponent<Player>().displayTime < PlayerPrefs.GetFloat("Level " + (Application.loadedLevel) + " Time", Mathf.Infinity))
				PlayerPrefs.SetFloat("Level " + (Application.loadedLevel) + " Time", other.gameObject.GetComponent<Player>().displayTime);
			if (Application.loadedLevel + 1 < Application.levelCount)
				Application.LoadLevel(Application.loadedLevel + 1);
			else
				Application.LoadLevel(0);
			if (GameObject.Find("Player").GetComponent<Player>().starCollected && PlayerPrefs.GetInt("Level " + (Application.loadedLevel) + " Star", 0) == 0)
			{
				PlayerPrefs.SetInt("Last Level Unlocked", PlayerPrefs.GetInt("Last Level Unlocked", GameObject.Find("Player").GetComponent<Player>().lastLevelUnlockedInit) + 1);
				PlayerPrefs.SetInt("Level " + (Application.loadedLevel) + " Star", 1);
				PlayerPrefs.SetInt("Perk Points", PlayerPrefs.GetInt("Perk Points", 0) + 1);
			}
		}
	}
}
