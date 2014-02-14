using UnityEngine;
using System.Collections;

public class Objective : Photon.MonoBehaviour {

	public float completionTime = 5;
	public int id;
	public bool inUse;
	public bool active = false;
	public bool textAdded = false;
	private float timeLeft;
	private bool finished = false;
	private Animator anim;
	private Network network;
	private Player player;
	private Intrigue intrigue;

	void Start () {
		network = GameObject.FindWithTag("Scripts").GetComponent<Network>();
		intrigue = GameObject.FindWithTag("Scripts").GetComponent<Intrigue>();
		player = GameObject.Find("Player").GetComponent<Player>();
		timeLeft = completionTime;
		anim = GetComponent<Animator>();
		anim.SetBool("Complete",false);
	}

	void Update(){

	}

	public void useObjective(GameObject user){
		if(active){
			if(timeLeft > 0){
				timeLeft -= Time.deltaTime;
				Debug.Log("Time Left: " + timeLeft);
			} else if(!finished) {
				photonView.RPC("objectiveComplete", PhotonTargets.All, this.id);
				timeLeft = 0;
				finished = true;
				Intrigue.playerGO.GetComponent<Spy>().photonView.RPC("addPlayerScore", PhotonTargets.AllBuffered, 100);
				photonView.RPC("addScore", PhotonTargets.AllBuffered, player.TeamID, 100);
				anim.SetBool("Complete",true);
				active = false;
				photonView.RPC("sendAnimBool",PhotonTargets.All,"Complete", true);
			}
		}
	}

	[RPC]
	void sendAnimBool(string name, bool value){
		anim.SetBool(name,value);
	}

	[RPC]
	void objectiveComplete(int id){
		if(id == this.id){
			finished = true;
			active = false;
			timeLeft = 0;
			intrigue.objectivesCompleted++;
			intrigue.objectives[id] = true;
		}	
	}

	[RPC]
	void addScore(int teamID, int scoreToAdd){
		if(teamID == this.player.TeamID){
			player.TeamScore += scoreToAdd;
		}
		else{
			player.EnemyScore += scoreToAdd;
		}
	}
}