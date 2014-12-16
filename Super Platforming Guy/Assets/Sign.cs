using UnityEngine;
using System.Collections;

public class Sign : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		transform.Find("Text").GetComponent<TextMesh>().text = transform.Find("Text").GetComponent<TextMesh>().text.Replace("Ω", "\n");
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.name == "Player")
			transform.Find("Text").GetComponent<Renderer>().enabled = true;
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.name == "Player")
			transform.Find("Text").GetComponent<Renderer>().enabled = false;
	}
}
