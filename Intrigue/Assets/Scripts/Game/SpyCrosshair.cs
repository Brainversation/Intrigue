using UnityEngine;
using System.Collections;

public class SpyCrosshair : MonoBehaviour {

	public Texture2D crosshairNormal;
	public Texture2D crosshairInteract;
	public bool canInteract = false;
	private Rect position;
	private static bool OriginalOn = true;
	
	void Start () {
	
		position = new Rect((Screen.width - crosshairNormal.width) / 2, (Screen.height - 
        crosshairNormal.height) /2, crosshairNormal.width, crosshairNormal.height);
	
	}
	void Update(){
		Debug.Log("there is a spy using this");
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if( Physics.Raycast(ray, out hit, 15.0f) ){
				if( hit.transform.tag == "Objective" && hit.transform.gameObject.GetComponent<Objective>().active ){
					canInteract = true;
				}
				else {
					canInteract = false;
				}
			}
			else
				canInteract = false;
	}

	// Update is called once per frame
	void OnGUI () {
		if(OriginalOn == true){
			if(canInteract==true)
				GUI.DrawTexture(position, crosshairInteract);
			else
				GUI.DrawTexture(position, crosshairNormal);
		}
	}
}
