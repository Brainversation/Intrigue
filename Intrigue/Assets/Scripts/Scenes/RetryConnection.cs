using UnityEngine;
using System.Collections;

public class RetryConnection : Photon.MonoBehaviour {

	private Player player;
	private bool showRetry = false;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
	
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
				PhotonNetwork.JoinRoom(player.RoomName);
			}
			if(GUILayout.Button("Exit")){
				PhotonNetwork.LoadLevel(0);
			}
		}
	}

	void OnJoinedRoom(){
		Debug.Log("HERE");
	}

	void OnDisconnectedFromPhoton(){
		showRetry = true;
	}

	void OnPhotonJoinRoomFailed(){
		Debug.Log("FAILED ROOM");
	}
}
