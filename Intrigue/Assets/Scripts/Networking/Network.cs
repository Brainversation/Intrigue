using UnityEngine;
using System.Collections;

public class Network : MonoBehaviour {

	private PhotonView photonView = null;
	private Vector2 scrollPositionChat = new Vector2(0, 0);
	private GUIStyle styleChat = new GUIStyle();
	private GameObject player = null;
	private string chatBox = "";
	private string textField = "";

	// Look up how to disconnect
	void Start () {
		PhotonNetwork.isMessageQueueRunning = true;
		// Get photonView component
		photonView = PhotonView.Get(this);

		this.styleChat.fontSize = 12;
		this.styleChat.normal.textColor = Color.white;

		this.player = PhotonNetwork.Instantiate(
						"Test_Player_"+PlayerPrefs.GetString("Team"),
						new Vector3(0, 1, 0),
						Quaternion.identity, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if( PhotonNetwork.connectionStateDetailed == PeerState.JoinedLobby ){
			PhotonNetwork.isMessageQueueRunning = false;
			Application.LoadLevel("MainMenu");
		}
	}

	void OnGUI(){
		// Tells us about the current network connection
		GUILayout.Label("Status: " + PhotonNetwork.connectionStateDetailed.ToString());
		GUILayout.Label( "Player Count:" + PhotonNetwork.playerList.Length );
		GUILayout.Label( "Your Id: " + PhotonNetwork.player.ID );
		GUILayout.Label( "Are You Master Server??" + PhotonNetwork.isMasterClient );
		GUILayout.Label( "Team: "+PlayerPrefs.GetString("Team"));
		
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
				PhotonNetwork.Destroy(player);
				PhotonNetwork.LeaveRoom();
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
