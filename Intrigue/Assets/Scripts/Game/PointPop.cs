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

public class PointPop : MonoBehaviour {

	private float a = 1;
	private float t = .01f;
	private Color temp;
	private MeshRenderer mr;

	void Start(){
		mr = GetComponent<MeshRenderer>();
		a = 1;
		transform.LookAt(Camera.main.transform, Vector3.up);
		transform.Rotate(0,180,0);
	}
	
	void Update () {
		if(t < 0){
			a -= (Time.deltaTime);
			transform.Translate(Vector3.up * Time.deltaTime, Space.World);
			temp = mr.material.color;
			temp.a = a;
			mr.material.color = temp;
			t = .01f;
		} else {
			t -= Time.deltaTime;
		}

		transform.LookAt(Camera.main.transform, Vector3.up);
		transform.Rotate(0,180,0);

		if(a < 0) Destroy(gameObject);
	}
}
