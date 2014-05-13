using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Intrigue : MonoBehaviour {

	private const float TIMELIMIT = 420;
	private const int MAXROUNDS = 3;

	private static int roundsLeft = MAXROUNDS;
	private static float timeLeft = TIMELIMIT;

	private int numSpies = 0;
	private int numGuards = 0;
	private int spawnIndex;
	private int readyCount = 0;
	private int winningTeamThisRound;
	private float totalObjActive;
	private Player player;
	private PhotonView photonView = null;
	private Transform spawnTrans;
	private List<Transform> spawns = new List<Transform>();
	private List<Transform> availableSpawns = new List<Transform>();
	private GameObject[] spawnObjects;
	private GameObject[] objArray;

	public static int numSpiesLeft;
	public static int numGuardsLeft;
	public static bool wantGameOver;
	public static GameObject playerGO = null;

	[HideInInspector] public string roundResult;
	[HideInInspector] public float objectivesCompleted = 0;
	[HideInInspector] public bool[] objectives;
	[HideInInspector] public bool[] mainObjectives;
	[HideInInspector] public bool gameOverFlag = false;
	[HideInInspector] public bool gameStart = false;
	[HideInInspector] public bool doneLoading = false;
	[HideInInspector] public float loadedGuests = 0;
	[HideInInspector] public float totalGuests;

	void Awake(){
		GameObject menuMusic = GameObject.Find("MenuMusic");
		if(menuMusic){
			Destroy(menuMusic);
		}
	}

	void Start () {
		//CHANGE GAMEOVER HERE ~~~~~~~~~~~~~~~~~~~~~~~
		wantGameOver = true;
		//~~~~~~~~~~~~~~~~~~~~~~~~
		photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();
		totalGuests = player.Guests;
		if(PhotonNetwork.isMasterClient){
			timeLeft = TIMELIMIT;
			spawnObjects = GameObject.FindGameObjectsWithTag("Respawn");
			for(int i=0; i < spawnObjects.Length; i++){
				spawns.Add(spawnObjects[i].transform);
			}
			availableSpawns = spawns;
			StartCoroutine(spawnGuests());

			objArray = GameObject.FindGameObjectsWithTag("Objective");
			totalObjActive = Mathf.RoundToInt(GameObject.FindGameObjectsWithTag("Objective").Length*.67f);
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
						
						}
						else{
							activationComplete = true;
							break;
						}
				
				}

			}
		} else {
			enabled = false;
		}
		
		objectives = new bool[GameObject.FindGameObjectsWithTag("Objective").Length];
		mainObjectives = new bool[GameObject.FindGameObjectsWithTag("ObjectiveMain").Length];

		joinGame();
	}

	void Update () {
		if(!gameStart) return;
		timeLeft -= Time.deltaTime;
		photonView.RPC("syncTime", PhotonTargets.Others, timeLeft);
		if( timeLeft <= 0 ||  numSpiesLeft<=0 || numGuardsLeft <=0 || objectivesCompleted == 2){
			if(wantGameOver){
				Debug.Log("Gameover! numSpiesLeft " + numSpiesLeft + " numGuardsLeft " + numGuardsLeft);
				if(timeLeft<=0){
					roundResult = "Time Limit Reached.\nGuards Win!";
					winningTeamThisRound = 2;
				}
				else if(numSpiesLeft<=0){
					roundResult = "All Spies Caught.\nGuards Win!";
					winningTeamThisRound = 2;
				}
				else if(numGuardsLeft<=0){
					roundResult = "All Guards Out.\nSpies Win!";
					winningTeamThisRound = 1;
				}
				else{
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

				photonView.RPC("callGameOver", PhotonTargets.AllBuffered, roundResult, winningTeamThisRound);
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
		photonView.RPC("addGuard",PhotonTargets.All);
		photonView.RPC("sendSpawnPoint", PhotonTargets.MasterClient);
	}

	void spawnSpy(){
		photonView.RPC("addSpy",PhotonTargets.All);
		photonView.RPC("sendSpawnPoint", PhotonTargets.MasterClient);
	}

	IEnumerator spawnGuests(){
		while(readyCount != PhotonNetwork.playerList.Length)
			yield return null;

		for( int x = 0; x < player.Guests; ++x)
		{	
			nextSpawnPoint();
			loadedGuests = x;
			int type = Mathf.RoundToInt(Random.Range(1,5));
			//Debug.Log("Guest type: " + type);
			PhotonNetwork.InstantiateSceneObject("Robot_Guest1"/*+type.ToString()*/, spawnTrans.position, spawnTrans.rotation, 0, null);
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

	void gameOver(){
		gameOverFlag = true;
		if(playerGO!=null){
			playerGO.GetComponentInChildren<Camera>().enabled = false;
			playerGO.GetComponentInChildren<AudioListener>().enabled = false;
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
			this.numSpies = Intrigue.numSpiesLeft = 0;
			this.numGuards = Intrigue.numGuardsLeft = 0;

			Debug.Log("Round Over, ended as " + (string)PhotonNetwork.player.customProperties["Team"]);
			//Swaps Teams
			if((string)PhotonNetwork.player.customProperties["Team"] == "Spy"){
				player.Team = "Guard";
			} else {
				player.Team = "Spy";
			}

			PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Team", player.Team}});	
			
			Debug.Log("Round Over, swapped to " + (string)PhotonNetwork.player.customProperties["Team"]);
			Debug.Log("Roundsleft: " + roundsLeft);
			PhotonNetwork.LoadLevel("Intrigue");
		} else {
			Debug.Log( "Game Over" );
			PhotonNetwork.LoadLevel("PostGame");
		}
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
		nextSpawnPoint();
		photonView.RPC("getSpawnPoint", info.sender, spawnTrans.position, spawnTrans.rotation);
	}

	[RPC]
	void getSpawnPoint(Vector3 position, Quaternion rotation){
		int type = Mathf.RoundToInt(Random.Range(1,5));
		Intrigue.playerGO = PhotonNetwork.Instantiate(
						"Robot_"+ (string)PhotonNetwork.player.customProperties["Team"] +"1"/*type.ToString()*/,
						position,
						rotation, 0);

		player.TeamID = (int)PhotonNetwork.player.customProperties["TeamID"];
		photonView.RPC("ready", PhotonTargets.All);
	}

	[RPC]
	void addSpy(){
		++this.numSpies;
		++Intrigue.numSpiesLeft;
		Debug.Log("Add Spy");
	}

	[RPC]
	void addGuard(){
		++this.numGuards;
		++Intrigue.numGuardsLeft;
		Debug.Log("Add Guard");
	}

	[RPC]
	void callGameOver(string resultFromMC, int winningTeam){
		winningTeamThisRound = winningTeam;
		roundResult = resultFromMC;
		player.PrevResult = resultFromMC;
		gameOver();
	}

	[RPC]
	void syncTime(float time){
		timeLeft = time;
	}

	[RPC]
	public void ready(){
		++this.readyCount;
	}
}
