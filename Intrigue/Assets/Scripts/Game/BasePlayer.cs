using UnityEngine;
using System.Collections;

public class BasePlayer : MonoBehaviour {
	
	protected Player player;
	protected Vector3 screenPoint = new Vector3(Screen.width/2, Screen.height/2, 0);
	protected Intrigue intrigue;
	protected UIPanel[] guiPanels;
	protected UILabel[] guiLabels;
	protected GameObject timeLabel;
	protected GameObject outLabel;

	public PhotonView photonView = null;
	public string localHandle = "";
	public int localPing = 0;
	public int remoteScore = 0;
	public bool textAdded = false;
	public bool isAssigned = false;
	public bool isOut = false;
	public GameObject hairHat;
	public GameObject allytext;

	//Yield function that waits specified amount of seconds
	IEnumerator Yielder(int seconds){
		yield return new WaitForSeconds(seconds);
	}

	void Start () {
		photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();
		intrigue = GameObject.FindWithTag("Scripts").GetComponent<Intrigue>();
		InvokeRepeating("syncPingAndScore", 1, 1F);
		
		if(photonView.isMine){
			localHandle = player.Handle;
			remoteScore = player.Score;
			photonView.RPC("giveHandle", PhotonTargets.OthersBuffered, player.Handle);
			photonView.RPC("giveScore", PhotonTargets.Others, player.Score);
			if(hairHat!=null)
				hairHat.GetComponent<Renderer>().enabled = false;
		} else {
			GetComponentInChildren<Camera>().enabled = false; 
			GetComponentInChildren<AudioListener>().enabled = false;
			GetComponentInChildren<MovementController>().enabled = false;
			GetComponentInChildren<MouseLook>().enabled = false; 
			GetComponentInChildren<Crosshair>().enabled = false;
			GetComponent<MouseLook>().enabled = false;
			guiPanels = GetComponentsInChildren<UIPanel>(true);
			foreach(UIPanel uiP in guiPanels){
				NGUITools.SetActive(uiP.gameObject, false);
			}
			enabled = false;
		}
	}
	
	void Update () {
	
	}

	void syncPingAndScore(){
		localPing = PhotonNetwork.GetPing();
		photonView.RPC("givePing", PhotonTargets.All, localPing);
	}

	public void outStarted(){
		Invoke("spectate", 5);
	}

	void spectate(){
		GetComponentInChildren<Camera>().enabled = false;
		foreach (GameObject teamMates in GameObject.FindGameObjectsWithTag(player.Team)){
			if(teamMates.gameObject != gameObject){
				teamMates.GetComponentInChildren<Camera>().enabled = true; 
				break;
			}
		}

		PhotonNetwork.Destroy(gameObject);
	}
}
