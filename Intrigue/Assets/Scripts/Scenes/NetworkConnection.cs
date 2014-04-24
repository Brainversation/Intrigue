﻿using UnityEngine;
using System.Collections;

public class NetworkConnection : Photon.MonoBehaviour {

	private Player player;
	private bool showRetry = false;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Player>();
	}

	void OnGUI(){
		GUILayout.Label("Status: " + PhotonNetwork.connectionStateDetailed.ToString());

		if(!showRetry) return;

		if(PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated || PhotonNetwork.connectionStateDetailed == PeerState.Disconnected){
			GUILayout.Label("Lost connection to room: " + player.RoomName + "\nWaiting for connection...");
			if(GUILayout.Button("Retry Connection")){
				PhotonNetwork.ConnectUsingSettings("0.1");
			}
		} else {
			GUILayout.Label("Rejoin room: " + player.RoomName + "?");
			if(GUILayout.Button("Join")){
				showRetry = false;
				PhotonNetwork.JoinRoom(player.RoomName);
			}
			if(GUILayout.Button("Exit")){
				showRetry = false;
				PhotonNetwork.LoadLevel(0);
			}
		}
	}

	void OnPhotonPlayerConnected(PhotonPlayer newPlayer){
		if(PhotonNetwork.player.ID == newPlayer.ID){
			// Do re-add stuff
		} else if(PhotonNetwork.isMasterClient){
			// Do re-add stuff
		}
	}

	void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer){
		Debug.Log("OnPhotonPlayerDisconnected: " +
					(string)photonPlayer.customProperties["Handle"] + " " +
					(string)photonPlayer.customProperties["Team"] );

		if( (string)photonPlayer.customProperties["Team"] == "Guard" ){
			photonView.RPC("removeGuard", PhotonTargets.All);
		} else {
			photonView.RPC("removeSpy", PhotonTargets.All);
		}
	}

	void OnMasterClientSwitched(PhotonPlayer newMasterClient){
		// call master switched stuff
		Debug.Log("OnMasterClientDisconnected: " +
					(string)newMasterClient.customProperties["Handle"] + " " +
					(string)newMasterClient.customProperties["Team"] );

		if(PhotonNetwork.player.ID == newMasterClient.ID){
			// Do switch
		}
	}

	void OnJoinedRoom(){
		Debug.Log("OnJoinedRoom");
	}

	void OnDisconnectedFromPhoton(){
		showRetry = true;
	}

	void OnPhotonJoinRoomFailed(){
		Debug.Log("FAILED ROOM");
	}

	[RPC]
	void removeSpy(){
		--Intrigue.numSpiesLeft;
	}

	[RPC]
	void removeGuard(){
		--Intrigue.numGuardsLeft;
	}
}
