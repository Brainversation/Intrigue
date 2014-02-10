using UnityEngine;
using System.Collections;

public class PregameLobby : MonoBehaviour {

	private PhotonView photonView = null;
	private Vector2 scrollPositionChat = new Vector2(0, 0);
	private GUIStyle styleChat = new GUIStyle();
	private string chatBox = "";
	private string textField = "";
	private bool isReady = false;
	private int readyCount = 0;
	private Player player;

	// Use this for initialization
	void Start () {
		PhotonNetwork.isMessageQueueRunning = true;
		// Get photonView component
		this.photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();
		this.styleChat.fontSize = 12;
		this.styleChat.normal.textColor = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		// Tells us about the current network connection
		GUILayout.Label("Status: " + PhotonNetwork.connectionStateDetailed.ToString());
		GUILayout.Label( "Player Count:" + PhotonNetwork.playerList.Length );
		GUILayout.Label( "Handle: " + player.Handle );
		GUILayout.Label( "Team: "+ player.Team);
		GUILayout.Label( "Id: " + PhotonNetwork.player.ID );
		GUILayout.Label( "Are You Master Client?? " + PhotonNetwork.isMasterClient );
		
		//Checks state of connection: Look up PeerState
		if( PhotonNetwork.connectionStateDetailed == PeerState.Joined ){
			//Chat Box
			this.scrollPositionChat = GUILayout.BeginScrollView(this.scrollPositionChat, GUILayout.Width ( Screen.width/4 ), GUILayout.MaxHeight(190), GUILayout.ExpandHeight (false));
			GUI.skin.box.alignment = TextAnchor.UpperLeft;
			GUILayout.Box(this.chatBox, this.styleChat, GUILayout.ExpandHeight(true));
			GUILayout.EndScrollView();


			GUI.SetNextControlName("ChatBox");
			textField = GUILayout.TextField( textField, 100 );
			if( ( GUILayout.Button("Send") ||
				(Event.current.type == EventType.keyDown && 
				 Event.current.character == '\n' &&
				 GUI.GetNameOfFocusedControl() == "ChatBox") ) 
				&& textField != ""  ){ 
				photonView.RPC("recieveMessage", PhotonTargets.AllBuffered, (this.player.Handle + ": " + textField + "\n") );
				textField = "";
				this.scrollPositionChat.y = Mathf.Infinity;
			}
			if( GUILayout.Button( "Leave Room" ) ){
				PhotonNetwork.LeaveRoom();
				Application.LoadLevel( "MainMenu" );
			}
			if(GUILayout.Button( "Play as Spy")){
				player.Team = "Spy";
			}
			if(GUILayout.Button( "Play as Guard")){
				player.Team = "Guard";
			}
			if( PhotonNetwork.isMasterClient ){
				player.Guests = Mathf.RoundToInt(GUILayout.HorizontalSlider(player.Guests, 0.0f, 15.0f));
				GUILayout.Label( "Number of Guests: " + player.Guests );
				if( (readyCount == PhotonNetwork.playerList.Length-1) && player.Team != "" && GUILayout.Button( "PLAY INTRIGUE") ){
					photonView.RPC("go", PhotonTargets.AllBuffered);
				}
			} else if( player.Team != "" && !isReady ){
				if( GUILayout.Button( "Ready" ) ){
					isReady = true;
					photonView.RPC("ready", PhotonTargets.MasterClient, 1);
				}
			} else if( isReady ){
				if( GUILayout.Button( "Not Ready?" ) ){
					isReady = false;
					photonView.RPC("ready", PhotonTargets.MasterClient, -1);
				}
			}
		}
	}

	void OnPhotonPlayerDisconnected(PhotonPlayer player){
		Debug.Log("OnPhotonPlayerDisconnected: " + player.ID );

		if (PhotonNetwork.isMasterClient){
			//Move Info towards to new master Client, but master client switches on its own
		}
	}

	IEnumerator loadGame() {
		AsyncOperation async = Application.LoadLevelAsync("Intrigue");
		yield return async;
		PhotonNetwork.isMessageQueueRunning = false;
		Debug.Log("Loading complete");
	}

	[RPC]
	public void recieveMessage(string s){
		this.chatBox += s;
	}

	[RPC]
	public void ready( int val ){
		this.readyCount += val;
	}
	
	[RPC]
	public void go(){
		StartCoroutine( loadGame() );
	}
}
