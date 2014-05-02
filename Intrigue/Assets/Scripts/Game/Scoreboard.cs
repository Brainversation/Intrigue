using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Scoreboard : MonoBehaviour {
	
	public GameObject scoreboard;
	//private PhotonView photonView = null;
	private Player player;
	public GameObject playerPrefab;
	public GameObject guardTable;
	public GameObject spyTable;
	public GameObject spyTeam;
	public GameObject guardTeam;
	public GameObject serverStats;
	private List<int> spies = new List<int>();
	private List<int> guards = new List<int>();
	private GameObject [] servers;
	private int [] serverCompletions = new int [] {0,0,0};

	// Use this for initialization
	void Start () {
		scoreboard.GetComponent<UIPanel>().alpha = 0;
		//this.photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();
		servers = GameObject.FindGameObjectsWithTag("ObjectiveMain");
		int i = 0;
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Score", player.Score}});

		foreach(PhotonPlayer play in PhotonNetwork.playerList){
			if((string)play.customProperties["Team"] == "Guard"){
				GameObject playerScoreInstance= NGUITools.AddChild(guardTable, playerPrefab);
				Vector3 temp = new Vector3(0f,(i)*30f,0f);
				playerScoreInstance.transform.localPosition -= temp;
				UILabel label = playerScoreInstance.GetComponent<UILabel>();
				label.user = play.ID;
				label.text = "";
			}else{
				GameObject playerScoreInstance= NGUITools.AddChild(spyTable, playerPrefab);
				Vector3 temp = new Vector3(0f,(i)*30f,0f);
				playerScoreInstance.transform.localPosition -= temp;
				UILabel label = playerScoreInstance.GetComponent<UILabel>();
				label.user = play.ID;
				label.text = "";
			}
			++i;
		}
		InvokeRepeating("reloadScoreboard", 1, 1);
		reloadScoreboard();
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKey(KeyCode.Tab) ){
			scoreboard.GetComponent<UIPanel>().alpha = 1;
		}
		else
			scoreboard.GetComponent<UIPanel>().alpha = 0;

		if(player.Team=="Spy"){
			spyTeam.GetComponent<UILabel>().text = "Spies : " + player.TeamScore;
			guardTeam.GetComponent<UILabel>().text = "Guards : " + player.EnemyScore;
		}
		else{
			spyTeam.GetComponent<UILabel>().text = "Spies : " + player.EnemyScore;
			guardTeam.GetComponent<UILabel>().text = "Guards : " + player.TeamScore;
		}


		
		//SERVER STATS
		foreach(GameObject serv in servers){
			int curServ = serv.GetComponent<ObjectiveMain>().objectiveName-1;
			serverCompletions[curServ] = serv.GetComponent<ObjectiveMain>().completionPercentage;
		}

		serverStats.GetComponent<UILabel>().text = "SERVERS:\n1: [FF0000]" + serverCompletions[0] + "%[-] 2: [FF0000]" + serverCompletions[1] + "%[-] 3:[FF0000] " + serverCompletions[2] + "%[-]";

	}

	void reloadScoreboard(){
		foreach(PhotonPlayer play in PhotonNetwork.playerList){
			string pingColor;
			if(play.customProperties["Ping"] != null)
				pingColor = calculatePingColor((int)play.customProperties["Ping"]);
			else
				pingColor = "[00FF00]";

			if((string)play.customProperties["Team"] == "Guard"){
				foreach(Transform gC in guardTable.transform){
					if(gC.gameObject.GetComponent<UILabel>().user == play.ID){
						UILabel label = gC.gameObject.GetComponent<UILabel>();
						label.user = play.ID;
						label.text = "[FF2B2B]" + (string)play.customProperties["Handle"] + "[-] - "+ (int)play.customProperties["Score"] + " ("+ pingColor + (int)play.customProperties["Ping"]+"[-]" + ") ms";
					}
				}
			}
			else if((string)play.customProperties["Team"] == "Spy"){
				foreach(Transform sC in spyTable.transform){
					if(sC.gameObject.GetComponent<UILabel>().user == play.ID){
						UILabel label = sC.gameObject.GetComponent<UILabel>();
						label.user = play.ID;
						label.text = "[00CCFF]" + (string)play.customProperties["Handle"] + "[-] - "+ (int)play.customProperties["Score"] + " ("+ pingColor + (int)play.customProperties["Ping"]+"[-]" + ") ms";
					}
				}
			}
		}
	}

	string calculatePingColor(int ping){
		string pingColor;
		if (ping<50)
			pingColor = "[00FF00]";
		else if(ping<100)
			pingColor = "[FF9D00]";
		else
			pingColor = "[FF0000]";
		return pingColor;
	}

}
