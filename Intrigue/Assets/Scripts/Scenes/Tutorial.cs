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

public class Tutorial : MonoBehaviour {

	public UIPanel[] panels;
	private int curSlide = 0;

	void Start () {
		curSlide = 0;
		updatePanels();
	
	}
	
	void updatePanels(){
		int i = 0;
		foreach(UIPanel pan in panels){
			if(i == curSlide)
				pan.alpha = 1;
			else
				pan.alpha = 0;

			++i;
		}
	}

	public void Next(){
		if(curSlide<=panels.Length-2){
			++curSlide;
			updatePanels();
		}
	}

	public void Previous(){
		if(curSlide>=1){
			--curSlide;
			updatePanels();	
		}
	}

	public void Back(){
		Application.LoadLevel("MainMenu");
	}
}
