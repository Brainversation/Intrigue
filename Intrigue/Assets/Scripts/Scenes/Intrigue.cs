using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Intrigue : MonoBehaviour {

	public GameObject GuardPrefab;
	public GameObject GuardAnimPrefab;
	public GameObject SpyPrefab;
	public GameObject SpyAnimPrefab;
	public GameObject GuestPrefab;
	public GameObject GuestAnimPrefab;
	public int numberOfGuests;

	public static int objectivesCompleted = 0;
	public static bool[] objectives;
	public static int numSpiesLeft;
	public static int numGuardsLeft;

	public static bool isSpectating = false;
	public static int playerCount = 0;
	// UNCOMMENT AFTER TRANSLATING Player.cs public static Player[] players;

	public static bool dedicatedServer = false;

	private int numSpies = 0;
	private int numGuards = 0;
	private static float timeLimit = 600;
	private int numObjectives =5;
	private float timeLeft = timeLimit;
	private PhotonView photonView = null;
	private GameObject[] spawnObjects;
	private List<Transform> spawns = new List<Transform>();
	private List<Transform> availableSpawns = new List<Transform>();
	private int spawnIndex;
	private Transform spawnTrans;
	private bool isOver = false;
	private bool wait = false;
	private int test;
	private int neededConnections = 1;

	void Start () {
		PhotonNetwork.isMessageQueueRunning = true;
		photonView = PhotonView.Get(this);
		if(PhotonNetwork.isMasterClient){
			spawnGuests();
		}
		if( PhotonNetwork.playerList.Length >= neededConnections )
			joinGame();
		else{
			//Attach to wait camera or put camera in wait position
			wait = true;
			Debug.Log("Waiting For other Players...");
		}
	}

	void Update () {
		if( !wait ){
			timeLeft -= Time.deltaTime;
		
			if( timeLeft <= 0 ||  numSpiesLeft<=0 || numGuardsLeft <=0 || ((objectivesCompleted/numObjectives)*100)>=50){
				isOver = true;
				Debug.Log("Game Over" + timeLeft + " " + numSpiesLeft + " " + numGuardsLeft + " " + objectivesCompleted);
				//networkView.RPC("gameOver",RPCMode.Others,isOver);
			}
			
		}else if( PhotonNetwork.playerList.Length >= neededConnections-1 ) {
			joinGame();
			wait = false;
		}
	}

	void joinGame(){
		string playerType = PlayerPrefs.GetString("playerType");
		if( playerType == "Guard")
			spawnGuard();
		else
			spawnSpy();
	}

	void spawnGuard(){
		photonView.RPC("addGuard",PhotonTargets.MasterClient);
		PhotonNetwork.Instantiate("GuardPrefab", GuardPrefab.transform.position, Quaternion.identity, 0);
	}

	void spawnSpy(){
		photonView.RPC("addSpy",PhotonTargets.MasterClient);
		PhotonNetwork.Instantiate("SpyPrefab", SpyPrefab.transform.position, Quaternion.identity, 1);
	}

	void spawnGuests(){
		for( int i = 0; i < numberOfGuests; ++i)
		{
			Vector3 temp = GuestPrefab.transform.position;
			temp.x += i;
			temp.z += i;
			PhotonNetwork.Instantiate("GuestPrefab", temp, Quaternion.identity, 3);
		}
	}

	[RPC]
	void addSpy(){
		++this.numSpies;
	}

	[RPC]
	void addGuard(){
		++this.numGuards;
	}

	[RPC]
	void gameOver(bool isOver){
		this.isOver = isOver;
	}
}
