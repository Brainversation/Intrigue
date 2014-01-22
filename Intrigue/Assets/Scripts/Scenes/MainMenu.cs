using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	private PhotonView photonView = null;
	private string roomName = "";
	
	public static string handle = "";

	// Use this for initialization
	void Start () {
		PhotonNetwork.isMessageQueueRunning = true;
		// What Photon settings to use and the version number
		PhotonNetwork.ConnectUsingSettings("0.1");

		// Get photonView component
		photonView = PhotonView.Get(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		// Tells us about the current network connection
		GUILayout.Label("Status: " + PhotonNetwork.connectionStateDetailed.ToString());

		if( PhotonNetwork.connectionStateDetailed == PeerState.JoinedLobby) {
			GUILayout.Label( "Room Name:");
			roomName = GUILayout.TextField(roomName, 25);

			GUILayout.Label( "Handle:");
			handle = GUILayout.TextField(handle, 25);

			if( roomName == "" || handle == "" ){
				GUILayout.Label( "Room Name and Handle must be filled to create room");
			} else if( GUILayout.Button("Create Room") ){
				PhotonNetwork.CreateRoom(roomName, true, true, 10);
				roomName = "";
			}

			if( handle == "" ){
				GUILayout.Label( "Handle must be filled to join room");
			} else {
				foreach(RoomInfo room in PhotonNetwork.GetRoomList())
				{
					GUILayout.Label(room.ToString());
					if(GUILayout.Button(room.name + " " + room.playerCount + "/" + room.maxPlayers)){
						PhotonNetwork.JoinRoom(room.name);
					}
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
		PhotonNetwork.isMessageQueueRunning = false;
		Application.LoadLevel("PregameLobby");
	}

	void OnPhotonJoinFailed()
	{
		Debug.Log("FAIL");
	}
}
