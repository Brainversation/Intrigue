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

public class BackupCamera : MonoBehaviour {
	private GameObject[] cams;
	private bool foundActive = false;

	// Use this for initialization
	void Awake () {
		gameObject.GetComponent<Camera>().enabled = false;
		gameObject.GetComponent<AudioListener>().enabled = false;
		gameObject.GetComponentInChildren<UIRoot>().enabled = false;
	}
	
	void Update(){
			cams = GameObject.FindGameObjectsWithTag("MainCamera");
			foundActive = false;
			foreach(GameObject c in cams){
				if(c.GetComponent<Camera>().enabled){
					foundActive = true;
				}
			}
			if(!foundActive){
				gameObject.GetComponent<Camera>().enabled = true;
				gameObject.GetComponent<AudioListener>().enabled = true;
				gameObject.GetComponentInChildren<UIRoot>().enabled = true;
			}
			else{
				gameObject.GetComponent<Camera>().enabled = false;
				gameObject.GetComponent<AudioListener>().enabled = false;
				gameObject.GetComponentInChildren<UIRoot>().enabled = false;
			}
		}
}
