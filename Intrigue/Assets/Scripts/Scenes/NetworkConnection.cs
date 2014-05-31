﻿/**************************************************************************
 **                                                                       *
 **                COPYRIGHT AND CONFIDENTIALITY NOTICE                   *
 **                                                                       *
 **    Copyright (c) 2014 Hacksaw Games. All rights reserved.             *
 **                                                                       *
 **    This software contains information confidential and proprietary    *
 **    to Hacksaw Games. It shall not be reproduced in whole or in        *
 **    part, or transferred to other documents, or disclosed to third     *
 **    parties, or used for any purpose other than that for which it was  *
 **    obtained, without the prior written consent of Hacksaw Games.      *
 **                                                                       *
 **************************************************************************/

using UnityEngine;
using System.Collections;

public class NetworkConnection : Photon.MonoBehaviour {
	
	public GameObject gui;

	private Player player;
	private bool showRetry = false;
	private Vector3 lastPos = Vector3.zero;

	void Start () {
		player = GameObject.Find("Player").GetComponent<Player>();
	}

	void OnGUI(){
		if(showRetry){
			if(PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated || PhotonNetwork.connectionStateDetailed == PeerState.Disconnected){
				GUILayout.Label("Lost connection to room: " + player.RoomName + "\nWaiting for connection...");
				if(GUILayout.Button("Retry Connection")){
					PhotonNetwork.ConnectUsingSettings("0.1");
				}
			} else {
				GUILayout.Label("Rejoin room: " + player.RoomName + "?");
				if(GUILayout.Button("Join")){
					PhotonNetwork.JoinRoom(player.RoomName);
				}
			}
			if(GUILayout.Button("Exit")){
				gui.SetActive(false);
				showRetry = false;
				PhotonNetwork.LoadLevel(0);
			}
		}
	}

	void OnPhotonPlayerConnected(PhotonPlayer newPlayer){
		if(PhotonNetwork.player.ID == newPlayer.ID){
			if(player.Team == "Guard")
				photonView.RPC("reAddGuard", PhotonTargets.All);
			else
				photonView.RPC("reAddSpy", PhotonTargets.All);
		}

		if((string)newPlayer.customProperties["Team"] == "Guard"){
			player.GetComponent<BasePlayer>().newEvent("[FF2B2B]" + (string)newPlayer.customProperties["Handle"]  + "[-][FFCC00] has reconnected![-]");
		}
		else{
			player.GetComponent<BasePlayer>().newEvent("[00CCFF]" + (string)newPlayer.customProperties["Handle"]  + "[-][FFCC00] has reconnected![-]");
		}

	}

	void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer){
		if( (string)photonPlayer.customProperties["Team"] == "Guard" ){
			if( !(bool)photonPlayer.customProperties["isOut"] )photonView.RPC("removeGuard", PhotonTargets.All);
			player.GetComponent<BasePlayer>().newEvent("[FF2B2B]" + (string)photonPlayer.customProperties["Handle"]  + "[-][FFCC00] has disconnected.[-]");
		} else {
			if( !(bool)photonPlayer.customProperties["isOut"] )photonView.RPC("removeSpy", PhotonTargets.All);
			player.GetComponent<BasePlayer>().newEvent("[00CCFF]" + (string)photonPlayer.customProperties["Handle"]  + "[-][FFCC00] has disconnected.[-]");
		}
	}

	void OnMasterClientSwitched(PhotonPlayer newMasterClient){
		if(PhotonNetwork.player.ID == newMasterClient.ID){
			GameObject.FindWithTag("Scripts").GetComponent<Intrigue>().enabled = true;
		}
	}

	void OnJoinedRoom(){
		gui.SetActive(false);
		showRetry = false;

		//Check if game over or same round or something, then add it

		if(lastPos == Vector3.zero){
			GameObject teamMates = GameObject.FindGameObjectsWithTag(player.Team)[0];
			teamMates.GetComponentInChildren<Camera>().enabled = true;
 			foreach(UIPanel uiP in teamMates.GetComponentsInChildren<UIPanel>(true)){
				if(uiP.gameObject.CompareTag("ChatArea") ||
				   uiP.gameObject.CompareTag("Scoreboard") ||
					uiP.gameObject.CompareTag("StunUI") ||
					uiP.gameObject.CompareTag("TimeLabel")){
					NGUITools.SetActive(uiP.gameObject, true);
				}
			}
		} else {
			Intrigue.playerGO = PhotonNetwork.Instantiate(
							"Robot_"+ player.Team+"1"/*type.ToString()*/,
							lastPos,
							Quaternion.identity, 0);
		}
	}

	void OnDisconnectedFromPhoton(){
		if(Intrigue.playerGO != null){
			lastPos = Intrigue.playerGO.transform.position;
		}
		gui.SetActive(true);
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

	[RPC]
	void reAddSpy(){
		++Intrigue.numSpiesLeft;
	}

	[RPC]
	void reAddGuard(){
		++Intrigue.numGuardsLeft;
	}
}
