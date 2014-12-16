using UnityEngine;
using System.Collections;

public class ChangeGUITexture : MonoBehaviour
{
	public float sizeMultiplier;
	
	// Use this for initialization
	void Start ()
	{
		GetComponent<GUITexture>().pixelInset = new Rect(GetComponent<GUITexture>().pixelInset.x * sizeMultiplier, GetComponent<GUITexture>().pixelInset.y * sizeMultiplier, GetComponent<GUITexture>().pixelInset.width * sizeMultiplier, GetComponent<GUITexture>().pixelInset.height * sizeMultiplier);
	}
}
