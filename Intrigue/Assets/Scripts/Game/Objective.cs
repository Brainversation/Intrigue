using UnityEngine;
using System.Collections;

public class Objective : Photon.MonoBehaviour {

	public float completionTime = 5;
	public int id;
	public bool inUse;
	private float timeLeft;
	private bool finished = false;
	private Animator anim;
	private Intrigue intrigue;
	private Player player;

	void Start () {
		intrigue = GameObject.FindWithTag("Scripts").GetComponent<Intrigue>();
		player = GameObject.Find("Player").GetComponent<Player>();
		timeLeft = completionTime;
		anim = GetComponent<Animator>();
		anim.SetBool("Complete",false);
	}

	void Update(){

	}

	public void useObjective(GameObject user){
		if(timeLeft > 0){

			timeLeft -= Time.deltaTime;
			Debug.Log("Time Left: " + timeLeft);
		}
		else if(!finished) {
			photonView.RPC("objectiveComplete", PhotonTargets.All, this.id);
			timeLeft = 0;
			finished = true;
			Debug.Log("Objective Complete");
			player.Score += 100;
			anim.SetBool("Complete",true);
			photonView.RPC("sendAnimBool",PhotonTargets.All,"Complete", true);
		}
	}

	[RPC]
	void sendAnimBool(string name, bool value){
		anim.SetBool(name,value);
	}

	[RPC]
	void objectiveComplete(int id){
		//Debug.Log("Objective ID " + id);
		if(id == this.id){
			//Debug.Log("awefaw ");
			finished = true;
			timeLeft = 0;
			//Debug.Log("every line ");
			intrigue.objectivesCompleted++;
			//Debug.Log("id = " + id + "Objectives length " + intrigue.objectives.Length);
			intrigue.objectives[id] = true;
		}	
	}
}