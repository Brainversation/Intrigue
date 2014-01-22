﻿using UnityEngine;
using System.Collections;

public class PregameLobby : MonoBehaviour {

	private PhotonView photonView = null;
	private Vector2 scrollPositionChat = new Vector2(0, 0);
	private GUIStyle styleChat = new GUIStyle();
	private GameObject cube = null;
	private string chatBox = "";
	private string textField = "";
	private float numOfGuests = 0.0f;

	public static string team = "";

	// Use this for initialization
	void Start () {
		PhotonNetwork.isMessageQueueRunning = true;
		// Get photonView component
		this.photonView = PhotonView.Get(this);
		this.styleChat.fontSize = 12;
		this.styleChat.normal.textColor = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		// Tells us about the current network connection
		GUILayout.Label("Status: " + PhotonNetwork.connectionStateDetailed.ToString());
		GUILayout.Label( "Player Count:" + PhotonNetwork.playerList.Length );
		GUILayout.Label( "Handle: " + MainMenu.handle );
		GUILayout.Label( "Team: "+ team);
		GUILayout.Label( "Id: " + PhotonNetwork.player.ID );
		GUILayout.Label( "Are You Master Client?? " + PhotonNetwork.isMasterClient );
		
		//Checks state of connection: Look up PeerState
		if( PhotonNetwork.connectionStateDetailed == PeerState.Joined ){
			//Chat Box
			this.scrollPositionChat = GUILayout.BeginScrollView(this.scrollPositionChat, GUILayout.Width ( Screen.width/4 ), GUILayout.MaxHeight(190), GUILayout.ExpandHeight (false));
			GUI.skin.box.alignment = TextAnchor.UpperLeft;
			GUILayout.Box(this.chatBox, this.styleChat, GUILayout.ExpandHeight(true));
			GUILayout.EndScrollView();


			GUI.SetNextControlName("ChatBox");
			textField = GUILayout.TextField( textField, 100 );
			if( ( GUILayout.Button("Send") ||
				(Event.current.type == EventType.keyDown && 
				 Event.current.character == '\n' &&
				 GUI.GetNameOfFocusedControl() == "ChatBox") ) 
				&& textField != ""  ){ 
				photonView.RPC("recieveMessage", PhotonTargets.AllBuffered, (PhotonNetwork.player.ID + ": " + textField) );
				textField = "";
				this.scrollPositionChat.y = Mathf.Infinity;
			}
			if( GUILayout.Button( "Leave Room" ) ){
				PhotonNetwork.LeaveRoom();
			}
			if(GUILayout.Button( "Play as Spy")){
				team = "Spy";
			}
			if(GUILayout.Button( "Play as Guard")){
				team = "Guard";
			}
			if( PhotonNetwork.isMasterClient ){
				numOfGuests = GUILayout.HorizontalSlider(numOfGuests, 0.0f, 5.0f);
				GUILayout.Label( "Number of Guests: " + Mathf.RoundToInt(numOfGuests) );
				if(GUILayout.Button( "PLAY INTRIGUE") && team != ""){
					PlayerPrefs.SetInt("numOfGuests", Mathf.RoundToInt(numOfGuests));
					photonView.RPC("go", PhotonTargets.AllBuffered);
				}
			}
		}
	}

	void OnPhotonPlayerDisconnected(PhotonPlayer player){
		Debug.Log("OnPhotonPlayerDisconnected: " + player.ID );

		if (PhotonNetwork.isMasterClient){
			//Move Info towards to new master Client, but master client switches on its own
		}
	}

	[RPC]
	public void recieveMessage(string s){
		this.chatBox += s + "\n";
	}
	
	[RPC]
	public void go(){
		PhotonNetwork.isMessageQueueRunning = false;
		Application.LoadLevel("Intrigue");
	}
}
