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

public class PokerAnimScript : MonoBehaviour {

	private Animator anim;
	private bool myTurn;

	void Start () {
		anim = GetComponent<Animator>();
		//anim.SetLayerWeight(1, 1);
		myTurn = false;
	}
	
	void Update () {
		if(!anim.GetCurrentAnimatorStateInfo(0).IsName("CardPlayerCardLook")){
			if(Random.Range(0,1000) > 900)
				StartCoroutine(turnHandler());
		}	
	}

	IEnumerator turnHandler(){
		yield return new WaitForSeconds(Random.Range(5, 15));
		myTurn = true;
		anim.SetBool("MyTurn", myTurn);
		StartCoroutine(WaitAndCallback(anim.GetCurrentAnimatorStateInfo(0).length));
	}

	IEnumerator WaitAndCallback(float waitTime){
		yield return new WaitForSeconds(waitTime);
		myTurn = false;
		anim.SetBool("MyTurn", myTurn);   
	}

}
