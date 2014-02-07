using UnityEngine;
using System.Collections;

public class Network : MonoBehaviour {

	private PhotonView photonView = null;
	private Vector2 scrollPositionChat = new Vector2(0, 0);
	private GUIStyle styleChat = new GUIStyle();
	private GUIStyle styleScore = new GUIStyle();
	private string chatBox = "";
	private string textField = "";
	private Player player;
	private Intrigue intrigue;

	// Look up how to disconnect
	void Start () {
		PhotonNetwork.isMessageQueueRunning = true;
		// Get photonView component
		photonView = PhotonView.Get(this);

		player = GameObject.Find("Player").GetComponent<Player>();
		intrigue = GameObject.FindWithTag("Scripts").GetComponent<Intrigue>();

		this.styleChat.fontSize = 12;
		this.styleChat.normal.textColor = Color.white;

		this.styleScore.fontSize = 40;
		this.styleScore.normal.textColor = Color.red;

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
		GUILayout.Label( "Handle: " + player.Handle );
		GUILayout.Label( "Team: "+ player.Team );
		GUILayout.Label( "TeamID: " + player.TeamID );
		GUILayout.Label( "Id: " + PhotonNetwork.player.ID );
		GUILayout.Label( "Are You Master Client?? " + PhotonNetwork.isMasterClient );
		
		//Checks state of connection: Look up PeerState
		if( PhotonNetwork.connectionStateDetailed == PeerState.Joined ){
			// //Chat Box
			// this.scrollPositionChat = GUILayout.BeginScrollView(this.scrollPositionChat, GUILayout.Width ( Screen.width/4 ), GUILayout.MaxHeight(190), GUILayout.ExpandHeight (false));
			// GUI.skin.box.alignment = TextAnchor.UpperLeft;
			// GUILayout.Box(this.chatBox, this.styleChat, GUILayout.ExpandHeight(true));
			// GUILayout.EndScrollView();


			// GUI.SetNextControlName ("ChatBox");
			// textField = GUILayout.TextField( textField, 100 );
			// if( ( GUILayout.Button("Send") ||
			// 	(Event.current.type == EventType.keyDown && 
			// 	 Event.current.character == '\n' &&
			// 	 GUI.GetNameOfFocusedControl() == "ChatBox") ) 
			// 	&& textField != ""  ){ 
			// 	photonView.RPC("recieveMessage", PhotonTargets.All, (this.player.Handle + ": " + textField + "\n") );
			// 	textField = "";
			// 	this.scrollPositionChat.y = Mathf.Infinity;
			// }
			if( GUILayout.Button( "Leave Room" ) ){
				if(Intrigue.playerGO)
					PhotonNetwork.Destroy(Intrigue.playerGO);
				PhotonNetwork.LeaveRoom();
			}
		}
		GUI.Label(new Rect((Screen.width/2)-200,20,400,100),"Allies: " + player.TeamScore + " - Enemies: " + player.EnemyScore, styleScore);

		GUI.Label(new Rect((Screen.width/2)-100,60,200,100),"Your Score: " + player.Score, styleScore);
	}

	void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer){
		Debug.Log("OnPhotonPlayerDisconnected: " + photonPlayer.ID );
		
		if( player.Team == "Guard" ){
			--Intrigue.numGuardsLeft;
		} else {
			--Intrigue.numSpiesLeft;
		}

		if (PhotonNetwork.isMasterClient){
			//Move Info towards to new master Client, but master client switches on its own
		}
	}

	[RPC]
	public void recieveMessage(string s){
		this.chatBox += s;
	}
}
