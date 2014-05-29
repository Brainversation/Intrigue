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

public class ServerIndicator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Camera.main!=null){
			inView();
			transform.LookAt(Camera.main.transform);
			transform.Rotate(Vector3.up, 180);
		}
	}

	void inView(){
		if(Vector3.Distance(Camera.main.transform.position, transform.position)<70){
			gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
		else{
			gameObject.GetComponent<MeshRenderer>().enabled = true;
		}
	}
}
