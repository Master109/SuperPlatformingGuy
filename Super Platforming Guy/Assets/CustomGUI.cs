using UnityEngine;
using System.Collections;

public class CustomGUI : MonoBehaviour
{
	public GUIType type;
	public GUISkin skin;
	public string action;
	public string[] actions;
	public string text;
	public bool eraseTextureOnStart;
	public bool active;

	// Use this for initialization
	void Start ()
	{
		if (eraseTextureOnStart)
			GetComponent<GUITexture>().texture = null;
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	void OnGUI ()
	{
		Vector2 screenPos = Camera.main.ViewportToScreenPoint(new Vector2(transform.position.x, 1 - transform.position.y));
		GUI.skin = skin;
		if (type == GUIType.Button && GUI.Button(new Rect(screenPos.x + GetComponent<GUITexture>().pixelInset.x, screenPos.y + GetComponent<GUITexture>().pixelInset.y, GetComponent<GUITexture>().pixelInset.width, GetComponent<GUITexture>().pixelInset.height), text))
			Camera.main.SendMessage("Eval", action);
		else if (type == GUIType.Label)
		{
			GUI.Label(new Rect(screenPos.x + GetComponent<GUITexture>().pixelInset.x, screenPos.y + GetComponent<GUITexture>().pixelInset.y, GetComponent<GUITexture>().pixelInset.width, GetComponent<GUITexture>().pixelInset.height), text);
			Camera.main.SendMessage("Eval", action);
		}
		else if (type == GUIType.Box)
		{
			GUI.Box(new Rect(screenPos.x + GetComponent<GUITexture>().pixelInset.x, screenPos.y + GetComponent<GUITexture>().pixelInset.y, GetComponent<GUITexture>().pixelInset.width, GetComponent<GUITexture>().pixelInset.height), text);
			Camera.main.SendMessage("Eval", action);
		}
		else if (type == GUIType.Toggle)
		{
			active = GUI.Toggle(new Rect(screenPos.x + GetComponent<GUITexture>().pixelInset.x, screenPos.y + GetComponent<GUITexture>().pixelInset.y, GetComponent<GUITexture>().pixelInset.width, GetComponent<GUITexture>().pixelInset.height), active, text);
			if (active)
				Camera.main.SendMessage("Eval", actions[0]);
			else
				Camera.main.SendMessage("Eval", actions[1]);
		}
	}

	public enum GUIType
	{
		Button = 1,
		Label = 2,
		Box = 3,
		Toggle = 4,
	}
}
