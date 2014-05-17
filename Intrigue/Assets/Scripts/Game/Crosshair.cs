using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public Texture2D crosshairNormal;
	public Texture2D crosshairInteract;
	public bool teamSpy;
	[HideInInspector] public bool canInteract = false;
	private Rect position;
	private Vector3 screenPoint = new Vector3(Screen.width/2, Screen.height/2, 0);
    private BasePlayer bp;

	// Use this for initialization
	void Start () {
		position = new Rect((Screen.width - crosshairNormal.width) / 2, (Screen.height - 
		crosshairNormal.height) /2, crosshairNormal.width, crosshairNormal.height);
		bp = GetComponent<BasePlayer>();
		teamSpy = GetComponent<Spy>() != null;
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
						} else if(hit.transform.tag == "Objective" && hit.transform.gameObject.GetComponent<Objective>().isActive && (Vector3.Distance(hit.transform.position, transform.position) )<=10){
							canInteract = true;
						}
					}
					else if( (hit.transform.tag == "Guard" || hit.transform.tag == "Guest") && Vector3.Distance(hit.transform.position, transform.position)<15){
							if(hit.transform.tag == "Guard" && !hit.transform.gameObject.GetComponent<Guard>().stunned){
								canInteract = true;
							}
							if(hit.transform.tag == "Guest" && !hit.transform.gameObject.GetComponent<BaseAI>().stunned){
								canInteract = true;
							}
					}
				}
			} else {
				//Guard
				Ray ray = Camera.main.ScreenPointToRay( screenPoint );
				RaycastHit hit;
				if( Physics.Raycast(ray, out hit, 15.0f, bp.layerMask) ){
					if(hit.transform.gameObject.layer == 9 || (hit.transform.gameObject.layer == 10 && !hit.transform.gameObject.GetComponent<BasePlayer>().isOut))
						canInteract = true;
				}
			}
		}
	}
	
	void OnGUI () {
		if(!Input.GetKey(KeyCode.Tab)){
			if(canInteract==true)
				GUI.DrawTexture(position, crosshairInteract);
			else
				GUI.DrawTexture(position, crosshairNormal);
		}
	}
}
