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
using BehaviorTree;

public class RelaxHotSpot : MonoBehaviour {

	[HideInInspector] public bool occupied = false;
	[HideInInspector] private GameObject currGuest;

	void OnTriggerEnter(Collider other){
		if (other.gameObject.layer == BasePlayer.GUEST && 
			other.gameObject.GetComponent<BaseAI>().destination == gameObject.transform.position){
			currGuest = other.gameObject;
			//other.gameObject.GetComponent<BaseAI>().status = Status.Waiting;
			occupied = true;
		}
	}

	protected virtual void OnTriggerExit(Collider other){
		if(other.gameObject.layer == BasePlayer.GUEST && other.gameObject == currGuest){				
			occupied = false;
		}
	}

}
