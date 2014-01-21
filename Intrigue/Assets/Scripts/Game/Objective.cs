using UnityEngine;
using System.Collections;

public class Objective : MonoBehaviour {

	public float completionTime;
	public int id;
	public AudioSource completeSound;

	private float timeLeft;
	private float timePrev;
	private bool caught = false;
	private PhotonView photonView;



	void Start () {
		timeLeft = timePrev = completionTime;
		timePrev = timeLeft - 1;	
		photonView = PhotonView.Get(this);
	}
	
	void Update () {
		if(this.caught){
			enabled = false;
			return;
		}	

		TextMesh[] tMeshes = GetComponentsInChildren<TextMesh>();

		foreach(TextMesh tM in tMeshes){
			if(tM.gameObject.CompareTag("Loadbar")){
				tM.text = Mathf.RoundToInt(100-((timeLeft/completionTime)*100)) + "%";
				if(timePrev >= timeLeft){
					photonView.RPC("syncPercent", PhotonTargets.AllBuffered, timeLeft);
					timePrev = timeLeft-1;
				} else if(timeLeft == 0){
					photonView.RPC("syncPercent", PhotonTargets.AllBuffered, timeLeft);
					if(PhotonNetwork.isMasterClient && !Intrigue.objectives[id]){
						++Intrigue.objectivesCompleted;
						Intrigue.objectives[id] = true;
						completeSound.Play();
					} else {
						photonView.RPC("objectiveComplete", PhotonTargets.MasterClient, this.id);
					}
					enabled = false;
				}
			}
		}
	}

	void inUse(){
		if(timeLeft > 0)
			timeLeft -= Time.deltaTime;
		else
			timeLeft = 0;
	}

	[RPC]
	void syncPercent(float timeLeft){
		this.timeLeft = timeLeft;
	}
	[RPC]
	void objectiveComplete(int id, PhotonMessageInfo info){
		Debug.Log("Objective ID " + id);
		if(!Intrigue.objectives[id]){
			++Intrigue.objectivesCompleted;
			Intrigue.objectives[id] = true;
			photonView.RPC("playSound", info.sender);
		}
	}

	[RPC]
	void playSound(){
		this.completeSound.Play();
	}
}
