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

public class Objective : Photon.MonoBehaviour {

	public const int OBJECTIVE = 11;
	
	public float completionTime = 5;
	public int id;
	public bool inUse;
	public bool isActive = false;
	public bool textAdded = false;
	public string objectiveType;
	public AudioSource tumbler;
	public AudioSource door;
	public GameObject pointPop;

	private float timeLeft;
	private bool finished = false;
	private Animator anim;

	void Start () {
		timeLeft = completionTime;
		anim = GetComponent<Animator>();
		anim.SetBool("Complete",false);
	}

	public void useObjective(GameObject user, int teamID){
		if(isActive){
			if(timeLeft > 0){
				if(!tumbler.isPlaying){
					photonView.RPC("playTumbler", PhotonTargets.All);
				}
				timeLeft -= Time.deltaTime;
				Intrigue.playerGO.GetComponent<Spy>().doingObjective = true;
				Intrigue.playerGO.GetComponent<Spy>().percentComplete = -((timeLeft-completionTime)/completionTime);
			} else if(!finished) {
				if(objectiveType=="Safe"){
					photonView.RPC("playComplete", PhotonTargets.All);
					photonView.RPC("stopTumbler", PhotonTargets.All);
				}
				photonView.RPC("objectiveComplete", PhotonTargets.All, this.id);
				timeLeft = 0;
				finished = true;
				user.GetComponent<Spy>().doingObjective = false;
				user.GetComponent<Spy>().photonView.RPC("addPlayerScore", PhotonTargets.All, 50, teamID);
				pointPop.GetComponent<TextMesh>().text = "+50";
				Instantiate(pointPop, transform.position + (transform.up * 3), transform.rotation);
				anim.SetBool("Complete",true);
				isActive = false;
				photonView.RPC("sendAnimBool",PhotonTargets.All,"Complete", true);
			}
		}


	}

	public void activate(){
		photonView.RPC("setActive", PhotonTargets.AllBuffered);
	}

	[RPC]
	void sendAnimBool(string name, bool value){
		anim.SetBool(name,value);
	}

	[RPC]
	void playComplete(){
		door.Play();
	}

	[RPC]
	void playTumbler(){
		tumbler.Play();
	}

	[RPC]
	void stopTumbler(){
		tumbler.Stop();
	}

	[RPC]
	void objectiveComplete(int id){
		if(id == this.id){
			finished = true;
			isActive = false;
			timeLeft = 0;
		}	
	}

	[RPC]
	void setActive(){
		isActive = true;
	}
}