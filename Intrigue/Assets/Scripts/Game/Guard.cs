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
	private Intrigue intrigue;
	private Network network;
	public GameObject allytext;
	public bool textAdded = false;
	public string localHandle = "No Handle";
	//Yield function that waits specified amount of seconds
	IEnumerator Yielder(int seconds){
		yield return new WaitForSeconds(seconds);
	}

	void Start(){
		intrigue = GameObject.FindWithTag("Scripts").GetComponent<Intrigue>();
		network = GameObject.FindWithTag("Scripts").GetComponent<Network>();
		photonView = PhotonView.Get(this);

		if(photonView.isMine){
			Debug.Log( "Guard" );
			player = GameObject.Find("Player").GetComponent<Player>();
			photonView.RPC("giveHandle", PhotonTargets.OthersBuffered, player.Handle);
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
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast (ray, out hit, 15)) {
			if(hit.transform.gameObject.CompareTag("Guest")||hit.transform.gameObject.CompareTag("Spy")){
				hit.transform.gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
			}
		}

		if ( Input.GetKeyUp (KeyCode.E) && !accusing ){
				if ( Physics.Raycast(ray, out hit, 15) ) {
					if(hit.transform.gameObject.CompareTag("Guest") || hit.transform.gameObject.CompareTag("Spy")){
							accusing = true;
							accused = hit.transform.gameObject;
						}
				}
		}

		//Create Ally Texts
		GameObject[] allies = GameObject.FindGameObjectsWithTag("Guard");
		foreach(GameObject ally in allies){
			if(ally!=gameObject){
				if(!ally.GetComponent<Guard>().textAdded){
					//Debug.Log("creating ally text");
					ally.GetComponent<Guard>().textAdded = true;
					GameObject textInstance = Instantiate(allytext, ally.transform.position,ally.transform.rotation) as GameObject;
					textInstance.GetComponent<AllyText>().target = ally.transform;
					textInstance.transform.parent = ally.transform;
					textInstance.GetComponent<TextMesh>().text = ally.GetComponent<Guard>().localHandle;
				}
				if((ally.GetComponentInChildren<TextMesh>().text == "No Handle") && ally.GetComponent<Guard>().textAdded){
					//Debug.Log("Changing Handle from: " + ally.GetComponentInChildren<TextMesh>().text + " to:" + ally.GetComponent<Spy>().localHandle);
					ally.GetComponentInChildren<TextMesh>().text = ally.GetComponent<Guard>().localHandle;
					
				}
			}
		}	
	}

	void OnGUI(){
		//GUI.skin.label.fontSize = 20;
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
		//GUI.Label(new Rect((Screen.width/2)-150,Screen.height-100,300,100), string.Format("{0}", player.Score));
		if( isSpectating ) GUI.Label(new Rect((Screen.width/2)-150,Screen.height-50,300,100), "Spectating!" );
	}

	void testAccusation(){
		if(accused != null && accused.CompareTag("Spy")){
			Debug.Log("You found a spy!");
			player.Score += 100;
			photonView.RPC("addScore", PhotonTargets.AllBuffered, player.TeamID, 100);
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
		Debug.Log("Trying to Spectate");
		GameObject[] guards = GameObject.FindGameObjectsWithTag("Guard");
		foreach (GameObject guard in guards){
			guard.GetComponentInChildren<Camera>().enabled = true; 
			isSpectating = true;
			Debug.Log("In For loop enabled a Camera");
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

	[RPC]
	void giveHandle(string handle){
		localHandle = handle;
	}

	[RPC]
	void addScore(int teamID, int scoreToAdd){
		network.AddScore(teamID, scoreToAdd);
	}
}
