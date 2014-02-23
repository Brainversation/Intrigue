using UnityEngine;
using System.Collections;

public class Pregame : MonoBehaviour {

	private PhotonView photonView = null;
	private Vector2 scrollPositionChat = new Vector2(0, 0);
	private string chatBox = "";
	private string textField = "";
	private bool isReady = false;
	private int readyCount = 0;
	private Player player;

	// Use this for initialization
	void Start () {
		Screen.lockCursor = false;
		PhotonNetwork.networkingPeer.NewSceneLoaded();
		// Get photonView component
		this.photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPhotonPlayerDisconnected(PhotonPlayer player){
		Debug.Log("OnPhotonPlayerDisconnected: " + player.ID );

		if (PhotonNetwork.isMasterClient){
			//Move Info towards to new master Client, but master client switches on its own
		}
	}

	[RPC]
	public void recieveMessage(string s){
		this.chatBox += s;
	}

	[RPC]
	public void ready( int val ){
		this.readyCount += val;
	}
	
	[RPC]
	public void go(){
		PhotonNetwork.LoadLevel("Intrigue");
	}
}
