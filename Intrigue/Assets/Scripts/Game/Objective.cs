using UnityEngine;
using System.Collections;

public class Objective : Photon.MonoBehaviour {

	public float completionTime = 5;
	public int id;
	public bool inUse;
	private float timeLeft;
	private bool finished = false;
	private Animator anim;

	void Start () {
		timeLeft = completionTime;
		anim = GetComponent<Animator>();
		anim.SetBool("Complete",false);
		photonView.RPC("sendAnimBool",PhotonTargets.All,"Complete", false);

	}

	void Update(){

	}

	public void useObjective(GameObject user){
		if(timeLeft > 0){

			timeLeft -= Time.deltaTime;
			Debug.Log("Time Left: " + timeLeft);
		}
		else if(!finished) {
			networkView.RPC("objectiveComplete", PhotonTargets.All, this.id);
			timeLeft = 0;
			finished = true;
			Debug.Log("Objective Complete");
			user.GetComponent<PhotonView>().owner.Score += 100;
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
		Debug.Log("Objective ID " + id);
		if(id == this.id){
			finished = true;
			++Intrigue.objectivesCompleted;
			Intrigue.objectives[id] = true;
		}	
	}
}