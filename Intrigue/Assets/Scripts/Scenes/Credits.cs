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

public class Credits : MonoBehaviour {
	
	void Awake(){
		GameObject menuMusic = GameObject.Find("MenuMusic");
		if(menuMusic){
			Destroy(menuMusic);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(transform.localPosition.y<3500){
			transform.Translate(new Vector3(0f,0.15f,0f) * Time.deltaTime);
		}
		else{
			Vector3 temp = new Vector3(0f,-113f,0f);
			transform.localPosition = temp;
		}
	}

	public void ReturnToMain(){
		Application.LoadLevel("MainMenu");
	}
}
