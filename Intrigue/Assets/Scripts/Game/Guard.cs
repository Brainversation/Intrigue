using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour
{
    
	bool accusing = false;
	GameObject accused;
	private PhotonView photonView = null;
	private GameObject[] guests = null;
	private GameObject[] spies = null;

	//Yield function that waits specified amount of seconds
	IEnumerator Yielder(int seconds){
		yield return new WaitForSeconds(seconds);
	}

	void Start(){
		photonView = PhotonView.Get(this);

		if(photonView.isMine){
			Debug.Log( "Guard" );
		} else {
			Debug.Log("Guard Deactivated");
			GetComponentInChildren<Camera>().enabled = false; 
			GetComponentInChildren<AudioListener>().enabled = false;
			GetComponentInChildren<MovementController>().enabled = false;
			GetComponentInChildren<MouseLook>().enabled = false; 
			GetComponent<MouseLook>().enabled = false;
			enabled = false;

		}
	}

	void Update () {

		if ( guests == null ){
			Yielder(2);
			guests = GameObject.FindGameObjectsWithTag("Guest");
			spies = GameObject.FindGameObjectsWithTag("Spy");
		}
			
		if(guests!=null){
			foreach (GameObject guest in guests){
				//guestAI = guest.GetComponent("GuestAi"); DEPRECATED
				//guestAI.outView();
				//<---------- NEED NEW WAY TO HIGHLIGHT GUESTS -------->
			}
		}
		if(spies!=null){
			foreach (GameObject spy in spies){
			
				/*
				spy.renderer.material.SetColor("_Color", Color.white);
				spyView = spy.GetComponent("InGuardView");
				spyView.outView(); 
				^ DEPRECATED ^
				<--------- NEED NEW WAY TO HIGHLIGHT SPIES ------> 
				*/
			}
		}

		//Highlights the currently targeted guest
		RaycastHit hit;
		Vector3 fwd = transform.TransformDirection (Vector3.forward);
		
		if (Physics.Raycast (transform.position, fwd, out hit, 5)) {
			if(hit.transform.gameObject.CompareTag("Guest")){
				/*
				guestAI = hit.transform.gameObject.GetComponent("GuestAi");
				guestAI.inView();
				^ DEPRECATED
				<------- NEED NEW WAY TO HIGHLIGHT GUESTS -------->
				*/
			}
			if(hit.transform.gameObject.CompareTag("Spy")){
				/*
				hit.transform.gameObject.renderer.material.SetColor("_Color", Color.red);
				spyView = hit.transform.gameObject.GetComponent("InGuardView");
				spyView.inView();
				^ DEPRECATED
				<------- NEED NEW WAY TO HIGHLIGHT SPIES -------->
				*/
			}
		}



		if ( Input.GetKeyUp (KeyCode.E) && !accusing ){
				if (Physics.Raycast (transform.position, fwd,out hit, 8)) {
					if(hit.transform.gameObject.CompareTag("Guest") || hit.transform.gameObject.CompareTag("Spy")){
							accusing = true;
							accused = hit.transform.gameObject;
						}
				}
		}	
	}

	void OnGUI(){
		GUI.skin.label.fontSize = 20;
		GUI.color = Color.black;
		GUILayout.Label( "Team: "+ photonView.owner.Team );
		if(accusing){
			GUI.Label(new Rect((Screen.width/2)-150,Screen.height-100,300,100),"E to Confirm Accusation \nSpace to Cancel.");
				if(Input.GetKeyUp(KeyCode.E)){
					accusing = false;
					testAccusation();
				}
				if(Input.GetKeyUp(KeyCode.Space)){
					accusing = false;
				}
		}
		GUI.Label(new Rect((Screen.width/2)-150,Screen.height-100,300,100), string.Format("{0}", photonView.owner.Score));
	}

	void testAccusation(){
		if(accused != null && accused.CompareTag("Spy")){
			Debug.Log("You found a spy!");
			photonView.owner.Score += 100;
			photonView.RPC("spyCaught", PhotonTargets.MasterClient);
			PhotonNetwork.Destroy(accused);
		}
		else{
			Debug.Log("You dun goofed");
			photonView.RPC("guardFailed", PhotonTargets.MasterClient);
			PhotonNetwork.Destroy(gameObject);
		}
		accusing = false;
	}

	void OnDestroy(){
		Debug.Log("Getting Destroyed");
		//Switch cam
	}

	[RPC]
	void sendMessage(string text, NetworkMessageInfo info){
	    Debug.Log(text + " from " + info.sender);
	}

	[RPC]
	void spyCaught(){
	    --Intrigue.numSpiesLeft;
	}

	[RPC]
	void guardFailed(){
	    --Intrigue.numGuardsLeft;
	}
}