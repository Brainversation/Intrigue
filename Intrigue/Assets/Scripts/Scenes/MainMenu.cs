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
using System.Collections;
using System.IO;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MainMenu : MonoBehaviour {


	public GameObject btnJoinServer_prefab;
	public GameObject serverTable;
	public GameObject reconnectWindow;
	public GameObject createRoomFailed;
	public GameObject retryConnect;
	public GameObject attemptingConnection;
	public GameObject mask;
	public GameObject handleWindow;
	public GameObject serverNameLabel;
	public GameObject uiCamera;
	public GameObject bg_texture;
	public GameObject quickMatchFailed;
	public UILabel connectionStatus;
	public UISprite serverCreator;
	public UILabel banInfo;
	public UIPanel banPanel;

	[HideInInspector] public GameObject createServerButton;
	[HideInInspector] public GameObject findServerButton;
	[HideInInspector] public GameObject optionsButtons;
	[HideInInspector] public bool connected = false;
	
	private bool handleSet = false;
	private bool serverSet = false;
	private Player player;
	private UITable serverListTable;
	private string playerPrefsPrefix;

	void Start () {
		StringCleaner.createStringLists();
		Settings.Start();
		createServerButton = GameObject.Find("CREATE SERVER");
		findServerButton = GameObject.Find("FIND SERVER");
		optionsButtons = GameObject.Find("OptionsButtons");
		Screen.lockCursor = false;
		Camera.main.aspect = (16f/9f);
		player = Player.Instance;
		if(PhotonNetwork.connectionStateDetailed != PeerState.JoinedLobby)
			connect();

		//Resetting Variables
		resetVariables();
		Intrigue.resetVariables();

		//Player Prefs
		if (Application.isEditor)
			playerPrefsPrefix = "PlayerEditor";
		else
			playerPrefsPrefix = "Player";

		if( PlayerPrefs.HasKey( playerPrefsPrefix + "Name" ) &&
				PlayerPrefs.GetString(playerPrefsPrefix + "Name") != string.Empty ){
			player.Handle = PlayerPrefs.GetString(playerPrefsPrefix + "Name");
			PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Handle", player.Handle}});		
			handleSet = true;
		}

		if( PlayerPrefs.HasKey( playerPrefsPrefix + "Room" ) &&
				PlayerPrefs.GetString(playerPrefsPrefix + "Room") != string.Empty  ){
			player.RoomName = PlayerPrefs.GetString(playerPrefsPrefix + "Room");
			serverSet = true;
		}

		if(handleSet){
			mask.GetComponent<TweenAlpha>().PlayForward();
			NGUITools.SetActive(handleWindow,false);
			handleWindow.GetComponent<TweenAlpha>().PlayReverse();
		}

		if(serverSet){
			serverNameLabel.GetComponent<UILabel>().text = player.RoomName;
		}
	}
	
	void Update () {
		if(!connected){
			connectionStatus.text = "[FF0000]No Internet Connection[-]";
			if(PhotonNetwork.connectionStateDetailed == PeerState.ConnectingToMasterserver){
				connectingAttempt();
			}
			else if (PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated || PhotonNetwork.connectionStateDetailed == PeerState.Disconnected) {
				noInternet();
			}
			else{
				yesInternet();
			}
		}
		else{
			 switch (PhotonNetwork.connectionStateDetailed)
            	{
					case PeerState.PeerCreated: connectionStatus.text = "[FF3300]Unable to Connect to Server";
						break;
					case PeerState.Disconnected: connectionStatus.text = "[FF3300]Unable to Connect to Server";
						break;
					case PeerState.Disconnecting: connectionStatus.text = "[FF3300]Disconnecting from Server";
						break;
					case PeerState.Authenticating: connectionStatus.text = "[FFCC00]Authenticating";
						break;
					case PeerState.ConnectingToGameserver: connectionStatus.text = "[FF3300]Connecting to Game...";
						break;
					case PeerState.ConnectingToMasterserver: connectionStatus.text = "[FF3300]Connecting to Server";
						break;
					case PeerState.ConnectingToNameServer: connectionStatus.text = "[FF3300]Connecting...";
						break;
					case PeerState.Joining: connectionStatus.text = "[009900]Joining Lobby...";
						break;
					case PeerState.Leaving: connectionStatus.text = "[009900]Disconnecting from Lobby";
						break;
					case PeerState.Uninitialized: connectionStatus.text = "[FF0000]Connection Uninitialized";
						break;
					case PeerState.Authenticated: connectionStatus.text = "[00FF00]Authenticated";
						break;		
					case PeerState.JoinedLobby: connectionStatus.text = "[00FF00]Connected to Server";
						break;
					case PeerState.DisconnectingFromMasterserver: connectionStatus.text = "[FF3300]Disconnecting from Server";
						break;	
					case PeerState.ConnectedToGameserver: connectionStatus.text = "[00FF00]Connected to Server";
						break;	
					default: connectionStatus.text = "[800000]Connection : " + PhotonNetwork.connectionStateDetailed.ToString();
						break;

            	}

            if(banPanel.alpha == 1){
    			long temp = Convert.ToInt64(PlayerPrefs.GetString("banTime"));
				TimeSpan timeSinceBan = System.DateTime.Now.Subtract(DateTime.FromBinary(temp));
				int minutes = timeSinceBan.Minutes;
				int seconds = timeSinceBan.Seconds;
				banInfo.text = "[FF2B2B]" + minutes + ":" + (60-seconds);
            }


		}
		checkInternet();
	}

	void resetVariables(){
		player.Score = 0;
		player.TeamID = 0;
		player.Team1Score = 0;
		player.Team2Score = 0;
		player.Team = "";
		Intrigue.resetVariables();
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Team", ""},
																{"Score", 0},
																{"Ready", false},
																{"Team1Score", 0},
																{"Team2Score", 0}});
	}

	void checkInternet(){
		if(PhotonNetwork.connectionStateDetailed == PeerState.ConnectingToMasterserver){
			connected = false;
		}
		else if (PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated || PhotonNetwork.connectionStateDetailed == PeerState.Disconnected) {
			connected = false;
		}
		else{
			connected = true;
		}
	}

	void connectingAttempt(){
		createServerButton.GetComponent<UIButton>().enabled = false;
		findServerButton.GetComponent<UIButton>().enabled = false;
		NGUITools.SetActive(reconnectWindow, true);
		reconnectWindow.GetComponentInChildren<TweenAlpha>().PlayForward();
		NGUITools.SetActive(retryConnect, false);
		NGUITools.SetActive(optionsButtons, false);
		NGUITools.SetActive(createRoomFailed, false);
		NGUITools.SetActive(quickMatchFailed, false);
	}

	void noInternet(){
		createServerButton.GetComponent<UIButton>().enabled = false;
		findServerButton.GetComponent<UIButton>().enabled = false;
		NGUITools.SetActive(reconnectWindow,true);
		NGUITools.SetActive(uiCamera, true);
		NGUITools.SetActive(retryConnect, true);
		NGUITools.SetActive(attemptingConnection, false);
	}

	void yesInternet(){
		NGUITools.SetActiveChildren(gameObject, true);
		NGUITools.SetActive(reconnectWindow,false);
		NGUITools.SetActive(createRoomFailed, false);
		NGUITools.SetActive(quickMatchFailed, false);
		NGUITools.SetActive(optionsButtons, false);
		createServerButton.GetComponent<UIButton>().enabled = true;
		findServerButton.GetComponent<UIButton>().enabled = true;
	}

	void onFindServerClicked(){
		int serverNum = 0;
		foreach (RoomInfo room in PhotonNetwork.GetRoomList()) {
			GameObject serverInfo = NGUITools.AddChild (serverTable, btnJoinServer_prefab);
			UILabel[] serverButText = serverInfo.GetComponentsInChildren<UILabel>();
			Vector3 temp = new Vector3(0.52f,-0.1f-((serverNum)*0.12f),0);
			serverInfo.transform.position+=temp;
			serverButText[0].text = room.name;
			serverButText[1].text = room.playerCount+"/"+room.maxPlayers;
			serverNum++;
		}
	}

	void refreshServerList(){
		foreach(Transform child in serverTable.transform){
				NGUITools.Destroy(child.gameObject);
		}
		onFindServerClicked();
	}


	void onOptionsClicked(){
		if(PlayerPrefs.GetString(playerPrefsPrefix + "Name") != string.Empty)
			PlayerPrefs.SetString(playerPrefsPrefix + "Name", player.Handle);
		PlayerPrefs.Save();
		Application.LoadLevel("Settings");
	}

	public void onCreditsClicked(){
		if(PlayerPrefs.GetString(playerPrefsPrefix + "Name") != string.Empty)
			PlayerPrefs.SetString(playerPrefsPrefix + "Name", player.Handle);
		PlayerPrefs.Save();
		Application.LoadLevel("Credits");
	}

	public void onTutorialClicked(){
		if(PlayerPrefs.GetString(playerPrefsPrefix + "Name") != string.Empty)
			PlayerPrefs.SetString(playerPrefsPrefix + "Name", player.Handle);
		PlayerPrefs.Save();
		Application.LoadLevel("Tutorial");
	}

	void onExitGameClicked(){
		Application.Quit();
	}

	void createTheServer(){
		UILabel serverName = GameObject.Find("serverName").GetComponent<UILabel>();
		if(serverName!= null && serverCreator.alpha == 1){
			player.RoomName = serverName.text;
			RoomOptions roomOp = new RoomOptions();
			roomOp.isVisible = true;
			roomOp.isOpen = true;
			roomOp.maxPlayers = 10;
			PhotonNetwork.CreateRoom(player.RoomName, roomOp, null);
		}
		
	}

	void getUserHandle(){
		UILabel playerName = GameObject.Find ("playerName").GetComponent<UILabel>();
		player.Handle = playerName.text;
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Handle", player.Handle}});		
		PlayerPrefs.SetString(playerPrefsPrefix + "Name", player.Handle);
	}

	void connect(){
		// What Photon settings to use and the version number
		connectingAttempt();
		PhotonNetwork.ConnectUsingSettings("0.1");
	}

	void quickMatch(){
		if(!isBanned())
			PhotonNetwork.JoinRandomRoom();
	}

	public void showBanInfo(){
		if( PlayerPrefs.HasKey("banTime") && PlayerPrefs.GetString("banTime") != string.Empty ){
			banPanel.alpha = 1;
			long temp = Convert.ToInt64(PlayerPrefs.GetString("banTime"));
			TimeSpan timeSinceBan = System.DateTime.Now.Subtract(DateTime.FromBinary(temp));
			Invoke("disableBanPanel",(60f-(float)timeSinceBan.TotalSeconds));
		}
	}

	public void disableBanPanel(){
		banPanel.alpha = 0;
	}

	public bool isBanned(){
		if( PlayerPrefs.HasKey("banTime") && PlayerPrefs.GetString("banTime") != string.Empty ){
			long temp = Convert.ToInt64(PlayerPrefs.GetString("banTime"));
			TimeSpan timeSinceBan = System.DateTime.Now.Subtract(DateTime.FromBinary(temp));
			if(timeSinceBan.TotalMinutes<1){
				showBanInfo();
				return true;
			}
		}
		return false;
	}

	void OnPhotonRandomJoinFailed(){
		RoomOptions roomOp = new RoomOptions();
		roomOp.isVisible = true;
		roomOp.isOpen = true;
		roomOp.maxPlayers = 10;
		PhotonNetwork.CreateRoom(player.Handle + "'s Quick Match", roomOp, null);
		player.RoomName = player.Handle + "'s Quick Match";
		NGUITools.SetActive(quickMatchFailed, true);
		CancelInvoke();
		Invoke("deactiveErrorMessages", 5);
	}

	void OnJoinedRoom(){
		if(!isBanned()){
			if(PlayerPrefs.GetString(playerPrefsPrefix + "Name") != string.Empty)
				PlayerPrefs.SetString(playerPrefsPrefix + "Name", player.Handle);
			if(PlayerPrefs.GetString(playerPrefsPrefix + "Room") != string.Empty)
				PlayerPrefs.SetString(playerPrefsPrefix + "Room", player.RoomName);
			PlayerPrefs.Save();
			PhotonNetwork.LoadLevel("Pregame");
		}
	}

	void OnPhotonJoinFailed(){
		NGUITools.SetActive(createRoomFailed, true);
		CancelInvoke();
		Invoke("deactiveErrorMessages", 5);
	}

	void OnPhotonCreateRoomFailed(){
		NGUITools.SetActive(createRoomFailed, true);
		CancelInvoke();
		Invoke("deactiveErrorMessages", 5);
	}

	public void deactiveErrorMessages(){
		NGUITools.SetActive(createRoomFailed, false);
		NGUITools.SetActive(quickMatchFailed, false);
	}
}
