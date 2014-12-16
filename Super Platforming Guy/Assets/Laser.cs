using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
	public float aimSpeed = .01f;
	RaycastHit2D hit;
	public Vector2 shootVec;
	public LayerMask whatBlocksLaser;
	public bool aim;
	public bool canKill = true;
	public float cantKillDuration = -1;
	public float canKillDuration = -1;
	float canKillSwitchTimer;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (GameObject.Find("Player").GetComponent<Player>().timeScale2 == 0)
		{
			if (GetComponent<Rigidbody2D>() != null)
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			return;
		}
		canKillSwitchTimer += Time.fixedDeltaTime;
		if ((canKill && canKillSwitchTimer > canKillDuration && canKillDuration != -1) || (!canKill && canKillSwitchTimer > cantKillDuration && cantKillDuration != -1))
		{
			canKill = !canKill;
			canKillSwitchTimer = 0;
		}
		if (canKill)
			GetComponent<LineRenderer>().SetColors(Color.red, Color.red);
		else
			GetComponent<LineRenderer>().SetColors(Color.green, Color.green);
		Vector2 toTarget = new Vector2();
		if (aim)
			toTarget = GameObject.Find("Player").transform.position - transform.position;
		else
			toTarget = transform.Find("Target").position - transform.position;
		if (!aim)
		{
			transform.Find("Target").RotateAround(transform.position, Vector3.forward, aimSpeed * Mathf.Rad2Deg);
			shootVec = toTarget;
		}
		else if (aimSpeed > 0)
			shootVec = (Vector2) Vector3.RotateTowards(shootVec, toTarget, aimSpeed, 0);
		hit = Physics2D.Raycast(transform.position, shootVec, Mathf.Infinity, whatBlocksLaser);
		if (hit.collider != null)
		{
			GetComponent<LineRenderer>().SetPosition(1, hit.point - (Vector2) transform.position);
			if (hit.collider.gameObject.name == "Player" && canKill)
			{
				Application.LoadLevel(Application.loadedLevel);
				hit.collider.gameObject.SendMessage("PlayDieAudio");
			}
		}
		else
			GetComponent<LineRenderer>().SetPosition(1, (Vector2) transform.position + (shootVec.normalized * 9999999999));
	}
}
