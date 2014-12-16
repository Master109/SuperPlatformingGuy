using UnityEngine;
using System.Collections;

public class MoveTile : MonoBehaviour
{
	public int forwardSpeed;
	bool movingForward;
	Vector2 initLoc;
	bool xMovement;

	// Use this for initialization
	void Start ()
	{
		initLoc = transform.position;
		xMovement = Mathf.RoundToInt(transform.rotation.eulerAngles.z) == 0 || Mathf.RoundToInt(transform.rotation.eulerAngles.z) == 180;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (GameObject.Find("Player").GetComponent<Player>().timeScale2 == 0)
			return;
		if (movingForward)
		{
			if (xMovement)
				GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.lossyScale.x * forwardSpeed, 0));
			else
				GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -transform.lossyScale.x * forwardSpeed));
		}
		else
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		if (xMovement)
			transform.position = new Vector2(transform.position.x, initLoc.y);
		else
			transform.position = new Vector2(initLoc.x, transform.position.y);
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.name == "Player")
			movingForward = true;
	}

	void OnCollisionStay2D (Collision2D coll)
	{
		if (coll.gameObject.name == "Player")
			movingForward = true;
	}

	void OnCollisionExit2D (Collision2D coll)
	{
		if (coll.gameObject.name == "Player")
			movingForward = false;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.name == "Player")
		{
			Application.LoadLevel(Application.loadedLevel);
			other.gameObject.SendMessage("PlayDieAudio");
		}
	}
}
