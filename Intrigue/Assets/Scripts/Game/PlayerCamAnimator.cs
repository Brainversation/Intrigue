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

public class PlayerCamAnimator : MonoBehaviour {
	
	public Transform startMarker;
	public Transform endMarker;
	public float camSpeed = 2.0f;
	public float rotSpeed = .5f;
	public float smooth = 5.0F;
	public GameObject playerObj;

	private float startTime;
	private float journeyLength;
	private Quaternion startRot;
	private Quaternion endRot;
	private bool started = false;

	void Update() {
		if(playerObj.GetComponent<BasePlayer>().isOut){
			if(!started){
				started = true;
				playerObj.GetComponent<BasePlayer>().outStarted();
				startTime = Time.time;
				startRot = transform.rotation;
				endRot = endMarker.rotation;
				journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
				gameObject.GetComponent<MouseLook>().enabled = false;
				playerObj.GetComponent<MouseLook>().enabled = false;
			}

			float distCovered = (Time.time - startTime) * camSpeed;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
			transform.rotation = Quaternion.Lerp(startRot, endRot, Time.time * rotSpeed);
		}
	}
}
