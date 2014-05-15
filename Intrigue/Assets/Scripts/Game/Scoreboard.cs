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
	private GameObject [] servers;
	private List<int> spies = new List<int>();
	private List<int> guards = new List<int>();

	// Use this for initialization
	void Start () {
		scoreboard.GetComponent<UIPanel>().alpha = 0;
		//this.photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Score", player.Score}});

		InvokeRepeating("reloadScoreboard", 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKey(KeyCode.Tab) ){
			scoreboard.GetComponent<UIPanel>().alpha = 1;
		}
		else
			scoreboard.GetComponent<UIPanel>().alpha = 0;

		//Set's the Team Scores
		PhotonPlayer play = PhotonNetwork.player;
		if(player.Team=="Spy"){
			if(player.TeamID == 1){
				spyTeam.GetComponent<UILabel>().text = ((int)play.customProperties["Team1Score"]).ToString();
				guardTeam.GetComponent<UILabel>().text = ((int)play.customProperties["Team2Score"]).ToString();
			}
			else{
				spyTeam.GetComponent<UILabel>().text = ((int)play.customProperties["Team2Score"]).ToString();
				guardTeam.GetComponent<UILabel>().text = ((int)play.customProperties["Team1Score"]).ToString();			
			}
		}
		else{
			if(player.TeamID == 1){
				spyTeam.GetComponent<UILabel>().text = ((int)play.customProperties["Team2Score"]).ToString();
				guardTeam.GetComponent<UILabel>().text = ((int)play.customProperties["Team1Score"]).ToString();
			}
			else{
				spyTeam.GetComponent<UILabel>().text = ((int)play.customProperties["Team1Score"]).ToString();
				guardTeam.GetComponent<UILabel>().text = ((int)play.customProperties["Team2Score"]).ToString();			
			}
		}	
	}



	void createGuardScoreInstance(PhotonPlayer play, string pingColor){
		int g = guards.Count-1;
		GameObject playerHandleInstance = NGUITools.AddChild(guardTable, playerPrefab);
		Vector3 temp = new Vector3(0f,(g)*30f,0f);
		playerHandleInstance.transform.localPosition -= temp;
		UILabel handlelabel = playerHandleInstance.GetComponent<UILabel>();
		handlelabel.user = play.ID;
		handlelabel.labelType = "handle";
		handlelabel.text = (string)play.customProperties["Handle"];

		GameObject playerScoreInstance = NGUITools.AddChild(guardTable, playerPrefab);
		Vector3 temp2 = new Vector3(-75f,(g)*30f,0f);
		playerScoreInstance.transform.localPosition -= temp2;
		UILabel scoreLabel = playerScoreInstance.GetComponent<UILabel>();
		scoreLabel.user = play.ID;
		scoreLabel.labelType = "score";
		scoreLabel.text = ((int)play.customProperties["Score"]).ToString();

		GameObject playerPingInstance = NGUITools.AddChild(guardTable, playerPrefab);
		Vector3 temp3 = new Vector3(-158f,(g)*30f,0f);
		playerPingInstance.transform.localPosition -= temp3;
		UILabel pingLabel = playerPingInstance.GetComponent<UILabel>();
		pingLabel.user = play.ID;
		pingLabel.labelType = "ping";
		pingLabel.text = pingColor + (int)play.customProperties["Ping"];
	}

	void createSpyScoreInstance(PhotonPlayer play, string pingColor){
		int s = spies.Count-1;
		GameObject playerHandleInstance = NGUITools.AddChild(spyTable, playerPrefab);
		Vector3 temp = new Vector3(0f,(s)*30f,0f);
		playerHandleInstance.transform.localPosition -= temp;
		UILabel handlelabel = playerHandleInstance.GetComponent<UILabel>();
		handlelabel.user = play.ID;
		handlelabel.labelType = "handle";
		handlelabel.text = (string)play.customProperties["Handle"];

		GameObject playerScoreInstance = NGUITools.AddChild(spyTable, playerPrefab);
		Vector3 temp2 = new Vector3(-75f,(s)*30f,0f);
		playerScoreInstance.transform.localPosition -= temp2;
		UILabel scoreLabel = playerScoreInstance.GetComponent<UILabel>();
		scoreLabel.user = play.ID;
		scoreLabel.labelType = "score";
		scoreLabel.text = ((int)play.customProperties["Score"]).ToString();

		GameObject playerPingInstance = NGUITools.AddChild(spyTable, playerPrefab);
		Vector3 temp3 = new Vector3(-158f,(s)*30f,0f);
		playerPingInstance.transform.localPosition -= temp3;
		UILabel pingLabel = playerPingInstance.GetComponent<UILabel>();
		pingLabel.user = play.ID;
		pingLabel.labelType = "ping";
		pingLabel.text = pingColor + (int)play.customProperties["Ping"];

	}

	void reloadScoreboard(){
		for(int x = guardTable.transform.childCount; x >0; --x){
			NGUITools.Destroy(guardTable.transform.GetChild(x-1).gameObject);
		}
		for(int x = spyTable.transform.childCount; x >0; --x){
			NGUITools.Destroy(spyTable.transform.GetChild(x-1).gameObject);
		}

		guards.Clear();
		spies.Clear();
		foreach(PhotonPlayer play in PhotonNetwork.playerList){
			string pingColor;
			if(play.customProperties["Ping"] != null)
				pingColor = calculatePingColor((int)play.customProperties["Ping"]);
			else
				pingColor = "[00FF00]";

			if((string)play.customProperties["Team"] == "Guard"){
				guards.Add(play.ID);
				spies.Remove(play.ID);
				createGuardScoreInstance(play, pingColor);
			}
			else if((string)play.customProperties["Team"] == "Spy"){
				spies.Add(play.ID);
				guards.Remove(play.ID);
				createSpyScoreInstance(play, pingColor);		
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
