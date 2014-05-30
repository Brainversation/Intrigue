/**************************************************************************
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
using System;
using System.Collections;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class Pregame : MonoBehaviour {

	public UITextList textList;
	public UIInput mInput;
	public GameObject readyCheck;
	public UILabel readyLabel;
	public UIToggle readyCheckToggle;
	public UISlider slider;
	public UILabel sliderLabel;
	public GameObject playerPrefab;
	public GameObject guardTable;
	public GameObject spyTable;
	public UIPanel gameStartingPanel;

	private PhotonView photonView = null;
	private Player player;
	private bool isReady = false;
	private bool allReady = false;
	private int readyCount = 0;
	private List<int> spies = new List<int>();
	private List<int> guards = new List<int>();
	private int prevGuestCount = 0;
	private bool gameStarting = false;

	private static bool isTesting = false;

	void Start(){
		this.photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();

		checkHandle();

		//Sets Chat Max Line Count
		mInput.label.maxLineCount = 1;

		//Disables Controlling the Guest Slider for Clients
		if(!PhotonNetwork.isMasterClient){
			slider.enabled = false;
		}
		else{
			textList.Add("[FF0000]Tip:[-][FFCC00] type [-]'/kick '[FFCC00] + [-]'player's handle'[FFCC00] to kick that player.");
		}

		//Updates Ping and Score Every X Seconds
		InvokeRepeating("syncPing", 0, 2F);

		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Handle", player.Handle},
																{"Ready", PhotonNetwork.isMasterClient},
																{"Ping", 100},
																{"Score", 0}});
		reloadScoreboard();
		if(spies.Count<=guards.Count)
			swapToSpy();
		else
			swapToGuard();

	}

	void Update(){
		//Checks the Ready Status of All Players
		readyStatus();

		//Updates the Slider for Guest Count
		updateGuestSlider();
	}


	public void OnSubmit(){
		if (textList != null)
		{
			// It's a good idea to strip out all symbols as we don't want user input to alter colors, add new lines, etc
			string text = NGUIText.StripSymbols(mInput.value);
			bool isCommand = false;
			if(PhotonNetwork.isMasterClient)
				isCommand = testCommands(text);

			text = StringCleaner.CleanString(text);
			if (!string.IsNullOrEmpty(text) && text.Length>=2 && !isCommand){
				if(player.Team == "Spy"){
					textList.Add("[8169FF]"+player.Handle+": [-]"+text);
					photonView.RPC("receiveMessage", PhotonTargets.Others, "[8169FF]"+player.Handle+": [-]"+text);
				} else if(player.Team == "Guard") {
					textList.Add("[FF2B2B]"+player.Handle+": [-]"+text);
					photonView.RPC("receiveMessage", PhotonTargets.Others, "[FF2B2B]"+player.Handle+": [-]"+text);
				} else {
					textList.Add(player.Handle+": [-]"+text);
					photonView.RPC("receiveMessage", PhotonTargets.Others, player.Handle+": [-]"+text);
				}
				mInput.value = "";
			}
		}
	}

	bool testCommands(string message){
		string commandTest = message.Substring(0, message.IndexOf(" "));
		string targetTest = message.Substring(message.IndexOf(" ") + 1);

		switch(commandTest){
			case "/kick": 
					foreach(PhotonPlayer player in PhotonNetwork.playerList){
						if(targetTest == (string)player.customProperties["Handle"] && (player != PhotonNetwork.player)){
							photonView.RPC("kickPlayer", player);
							mInput.value = "";
							return true;
						}
					}
				break;

			case "/gameover":
					if(targetTest == "true" || targetTest == "True"){
						Intrigue.wantGameOver = true;
						mInput.value = "";
						textList.Add("[FFCC00]GameOver set to true");
						return true;
					}
					else if(targetTest == "false" || targetTest == "False"){
						Intrigue.wantGameOver = false;
						textList.Add("[FFCC00]GameOver set to false");
						mInput.value = "";
						return true;
					}
				break;
		}

		return false;
	}

	void checkHandle(){
		int successfulAttempts = 0;
		while(successfulAttempts<1){
			foreach(PhotonPlayer play in PhotonNetwork.playerList){
				if(player.Handle == (string)play.customProperties["Handle"] && (PhotonNetwork.player != play)){
					player.Handle = player.Handle + "1";
					PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Handle", player.Handle}});
					successfulAttempts--;
				}
				successfulAttempts++;
			}
		}
		
	}

	void syncPing(){
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Ping", PhotonNetwork.GetPing()}});
		reloadScoreboard();
	}

	void OnPhotonPlayerConnected(PhotonPlayer newPlayer){
		if(PhotonNetwork.isMasterClient){
			photonView.RPC("guestCount", PhotonTargets.Others, player.Guests);
			//photonView.RPC("receiveMessage", PhotonTargets.All,(string)newPlayer.customProperties["Handle"] + " [FFCC00]has joined the lobby![-]");
		}
		textList.Add((string)newPlayer.customProperties["Handle"] + " [FFCC00]has joined the lobby![-]");
	}

	void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer){
		photonView.RPC("reloadScoreboard", PhotonTargets.All);
		textList.Add((string)photonPlayer.customProperties["Handle"] + " [FFCC00]has disconnected.[-]");
	}

	void OnMasterClientSwitched(PhotonPlayer newMasterClient){
		textList.Add((string)newMasterClient.customProperties["Handle"] + "[FFCC00] is now the host.[-]");
		if(PhotonNetwork.player.ID == newMasterClient.ID){
			isReady = true;
			PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Ready", true}});
			slider.enabled = true;
		}
	}

	void updateGuestSlider(){
		if(PhotonNetwork.isMasterClient){
			player.Guests = Mathf.RoundToInt(slider.value*150);
			sliderLabel.text = "Guest Count: " + player.Guests;
			if(player.Guests != prevGuestCount){
				photonView.RPC("guestCount", PhotonTargets.Others, player.Guests);
				prevGuestCount = player.Guests;
			}
		}
		else{
			sliderLabel.text = ("Guest Count: " + player.Guests.ToString());
		}
	}

	void swapToSpy(){
		if(player.Team == "Spy" || gameStarting) return;
		player.Team = "Spy";
		player.TeamID = 1;
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Team", "Spy"}});
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"TeamID", "1"}});
		photonView.RPC("reloadScoreboard", PhotonTargets.All);
	}

	void swapToGuard(){
		if(player.Team == "Guard" || gameStarting) return;
		player.Team = "Guard";
		player.TeamID = 2;
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Team", "Guard"}});
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"TeamID", "2"}});
		photonView.RPC("reloadScoreboard", PhotonTargets.All);
	}

	void readyStatus(){
		//Ready Button
		if(PhotonNetwork.isMasterClient){
			allReady = true;
			foreach(PhotonPlayer playy in PhotonNetwork.playerList){
				if((bool)playy.customProperties["Ready"] == false){
					allReady = false;
					break;
				}
			}

			if( ((spies.Count > 0 && guards.Count > 0) || isTesting) &&
					allReady && !string.IsNullOrEmpty(player.Team)){
				readyLabel.text = "START GAME";
				readyLabel.fontSize = 28;
				readyCheckToggle.value = true;
			} else {	
				if(string.IsNullOrEmpty(player.Team)){
					readyLabel.text = "CHOOSE TEAM";
					readyLabel.fontSize = 28;
					readyCheckToggle.value = false;

				} else {
					readyLabel.text = "WAITING FOR OTHERS";
					readyLabel.fontSize = 24;
					readyCheckToggle.value = false;
				}
			}
		} else {
			if(!string.IsNullOrEmpty(player.Team)){
				readyLabel.text = "READY";
				readyLabel.fontSize = 40;
			} else {
				readyLabel.text = "CHOOSE TEAM";
				readyLabel.fontSize = 28;
			}
		}
	}

	void readyClick(){
		if(PhotonNetwork.isMasterClient){
			if( ((spies.Count > 0 && guards.Count > 0) || isTesting) &&
				readyCount == PhotonNetwork.playerList.Length-1 &&
				!string.IsNullOrEmpty(player.Team)){
				PhotonNetwork.room.visible = false;
				StartCoroutine(go());
			} else if(string.IsNullOrEmpty(player.Team)){
				textList.Add("[FF0000]Error:[-][FFCC00] Please choose a team.");
			} else {
				textList.Add("[FF0000]Error:[-][FFCC00] Each team must have at least one player!");
			}
		} else if(!gameStarting){
			if(isReady){
				isReady = false;
				readyCheckToggle.value = false;
				PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Ready", false}});
				photonView.RPC("ready", PhotonTargets.MasterClient, -1);
				photonView.RPC("reloadScoreboard", PhotonTargets.All);
			} else {
				if(!string.IsNullOrEmpty(player.Team)){
					isReady = true;
					readyCheckToggle.value = true;
					PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Ready", true}});
					photonView.RPC("ready", PhotonTargets.MasterClient, 1);
					photonView.RPC("reloadScoreboard", PhotonTargets.All);
				} else {
					textList.Add("[FF0000]Error:[-][FFCC00] Please choose a team.");
				}
			}

		}
	}
	
	void leaveLobby(){
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LoadLevel( "MainMenu" );
	}


	[RPC]
	void reloadScoreboard(){
		for(int x = guardTable.transform.childCount; x >0; --x){
			NGUITools.Destroy(guardTable.transform.GetChild(x-1).gameObject);
		}
		for(int x = spyTable.transform.childCount; x >0; --x){
			NGUITools.Destroy(spyTable.transform.GetChild(x-1).gameObject);
		}

		guards.Clear();
		spies.Clear();

		foreach(PhotonPlayer play in PhotonNetwork.playerList){
			string pingColor;
			if(play.customProperties["Ping"] != null)
				pingColor = calculatePingColor((int)play.customProperties["Ping"]);
			else
				pingColor = "[00FF00]";

			if((string)play.customProperties["Team"] == "Guard"){
				guards.Add(play.ID);
				spies.Remove(play.ID);
				GameObject playerInfo = NGUITools.AddChild(guardTable, playerPrefab);
				Vector3 temp = new Vector3(0f,(guards.Count-1)*0.1f,0);
				playerInfo.transform.position-=temp;
				UILabel label = playerInfo.GetComponent<UILabel>();
				label.user = play.ID;
				if((bool)play.customProperties["Ready"])
					label.text = "[FFFFFF]" + (string)play.customProperties["Handle"] + "   [[00FF00]READY[-]]   ("+ pingColor + (int)play.customProperties["Ping"]+"[-]" + ") ms";
				else
					label.text = "[FFFFFF]" + (string)play.customProperties["Handle"] + "   [[FF0000]READY[-]]   ("+ pingColor + (int)play.customProperties["Ping"]+"[-]" + ") ms";			
			}
			else if((string)play.customProperties["Team"] == "Spy"){
				spies.Add(play.ID);
				guards.Remove(play.ID);
				GameObject playerInfo = NGUITools.AddChild(spyTable, playerPrefab);
				Vector3 temp = new Vector3(0f,(spies.Count-1)*0.1f,0);
				playerInfo.transform.position-=temp;
				UILabel label = playerInfo.GetComponent<UILabel>();
				label.user = play.ID;
				if((bool)play.customProperties["Ready"])
					label.text = "[FFFFFF]" + (string)play.customProperties["Handle"] + "   [[00FF00]READY[-]]   ("+ pingColor + (int)play.customProperties["Ping"]+"[-]" + ") ms";
				else
					label.text = "[FFFFFF]" + (string)play.customProperties["Handle"] + "   [[FF0000]READY[-]]   ("+ pingColor + (int)play.customProperties["Ping"]+"[-]" + ") ms";
			}
		}
	}

	string calculatePingColor(int ping){
		string pingColor;
		if (ping<50)
			pingColor = "[00FF00]";
		else if(ping<100)
			pingColor = "[FF9D00]";
		else
			pingColor = "[FF0000]";
		return pingColor;
	}

	IEnumerator go(){
		photonView.RPC("sendStarting", PhotonTargets.Others);
		foreach(PhotonPlayer p in PhotonNetwork.playerList){
			if(PhotonNetwork.player != p){
				photonView.RPC("sendGo", p);
				yield return new WaitForSeconds(1f);
			}
		}
		sendGo();
	}

	[RPC]
	public void sendStarting(){
		gameStarting = true;
		gameStartingPanel.alpha = 1;
	}

	[RPC]
	public void sendGo(){
		CancelInvoke();
		if(player.Team == "Spy")
			PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"TeamID", 1}});
		else
			PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"TeamID", 2}});
		PhotonNetwork.LoadLevel("Intrigue");
	}

	[RPC]
	public void ready(int val){
		this.readyCount +=val;
	}

	[RPC]
	public void receiveMessage(string s){
		textList.Add(s);
	}

	[RPC]
	void guestCount(int guests){
		player.Guests = guests;
	}

	[RPC]
	void kickPlayer(){
		photonView.RPC("receiveMessage", PhotonTargets.Others, player.Handle + "[FF0000] has been kicked by the host");
		PlayerPrefs.SetString("banTime", System.DateTime.Now.ToBinary().ToString());
		Debug.Log("Banned at: " + System.DateTime.Now.ToString());
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LoadLevel( "MainMenu" );
	}
}
