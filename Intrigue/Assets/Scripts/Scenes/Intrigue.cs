using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Intrigue : MonoBehaviour {

	public int numberOfGuests;

	public static int objectivesCompleted = 0;
	public static bool[] objectives;
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

	public static GameObject player = null;



	void Start () {
		PhotonNetwork.isMessageQueueRunning = true;
		photonView = PhotonView.Get(this);
		numberOfGuests = PlayerPrefs.GetInt("numOfGuests");
		if(PhotonNetwork.isMasterClient){
				spawnObjects = GameObject.FindGameObjectsWithTag("Respawn");
				Debug.Log("numspawns " + spawnObjects.Length);
				for(int i=0; i < spawnObjects.Length; i++){
					spawns.Add(spawnObjects[i].transform);
				}
				availableSpawns = spawns;
			spawnGuests();
		}
		joinGame();
	}

	void Update () {
		timeLeft -= Time.deltaTime;
		
		if( timeLeft <= (timeLimit-10) ){
			if( timeLeft <= 0 ||  numSpiesLeft<=0 || numGuardsLeft <=0 || ((objectivesCompleted/numObjectives)*100)>=50){
				// Debug.Log("Game Over: \nTimeLeft: " + timeLeft + " SpiesLeft: " + numSpiesLeft + " GuardsLeft: " + numGuardsLeft + " ObjectivesCompleted:" + objectivesCompleted);
				//networkView.RPC("gameOver",PhotonTargets.AllBuffered);
			}
		}
	}

	void joinGame(){
		if( PregameLobby.team == "Guard")
			spawnGuard();
		else
			spawnSpy();
	}

	void spawnGuard(){
		photonView.RPC("addGuard",PhotonTargets.MasterClient);
		photonView.RPC("getSpawnPoint", PhotonTargets.MasterClient);
	}

	void spawnSpy(){
		photonView.RPC("addSpy",PhotonTargets.MasterClient);
		photonView.RPC("getSpawnPoint", PhotonTargets.MasterClient);
	}

	void spawnGuests(){
		for( int x = 0; x < numberOfGuests; ++x)
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
	void addSpy(){
		++this.numSpies;
	}

	[RPC]
	void sendSpawnPoint(PhotonPlayer player){
		spawnIndex = Random.Range(0,availableSpawns.Count-1);
		spawnTrans = availableSpawns[spawnIndex];
		availableSpawns.RemoveAt(spawnIndex);
		photonView.RPC("getSpawnPoint", player, spawnTrans.position, spawnTrans.rotation);
	}

	[RPC]
	void getSpawnPoint(Vector3 position, Quaternion rotation){
		player = PhotonNetwork.Instantiate(
						"Robot_"+ PregameLobby.team,
						position,
						rotation, 0);
	}


	[RPC]
	void addGuard(){
		++this.numGuards;
	}

	[RPC]
	void gameOver(){
		//Reset or Go to post game
	}
}
