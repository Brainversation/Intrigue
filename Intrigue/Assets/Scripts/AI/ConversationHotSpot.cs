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
using System.Collections.Generic;
using BehaviorTree;

public class ConversationHotSpot : MonoBehaviour {

	private List<GameObject> queue = new List<GameObject>();
	private List<GameObject> players = new List<GameObject>();
	private Vector3[] spots = null;
	private Vector3 center;
	private int population = 0;
	private int talkerIndex = 0;
	private float talkerTime = 5;
	private float radius;
	private float slice;
	private bool canDestroy = false;

	public static int max = 5;

	void Start(){
		if(!BaseAI.aiTesting && !PhotonNetwork.isMasterClient){
			enabled = false;
			return;
		}

		Invoke("failSafe", 5);

		// Builds pie for AI positions
		radius = GetComponent<NavMeshObstacle>().radius = max/2;
		GetComponent<SphereCollider>().radius = max*1.5f;
		center = transform.TransformPoint(GetComponent<SphereCollider>().center);
		slice = 360/max;
		spots = new Vector3[max];
		for(int i = 0; i < spots.Length; ++i){
			spots[i] = new Vector3(findX(i+1), center.y, findZ(i+1));
		}
	}

	void Update () {
		if( canDestroy && population == 0){
			if(BaseAI.aiTesting)
				Destroy(gameObject);
			else
				PhotonNetwork.Destroy(gameObject);
		} else {
			for(int i = 0; i < queue.Count; ++i){
				if(queue[i].gameObject.layer == BasePlayer.GUARD || queue[i].gameObject.layer == BasePlayer.SPY ||
					!queue[i].GetComponent<BaseAI>().agent.hasPath){
						if(i == talkerIndex && queue.Count > 1){
							queue[i].GetComponent<Animator>().SetBool("Converse", true);
							talkerTime -= Time.deltaTime;
						}

						if(talkerTime < 0){
							queue[talkerIndex].GetComponent<Animator>().SetBool("Converse", false);
							talkerIndex = Random.Range(0, queue.Count);
							talkerTime = 5;
						}

						if(queue[i].gameObject.layer == BasePlayer.GUEST)
							queue[i].transform.LookAt(center);
				}
			}
		}


	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.layer == BasePlayer.GUEST &&
		   other.gameObject.GetComponent<BaseAI>().destination == gameObject.transform.position){
			BaseAI script = other.gameObject.GetComponent<BaseAI>();
			script.agent.SetDestination(spots[queue.Count]);
			script.anim.SetBool("Speed", true);
			script.distFromDest = 2;
			script.status = Status.Waiting;
			++population;
			queue.Add(other.gameObject);
			script.inConvo = true;
		}

		//Activate GUI that says enter conversation hotspot for player
		if(other.gameObject.layer == BasePlayer.GUARD || other.gameObject.layer == BasePlayer.SPY){
			other.gameObject.GetComponent<BasePlayer>().conversationGUI.alpha = 1;
			players.Add(other.gameObject);
		}
	}

	void OnTriggerStay(Collider other){
		if(other.gameObject.layer == 8 || other.gameObject.layer == 10){
			// If player presses E and the GUI element is present, then add to convo
			if(other.gameObject.GetComponent<BasePlayer>().conversationGUI.alpha == 1 && Input.GetKeyUp(Settings.Interact)){
				++population;
				queue.Add(other.gameObject);
				other.gameObject.GetComponent<BasePlayer>().inConvoGUI.alpha = 1;
				other.gameObject.GetComponent<BasePlayer>().conversationGUI.alpha = 0;
			} else if(Input.GetKeyUp(Settings.Cancel)){
				other.gameObject.GetComponent<BasePlayer>().conversationGUI.alpha = 0;
			}
		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.layer == BasePlayer.GUEST && other.GetComponent<BaseAI>().inConvo){
			--population;
			queue.Remove(other.gameObject);
			other.GetComponent<BaseAI>().inConvo = false;
			if(other.gameObject.GetComponent<Animator>().GetBool("Converse"))
				other.gameObject.GetComponent<Animator>().SetBool("Converse", false);
		} else if (other.gameObject.layer == BasePlayer.GUARD || other.gameObject.layer == BasePlayer.SPY){
			players.Remove(other.gameObject);
			other.gameObject.GetComponent<BasePlayer>().conversationGUI.alpha = 0;
			if(queue.Contains(other.gameObject)){
				--population;
				queue.Remove(other.gameObject);
				other.gameObject.GetComponent<BasePlayer>().inConvoGUI.alpha = 0;
				if(other.gameObject.GetComponent<Animator>().GetBool("Converse"))
					other.gameObject.GetComponent<Animator>().SetBool("Converse", false);
			}
		}
	}

	// Destroy the conversation GUI if the convo hot spot is destroyed
	void OnDestroy(){
		foreach(GameObject g in players){
			g.GetComponent<BasePlayer>().conversationGUI.alpha = 0;
			g.GetComponent<BasePlayer>().inConvoGUI.alpha = 0;
		}
	}

	void failSafe(){
		canDestroy = true;
	}

	float findX(int index){
		return center.x + radius*Mathf.Cos(slice * index);
	}

	float findZ(int index){
		return center.z + radius*Mathf.Sin(slice * index);
	}

	void OnMasterClientSwitched(PhotonPlayer newMasterClient){
		if(PhotonNetwork.player.ID == newMasterClient.ID){
			enabled = true;
		}
	}
}
