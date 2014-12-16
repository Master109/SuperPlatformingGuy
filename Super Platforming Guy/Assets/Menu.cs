using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = new Vector2(.5f, transform.position.y);
	}
}
