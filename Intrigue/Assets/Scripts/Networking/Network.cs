using UnityEngine;
using System.Collections;

public class Network : MonoBehaviour {

	private string textField = "";
	private string chatBox = "";
	private PhotonView photonView = null;
	private Vector2 scrollPositionChat = new Vector2(0, 0);
	private GUIStyle styleChat = new GUIStyle();


	// Look up how to disconnect
	void Start () {
		// What Photon settings to use and the version number
		PhotonNetwork.ConnectUsingSettings("0.1");

		// Get photonView component
		photonView = PhotonView.Get(this);

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
		GUILayout.Label( "Your Id: " + PhotonNetwork.player.ID );
		GUILayout.Label( "Are You Master Server??" + PhotonNetwork.isMasterClient );
		
		//Checks state of connection: Look up PeerState
		if (PhotonNetwork.connectionStateDetailed == PeerState.Joined){
			//Chat Box
			scrollPositionChat = GUILayout.BeginScrollView(scrollPositionChat, GUILayout.Width ( Screen.width / 2 - 400), GUILayout.MaxHeight(190), GUILayout.ExpandHeight (false));
			GUI.skin.box.alignment = TextAnchor.UpperLeft;
			GUILayout.Box(this.chatBox, this.styleChat, GUILayout.ExpandHeight (true));
			GUILayout.EndScrollView();

			textField = GUILayout.TextField( textField, 25 );
			if( GUILayout.Button("Send") && textField != "" ){ 
				photonView.RPC("recieveMessage", PhotonTargets.All, textField);
				textField = "";
			}
		} else {
			if( GUILayout.Button("Create Room") ){
			PhotonNetwork.CreateRoom("Intrigue");
			}

			foreach(RoomInfo room in PhotonNetwork.GetRoomList())
			{
				if(GUILayout.Button(room.name + " " + room.playerCount + "/" + room.maxPlayers)){
					PhotonNetwork.JoinRoom(room.name);
				}
			}
		}
	}

	// Called after joining a room
	void OnJoinedLobby(){
		Debug.Log("joined lobby");
	}

	void OnJoinedRoom(){
		Debug.Log("joined room");
	}

	void OnPhotonJoinFailed()
	{
		Debug.Log("FAIL");
	}

	void OnPhotonPlayerDisconnected(PhotonPlayer player){
		Debug.Log("OnPhotonPlayerDisconnected: " + player.ID );

		if (PhotonNetwork.isMasterClient){
			//Move Info towards to new master Client, but master client switches on its own
		}
	}

	[RPC]
	public void recieveMessage(string s){
		this.chatBox += PhotonNetwork.player.ID + ": " + s + "\n";
	}
}
