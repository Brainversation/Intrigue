using UnityEngine;
using System.Collections;

public class ObjectiveMain : Photon.MonoBehaviour {
	[HideInInspector]
	public float completionTime = 60;
	public int objectiveName;
	public TextMesh completionPercent;
	public TextMesh status;
	public int id;
	public bool inUse = false;
	public bool isActive = true;
	public bool textAdded = false;
	public string objectiveType;
	private float timeLeft;
	private bool finished = false;
	private Player player;
	private Intrigue intrigue;
	private float distMultiplier;

	void Start () {
		intrigue = GameObject.FindWithTag("Scripts").GetComponent<Intrigue>();
		player = GameObject.Find("Player").GetComponent<Player>();
		timeLeft = completionTime;
		status.text = "SERVER: <"+objectiveName+">";
	}

	void Update(){
		completionPercent.text = Mathf.RoundToInt(100-((timeLeft/completionTime)*100)).ToString() + "%";
	}

	public void useObjective(GameObject user){
		if(isActive){
			if(Vector3.Distance(user.transform.position, transform.position)>=60)
				distMultiplier = 1;
			else
				distMultiplier = 6-(5*((Vector3.Distance(user.transform.position, transform.position)/60)));
			if(timeLeft > 0){
				timeLeft -= Time.deltaTime*distMultiplier;
				photonView.RPC("updateTimeLeft", PhotonTargets.All, timeLeft);
				inUse = true;
				Intrigue.playerGO.GetComponent<Spy>().doingObjective = true;
				Intrigue.playerGO.GetComponent<Spy>().percentComplete = -((timeLeft-completionTime)/completionTime);
				if(objectiveType=="Computer"){
					photonView.RPC("playAudio", PhotonTargets.All);
				}
			} else if(!finished) {
				if(objectiveType=="Safe"){
					photonView.RPC("playAudio", PhotonTargets.All);
				}
				photonView.RPC("objectiveComplete", PhotonTargets.All, this.id);
				timeLeft = 0;
				inUse = false;
				finished = true;
				user.GetComponent<Spy>().doingObjective = false;
				user.GetComponent<Spy>().photonView.RPC("addPlayerScore", PhotonTargets.All, 200);
				isActive = false;
			}
		}


	}

	public void activate(){
		photonView.RPC("setActive", PhotonTargets.AllBuffered);
	}

	[RPC]
	void playAudio(){
		audio.Play();
	}

	[RPC]
	void updateTimeLeft(float tL){
		timeLeft = tL;
	}

	[RPC]
	void objectiveComplete(int id){
		if(id == this.id){
			finished = true;
			isActive = false;
			timeLeft = 0;
			intrigue.objectivesCompleted++;
			intrigue.mainObjectives[objectiveName-1] = true;
		}	
	}

	[RPC]
	void setActive(){
		isActive = true;
	}

}