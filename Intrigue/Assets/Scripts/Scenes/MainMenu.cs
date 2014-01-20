using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	private PhotonView photonView = null;

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
		GUILayout.Label( "Player Count:" + PhotonNetwork.playerList.Length );
		GUILayout.Label( "Your Id: " + PhotonNetwork.player.ID );
		GUILayout.Label( "Are You Master Server?? " + PhotonNetwork.isMasterClient );

		if( PhotonNetwork.connectionStateDetailed == PeerState.JoinedLobby) {
			if( GUILayout.Button("Create Room") ){
				PhotonNetwork.CreateRoom("Intrigue", true, true, 10);
			}

			foreach(RoomInfo room in PhotonNetwork.GetRoomList())
			{
				GUILayout.Label(room.ToString());
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
		PhotonNetwork.isMessageQueueRunning = false;
		Application.LoadLevel("PregameLobby");
	}

	void OnPhotonJoinFailed()
	{
		Debug.Log("FAIL");
	}
}
