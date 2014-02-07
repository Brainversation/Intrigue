using UnityEngine;
using System.Collections;

public class Spy : MonoBehaviour
{

	private bool isSpectating = false;
	private Player player;
	public PhotonView photonView = null;
	public GameObject allytext;
	public bool textAdded = false;
	public string localHandle = "No Handle";

	public int remoteScore = 0;

	private GameObject[] guards = null;
	private GameObject[] spies = null;
	private Rect windowRect = new Rect(20, 20, 120, 50);

	//Yield function that waits specified amount of seconds
	IEnumerator Yielder(int seconds){
		yield return new WaitForSeconds(seconds);
	}

	void Start(){
		photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();

		if(photonView.isMine){
			Debug.Log( "Spy" );
			photonView.RPC("giveHandle", PhotonTargets.OthersBuffered, player.Handle);

		} else {
			Debug.Log("Spy Deactivated");
			GetComponentInChildren<Camera>().enabled = false; 
			GetComponentInChildren<AudioListener>().enabled = false;
			GetComponentInChildren<MovementController>().enabled = false;
			GetComponentInChildren<MouseLook>().enabled = false; 
			GetComponent<MouseLook>().enabled = false;
			enabled = false;
		}
	}

	void OnGUI() {
		GUI.skin.label.fontSize = 20;
		GUI.color = Color.black;
		if( isSpectating ) GUI.Label(new Rect((Screen.width/2)-150,Screen.height-50,300,100), "Spectating!" );

		if( Input.GetKey(KeyCode.Tab) ){
			guards = GameObject.FindGameObjectsWithTag("Guard");
			spies = GameObject.FindGameObjectsWithTag("spies");
			windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "Teams");
		}
	}

	void DoMyWindow(int windowID) {

		foreach(GameObject g in guards){
			GUILayout.Label(g.GetComponent<Guard>().localHandle + " " + g.GetComponent<Guard>().remoteScore);
		}
		foreach(GameObject s in spies){
			GUILayout.Label(s.GetComponent<Guard>().localHandle + " " + s.GetComponent<Guard>().remoteScore);
		}

	}

	void Update () {
		//Interact Raycasts
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Input.GetKey("e")){
			RaycastHit hit;
			Debug.DrawRay(ray.origin,ray.direction*15f,Color.green);
			if( Physics.Raycast(ray, out hit, 15.0f) ){
				if( hit.transform.tag == "Objective" ){
					Objective hitObjective = hit.transform.GetComponent<Objective>();
					Debug.Log("Hit Objective");
					hitObjective.useObjective(gameObject);
				}
			}
		}

		//Create Ally Texts
		GameObject[] allies = GameObject.FindGameObjectsWithTag("Spy");
		foreach(GameObject ally in allies){
			if(ally!=gameObject){
				if(!ally.GetComponent<Spy>().textAdded){
					//Debug.Log("creating ally text");
					ally.GetComponent<Spy>().textAdded = true;
					GameObject textInstance = Instantiate(allytext, ally.transform.position,ally.transform.rotation) as GameObject;
					textInstance.GetComponent<AllyText>().target = ally.transform;
					textInstance.transform.parent = ally.transform;
					textInstance.GetComponent<TextMesh>().text = ally.GetComponent<Spy>().localHandle;
				}
				if((ally.GetComponentInChildren<TextMesh>().text == "No Handle") && ally.GetComponent<Spy>().textAdded){
					//Debug.Log("Changing Handle from: " + ally.GetComponentInChildren<TextMesh>().text + " to:" + ally.GetComponent<Spy>().localHandle);
					ally.GetComponentInChildren<TextMesh>().text = ally.GetComponent<Spy>().localHandle;
					
				}
			}
		}
	}


	void spectate(){
		Debug.Log("Trying to Spectate");
		GameObject[] spies = GameObject.FindGameObjectsWithTag("Spy");
		if(spies.Length == 0){
			spies = GameObject.FindGameObjectsWithTag("Guard");
		}
		foreach (GameObject spy in spies){
			spy.GetComponentInChildren<Camera>().enabled = true;
			isSpectating = true;
			Debug.Log("In For loop enabled a Camera");
			break;
		}
	}

	[RPC]
	void destroySpy(){
		if( photonView.isMine){
			spectate();
			PhotonNetwork.Destroy(gameObject);
		}
	}

	[RPC]
	void giveHandle(string handle){
		localHandle = handle;
	}

	[RPC]
	void addPlayerScore(int scoreToAdd){
		if(photonView.isMine){
			player.Score += scoreToAdd;
		}
		else{
			remoteScore += scoreToAdd;
		}
	}
}
