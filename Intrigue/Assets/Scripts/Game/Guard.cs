using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour
{
	private bool accusing = false;
	private bool isSpectating = false;
	private GameObject accused;
	private PhotonView photonView = null;
	private GameObject[] guests = null;
	private GameObject[] spies = null;
	private Player player;

	//Yield function that waits specified amount of seconds
	IEnumerator Yielder(int seconds){
		yield return new WaitForSeconds(seconds);
	}

	void Start(){
		photonView = PhotonView.Get(this);

		if(photonView.isMine){
			Debug.Log( "Guard" );
			player = GameObject.Find("Player").GetComponent<Player>();
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
		guests = GameObject.FindGameObjectsWithTag("Guest");
		spies = GameObject.FindGameObjectsWithTag("Spy");
		
		if(guests!=null){
			foreach (GameObject guest in guests){
				guest.GetComponentInChildren<Renderer>().material.color = Color.white;
			}
		}
		if(spies!=null){
			foreach (GameObject spy in spies){
				spy.GetComponentInChildren<Renderer>().material.color = Color.white;
			}
		}

		//Highlights the currently targeted guest
		RaycastHit hit;
		Vector3 fwd = transform.TransformDirection (Vector3.forward);
		
		if (Physics.Raycast (transform.position, fwd, out hit, 15)) {
			if(hit.transform.gameObject.CompareTag("Guest")||hit.transform.gameObject.CompareTag("Spy")){
				hit.transform.gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
			}
		}

		if ( Input.GetKeyUp (KeyCode.E) && !accusing ){
				if ( Physics.Raycast(transform.position, fwd, out hit, 15) ) {
					if(hit.transform.gameObject.CompareTag("Guest") || hit.transform.gameObject.CompareTag("Spy")){
							accusing = true;
							accused = hit.transform.gameObject;
						}
				}
		}	
	}

	void OnGUI(){
		GUI.skin.label.fontSize = 30;
		GUI.color = Color.black;
		if(accusing){
			GUI.Label(new Rect((Screen.width/2)-150,Screen.height-100,300,100),"E to Confirm Accusation \nSpace to Cancel.");
				if(Input.GetKeyUp(KeyCode.E)){
					accusing = false;
					testAccusation();
				}
				if(Input.GetKeyUp(KeyCode.Space)){
					accusing = false;
					accused = null;
				}
		}
		GUI.Label(new Rect((Screen.width/2)-150,Screen.height-100,300,100), string.Format("{0}", player.Score));
	}

	void testAccusation(){
		if(accused != null && accused.CompareTag("Spy")){
			Debug.Log("You found a spy!");
			player.Score += 100;
			photonView.RPC("spyCaught", PhotonTargets.MasterClient);
			accused.GetComponent<PhotonView>().RPC("destroySpy", PhotonTargets.All);
		}
		else{
			Debug.Log("You dun goofed");
			photonView.RPC("guardFailed", PhotonTargets.MasterClient);
			spectate();
			PhotonNetwork.Destroy(gameObject);
		}
		accusing = false;
		accused = null;
	}

	void spectate(){
		GameObject[] guards = GameObject.FindGameObjectsWithTag("Guard");
		foreach (GameObject guard in guards){
			guard.GetComponentInChildren<Camera>().enabled = true; 
			isSpectating = true;
			break;
		}
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
