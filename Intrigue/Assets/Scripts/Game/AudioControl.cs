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
using System.Collections.Generic;
using System.Text;

public class AudioControl : MonoBehaviour {

	private AudioSource[] AudioSources;

	void Start () {
		AudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		Invoke("SetAudio",0.1f);
	}
	
	public void SetAudio(){
		if(AudioSources!=null){
			foreach(AudioSource audio in AudioSources){
				if(audio!=null){
					if(audio.gameObject.tag == "Music")
						audio.volume = Mathf.Max(Settings.MasterVolume * Settings.AmbientVolume,0);
					else
						audio.volume = Mathf.Max(Settings.MasterVolume * Settings.GameVolume,0);
				}
			}
		}
	}
}
