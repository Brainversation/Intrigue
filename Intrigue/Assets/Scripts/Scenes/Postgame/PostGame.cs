using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PostGame : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject guardTable;
	public GameObject spyTable;

	private PhotonView photonView = null;
	private Player player;
	private GameObject team1s;
	private GameObject team2s;
	private GameObject wins;
	private List<string> team1= new List<string>();
	private List<string> team2 = new List<string>();

	// Use this for initialization 
	void Start(){
		Screen.lockCursor = false;
		this.photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();


		foreach(PhotonPlayer play in PhotonNetwork.playerList){
			if((int)play.customProperties["TeamID"] == 1){
				Debug.Log("Added team 1");
				addTeam1((string)play.customProperties["Handle"], (int)play.customProperties["Score"], play.ID);
			}
			else{
				Debug.Log("Added team 1");
				addTeam2((string)play.customProperties["Handle"], (int)play.customProperties["Score"], play.ID);
			}
		}

		team1s = GameObject.FindGameObjectWithTag("Team1Score");
		team2s = GameObject.FindGameObjectWithTag("Team2Score");
		wins = GameObject.FindGameObjectWithTag("Winner");

		if(player.TeamID==1){
			team1s.GetComponent<UILabel>().text = "TEAM 1: " + "[FFFFFF]"+player.TeamScore;
			team2s.GetComponent<UILabel>().text = "TEAM 2: " + "[FFFFFF]"+player.EnemyScore;
			if(player.TeamScore>player.EnemyScore)
				wins.GetComponent<UILabel>().text = "TEAM 1 WINS";
			else
				wins.GetComponent<UILabel>().text = "TEAM 2 WINS";				
		}
		else{
			team1s.GetComponent<UILabel>().text = "TEAM 1: " + "[FFFFFF]"+player.EnemyScore;
			team2s.GetComponent<UILabel>().text = "TEAM 2: " + "[FFFFFF]"+player.TeamScore;
			if(player.TeamScore<player.EnemyScore)
				wins.GetComponent<UILabel>().text = "TEAM 1 WINS";
			else
				wins.GetComponent<UILabel>().text = "TEAM 2 WINS";	
		}

	}
	
	// Update is called once per frame
	void Update(){

	}

	void leaveLobby(){
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LoadLevel( "MainMenu" );
	}

	void addTeam1(string handle, int score, int playerID){
		team1.Add(handle);
		GameObject playerInfo = NGUITools.AddChild(spyTable, playerPrefab);
		Vector3 temp = new Vector3(0f,(team1.Count-1)*0.1f,0);
		playerInfo.transform.localPosition-=temp;
		UILabel label = playerInfo.GetComponent<UILabel>();
		label.user = playerID;
		label.text = "[00CCFF]" + handle + "[-] - " + score;
	}

	void addTeam2(string handle, int score, int playerID){
		team2.Add(handle);
		GameObject playerInfo = NGUITools.AddChild(guardTable, playerPrefab);
		Vector3 temp = new Vector3(0f,(team2.Count-1)*0.1f,0);
		playerInfo.transform.position-=temp;
		UILabel label = playerInfo.GetComponent<UILabel>();
		label.user = playerID;
		label.text = "[FF2B2B]" + handle + "[-] - " + score;
	}
}
