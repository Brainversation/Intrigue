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

public class Crosshair : MonoBehaviour {

	public Texture2D crosshairNormal;
	public Texture2D crosshairInteract;
	public bool teamSpy;

	[HideInInspector] public bool canInteract = false;

	public static GameObject currGo = null;
	private Rect position;

	// Use this for initialization
	void Start () {
		position = new Rect((Screen.width - crosshairNormal.width)/2,
								(Screen.height - crosshairNormal.height)/2,
									crosshairNormal.width, crosshairNormal.height);
		teamSpy = GetComponent<Spy>() != null;
	}

	void Update(){
		canInteract = false;
		if(Camera.main != null && currGo != null){
			//Spy
			if(teamSpy){
				if( currGo.tag == "ObjectiveMain" || currGo.tag =="Objective"){
					if(currGo.tag == "ObjectiveMain" && currGo.GetComponent<ObjectiveMain>().isActive ){
						canInteract = true;
					} else if(currGo.tag == "Objective" && currGo.GetComponent<Objective>().isActive && (Vector3.Distance(currGo.transform.position, transform.position) )<=10){
						canInteract = true;
					}
				}else if( (currGo.tag == "Guard" || currGo.tag == "Guest") && Vector3.Distance(currGo.transform.position, transform.position)<15){
						if(currGo.tag == "Guard" && !currGo.GetComponent<Guard>().stunned){
							canInteract = true;
						}
						if(currGo.tag == "Guest" && !currGo.GetComponent<BaseAI>().stunned){
							canInteract = true;
						}
				}
			//Guard
			} else if(currGo.layer == 9 || (currGo.layer == 10 && !currGo.GetComponent<BasePlayer>().isOut)){
					canInteract = true;
			}
			currGo = null;
		}
	}
	
	void OnGUI () {
		if(!Input.GetKey(KeyCode.Tab)){
			if(canInteract==true)
				GUI.DrawTexture(position, crosshairInteract);
			else
				GUI.DrawTexture(position, crosshairNormal);
		}
	}
}
