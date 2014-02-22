using UnityEngine;
using System.Collections;
using System.IO;

public class MainMenu : MonoBehaviour {

	private Player player;
	private int menuItemClicked;
	private int numOfServers;
	private UITable serverListTable;
	public GameObject btnJoinServer_prefab;
	public GameObject serverTable;

	void Start () {
		Screen.lockCursor = false;
		PhotonNetwork.isMessageQueueRunning = true;
		player = GameObject.Find("Player").GetComponent<Player>();
		menuItemClicked = -1;
		numOfServers = 0;
		connect();
	}
	
	// Update is called once per frame
	void Update () {
		// Checked to see if connected to master server
//		if (PhotonNetwork.connectionStateDetailed == PeerState.JoinedLobby && menuItemClicked == 0) {
//			//Debug.Log("Server Found");
//
//		}
	}

	void onFindServerClicked(){
		menuItemClicked = 0;
		int serverNum = 0;
		foreach (RoomInfo room in PhotonNetwork.GetRoomList()) {
			GameObject serverInfo = NGUITools.AddChild (serverTable, btnJoinServer_prefab);
			UILabel[] serverButText = serverInfo.GetComponentsInChildren<UILabel>();
			Vector3 temp = new Vector3(0.52f,-0.1f+(serverNum)*0.12f,0);
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
		Debug.Log("Refreshed Servers");
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
	}

	void connect(){
		// What Photon settings to use and the version number
		PhotonNetwork.ConnectUsingSettings("0.1");
	}

	// Called after joining a lobby(Connecting To Server)
	void OnJoinedLobby(){
	}

	void OnJoinedRoom(){
		PhotonNetwork.LoadLevel("Pregame");
	}

	void OnPhotonJoinFailed(){
		Debug.Log("OnPhotonJoinFailed");
	}
}
