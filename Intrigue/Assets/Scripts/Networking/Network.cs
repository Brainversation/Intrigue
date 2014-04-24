using UnityEngine;
using System.Collections;

public class Network : MonoBehaviour {

	private string chatBox = "";
	//private Player player;

	void Start () {
		// Get photonView component
		//player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		if( PhotonNetwork.connectionStateDetailed == PeerState.JoinedLobby ){
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

	[RPC]
	public void recieveMessage(string s){
		this.chatBox += s;
	}
}
