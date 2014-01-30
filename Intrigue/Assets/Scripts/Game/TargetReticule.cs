using UnityEngine;
using System.Collections;

public class TargetReticule : MonoBehaviour {

	public Texture2D crosshairTexture;
	private Rect position;
	private static bool OriginalOn = true;
	
	void Start () {
	
		position = new Rect((Screen.width - crosshairTexture.width) / 2, (Screen.height - 
        crosshairTexture.height) /2, crosshairTexture.width, crosshairTexture.height);
	
	}
	
	// Update is called once per frame
	void OnGUI () {
		if(OriginalOn == true){
			GUI.DrawTexture(position, crosshairTexture);
		}
	}
}
