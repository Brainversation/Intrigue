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

	private GameObject[] spawnObjects;
	private List<Transform> spawns = new List<Transform>();
	private List<Transform> availableSpawns = new List<Transform>();
	private int spawnIndex;
	private Transform spawnTrans;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
