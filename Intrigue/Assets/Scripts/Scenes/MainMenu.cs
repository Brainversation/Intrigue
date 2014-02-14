using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	private Player player;

	void Start () {
		Screen.lockCursor = false;
		PhotonNetwork.isMessageQueueRunning = true;
		
		connect();

		player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		// Tells us about the current network connection
		GUILayout.Label("Status: " + PhotonNetwork.connectionStateDetailed.ToString());
		if( PhotonNetwork.connectionStateDetailed == PeerState.JoinedLobby) {
			GUILayout.Label( "Room Name:");
			player.RoomName = GUILayout.TextField(player.RoomName, 25);

			GUILayout.Label( "Handle:");
			player.Handle = GUILayout.TextField(player.Handle, 25);

			if( player.RoomName == "" || player.Handle == "" ){
				GUILayout.Label( "Room Name and Handle must be filled to create room" );
			} else if( GUILayout.Button("Create Room") ){
				PhotonNetwork.CreateRoom(player.RoomName, true, true, 10);
			}

			if( player.Handle == "" ){
				GUILayout.Label( "Handle must be filled to join room");
			} else {
				foreach(RoomInfo room in PhotonNetwork.GetRoomList())
				{
					if(GUILayout.Button(room.name + " " + room.playerCount + "/" + room.maxPlayers)){
						PhotonNetwork.JoinRoom(room.name);
					}
				}
			}
		} else if( PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated) {
			GUILayout.Label("No Internet Connection! Connect to internet and press retry");
			if(GUILayout.Button("Retry")){
				connect();
			}
		}
	}

	void connect(){
		// What Photon settings to use and the version number
		PhotonNetwork.ConnectUsingSettings("0.1");
	}

	// Called after joining a room
	void OnJoinedLobby(){
		Debug.Log("joined lobby");
	}

	void OnJoinedRoom(){
		Debug.Log("joined room");
		PhotonNetwork.isMessageQueueRunning = false;
		Application.LoadLevel("Pregame");
	}

	void OnPhotonJoinFailed()
	{
		Debug.Log("OnPhotonJoinFailed");
	}
}
