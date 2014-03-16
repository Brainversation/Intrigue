using UnityEngine;
using System.Collections;

public class Network : MonoBehaviour {

	private string chatBox = "";
	private Player player;

	// Look up how to disconnect
	void Start () {
		PhotonNetwork.networkingPeer.NewSceneLoaded();
		// Get photonView component
		PhotonNetwork.sendRate = 60;
		PhotonNetwork.sendRateOnSerialize = 4;
		player = GameObject.Find("Player").GetComponent<Player>();
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
		//Scoreboard sb = Intrigue.playerGO.GetComponentInChildren<Scoreboard>();
		//sb.GetComponent<PhotonView>().RPC("removeName", PhotonTargets.All, player.Handle, player.Team);
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
