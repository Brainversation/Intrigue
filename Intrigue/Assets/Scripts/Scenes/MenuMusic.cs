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

public class MenuMusic : MonoBehaviour {

	void Awake() {
	    // see if we've got game music still playing
	    GameObject gameMusic = GameObject.Find("GameMusic");
	    if (gameMusic) {
	        // kill game music
	        Destroy(gameMusic);
	    }
	    // make sure we survive going to different scenes
	    DontDestroyOnLoad(gameObject);
	}

	void Update(){
		GameObject[] musics = GameObject.FindGameObjectsWithTag("Music");
		if(musics.Length>=2){
			Destroy(musics[1]);
		}
	}
}
