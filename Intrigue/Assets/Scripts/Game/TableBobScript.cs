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

public class TableBobScript : MonoBehaviour {

	private Vector3 pos1;
	private Vector3 pos2;
	private Vector3 moveTo;
	public float offset;	
	public float moveSpeed;


	// Use this for initialization
	void Start () {
		pos1 = transform.position;
		pos2 = pos1 + (offset*Vector3.down);
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position == pos1){
			moveTo = pos2;
		}

		if(transform.position == pos2){
			moveTo = pos1;
		}

		transform.position = Vector3.MoveTowards(transform.position, moveTo, moveSpeed);
	}
}
