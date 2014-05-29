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

public class AnimateBoss : MonoBehaviour {

	private bool done = true;
	private Animator anim;

	void Start () {
		anim = GetComponent<Animator>();
	}
	
	void Update () {
		if(done){
			done = false;
			StartCoroutine(StartPointing(Random.Range(5,10)));
		}
	}

	IEnumerator StartPointing(float time){
		yield return new WaitForSeconds(time);
		anim.SetBool("Pointing", true);
		StartCoroutine(EndPointing(anim.GetCurrentAnimatorStateInfo(0).length));
	}

	IEnumerator EndPointing(float time){
		yield return new WaitForSeconds(time);
		anim.SetBool("Pointing", false);
		done = true;
	}
}
