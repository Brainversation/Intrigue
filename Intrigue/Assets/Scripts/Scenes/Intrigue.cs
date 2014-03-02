using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Intrigue : MonoBehaviour {

	public bool wantGameOver = true;

	[HideInInspector]
	public float objectivesCompleted = 0;
	[HideInInspector]
	public bool[] objectives;
	public LoadingScreen loadingScreen;
	private int numObjectives = 0;


	public static int numSpiesLeft;
	public static int numGuardsLeft;
	private int numSpies = 0;
	private int numGuards = 0;

	private static float timeLimit = 600;
	private float timeLeft = timeLimit;

	private PhotonView photonView = null;	private GameObject[] spawnObjects;
	private List<Transform> spawns = new List<Transform>();
	private List<Transform> availableSpawns = new List<Transform>();
	private GameObject[] objArray;

	private int spawnIndex;
	private Transform spawnTrans;

	public static GameObject playerGO = null;

	private static int rounds = 3;
	private static int roundsLeft = rounds;
	private float totalObjActive;
	private Player player;
	private Network network;
	public GameObject jailSpawn;
	public LoadingScreen loadingBackup;

	void Awake(){
		GameObject menuMusic = GameObject.Find("MenuMusic");
		if(menuMusic){
			Destroy(menuMusic);
		}
	}

	void Start () {
		numObjectives = GameObject.FindGameObjectsWithTag("Objective").Length;
		PhotonNetwork.networkingPeer.NewSceneLoaded();
		photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();
		network = GameObject.FindWithTag("Scripts").GetComponent<Network>();


		if(PhotonNetwork.isMasterClient){
			spawnObjects = GameObject.FindGameObjectsWithTag("Respawn");
			Debug.Log("numspawns " + spawnObjects.Length);
			for(int i=0; i < spawnObjects.Length; i++){
				spawns.Add(spawnObjects[i].transform);
			}
			availableSpawns = spawns;
			spawnGuests();

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
		
		numObjectives = GameObject.FindGameObjectsWithTag("Objective").Length;
		objectives = new bool[numObjectives];
		// set all objectives to incomplete at start
		for(int i = 0; i < objectives.Length; i++){
			objectives[i] = false;
		}

		joinGame();
	}

	void Update () {
		timeLeft -= Time.deltaTime;
		photonView.RPC("syncTime", PhotonTargets.OthersBuffered, timeLeft);
		if( timeLeft <= (timeLimit-10) ){
			if( timeLeft <= 0 ||  numSpiesLeft<=0 || numGuardsLeft <=0 || ((objectivesCompleted/totalObjActive)*100)>50){
				if(wantGameOver){
					Debug.Log("Game Over: \nTimeLeft: " + timeLeft + " SpiesLeft: " + numSpiesLeft + " GuardsLeft: " + numGuardsLeft + " ObjectivesCompleted:" + objectivesCompleted + " numObjectives:" + numObjectives);
					photonView.RPC("callGameOver", PhotonTargets.All);
				}
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

	void spawnGuests(){
		for( int x = 0; x < player.Guests; ++x)
		{	
			nextSpawnPoint();
			//int type = Mathf.RoundToInt(Random.Range(1,4));
			//Debug.Log("Guest type: " + type);
			PhotonNetwork.InstantiateSceneObject("Robot_Guest1"/*+type.ToString()*/, spawnTrans.position, spawnTrans.rotation, 0, null);
		}
	}

	void nextSpawnPoint(){
		spawnIndex = Random.Range(0,availableSpawns.Count-1);
		spawnTrans = availableSpawns[spawnIndex];
		availableSpawns.RemoveAt(spawnIndex);
	}

	void gameOver(){
		
		if(playerGO!=null)
			playerGO.GetComponentInChildren<Camera>().enabled = false;
		loadingScreen = loadingBackup;

		if(roundsLeft > 0){
			Debug.Log( "Reset" );
			--roundsLeft;
			photonView.RPC("syncRounds", PhotonTargets.OthersBuffered, roundsLeft);
			//PhotonNetwork.isMessageQueueRunning = false;
			enabled = false;
			this.numSpies = Intrigue.numSpiesLeft = 0;
			this.numGuards = Intrigue.numGuardsLeft = 0;
			//PhotonNetwork.LoadLevel( Application.loadedLevel );
			loadingScreen.StartLoadingLevel("Intrigue");
		} else {
			Debug.Log( "Game Over" );
			//PhotonNetwork.isMessageQueueRunning = false;
			//PhotonNetwork.LoadLevel("PostGame");
			loadingScreen.StartLoadingLevel("PostGame");
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
		//int type = Mathf.RoundToInt(Random.Range(1,3));
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
	void callGameOver(){
		gameOver();
	}

	[RPC]
	void syncTime(float time){
		this.timeLeft = time;
	}

	[RPC]
	void syncRounds(int rounds){
		roundsLeft = rounds;
	}
}
