/**************************************************************************
 **                                                                       *
 **                COPYRIGHT AND CONFIDENTIALITY NOTICE                   *
 **                                                                       *
 **    Copyright (c) 2014 Hacksaw Games. All rights reserved.             *
 **                                                                       *
 **    This software contains information confidential and proprietary    *
 **    to Hacksaw Games. It shall not be reproduced in whole or in        *
 **    part, or transferred to other documents, or disclosed to third     *
 **    parties, or used for any purpose other than that for which it was  *
 **    obtained, without the prior written consent of Hacksaw Games.      *
 **                                                                       *
 **************************************************************************/

using UnityEngine;
using System.Collections;

public class ObjectiveMain : Photon.MonoBehaviour {
	[HideInInspector]
	public float completionTime = 60;
	public int objectiveName;
	public TextMesh completionPercent;
	public TextMesh status;
	public int id;
	public int completionPercentage;
	public bool inUse = false;
	public bool isActive = true;
	public bool textAdded = false;
	public string objectiveType;
	public GameObject pointPop;

	private float timeLeft;
	private bool finished = false;
	private Intrigue intrigue;
	private float distMultiplier;

	void Start () {
		objectiveType = "Server";
		intrigue = GameObject.FindWithTag("Scripts").GetComponent<Intrigue>();
		timeLeft = completionTime;
		status.text = "SERVER: <"+objectiveName+">";
	}

	void Update(){
		completionPercentage = Mathf.RoundToInt(100-((timeLeft/completionTime)*100));
		completionPercent.text = completionPercentage.ToString() + "%";
	}

	public void useObjective(GameObject user, int teamID){
		if(isActive){
			if(Vector3.Distance(user.transform.position, transform.position)>=60)
				distMultiplier = 1;
			else
				distMultiplier = 6-(5*((Vector3.Distance(user.transform.position, transform.position)/60)));
			if(timeLeft > 0){
				timeLeft -= Time.deltaTime*distMultiplier;
				photonView.RPC("updateTimeLeft", PhotonTargets.All, timeLeft);
				inUse = true;
				photonView.RPC("setInUse", PhotonTargets.Others, true);
				Intrigue.playerGO.GetComponent<Spy>().doingObjective = true;
				Intrigue.playerGO.GetComponent<Spy>().percentComplete = -((timeLeft-completionTime)/completionTime);

			} else if(!finished) {
				photonView.RPC("objectiveComplete", PhotonTargets.All, this.id);
				timeLeft = 0;
				inUse = false;
				photonView.RPC("setInUse", PhotonTargets.Others, false);
				finished = true;
				user.GetComponent<Spy>().doingObjective = false;
				user.GetComponent<Spy>().photonView.RPC("addPlayerScore", PhotonTargets.All, 200, teamID);
				pointPop.GetComponent<TextMesh>().text = "+200";
				Instantiate(pointPop, transform.position + (Vector3.up * GetComponent<Collider>().bounds.size.y), transform.rotation);
				user.GetComponent<BasePlayer>().newEvent("[00CCFF]"+user.GetComponent<BasePlayer>().localHandle +"[-] [FFCC00]has downloaded [-][33CCCC]Objective " + objectiveName + "[-][00CCFF]![-]");
				isActive = false;
			}
		}
	}

	public void activate(){
		photonView.RPC("setActive", PhotonTargets.AllBuffered);
	}

	[RPC]
	void setInUse(bool state){
		inUse = state;
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