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

public class RepairCapsule : QueueHotSpot {

	private bool repair = false;

	public override void Update(){
		base.Update();
		if(	queue.Count >= 1 &&
			!queue[0].GetComponent<NavMeshAgent>().hasPath &&
			queue[0].GetComponent<BaseAI>().isYourTurn){
			
			Quaternion temp = gameObject.transform.rotation;
			temp.x = 0;
			temp.z = 0;
			queue[0].transform.rotation = temp;
			queue[0].transform.Rotate(0,-90,0);
			Vector3 temp2 = gameObject.transform.position;
			temp2.y = queue[0].transform.position.y;
			queue[0].transform.position = temp2;
			if(!IsInvoking() && !repair)
				Invoke("doRepair", 2.5f);
		}
	}

	public void doRepair(){
		repair = true;
	}

	protected override void OnTriggerExit(Collider other){
		base.OnTriggerExit(other);
		if(other.gameObject.layer == BasePlayer.GUEST && queue.Contains(other.gameObject)){
			repair = false;
		}
	}
}
