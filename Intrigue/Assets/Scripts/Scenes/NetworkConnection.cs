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
				gui.SetActive(false);
				showRetry = false;
				PhotonNetwork.LoadLevel(0);
			}
		}
	}

	void OnPhotonPlayerConnected(PhotonPlayer newPlayer){
		if(PhotonNetwork.player.ID == newPlayer.ID){
			Debug.Log("Reconnected");
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
			photonView.RPC("removeGuard", PhotonTargets.All);
			player.GetComponent<BasePlayer>().newEvent("[FF2B2B]" + (string)photonPlayer.customProperties["Handle"]  + "[-][FFCC00] has disconnected.[-]");
		} else {
			photonView.RPC("removeSpy", PhotonTargets.All);
			player.GetComponent<BasePlayer>().newEvent("[00CCFF]" + (string)photonPlayer.customProperties["Handle"]  + "[-][FFCC00] has disconnected.[-]");
		}
	}

	void OnMasterClientSwitched(PhotonPlayer newMasterClient){
		if(PhotonNetwork.player.ID == newMasterClient.ID){
			Debug.Log("Master Switch");
			GameObject.FindWithTag("Scripts").GetComponent<Intrigue>().enabled = true;
		}
	}

	void OnJoinedRoom(){
		Debug.Log("OnJoinedRoom");
		gui.SetActive(false);
		showRetry = false;

		//Check if game over or same round or something, then add it

		Intrigue.playerGO = PhotonNetwork.Instantiate(
						"Robot_"+ player.Team+"1"/*type.ToString()*/,
						lastPos,
						Quaternion.identity, 0);
	}

	void OnDisconnectedFromPhoton(){
		Debug.Log("OnDisconnect");
		lastPos = Intrigue.playerGO.transform.position;
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
