using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Intrigue : MonoBehaviour {

	public int objectivesCompleted = 0;
	public bool[] objectives;
	private int numObjectives = 5;

	public static int numSpiesLeft;
	public static int numGuardsLeft;
	private int numSpies = 0;
	private int numGuards = 0;

	private static float timeLimit = 600;
	private float timeLeft = timeLimit;

	private PhotonView photonView = null;

	private GameObject[] spawnObjects;
	private List<Transform> spawns = new List<Transform>();
	private List<Transform> availableSpawns = new List<Transform>();
	private int spawnIndex;
	private Transform spawnTrans;

	public static GameObject playerGO = null;

	private static int rounds = 1;
	private static int roundsLeft = rounds;

	private Player player;

	void Start () {
		PhotonNetwork.isMessageQueueRunning = true;
		photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();

		if(PhotonNetwork.isMasterClient){
			spawnObjects = GameObject.FindGameObjectsWithTag("Respawn");
			Debug.Log("numspawns " + spawnObjects.Length);
			for(int i=0; i < spawnObjects.Length; i++){
				spawns.Add(spawnObjects[i].transform);
			}
			availableSpawns = spawns;
			spawnGuests();
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
		Debug.Log("Game Over: \nTimeLeft: " + timeLeft + " SpiesLeft: " + numSpiesLeft + " GuardsLeft: " + numGuardsLeft + " ObjectivesCompleted:" + objectivesCompleted + " numObjectives:" + numObjectives);

		if( timeLeft <= (timeLimit-10) ){
			if( timeLeft <= 0 ||  numSpiesLeft<=0 || numGuardsLeft <=0 || ((objectivesCompleted/numObjectives)*100)>=50){
				photonView.RPC("gameOver", PhotonTargets.All);
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
			PhotonNetwork.Instantiate("Robot_Guest", spawnTrans.position, spawnTrans.rotation, 0);
		}
	}

	void nextSpawnPoint(){
		spawnIndex = Random.Range(0,availableSpawns.Count-1);
		spawnTrans = availableSpawns[spawnIndex];
		availableSpawns.RemoveAt(spawnIndex);
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
		playerGO = PhotonNetwork.Instantiate(
						"Robot_"+ player.Team,
						position,
						rotation, 0);
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
	void gameOver(){
		//Reset or Go to post game
		if(Intrigue.playerGO)
				PhotonNetwork.Destroy(Intrigue.playerGO);

		if(roundsLeft > 0){
			Debug.Log( "Reset" );
			--roundsLeft;
			PhotonNetwork.isMessageQueueRunning = false;
			enabled = false;
			this.numSpies = Intrigue.numSpiesLeft = 0;
			this.numGuards = Intrigue.numGuardsLeft = 0;
			Application.LoadLevel( Application.loadedLevel );
		} else {
			Debug.Log( "Game Over" );
			PhotonNetwork.LeaveRoom();
			Application.LoadLevel( "MainMenu" );
		}
	}
}
