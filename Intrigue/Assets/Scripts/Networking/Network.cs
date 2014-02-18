using UnityEngine;
using System.Collections;

public class Network : MonoBehaviour {

	private PhotonView photonView = null;
	private Vector2 scrollPositionChat = new Vector2(0, 0);
	private GUIStyle styleChat = new GUIStyle();
	private GUIStyle styleScore = new GUIStyle();
	private string chatBox = "";
	private string textField = "";
	private Player player;
	private Intrigue intrigue;

	// Look up how to disconnect
	void Start () {
		PhotonNetwork.networkingPeer.NewSceneLoaded();
		// Get photonView component
		photonView = PhotonView.Get(this);

		player = GameObject.Find("Player").GetComponent<Player>();
		intrigue = GameObject.FindWithTag("Scripts").GetComponent<Intrigue>();

		this.styleChat.fontSize = 12;
		this.styleChat.normal.textColor = Color.white;

		this.styleScore.fontSize = 40;
		this.styleScore.normal.textColor = Color.red;

	}
	
	// Update is called once per frame
	void Update () {
		if( PhotonNetwork.connectionStateDetailed == PeerState.JoinedLobby ){
			PhotonNetwork.isMessageQueueRunning = false;
			PhotonNetwork.LoadLevel("MainMenu");
		}

	}

	void OnGUI(){
		// Tells us about the current network connection
		
		GUILayout.Label( "Are You Master Client?? " + PhotonNetwork.isMasterClient );
		
		//Checks state of connection: Look up PeerState
		if( PhotonNetwork.connectionStateDetailed == PeerState.Joined ){
			if( GUILayout.Button( "Leave Room" ) ){
				if(Intrigue.playerGO)
					PhotonNetwork.Destroy(Intrigue.playerGO);
				PhotonNetwork.LeaveRoom();
			}
		}
	}

	void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer){
		Debug.Log("OnPhotonPlayerDisconnected: " + photonPlayer.ID );
		
		if( player.Team == "Guard" ){
			--Intrigue.numGuardsLeft;
		} else {
			--Intrigue.numSpiesLeft;
		}

		if (PhotonNetwork.isMasterClient){
			//Move Info towards to new master Client, but master client switches on its own
		}
	}

	[RPC]
	public void recieveMessage(string s){
		this.chatBox += s;
	}
}
