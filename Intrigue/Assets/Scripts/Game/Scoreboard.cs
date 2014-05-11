﻿using UnityEngine;
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
	private int [] serverCompletions = new int [] {0,0,0};

	// Use this for initialization
	void Start () {
		scoreboard.GetComponent<UIPanel>().alpha = 0;
		//this.photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();
		int s = 0;
		int g = 0;
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Score", player.Score}});

		foreach(PhotonPlayer play in PhotonNetwork.playerList){
			if((string)play.customProperties["Team"] == "Guard"){
				
				GameObject playerHandleInstance = NGUITools.AddChild(guardTable, playerPrefab);
				Vector3 temp = new Vector3(0f,(g)*30f,0f);
				playerHandleInstance.transform.localPosition -= temp;
				UILabel handlelabel = playerHandleInstance.GetComponent<UILabel>();
				handlelabel.user = play.ID;
				handlelabel.labelType = "handle";
				handlelabel.text = "";

				GameObject playerScoreInstance = NGUITools.AddChild(guardTable, playerPrefab);
				Vector3 temp2 = new Vector3(-75f,(g)*30f,0f);
				playerScoreInstance.transform.localPosition -= temp2;
				UILabel scoreLabel = playerScoreInstance.GetComponent<UILabel>();
				scoreLabel.user = play.ID;
				scoreLabel.labelType = "score";
				scoreLabel.text = "";

				GameObject playerPingInstance = NGUITools.AddChild(guardTable, playerPrefab);
				Vector3 temp3 = new Vector3(-158f,(g)*30f,0f);
				playerPingInstance.transform.localPosition -= temp3;
				UILabel pingLabel = playerPingInstance.GetComponent<UILabel>();
				pingLabel.user = play.ID;
				pingLabel.labelType = "ping";
				pingLabel.text = "";

				++g;
			}else{

				GameObject playerHandleInstance = NGUITools.AddChild(spyTable, playerPrefab);
				Vector3 temp = new Vector3(0f,(g)*30f,0f);
				playerHandleInstance.transform.localPosition -= temp;
				UILabel handlelabel = playerHandleInstance.GetComponent<UILabel>();
				handlelabel.user = play.ID;
				handlelabel.labelType = "handle";
				handlelabel.text = "";

				GameObject playerScoreInstance = NGUITools.AddChild(spyTable, playerPrefab);
				Vector3 temp2 = new Vector3(-75f,(g)*30f,0f);
				playerScoreInstance.transform.localPosition -= temp2;
				UILabel scoreLabel = playerScoreInstance.GetComponent<UILabel>();
				scoreLabel.user = play.ID;
				scoreLabel.labelType = "score";
				scoreLabel.text = "";

				GameObject playerPingInstance = NGUITools.AddChild(spyTable, playerPrefab);
				Vector3 temp3 = new Vector3(-158f,(g)*30f,0f);
				playerPingInstance.transform.localPosition -= temp3;
				UILabel pingLabel = playerPingInstance.GetComponent<UILabel>();
				pingLabel.user = play.ID;
				pingLabel.labelType = "ping";
				pingLabel.text = "";

				++s;
			}
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
			spyTeam.GetComponent<UILabel>().text = player.TeamScore.ToString();
			guardTeam.GetComponent<UILabel>().text = player.EnemyScore.ToString();
		}
		else{
			spyTeam.GetComponent<UILabel>().text = player.EnemyScore.ToString();
			guardTeam.GetComponent<UILabel>().text = player.TeamScore.ToString();
		}


		
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
						switch(label.labelType){
							case "handle":
								label.text = "[FFFFFF]" + (string)play.customProperties["Handle"];
								break;
							case "score":
								label.text = "[FFFFFF]" + (int)play.customProperties["Score"];
								break;
							case "ping":
								label.text = pingColor + (int)play.customProperties["Ping"]+"[-]";
								break;
						}

					}
				}
			}
			else if((string)play.customProperties["Team"] == "Spy"){
				foreach(Transform sC in spyTable.transform){
					if(sC.gameObject.GetComponent<UILabel>().user == play.ID){
						UILabel label = sC.gameObject.GetComponent<UILabel>();
						
						switch(label.labelType){
							case "handle":
								label.text = "[FFFFFF]" + (string)play.customProperties["Handle"];
								break;
							case "score":
								label.text = "[FFFFFF]" + (int)play.customProperties["Score"];
								break;
							case "ping":
								label.text = pingColor + (int)play.customProperties["Ping"]+"[-]";
								break;
						}
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
