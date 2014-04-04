using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Intrigue : MonoBehaviour {


	private float totalObjActive;
	private Player player;
	private int winningTeamThisRound;
	private int numSpies = 0;
	private int numGuards = 0;
	private static float timeLimit = 600;
	private float timeLeft = timeLimit;
	private PhotonView photonView = null;	
	private GameObject[] spawnObjects;
	private List<Transform> spawns = new List<Transform>();
	private List<Transform> availableSpawns = new List<Transform>();
	private GameObject[] objArray;
	private int spawnIndex;
	private Transform spawnTrans;
	private int readyCount = 0;

	private static int rounds = 3;
	private static int roundsLeft = rounds;

	[HideInInspector] public string roundResult;
	[HideInInspector] public float objectivesCompleted = 0;
	[HideInInspector] public bool[] objectives;
	[HideInInspector] public bool gameOverFlag = false;
	[HideInInspector] public bool gameStart = false;
	[HideInInspector] public float loadedGuests = 0;
	[HideInInspector] public float totalGuests;
	public bool wantGameOver;
	
	public static int numSpiesLeft;
	public static int numGuardsLeft;
	public static GameObject playerGO = null;

	void Awake(){
		GameObject menuMusic = GameObject.Find("MenuMusic");
		if(menuMusic){
			Destroy(menuMusic);
		}
	}

	void Start () {
		//wantGameOver = true;
		photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();
		totalGuests = player.Guests;
		if(PhotonNetwork.isMasterClient){
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
		joinGame();
	}

	void Update () {
		if(!gameStart) return;
		timeLeft -= Time.deltaTime;
		photonView.RPC("syncTime", PhotonTargets.OthersBuffered, timeLeft);
		if( timeLeft <= 0 ||  numSpiesLeft<=0 || numGuardsLeft <=0 || ((objectivesCompleted/totalObjActive)*100)>50){
			if(wantGameOver){
				Debug.Log("HERE!!!!!!!!!!!");
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
				photonView.RPC("callGameOver", PhotonTargets.All, roundResult, winningTeamThisRound);
			}
		}
	}

	void joinGame(){
		if( player.Team == "Guard")
			spawnGuard();
		else
			spawnSpy();
	}

	void spawnGuard(){
		photonView.RPC("addGuard",PhotonTargets.MasterClient);
		photonView.RPC("sendSpawnPoint", PhotonTargets.MasterClient);
	}

	void spawnSpy(){
		photonView.RPC("addSpy",PhotonTargets.MasterClient);
		photonView.RPC("sendSpawnPoint", PhotonTargets.MasterClient);
	}

	IEnumerator spawnGuests(){
		while(readyCount != PhotonNetwork.playerList.Length)
			yield return null;

		for( int x = 0; x < player.Guests; ++x)
		{	
			nextSpawnPoint();
			loadedGuests = x;
			//int type = Mathf.RoundToInt(Random.Range(1,4));
			//Debug.Log("Guest type: " + type);
			PhotonNetwork.InstantiateSceneObject("Robot_Guest1"/*+type.ToString()*/, spawnTrans.position, spawnTrans.rotation, 0, null);
			yield return  new WaitForSeconds(.1f);
		}

		// Send turn off loading
		gameStart = true;
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
			if(player.Team == "Spy"){
				player.TeamScore += 300;
			}
			else{
				player.EnemyScore += 300;
			}
		}
		else{
			if(player.Team == "Spy"){
				player.EnemyScore += 300;
			}
			else{
				player.TeamScore += 300;
			}
		}

		if(roundsLeft > 0){
			--roundsLeft;
			enabled = false;
			this.numSpies = Intrigue.numSpiesLeft = 0;
			this.numGuards = Intrigue.numGuardsLeft = 0;
			if(player.Team == "Spy"){
				player.Team = "Guard";
			} else {
				player.Team = "Spy";
			}
			PhotonNetwork.LoadLevel("Intrigue");
		} else {
			Debug.Log( "Game Over" );
			roundsLeft = rounds;
			PhotonNetwork.LoadLevel("PostGame");
		}
	}

	public float GetTimeLeft{
		get{
			return this.timeLeft;
		}
	}

	public int GetRoundsLeft{
		get{
			return roundsLeft;
		}
	}

	public int GetRounds{
		get{
			return rounds;
		}
	}


	[RPC]
	void sendSpawnPoint(PhotonMessageInfo info){
		spawnIndex = Random.Range(0,availableSpawns.Count-1);
		spawnTrans = availableSpawns[spawnIndex];
		availableSpawns.RemoveAt(spawnIndex);
		photonView.RPC("getSpawnPoint", info.sender, spawnTrans.position, spawnTrans.rotation);
	}

	[RPC]
	void getSpawnPoint(Vector3 position, Quaternion rotation){
		Intrigue.playerGO = PhotonNetwork.Instantiate(
						"Robot_"+ player.Team+"1"/*type.ToString()*/,
						position,
						rotation, 0);

		if (roundsLeft == rounds){
			if(player.Team == "Spy")
				player.TeamID = 1;
			else
				player.TeamID = 2;
		}
		photonView.RPC("ready", PhotonTargets.MasterClient);
	}

	[RPC]
	void addSpy(){
		Debug.Log("Adding spy");
		++this.numSpies;
		++Intrigue.numSpiesLeft;
	}

	[RPC]
	void addGuard(){
		Debug.Log("Adding guard");
		++this.numGuards;
		++Intrigue.numGuardsLeft;
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
		this.timeLeft = time;
	}

	[RPC]
	public void ready(){
		++this.readyCount;
	}
}
