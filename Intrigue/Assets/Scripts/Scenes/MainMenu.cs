using UnityEngine;
using System.Collections;
using System.IO;

public class MainMenu : MonoBehaviour {

	private Player player;
	private bool internetOn = false;
	private int numOfServers;
	private UITable serverListTable;
	public GameObject btnJoinServer_prefab;
	public GameObject serverTable;
	private string filePath;
	private StreamWriter file;
	private bool handleSet = false;
	private bool serverSet = false;
	public GameObject reconnectWindow;
	public GameObject retryConnect;
	public GameObject attemptingConnection;
	public GameObject mask;
	public GameObject handleWindow;
	public GameObject serverNameLabel;
	public LoadingScreen loadingScreen;
	public GameObject uiCamera;
	public GameObject bg_texture;
	[HideInInspector] public GameObject createServerButton;
	[HideInInspector] public GameObject findServerButton;
	void Start () {
		createServerButton = GameObject.Find("CREATE SERVER");
		findServerButton = GameObject.Find("FIND SERVER");
		Screen.lockCursor = false;
		PhotonNetwork.networkingPeer.NewSceneLoaded();
		player = GameObject.Find("Player").GetComponent<Player>();
		numOfServers = 0;
		connect();
		player.Score = 0;
		player.TeamID = 0;
		player.TeamScore = 0;
		player.EnemyScore = 0;
		//File Stuff
		if (Application.isEditor)
			filePath = Application.persistentDataPath + "/PlayerEditor.txt";
		else
			filePath = Application.persistentDataPath + "/Player.txt";
		if(File.Exists(filePath) ){
			string line = File.ReadAllText(filePath);
			int i = 0;
			foreach(string l in line.Split('\n')){
				if(i == 0){
					player.Handle = l;
					if(player.Handle!="" && player.Handle!=null)
						handleSet = true;
				} else if(i == 1){
					player.RoomName = l;
					if(player.RoomName!="" && player.RoomName!=null)
						serverSet = true;
				}
				++i;
			}
		}
		file = File.CreateText(filePath);

		if(handleSet){
			mask.GetComponent<TweenAlpha>().PlayForward();
			NGUITools.SetActive(handleWindow,false);
			handleWindow.GetComponent<TweenAlpha>().PlayReverse();
		}
		if(serverSet){
			serverNameLabel.GetComponent<UILabel>().text = player.RoomName;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(PhotonNetwork.connectionStateDetailed == PeerState.Connecting){
			connectingAttempt();
		}
		else if (PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated || PhotonNetwork.connectionStateDetailed == PeerState.Disconnected) {
			internetOn = false;
			noInternet();
		}
		else{
			internetOn = true;
			yesInternet();
		}
	}

	void connectingAttempt(){
		//NGUITools.SetActiveChildren(gameObject,false);
		createServerButton.GetComponent<UIButton>().enabled = false;
		findServerButton.GetComponent<UIButton>().enabled = false;
		NGUITools.SetActive(reconnectWindow, true);
		reconnectWindow.GetComponentInChildren<TweenAlpha>().PlayForward();
		NGUITools.SetActive(retryConnect, false);
	}

	void noInternet(){
		//NGUITools.SetActiveChildren(gameObject,false);
		createServerButton.GetComponent<UIButton>().enabled = false;
		findServerButton.GetComponent<UIButton>().enabled = false;
		NGUITools.SetActive(reconnectWindow,true);
		NGUITools.SetActive(uiCamera, true);
		//NGUITools.SetActive(bg_texture, true);
		NGUITools.SetActive(attemptingConnection, false);
	}

	void yesInternet(){
		NGUITools.SetActiveChildren(gameObject, true);
		NGUITools.SetActive(reconnectWindow,false);
		createServerButton.GetComponent<UIButton>().enabled = true;
		findServerButton.GetComponent<UIButton>().enabled = true;
	}

	void onFindServerClicked(){
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
	}

	void onOptionsClicked(){
	}

	void onCreditsClicked(){
	}

	void onExitGameClicked(){
		Application.Quit();
	}

	void createTheServer(){
		UIInput serverName = GameObject.Find("serverName").GetComponent<UIInput>();
		player.RoomName = serverName.value;
		PhotonNetwork.CreateRoom(player.RoomName, true, true, 10);
	}

	void getUserHandle(){
		UIInput playerName = GameObject.Find ("playerName").GetComponent<UIInput> ();
		player.Handle = playerName.value;
	}

	void connect(){
		// What Photon settings to use and the version number
		connectingAttempt();
		PhotonNetwork.ConnectUsingSettings("0.1");
	}

	// Called after joining a lobby(Connecting To Server)
	void OnJoinedLobby(){

	}

	void OnJoinedRoom(){
		file.WriteLine(player.Handle);
		file.WriteLine(player.RoomName);
		file.Close();
		loadingScreen.StartLoadingLevel("Pregame");
		//PhotonNetwork.LoadLevel("Pregame");
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
