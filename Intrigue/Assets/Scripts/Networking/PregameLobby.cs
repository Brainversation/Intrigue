using UnityEngine;
using System.Collections;

public class PregameLobby : MonoBehaviour {

	private PhotonView photonView = null;
	private Vector2 scrollPositionChat = new Vector2(0, 0);
	private GUIStyle styleChat = new GUIStyle();
	private GameObject cube = null;
	private string chatBox = "";
	private string textField = "";

	// Use this for initialization
	void Start () {
		PhotonNetwork.isMessageQueueRunning = true;
		// Get photonView component
		photonView = PhotonView.Get(this);
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
		GUILayout.Label( "Your Id: " + PhotonNetwork.player.ID );
		GUILayout.Label( "Are You Master Server??" + PhotonNetwork.isMasterClient );
		GUILayout.Label( "Team: "+ PlayerPrefs.GetString("Team","Undecided"));
		
		//Checks state of connection: Look up PeerState
		if( PhotonNetwork.connectionStateDetailed == PeerState.Joined ){
			//Chat Box
			this.scrollPositionChat = GUILayout.BeginScrollView(this.scrollPositionChat, GUILayout.Width ( Screen.width/4 ), GUILayout.MaxHeight(190), GUILayout.ExpandHeight (false));
			GUI.skin.box.alignment = TextAnchor.UpperLeft;
			GUILayout.Box(this.chatBox, this.styleChat, GUILayout.ExpandHeight(true));
			GUILayout.EndScrollView();


			GUI.SetNextControlName ("ChatBox");
			textField = GUILayout.TextField( textField, 100 );
			if( ( GUILayout.Button("Send") ||
				(Event.current.type == EventType.keyDown && 
				 Event.current.character == '\n' &&
				 GUI.GetNameOfFocusedControl() == "ChatBox") ) 
				&& textField != ""  ){ 
				photonView.RPC("recieveMessage", PhotonTargets.All, textField);
				textField = "";
				this.scrollPositionChat.y = Mathf.Infinity;
			}
			if( GUILayout.Button( "Leave Room" ) ){
				PhotonNetwork.Destroy(cube);
				PhotonNetwork.LeaveRoom();
			}
			if(GUILayout.Button( "Play as Spy")){
				PlayerPrefs.SetString("Team","Spy");
			}
			if(GUILayout.Button( "Play as Guard")){
				PlayerPrefs.SetString( "Team", "Guard");
			}
			if(GUILayout.Button( "PLAY INTRIGUE") && PlayerPrefs.GetString("Team")!="Undecided"){
				PhotonNetwork.isMessageQueueRunning = false;
				Application.LoadLevel("Intrigue");
			}
		}
	}

	void OnPhotonPlayerDisconnected(PhotonPlayer player){
		Debug.Log("OnPhotonPlayerDisconnected: " + player.ID );

		if (PhotonNetwork.isMasterClient){
			//Move Info towards to new master Client, but master client switches on its own
		}
	}

	[RPC]
	public void recieveMessage(string s){
		this.chatBox += PhotonNetwork.player.ID + ": " + s + "\n";
	}
}
