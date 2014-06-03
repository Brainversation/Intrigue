/**************************************************************************
 **                                                                       *
 **                COPYRIGHT AND CONFIDENTIALITY NOTICE                   *
 **                                                                       *
 **    Copyright (c) 2014 Hacksaw Games. All rights reserved.             *
 **                                                                       *
 **    This software contains information confidential and proprietary    *
 **    to Hacksaw Games. It shall not be reproduced in whole or in        *
 **    part, or transferred to other documents, or disclosed to third     *
 **    parties, or used for any purpose other than that for which it was  *
 **    obtained, without the prior written consent of Hacksaw Games.      *
 **                                                                       *
 **************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Intrigue : MonoBehaviour {

	public GameObject loadingScreenObject;

	private const float TIMELIMIT = 420;
	private const int MAXROUNDS = 3;

	private static int roundsLeft = MAXROUNDS;
	private static float timeLeft = TIMELIMIT;
	private static PhotonView photonView = null;

	private int numSpies = 0;
	private int numGuards = 0;
	private int spawnIndex;
	private int readyCount = 0;
	private int winningTeamThisRound;
	private float totalObjActive;
	private Player player;
	private Transform spawnTrans;
	private List<Transform> spawns = new List<Transform>();
	private List<Transform> availableSpawns = new List<Transform>();
	private Dictionary<int, bool> spawnedPlayers = new Dictionary<int, bool>();
	private GameObject[] spawnObjects;
	private GameObject[] objArray;

	[HideInInspector] public string roundResult;
	[HideInInspector] public bool[] objectives;
	[HideInInspector] public bool[] mainObjectives;
	[HideInInspector] public bool gameStart = false;
	[HideInInspector] public bool doneLoading = false;
	[HideInInspector] public float loadedGuests = 0;
	[HideInInspector] public float totalGuests;
	[HideInInspector] public static bool finalRoundOver = false;
	[HideInInspector] public static int objectivesCompleted = 0;
	[HideInInspector] public static List<int> roundResults = new List<int>();
	
	public static int numSpiesLeft;
	public static int numGuardsLeft;
	public static GameObject playerGO = null;
	public static bool gameOverFlag = false;
	
	// CHANGE GAMEOVER HERE
	public static bool wantGameOver = true;

	void Awake(){
		GameObject menuMusic = GameObject.Find("MenuMusic");
		if(menuMusic){
			Destroy(menuMusic);
		}
	}

	void Start () {
		photonView = PhotonView.Get(this);
		player = Player.Instance;
		totalGuests = player.Guests;
		objArray = GameObject.FindGameObjectsWithTag("Objective");


		//Master Client Stuff
		if(PhotonNetwork.isMasterClient){
			timeLeft = TIMELIMIT;
			spawnObjects = GameObject.FindGameObjectsWithTag("Respawn");
			for(int i=0; i < spawnObjects.Length; i++){
				spawns.Add(spawnObjects[i].transform);
			}
			availableSpawns = spawns;
			StartCoroutine(spawnGuests());

			totalObjActive = Mathf.RoundToInt(objArray.Length*.67f);
			int totalCurActive = 0;
			bool activationComplete = false;
			Objective objscriptref;

			while (!activationComplete){
				foreach(GameObject obj in objArray){
					objscriptref = obj.GetComponent<Objective>();
					if(totalCurActive<totalObjActive){
						if((Random.Range(0f,1f)>0.50f) && !objscriptref.isActive ){
							objscriptref.activate();
							totalCurActive++;
						}	
					}else{
						activationComplete = true;
						break;
					}
				}
			}
		} else {
			enabled = false;
		}

		
		objectives = new bool[objArray.Length];
		mainObjectives = new bool[GameObject.FindGameObjectsWithTag("ObjectiveMain").Length];
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Team", player.Team}});	
		joinGame();
	}

	void Update () {
		if(!gameStart) return;


		timeLeft -= Time.deltaTime;
		photonView.RPC("syncTime", PhotonTargets.Others, timeLeft);
		if( timeLeft <= 0 || numSpiesLeft<=0 || numGuardsLeft <=0 || objectivesCompleted == 2){
			if(wantGameOver && !finalRoundOver){
				if(timeLeft<=0){
					roundResult = "Time Limit Reached.\nGuards Win!";
					winningTeamThisRound = 2;
				} else if(numSpiesLeft<=0){
					roundResult = "All Spies Caught.\nGuards Win!";
					winningTeamThisRound = 2;
				} else if(numGuardsLeft<=0){
					roundResult = "All Guards Out.\nSpies Win!";
					winningTeamThisRound = 1;

				} else{
					roundResult = "Objectives Completed.\nSpies Win!";
					winningTeamThisRound = 1;
				}


				//Converts winningTeamThisRound from Spies/Guard to Team1/Team2
				if(player.Team == "Spy" && winningTeamThisRound == 1){
					winningTeamThisRound = player.TeamID;
				} 
				else if(player.Team == "Spy" && winningTeamThisRound == 2){
					if(player.TeamID == 1)
						winningTeamThisRound = 2;
					else
						winningTeamThisRound = 1;
				} 
				else if(player.Team == "Guard" && winningTeamThisRound == 2){
					winningTeamThisRound = player.TeamID;
				}
				else if(player.Team == "Guard" && winningTeamThisRound == 1){
					if(player.TeamID == 1)
						winningTeamThisRound = 2;
					else
						winningTeamThisRound = 1;
				}
				roundResults.Add(winningTeamThisRound);
				photonView.RPC("syncRoundResults", PhotonTargets.OthersBuffered, winningTeamThisRound);
				photonView.RPC("gameOver", PhotonTargets.AllBuffered, roundResult, winningTeamThisRound);
			}
		}
	}

	void joinGame(){
		if( (string)PhotonNetwork.player.customProperties["Team"] == "Guard")
			spawnGuard();
		else 
			spawnSpy();
	}

	void spawnGuard(){
		photonView.RPC("addGuard", PhotonTargets.MasterClient);
		photonView.RPC("sendSpawnPoint", PhotonTargets.MasterClient);
	}

	void spawnSpy(){
		photonView.RPC("addSpy", PhotonTargets.MasterClient);
		photonView.RPC("sendSpawnPoint", PhotonTargets.MasterClient);
	}

	IEnumerator spawnGuests(){
		while(readyCount != PhotonNetwork.playerList.Length)
			yield return null;

		for(int x = 0; x < player.Guests; ++x){	
			nextSpawnPoint();
			loadedGuests = x;
			int type = Mathf.RoundToInt(Random.Range(1,5));
			PhotonNetwork.InstantiateSceneObject("Robot_Guest"+type.ToString(), spawnTrans.position, spawnTrans.rotation, 0, null);
			yield return new WaitForSeconds(.1f);
		}

		// Send turn off loading
		photonView.RPC("sendDoneLoading", PhotonTargets.AllBuffered);
	}

	void nextSpawnPoint(){
		spawnIndex = Random.Range(0,availableSpawns.Count-1);
		spawnTrans = availableSpawns[spawnIndex];
		availableSpawns.RemoveAt(spawnIndex);
	}

	public float GetTimeLeft{
		get{
			return timeLeft;
		}
	}

	public int GetRoundsLeft{
		get{
			return roundsLeft;
		}
	}

	public int GetRounds{
		get{
			return MAXROUNDS;
		}
	}

	public static void resetVariables(){
		roundsLeft = MAXROUNDS;
		timeLeft = TIMELIMIT;
		objectivesCompleted = 0;
		finalRoundOver = false;
		roundResults.Clear();
	}

	public static IEnumerator sendResetRound(){
		foreach(PhotonPlayer p in PhotonNetwork.playerList){
			if(PhotonNetwork.player != p){
				Intrigue.photonView.RPC("resetRound", p);
				yield return new WaitForSeconds(0.5f);
			}
		}
		Intrigue.photonView.RPC("resetRound", PhotonTargets.MasterClient);
	}

	[RPC]
	public void resetRound(){
		this.numSpies = Intrigue.numSpiesLeft =
		this.numGuards = Intrigue.numGuardsLeft =
		Intrigue.objectivesCompleted = 0;
		player.PrevResult = "Host Disconnected Resetting Round";
		PhotonNetwork.LoadLevel("Intrigue");
	}

	[RPC]
	void sendGameStart(){
		gameStart = true;
	}

	[RPC]
	void sendDoneLoading(){
		doneLoading = true;
	}

	[RPC]
	void sendSpawnPoint(PhotonMessageInfo info){
		if(spawnedPlayers.ContainsKey(info.sender.ID)){
			return;
		} else {
			spawnedPlayers.Add(info.sender.ID, true);
		}
		nextSpawnPoint();
		photonView.RPC("getSpawnPoint", info.sender, spawnTrans.position, spawnTrans.rotation);
	}

	[RPC]
	void getSpawnPoint(Vector3 position, Quaternion rotation){
		int type = Mathf.RoundToInt(Random.Range(1,5));

		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Team", player.Team}});

		Intrigue.playerGO = PhotonNetwork.Instantiate(
						"Robot_"+ (string)PhotonNetwork.player.customProperties["Team"] + type.ToString(),
						position,
						rotation, 0);

		player.TeamID = (int)PhotonNetwork.player.customProperties["TeamID"];
		photonView.RPC("ready", PhotonTargets.MasterClient);
	}

	[RPC]
	void addSpy(){
		++this.numSpies;
		++Intrigue.numSpiesLeft;
	}

	[RPC]
	void addGuard(){
		++this.numGuards;
		++Intrigue.numGuardsLeft;
	}


	[RPC]
	void syncTime(float time){
		timeLeft = time;
	}

	[RPC]
	public void ready(){
		++this.readyCount;
	}

	[RPC]
	void syncRoundResults(int winningTeam){
		roundResults.Add(winningTeam);
	}

	[RPC]
	void gameOver(string resultFromMC, int winningTeam){
		winningTeamThisRound = winningTeam;
		roundResult = resultFromMC;
		player.PrevResult = resultFromMC;
		gameOverFlag = true;
		if(playerGO!=null){
			playerGO.GetComponentInChildren<Camera>().enabled = false;
			playerGO.GetComponentInChildren<AudioListener>().enabled = false;
		}

		if(PhotonNetwork.isMasterClient){
			PhotonNetwork.DestroyAll();
		}

		//Add bonus points for winning round
		if(winningTeamThisRound==1){
			player.Team1Score += 300;
			PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Team1Score", player.Team1Score}});
		}
		else{
			player.Team2Score += 300;
			PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Team2Score", player.Team2Score}});
		}

		if(roundsLeft > 0){
			--roundsLeft;
			enabled = false;
			this.numSpies = Intrigue.numSpiesLeft =
			this.numGuards = Intrigue.numGuardsLeft =
			Intrigue.objectivesCompleted = 0;

			//Swaps Teams
			if(player.Team == "Spy"){
				player.Team = "Guard";
			} else {
				player.Team = "Spy";
			}

			PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Team", player.Team}});	
			
			PhotonNetwork.LoadLevel("Intrigue");
		} else {
			finalRoundOver = true;
			loadingScreenObject.SetActive(true);
		}
	}
}
