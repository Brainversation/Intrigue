using UnityEngine;
using System.Collections;

public class NetworkConnection : Photon.MonoBehaviour {

	private Player player;
	private bool showRetry = false;
	private GameObject loading;
	private Vector3 lastPos = Vector3.zero;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Player>();
		loading = GameObject.Find("LoadingScreen");
	}

	void OnGUI(){
		GUILayout.Label("Status: " + PhotonNetwork.connectionStateDetailed.ToString());

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
				showRetry = false;
				PhotonNetwork.LoadLevel(0);
			}
		} else {
			if(GUILayout.Button("Disconnect")){
				PhotonNetwork.networkingPeer.Disconnect();
			}

			if(GUILayout.Button("GameOver")){
				Intrigue.wantGameOver = true;
			}
		}
	}

	void OnPhotonPlayerConnected(PhotonPlayer newPlayer){
		if(PhotonNetwork.player.ID == newPlayer.ID){
			Debug.Log("Reconnected");
			if(player.Team == "Guard")
				photonView.RPC("addGuard", PhotonTargets.All);
			else
				photonView.RPC("addSpy", PhotonTargets.All);

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
			Debug.Log("Master Switch");
			GameObject.FindWithTag("Scripts").GetComponent<Intrigue>().enabled = true;
		}
	}

	void OnJoinedRoom(){
		Debug.Log("OnJoinedRoom");
		showRetry = false;
		Intrigue.playerGO = PhotonNetwork.Instantiate(
						"Robot_"+ player.Team+"1"/*type.ToString()*/,
						lastPos,
						Quaternion.identity, 0);
	}

	void OnDisconnectedFromPhoton(){
		Debug.Log("OnDisconnect");
		lastPos = Intrigue.playerGO.transform.position;
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
	void addSpy(){
		++Intrigue.numSpiesLeft;
	}

	[RPC]
	void addGuard(){
		++Intrigue.numGuardsLeft;
	}
}
