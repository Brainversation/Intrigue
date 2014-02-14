using UnityEngine;
using System.Collections;
using System.IO;

public class MainMenu : MonoBehaviour {

	private Player player;
	private int menuItemClicked;

	void Start () {
		Screen.lockCursor = false;
		PhotonNetwork.isMessageQueueRunning = true;
		player = GameObject.Find("Player").GetComponent<Player>();
		
		if(File.Exists(filePath) ){
			string line = File.ReadAllText(filePath);
			int i = 0;
			foreach(string l in line.Split('\n')){
				if(i == 0){
					player.Handle = l;
				} else if(i == 1){
					player.RoomName = l;
				}
				++i;
			}
		}
		file = File.CreateText(filePath);
	}
	
	// Update is called once per frame
	void Update () {
		// Checked to see if connected to master server
//		if (PhotonNetwork.connectionStateDetailed == PeerState.JoinedLobby) {
//			Debug.Log("Server Found");
//		}
	}

	void onFindServerClicked(){
		menuItemClicked = 0;
	}

	void onCreateServerClicked(){
		menuItemClicked = 1;
	}

	void onOptionsClicked(){
		menuItemClicked = 2;
	}

	void onCreditsClicked(){
		menuItemClicked = 3;
	}

	void onExitGameClicked(){
		menuItemClicked = 4;
		Application.Quit();
	}

	void createTheServer(){
		UIInput serverName = GameObject.Find ("serverName").GetComponent<UIInput> ();
		player.RoomName = serverName.text;
		PhotonNetwork.CreateRoom(player.RoomName, true, true, 10);
	}

	void getUserHandle(){
		UIInput playerName = GameObject.Find ("playerName").GetComponent<UIInput> ();
		player.Handle = playerName.text;
		Debug.Log (player.Handle);
	}

//	void OnGUI(){
//		// Tells us about the current network connection
//		GUILayout.Label("Status: " + PhotonNetwork.connectionStateDetailed.ToString());
//		if( PhotonNetwork.connectionStateDetailed == PeerState.JoinedLobby) {
//			GUILayout.Label( "Room Name:");
//			player.RoomName = GUILayout.TextField(player.RoomName, 25);
//
//			GUILayout.Label( "Handle:");
//			player.Handle = GUILayout.TextField(player.Handle, 25);
//
//			if( player.RoomName == "" || player.Handle == "" ){
//				GUILayout.Label( "Room Name and Handle must be filled to create room" );
//			} else if( GUILayout.Button("Create Room") ){
//				PhotonNetwork.CreateRoom(player.RoomName, true, true, 10);
//			}
//
//			if( player.Handle == "" ){
//				GUILayout.Label( "Handle must be filled to join room");
//			} else {
//				foreach(RoomInfo room in PhotonNetwork.GetRoomList())
//				{
//					if(GUILayout.Button(room.name + " " + room.playerCount + "/" + room.maxPlayers)){
//						PhotonNetwork.JoinRoom(room.name);
//					}
//				}
//			}
//		} else if( PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated) {
//			GUILayout.Label("No Internet Connection! Connect to internet and press retry");
//			if(GUILayout.Button("Retry")){
//				connect();
//			}
//		}
//	}

	void connect(){
		// What Photon settings to use and the version number
		PhotonNetwork.ConnectUsingSettings("0.1");
	}

	// Called after joining a lobby(Connecting To Server)
	void OnJoinedLobby(){
		//Debug.Log("joined lobby");
	}

	void OnJoinedRoom(){
		//Debug.Log("joined room");
		PhotonNetwork.isMessageQueueRunning = false;
		Application.LoadLevel("PregameLobby");
	}

	void OnPhotonJoinFailed(){
		Debug.Log("OnPhotonJoinFailed");
	}

	void OnApplicationQuit() {
		file.WriteLine(player.Handle);
		file.WriteLine(player.RoomName);
		file.Close();
	}
}
