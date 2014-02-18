using UnityEngine;
using System.Collections;

public class Network : MonoBehaviour {

	private PhotonView photonView = null;
	private Vector2 scrollPositionChat = new Vector2(0, 0);
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

	}
	
	// Update is called once per frame
	void Update () {
		if( PhotonNetwork.connectionStateDetailed == PeerState.JoinedLobby ){
			PhotonNetwork.isMessageQueueRunning = false;
			PhotonNetwork.LoadLevel("MainMenu");
		}

	}

	void OnGUI(){
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
