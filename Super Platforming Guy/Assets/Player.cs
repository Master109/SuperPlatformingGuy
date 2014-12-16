using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public float maxSpeed = 10f;
	float speed = 20f;
	bool facingRight = true;
	float move = 0f;
	bool grounded = false;
	public float groundCheckRadius = 0.2f;
	public LayerMask whatIsGround;
	public int jumpForce = 500;
	public Transform wallCheck1;
	public Transform wallCheck2;
	public float wallCheckRadius = 0.2f;
	Animator anim;
	public float groundHoverAmount = .1f;
	public GUISkin[] guiSkins;
	float numOfDecimalPlaces = 10f;
	public int pushOfWallForce = 500;
	float extraXVel = 0;
	bool onWall = false;
	public float displayTime;
	public Material[] skyboxs;
	string gameUpdateID = "1.00 -- Added perk shop";
	public bool starCollected;
	ArrayList starPics = new ArrayList();
	public GameObject starPic;
	public int lastLevelUnlockedInit = 2;
	public AudioSource[] audios;
	public int[] walkAudios;
	int currentWalkAudio = 0;
	float playWalkAudioTimer;
	public float walkAudioPlayRate;
	public int[] musicAudios;
	int currentMusicAudio = 0;
	public int musicDelay;
	public int[] jumpAudios;
	public int[] dieAudios;
	public bool canControl;
	public string[] actions;
	public int[] timesToRepeatAction;
	ArrayList actionsWithRepeats;
	float xAxis;
	float jumpAxis;
	int currentAction;
	public float introDuration = 1;
	float introTimer;
	float twirlEffectAngleInit;
	string[] menuOptions;
	int selectedIndex = 0;
	bool beingDelayed;
	float menuVertical2Axis = 0;
	public float menuVertical2AxisGravity = -1;
	public float menuVertical2AxisSensitivity = 0.0075f;
	public float timeScale2 = 1;
	ArrayList rigidbodyVelocities = new ArrayList();
	ArrayList rigidbodyPositions = new ArrayList();
	ArrayList rigidbodies = new ArrayList();
	public float actionsPerSecond;
	float actionTimer;
	float pXAxis;
	public int extraMenuOptions = 5;
	public int[] perkCosts;
	public int[] perkMaxCosts;
	
	void Awake ()
	{
		twirlEffectAngleInit = Camera.main.GetComponent<VortexEffect>().angle;
		currentMusicAudio = Mathf.RoundToInt(Random.Range(0, musicAudios.Length));
		DontDestroyOnLoad(GameObject.Find("ReadSceneNames"));
		anim = GetComponent<Animator>();
		if (GameObject.Find("Player") == null)
		{
			DontDestroyOnLoad(gameObject);
			name = "Player";
		}
		if (gameUpdateID != "" && PlayerPrefs.GetInt(gameUpdateID, 0) == 0)
		{
			PlayerPrefs.DeleteAll();
			PlayerPrefs.SetInt(gameUpdateID, 1);
		}
		menuOptions = new string[Application.levelCount - 1 + extraMenuOptions];
		for (int i = 0; i < menuOptions.Length; i ++)
		{
			if (i == 0)
				menuOptions[i] = "Menu";
			else if (i == menuOptions.Length - 4)
				menuOptions[i] = "Sprint Speed";
			else if (i == menuOptions.Length - 3)
				menuOptions[i] = "Decrease Gravity";
			else if (i == menuOptions.Length - 2)
				menuOptions[i] = "Mute";
			else if (i == menuOptions.Length - 1)
				menuOptions[i] = "Clear data";
			else
				menuOptions[i] = GameObject.Find("ReadSceneNames").GetComponent<ReadSceneNames>().scenes[i];
		}
		if (menuVertical2AxisSensitivity == -1)
			menuVertical2AxisSensitivity = menuVertical2AxisGravity;
		if (menuVertical2AxisGravity == -1)
			menuVertical2AxisGravity = menuVertical2AxisSensitivity;
	}
	
	// Use this for initialization
	void Start ()
	{
		introTimer = introDuration;
		Camera.main.GetComponent<VortexEffect>().angle = twirlEffectAngleInit;
		currentAction = 0;
		actionsWithRepeats = new ArrayList();
		for (int i = 0; i < actions.Length; i ++)
		{
			string s = (string) actions[i];
			for (int i2 = 0; i2 < timesToRepeatAction[i]; i2 ++)
				actionsWithRepeats.Add(s);
		}
		if (GameObject.Find("Player2") != null)
		{
			GameObject.Find("Player").transform.position = GameObject.Find("Player2").transform.position;
			GameObject.Find("Player").transform.localScale = Vector3.one;
			facingRight = true;
			GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			Destroy(GameObject.Find("Player2"));
		}
		displayTime = 0;
		if (Application.loadedLevelName.Contains("1-"))
		{
			transform.Find("Camera").GetComponent<Skybox>().material = skyboxs[0];
			Camera.main.GetComponent<FastBloom>().enabled = true;
			Camera.main.GetComponent<ContrastStretchEffect>().enabled = false;
			Camera.main.GetComponent<ContrastEnhance>().enabled = false;
		}
		else if (Application.loadedLevelName.Contains("2-"))
		{
			transform.Find("Camera").GetComponent<Skybox>().material = skyboxs[1];
			Camera.main.GetComponent<FastBloom>().enabled = false;
			Camera.main.GetComponent<ContrastStretchEffect>().enabled = true;
			Camera.main.GetComponent<ContrastEnhance>().enabled = true;
		}
		else if (Application.loadedLevelName.Contains("3-"))
		{
			transform.Find("Camera").GetComponent<Skybox>().material = skyboxs[2];
			Camera.main.GetComponent<FastBloom>().enabled = false;
			Camera.main.GetComponent<ContrastStretchEffect>().enabled = false;
			Camera.main.GetComponent<ContrastEnhance>().enabled = false;
		}
		starCollected = false;
		
		rigidbodyVelocities.Clear ();
		rigidbodyPositions.Clear ();
		rigidbodies.Clear ();
		foreach (Rigidbody2D r in Rigidbody2D.FindObjectsOfType<Rigidbody2D>())
		{
			rigidbodyVelocities.Add (r.velocity);
			rigidbodyPositions.Add (r.position);
			rigidbodies.Add (r);
		}
		//jumpAxis = 1;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (timeScale2 == 0)
		{
			foreach (Rigidbody2D r in Rigidbody2D.FindObjectsOfType<Rigidbody2D>())
			{
				r.velocity = Vector2.zero;
				r.position = (Vector2) rigidbodyPositions[rigidbodies.IndexOf(r)];
			}
			return;
		}
		introTimer -= Time.fixedDeltaTime * timeScale2;
		if (introTimer >= 0)
			Camera.main.GetComponent<VortexEffect>().angle = twirlEffectAngleInit / introDuration * introTimer;
		else
			Camera.main.GetComponent<VortexEffect>().angle = 0;
		playWalkAudioTimer += Time.fixedDeltaTime * timeScale2;
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("GroundCheck"))
		{
			if (go.transform.IsChildOf(transform))
			{
				grounded = Physics2D.OverlapCircle(go.transform.position, groundCheckRadius, whatIsGround);
				if (grounded)
				{
					transform.position = new Vector2(transform.position.x, transform.position.y + groundHoverAmount);
					if (Physics2D.OverlapCircle(go.transform.position, groundCheckRadius, whatIsGround).gameObject.name.Contains("Grass") && Mathf.Abs(move) > 0 && playWalkAudioTimer > walkAudioPlayRate / Mathf.Abs(move))
					{
						playWalkAudioTimer = 0;
						audios[walkAudios[currentWalkAudio]].Play();
						currentWalkAudio ++;
						if (currentWalkAudio >= walkAudios.Length)
							currentWalkAudio = 0;
					}
					break;
				}
			}
		}
		if (Input.GetKey(KeyCode.Z))
			GetComponent<Rigidbody2D>().gravityScale = 1 - PlayerPrefs.GetFloat("Decrease Gravity", 0);
		else
			GetComponent<Rigidbody2D>().gravityScale = 1;
		if (Input.GetKey(KeyCode.LeftShift))
			speed = maxSpeed + PlayerPrefs.GetFloat("Sprint Speed", 0);
		else
			speed = maxSpeed;
		move = Input.GetAxis("Horizontal") * speed;
		if (Input.GetJoystickNames().Length == 0)
		{
			if (!onWall && ((move > 0 && !facingRight) || (move < 0 && facingRight)))
				Flip ();
		}
		else
		{
			if (move != 0 && Input.GetAxisRaw("Horizontal") == -transform.localScale.x)
				Flip ();
		}
		if (!canControl)
			move = xAxis * speed;
		if (move != 0 && Physics2D.OverlapCircle(wallCheck1.position, wallCheckRadius, whatIsGround) && !Physics2D.OverlapCircle(wallCheck2.position, wallCheckRadius, whatIsGround))
			transform.position = new Vector2(transform.position.x, transform.position.y + Vector2.Distance(wallCheck1.position, wallCheck2.position));
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("WallCheck"))
		{
			if (go.transform.IsChildOf(transform) && Physics2D.OverlapCircle(go.transform.position, wallCheckRadius, whatIsGround) && !Physics2D.OverlapCircle(go.transform.position, wallCheckRadius, whatIsGround).gameObject.name.Contains("(Wall Jump)"))
			{
				extraXVel = 0;
				move = 0;
				break;
			}
		}
		onWall = false;
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("WallCheck"))
		{
			if (go.transform.IsChildOf(transform) && Physics2D.OverlapCircle(go.transform.position, wallCheckRadius, whatIsGround) && Physics2D.OverlapCircle(go.transform.position, wallCheckRadius, whatIsGround).gameObject.name.Contains("(Wall Jump)"))
			{
				onWall = true;
				extraXVel = 0;
				int x = 0;
				if (transform.position.x > go.transform.position.x)
					x = 1;
				else
					x = -1;
				if ((move > 0 && x < 0) || (move < 0 && x > 0))
					move = 0;
				if ((Input.GetAxisRaw("Horizontal") == x || xAxis == x || (Input.GetJoystickNames().Length > 0 && Mathf.Sign(Input.GetAxisRaw("Horizontal")) != Mathf.Sign(pXAxis) && Mathf.RoundToInt(pXAxis) == -x)) && (Input.GetAxisRaw("Jump") == 1 || jumpAxis == 1))
				{
					GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
					GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce);
					int r = Mathf.RoundToInt(Random.Range(0, jumpAudios.Length));
					audios[jumpAudios[r]].Play();
					extraXVel = x * pushOfWallForce;
					grounded = false;
					onWall = false;
					break;
				}
			}
		}
		extraXVel *= GetComponent<Rigidbody2D>().drag;
		if (move != 0)
			GetComponent<Rigidbody2D>().velocity = new Vector2(move + extraXVel, GetComponent<Rigidbody2D>().velocity.y);
		anim.SetFloat("Speed", Mathf.Abs(move));
		anim.speed = anim.GetFloat("Speed");
		if (Input.GetKeyDown(KeyCode.R))
			Application.LoadLevel(Application.loadedLevel);
		jumpAxis = 0;
	}
	
	void Update ()
	{
		if (!canControl)
		{
			actionTimer += Time.deltaTime;
			Input.ResetInputAxes();
			xAxis = 0;
			jumpAxis = 0;
			if (actionsWithRepeats.Count > currentAction)
			{
				string s = (string) actionsWithRepeats[currentAction];
				if (s.Contains("Right"))
					xAxis = .9f;
				else if (s.Contains("Left"))
					xAxis = -.9f;
				if (s.Contains("Jump"))
				{
					jumpAxis = 1;
					if (grounded)
					{
						for (int i = currentAction + 1; i < actionsWithRepeats.Count; i ++)
						{
							s = (string) actionsWithRepeats[i];
							if (!s.Contains("Jump"))
							{
								currentAction = i;
								actionTimer = 0;
								break;
							}
						}
					}
				}
				if (actionTimer > 1 / actionsPerSecond)
				{
					actionTimer = 0;
					currentAction ++;
				}
			}
		}
		
		if (Input.GetAxisRaw("MenuVertical2") != 0 || Input.GetAxisRaw("MenuVertical") != 0)
		{
			if (Mathf.Sign(menuVertical2Axis) != Mathf.Sign(Input.GetAxisRaw("MenuVertical2") + Input.GetAxisRaw("MenuVertical")))
				menuVertical2Axis = 0;
			menuVertical2Axis += (Input.GetAxisRaw("MenuVertical2") + Input.GetAxisRaw("MenuVertical")) * menuVertical2AxisSensitivity;
		}
		else
		{
			if (menuVertical2Axis > menuVertical2AxisGravity)
				menuVertical2Axis -= menuVertical2AxisGravity;
			else if (menuVertical2Axis < -menuVertical2AxisGravity)
				menuVertical2Axis += menuVertical2AxisGravity;
			else
				menuVertical2Axis = 0;
		}
		if (menuVertical2Axis > 1)
			menuVertical2Axis = 1;
		else if (menuVertical2Axis < -1)
			menuVertical2Axis = -1;
		
		Vector2 menuDirection = new Vector2(0,  + menuVertical2Axis);
		if (menuDirection.magnitude > 0 && timeScale2 == 0 && !beingDelayed)
			selectedIndex = MenuSelection(menuOptions, selectedIndex, menuDirection, .3f - menuDirection.magnitude);
		if (Input.GetButtonDown("MenuOk") || Input.GetButtonDown("MenuOk2"))
			HandleSelection();
		if (timeScale2 == 0)
			return;
		displayTime += Time.deltaTime * timeScale2;
		if (grounded && (Input.GetButtonDown("Jump") || jumpAxis == 1))
		{
			GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce);
			int r = Mathf.RoundToInt(Random.Range(0, jumpAudios.Length));
			audios[jumpAudios[r]].Play();
			grounded = false;
		}
		for (int i = 0; i < musicAudios.Length; i ++)
			if (audios[musicAudios[i]].isPlaying || AudioListener.pause)
				return;
		audios[musicAudios[currentMusicAudio]].PlayDelayed((ulong) musicDelay);
		currentMusicAudio ++;
		if (currentMusicAudio >= musicAudios.Length)
			currentMusicAudio = 0;
		StartCoroutine(UpdateInput ());
	}
	
	IEnumerator UpdateInput ()
	{
		yield return new WaitForFixedUpdate();
		pXAxis = Input.GetAxis("Horizontal");
	}
	
	void Flip ()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	void OnGUI ()
	{
		GUI.skin = guiSkins[1];
		if (timeScale2 == 1)
		{
			if (Application.loadedLevelName.Contains("1-"))
				GUI.skin = guiSkins[0];
			GUI.Label(new Rect(0, 25, Screen.width, 50), "" + Mathf.Round(displayTime * numOfDecimalPlaces) / numOfDecimalPlaces);
			if (GUI.Button(new Rect(0, 0, 100, 25), "Menu"))
			{
				selectedIndex = 0;
				HandleSelection();
			}
		}
		else
		{
			if (Application.loadedLevelName.Contains("1-"))
				GUI.skin = guiSkins[2];
			else if (Application.loadedLevelName.Contains("2-"))
				GUI.skin = guiSkins[3];
			GUI.SetNextControlName("Menu");
			if (GUI.Button(new Rect(0, 0, 100, 25), "Resume"))
			{
				selectedIndex = 0;
				HandleSelection();
				return;
			}
			int x = -1;
			int minY = 1;
			int y = minY;
			for (int i = 0; i < starPics.Count; i ++)
				Destroy((GameObject) starPics[i]);
			starPics.Clear ();
			for (int i = 1; i < Application.levelCount; i ++)
			{
				string levelName = GameObject.Find("ReadSceneNames").GetComponent<ReadSceneNames>().scenes[i];
				y ++;
				if (levelName.Substring(levelName.IndexOf("-") + 1) != "B" && int.Parse(levelName.Substring(levelName.IndexOf("-") + 1)) == minY)
				{
					y = minY;
					x ++;
				}
				if (PlayerPrefs.GetInt("Last Level Unlocked", lastLevelUnlockedInit) >= i)
				{
					GUI.SetNextControlName(levelName);
					if (GUI.Button(new Rect(250 * x, y * 25 + 40, 100, 25), levelName))
					{
						selectedIndex = i;
						HandleSelection();
						return;
					}
					Vector2 createLoc = Camera.main.ScreenToViewportPoint(new Vector2(250 * x + 100, y * 25 + 60));
					GameObject go = (GameObject) Instantiate(starPic, new Vector2(createLoc.x, 1 - createLoc.y), Quaternion.identity);
					if (PlayerPrefs.GetInt("Level " + i + " Star", 0) == 1)
						go.GetComponent<GUITexture>().color = new Color(.5f, .5f, .5f, .5f);
					go.GetComponent<GUITexture>().enabled = true;
					starPics.Add (go);
					if (PlayerPrefs.GetFloat("Level " + i + " Time", Mathf.Infinity) == Mathf.Infinity)
						GUI.Label(new Rect(250 * x + 116, y * 25 + 40, 100, 25), "Time: Not beaten");
					else
						GUI.Label(new Rect(250 * x + 116, y * 25 + 40, 100, 25), "Time: " + PlayerPrefs.GetFloat("Level " + i + " Time", Mathf.Infinity));
				}
				else
				{
					GUI.Button(new Rect(250 * x, y * 25 + 40, 100, 25), "Locked");
				}
			}
			GUI.SetNextControlName("Clear data");
			if (GUI.Button(new Rect(0, Screen.height - 25, 100, 25), "Clear Data"))
			{
				selectedIndex = menuOptions.Length - 1;
				HandleSelection();
			}
			else
			{
				GUI.SetNextControlName("Mute");
				if (AudioListener.volume == 1 && GUI.Button(new Rect(0, Screen.height - 50, 100, 25), "Mute"))
				{
					selectedIndex = menuOptions.Length - 2;
					HandleSelection();
				}
				else if (AudioListener.volume == 0 && GUI.Button(new Rect(0, Screen.height - 50, 100, 25), "Unmute"))
				{
					selectedIndex = menuOptions.Length - 2;
					HandleSelection();
				}
			}
			GUI.Box(new Rect(0, Screen.height - 350, 200, 175), "Perk Shop");
			GUI.Label(new Rect(0, Screen.height - 325, 400, 100), "Perk Points: " + PlayerPrefs.GetInt("Perk Points", 0));
			GUI.SetNextControlName("Decrease Gravity");
			if (GUI.Button(new Rect(100, Screen.height - 300, 100, 25), "Decrease Gravity"))
			{
				selectedIndex = menuOptions.Length - 3;
				HandleSelection();
			}
			GUI.SetNextControlName("Sprint Speed");
			if (GUI.Button(new Rect(0, Screen.height - 300, 100, 25), "Sprint Speed"))
			{
				selectedIndex = menuOptions.Length - 4;
				HandleSelection();
			}
			GUI.Label(new Rect(0, Screen.height - 275, 400, 100), "Costs");
			string text = "" + PlayerPrefs.GetInt("Decrease Gravity Cost", perkCosts[1]);
			if (PlayerPrefs.GetInt("Decrease Gravity Cost") > perkMaxCosts[1])
				text = "MAXED OUT";
			GUI.Label(new Rect(100, Screen.height - 250, 400, 100), "" + text);
			text = "" + PlayerPrefs.GetInt("Sprint Speed Cost", perkCosts[0]);
			if (PlayerPrefs.GetInt("Sprint Speed Cost") > perkMaxCosts[0])
				text = "MAXED OUT";
			GUI.Label(new Rect(0, Screen.height - 250, 400, 100), "" + text);
			GUI.Label(new Rect(0, Screen.height - 225, 400, 100), "How to use (if owned)");
			text = "";
			if (PlayerPrefs.GetInt("Sprint Speed Cost") > perkCosts[0])
				text = "Hold left shift";
			GUI.Label(new Rect(0, Screen.height - 200, 400, 100), "" + text);
			text = "";
			if (PlayerPrefs.GetInt("Decrease Gravity Cost") > perkCosts[1])
				text = "Hold Z";
			GUI.Label(new Rect(100, Screen.height - 200, 400, 100), "" + text);
		}
		GUI.FocusControl (menuOptions[selectedIndex]);
	}
	
	int MenuSelection (string[] menuItems, int selectedItem, Vector2 direction, float delay)
	{
		StartCoroutine(Delay (delay));
		if (direction.y < 0)
		{
			if (selectedItem == 0)
				selectedItem = menuItems.Length - 1;
			else
			{
				selectedItem --;
				if (PlayerPrefs.GetInt("Last Level Unlocked", lastLevelUnlockedInit) < selectedItem && selectedItem < menuItems.Length - 4)
					selectedItem = PlayerPrefs.GetInt("Last Level Unlocked", lastLevelUnlockedInit);
			}
		}
		else if (direction.y > 0)
		{
			if (selectedItem == menuItems.Length - 1)
				selectedItem = 0;
			else
			{
				selectedItem ++;
				if (PlayerPrefs.GetInt("Last Level Unlocked", lastLevelUnlockedInit) < selectedItem && selectedItem < menuItems.Length - 4)
					selectedItem = menuItems.Length - 4;
			}
		}
		return selectedItem;
	}
	
	void HandleSelection()
	{
		GUI.FocusControl (menuOptions[selectedIndex]);
		for (int i = 0; i < menuOptions.Length; i ++)
		{
			if (selectedIndex == i)
			{
				if (i == 0)
				{
					if (timeScale2 == 0)
					{
						foreach (Rigidbody2D r in Rigidbody2D.FindObjectsOfType<Rigidbody2D>())
							r.velocity = (Vector2) rigidbodyVelocities[rigidbodies.IndexOf(r)];
						foreach (GameObject go in starPics)
							go.GetComponent<GUITexture>().enabled = false;
					}
					else
					{
						rigidbodyVelocities.Clear ();
						rigidbodyPositions.Clear ();
						rigidbodies.Clear ();
						foreach (Rigidbody2D r in Rigidbody2D.FindObjectsOfType<Rigidbody2D>())
						{
							rigidbodyVelocities.Add (r.velocity);
							rigidbodyPositions.Add (r.position);
							rigidbodies.Add (r);
						}
					}
					if (!Input.GetKey(KeyCode.Space))
						timeScale2 = 1 - timeScale2;
				}
				else if (i == menuOptions.Length - 4)
				{
					if (PlayerPrefs.GetInt("Perk Points", 0) >= PlayerPrefs.GetInt("Sprint Speed Cost", perkCosts[0]) && PlayerPrefs.GetInt("Sprint Speed Cost", perkCosts[0]) <= perkMaxCosts[0])
					{
						PlayerPrefs.SetInt("Perk Points", PlayerPrefs.GetInt("Perk Points", 0) - PlayerPrefs.GetInt("Sprint Speed Cost", perkCosts[0]));
						PlayerPrefs.SetInt("Sprint Speed Cost", PlayerPrefs.GetInt("Sprint Speed Cost", perkCosts[0]) + 1);
						PlayerPrefs.SetFloat("Sprint Speed", PlayerPrefs.GetFloat("Sprint Speed", 0) + 10f);
					}
				}
				else if (i == menuOptions.Length - 3)
				{
					if (PlayerPrefs.GetInt("Perk Points", 0) >= PlayerPrefs.GetInt("Decrease Gravity Cost", perkCosts[1]) && PlayerPrefs.GetInt("Decrease Gravity Cost") <= perkMaxCosts[1])
					{
						PlayerPrefs.SetInt("Perk Points", PlayerPrefs.GetInt("Perk Points", 0) - PlayerPrefs.GetInt("Decrease Gravity Cost", perkCosts[1]));
						PlayerPrefs.SetInt("Decrease Gravity Cost", PlayerPrefs.GetInt("Decrease Gravity Cost", perkCosts[1]) + 1);
						PlayerPrefs.SetFloat("Decrease Gravity", PlayerPrefs.GetFloat("Decrease Gravity", 0) + .25f);
					}
				}
				else if (i == menuOptions.Length - 2)
				{
					AudioListener.volume = 1 - AudioListener.volume;
					AudioListener.pause = !AudioListener.pause;
				}
				else if (i == menuOptions.Length - 1)
				{
					PlayerPrefs.DeleteAll();
					PlayerPrefs.SetInt("Last Level Unlocked", lastLevelUnlockedInit);
					foreach (GameObject g in GameObject.FindObjectsOfType<GameObject>())
						Destroy (g);
					Application.LoadLevel(0);
				}
				else
				{
					Debug.Log(i);
					Application.LoadLevel(selectedIndex);
				}
			}
		}
	}
	
	IEnumerator Delay (float time)
	{
		beingDelayed = true;
		yield return new WaitForSeconds(time);
		beingDelayed = false;
	}
	
	public void PlayDieAudio ()
	{
		int r = Mathf.RoundToInt(Random.Range(0, dieAudios.Length));
		audios[dieAudios[r]].Play();
	}
	
	void OnLevelWasLoaded ()
	{
		Start ();
	}
	
	void OnApplicationQuit()
	{
		PlayerPrefs.Save();
	}
}