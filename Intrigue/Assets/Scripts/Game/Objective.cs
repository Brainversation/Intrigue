using UnityEngine;
using System.Collections;

public class Objective : MonoBehaviour {

	public float completionTime = 5;
	public int id;
	private float timeLeft;
	private bool finished = false;

	void Start () {
		timeLeft = completionTime;
	}

	void Update(){

	}

	public void useObjective(GameObject user){
		if(timeLeft > 0){
			timeLeft -= Time.deltaTime;
			Debug.Log("Time Left: " + timeLeft);
		}
		else if(!finished) {
			timeLeft = 0;
			finished = true;
			Debug.Log("Objective Complete");
			user.GetComponent<PhotonView>().owner.Score += 100;
		}
	}
}