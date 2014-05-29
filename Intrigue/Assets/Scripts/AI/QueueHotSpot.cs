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

public class QueueHotSpot : MonoBehaviour {

	[HideInInspector] public List<GameObject> queue = new List<GameObject>();
	public Transform actionDestination;

	public virtual void Update(){
		if(queue.Count >= 1 && !queue[0].GetComponent<BaseAI>().isYourTurn){
			queue[0].GetComponent<BaseAI>().isYourTurn = true;
			queue[0].GetComponent<BaseAI>().destination = actionDestination.position;
			queue[0].GetComponent<BaseAI>().distFromDest = 1f;
			queue[0].GetComponent<NavMeshAgent>().SetDestination(actionDestination.position);
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Guest" &&
			other.gameObject.GetComponent<BaseAI>().destination == gameObject.transform.position){
			other.gameObject.GetComponent<BaseAI>().status = Status.Waiting;
			queue.Add(other.gameObject);
		}
	}

	protected virtual void OnTriggerExit(Collider other){
		if(other.tag == "Guest" && other.gameObject.GetComponent<BaseAI>().isYourTurn){
			queue.Remove(other.gameObject);
			other.gameObject.GetComponent<BaseAI>().isYourTurn = false;
		}
	}
}
