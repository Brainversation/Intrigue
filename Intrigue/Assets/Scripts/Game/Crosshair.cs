using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public Texture2D crosshairNormal;
	public Texture2D crosshairInteract;
	public bool teamSpy;
	[HideInInspector]
	public bool canInteract = false;
	private Rect position;
	private static bool OriginalOn = true;
	private Vector3 screenPoint = new Vector3(Screen.width/2, Screen.height/2, 0);

	// Use this for initialization
	void Start () {
		position = new Rect((Screen.width - crosshairNormal.width) / 2, (Screen.height - 
        crosshairNormal.height) /2, crosshairNormal.width, crosshairNormal.height);
	}

	void Update(){
		//Spy
		if(Camera.main!=null){
		canInteract = false;
			if(teamSpy){
				Ray ray = Camera.main.ScreenPointToRay( screenPoint );
				RaycastHit hit;
				if( Physics.Raycast(ray, out hit, 1000f) ){
					if( hit.transform.tag == "ObjectiveMain" || hit.transform.tag =="Objective"){
						if(hit.transform.tag == "ObjectiveMain" && hit.transform.gameObject.GetComponent<ObjectiveMain>().isActive ){
							canInteract = true;
						} else if(hit.transform.tag == "Objective" && hit.transform.gameObject.GetComponent<Objective>().isActive && (Vector3.Distance(hit.transform.position, transform.position) )<=7){
							canInteract = true;
						}
					}
					else if( (hit.transform.tag == "Guard" || hit.transform.tag == "Guest") && Vector3.Distance(hit.transform.position, transform.position)<10){
							canInteract = true;
					}
				}
			}
			else{
				//Guard
				Ray ray = Camera.main.ScreenPointToRay( screenPoint );
				RaycastHit hit;
				if( Physics.Raycast(ray, out hit, 15.0f) ){
					if( hit.transform.tag == "Spy" || hit.transform.tag == "Guest"){
						canInteract = true;
					}
				}
			}
		}
	}
	
	void OnGUI () {
		if(OriginalOn == true){
			if(!Input.GetKey(KeyCode.Tab)){
				if(canInteract==true)
					GUI.DrawTexture(position, crosshairInteract);
				else
					GUI.DrawTexture(position, crosshairNormal);
			}
		}
	}

}
