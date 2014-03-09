using UnityEngine;
using System.Collections;
using System.IO;

public class MainMenu : MonoBehaviour {

	private Player player;
	private UITable serverListTable;
	public GameObject btnJoinServer_prefab;
	public GameObject serverTable;
	private bool handleSet = false;
	private bool serverSet = false;
	private string playerPrefsPrefix;
	public GameObject reconnectWindow;
	public GameObject createRoomFailed;
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
	[HideInInspector] public GameObject optionsButtons;
	[HideInInspector] public bool connected = false;

	void Start () {
		createServerButton = GameObject.Find("CREATE SERVER");
		findServerButton = GameObject.Find("FIND SERVER");
		optionsButtons = GameObject.Find("OptionsButtons");
		Screen.lockCursor = false;
		PhotonNetwork.networkingPeer.NewSceneLoaded();
		player = GameObject.Find("Player").GetComponent<Player>();
		connect();
		player.Score = 0;
		player.TeamID = 0;
		player.TeamScore = 0;
		player.EnemyScore = 0;

		//Player Prefs
		if (Application.isEditor)
			playerPrefsPrefix = "PlayerEditor";
		else
			playerPrefsPrefix = "Player";

		if( PlayerPrefs.HasKey( playerPrefsPrefix + "Name" ) &&
				PlayerPrefs.GetString(playerPrefsPrefix + "Name") != "" ){
			player.Handle = PlayerPrefs.GetString(playerPrefsPrefix + "Name");
			handleSet = true;
		}

		if( PlayerPrefs.HasKey( playerPrefsPrefix + "Room" ) &&
				PlayerPrefs.GetString(playerPrefsPrefix + "Room") != ""  ){
			player.RoomName = PlayerPrefs.GetString(playerPrefsPrefix + "Room");
			serverSet = true;
		}

		if(handleSet){
			mask.GetComponent<TweenAlpha>().PlayForward();
			NGUITools.SetActive(handleWindow,false);
			handleWindow.GetComponent<TweenAlpha>().PlayReverse();
		}

		if(serverSet){
			serverNameLabel.GetComponent<UILabel>().text = player.RoomName;
		}
	}
	
	void Update () {
		if(!connected){
			if(PhotonNetwork.connectionStateDetailed == PeerState.Connecting){
				connectingAttempt();
			}
			else if (PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated || PhotonNetwork.connectionStateDetailed == PeerState.Disconnected) {
				noInternet();
			}
			else{
				yesInternet();
			}
		}
		checkInternet();
	}

	void checkInternet(){
		if(PhotonNetwork.connectionStateDetailed == PeerState.Connecting){
			connected = false;
		}
		else if (PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated || PhotonNetwork.connectionStateDetailed == PeerState.Disconnected) {
			connected = false;
		}
		else{
			connected = true;
		}
	}

	void connectingAttempt(){
		createServerButton.GetComponent<UIButton>().enabled = false;
		findServerButton.GetComponent<UIButton>().enabled = false;
		NGUITools.SetActive(reconnectWindow, true);
		reconnectWindow.GetComponentInChildren<TweenAlpha>().PlayForward();
		NGUITools.SetActive(retryConnect, false);
		NGUITools.SetActive(optionsButtons, false);
		NGUITools.SetActive(createRoomFailed, false);
	}

	void noInternet(){
		createServerButton.GetComponent<UIButton>().enabled = false;
		findServerButton.GetComponent<UIButton>().enabled = false;
		NGUITools.SetActive(reconnectWindow,true);
		NGUITools.SetActive(uiCamera, true);
		NGUITools.SetActive(attemptingConnection, false);
	}

	void yesInternet(){
		NGUITools.SetActiveChildren(gameObject, true);
		NGUITools.SetActive(reconnectWindow,false);
		NGUITools.SetActive(createRoomFailed, false);
		NGUITools.SetActive(optionsButtons, false);
		optionsButtons.SetActive(false);
		createServerButton.GetComponent<UIButton>().enabled = true;
		findServerButton.GetComponent<UIButton>().enabled = true;
	}

	void onFindServerClicked(){
		int serverNum = 0;
		foreach (RoomInfo room in PhotonNetwork.GetRoomList()) {
			GameObject serverInfo = NGUITools.AddChild (serverTable, btnJoinServer_prefab);
			UILabel[] serverButText = serverInfo.GetComponentsInChildren<UILabel>();
			Vector3 temp = new Vector3(0.52f,-0.1f-((serverNum)*0.12f),0);
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
		UILabel serverName = GameObject.Find("serverName").GetComponent<UILabel>();
		player.RoomName = serverName.text;
		PhotonNetwork.CreateRoom(player.RoomName, true, true, 10);
	}

	void getUserHandle(){
		UILabel playerName = GameObject.Find ("playerName").GetComponent<UILabel> ();
		player.Handle = playerName.text;
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
		PlayerPrefs.SetString(playerPrefsPrefix + "Name", player.Handle);
		PlayerPrefs.SetString(playerPrefsPrefix + "Room", player.RoomName);
		PlayerPrefs.Save();
		loadingScreen.StartLoadingLevel("Pregame");
	}

	void OnPhotonJoinFailed(){
		Debug.Log("OnPhotonJoinFailed");
	}

	void OnPhotonCreateRoomFailed(){
		NGUITools.SetActive(createRoomFailed, true);
		StartCoroutine(wait());
	}

	IEnumerator wait(){
		yield return new WaitForSeconds(5);
		NGUITools.SetActive(createRoomFailed, false);
	}
}
