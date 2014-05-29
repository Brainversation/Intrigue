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

public class OptionsSwitch : MonoBehaviour {

	private GameObject mainButtons;
	private GameObject optionsButtons;

	// Use this for initialization
	void Start () {
		mainButtons = GameObject.Find("MainMenuButtons");
		optionsButtons = GameObject.Find("OptionsButtons");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick(){
		mainButtons.SetActive(false);
		NGUITools.SetActive(mainButtons, false);
		optionsButtons.SetActive(true);
		NGUITools.SetActive(optionsButtons, true);
	}
}
