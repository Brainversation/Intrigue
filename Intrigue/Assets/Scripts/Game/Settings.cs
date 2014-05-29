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
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Settings : MonoBehaviour {

	public static float MouseSensitivityX;
	public static float MouseSensitivityY;
	public static string Interact;
	public static string Mark;
	public static string Stun;
	public static string Cancel;
	public static string Sprint;

	public static float MasterVolume;
	public static float AmbientVolume;
	public static float GameVolume;

	public static int QualitySetting;

	public static List<string> BoundKeys = new List<string>();

	private static string playerPrefsPrefix;

	// Use this for initialization
	public static void Start () {

		//Player Prefs
		if (Application.isEditor)
			playerPrefsPrefix = "PlayerEditor";
		else
			playerPrefsPrefix = "Player";

		//QualitySetting
		if( PlayerPrefs.HasKey( playerPrefsPrefix + "QualitySetting" )){
			QualitySetting = PlayerPrefs.GetInt(playerPrefsPrefix + "QualitySetting");
		}else{
			QualitySetting = 3;
			PlayerPrefs.SetInt(playerPrefsPrefix + "QualitySetting", QualitySetting);
		}

		//Master Volume
		if( PlayerPrefs.HasKey( playerPrefsPrefix + "MasterVolume" )){
			MasterVolume = PlayerPrefs.GetFloat(playerPrefsPrefix + "MasterVolume");
		}else{
			MasterVolume = 1f;
			PlayerPrefs.SetFloat(playerPrefsPrefix + "MasterVolume", MasterVolume);
		}
		
		//Game Volume
		if( PlayerPrefs.HasKey( playerPrefsPrefix + "GameVolume" )){
			GameVolume = PlayerPrefs.GetFloat(playerPrefsPrefix + "GameVolume");
		}else{
			GameVolume = 1f;
			PlayerPrefs.SetFloat(playerPrefsPrefix + "GameVolume", GameVolume);
		}

		//Ambient Volume
		if( PlayerPrefs.HasKey( playerPrefsPrefix + "AmbientVolume" )){
			AmbientVolume = PlayerPrefs.GetFloat(playerPrefsPrefix + "AmbientVolume");
		}else{
			AmbientVolume = 1f;
			PlayerPrefs.SetFloat(playerPrefsPrefix + "AmbientVolume", AmbientVolume);
		}

		//Mouse SensitivityX
		if( PlayerPrefs.HasKey( playerPrefsPrefix + "MouseSensitivityX" )){
			MouseSensitivityX = PlayerPrefs.GetFloat(playerPrefsPrefix + "MouseSensitivityX");
		}else{
			MouseSensitivityX = 15;
			PlayerPrefs.SetFloat(playerPrefsPrefix + "MouseSensitivityX", MouseSensitivityX);
		}

		//Mouse SensitivityY
		if( PlayerPrefs.HasKey( playerPrefsPrefix + "MouseSensitivityY" )){
			MouseSensitivityY = PlayerPrefs.GetFloat(playerPrefsPrefix + "MouseSensitivityY");
		}else{
			MouseSensitivityY = 15;
			PlayerPrefs.SetFloat(playerPrefsPrefix + "MouseSensitivityY", MouseSensitivityY);
		}

		//Sprint Button
		if( PlayerPrefs.HasKey( playerPrefsPrefix + "Sprint" )){
			Sprint = PlayerPrefs.GetString(playerPrefsPrefix + "Sprint");
		}else{
			Sprint = "left shift";
			PlayerPrefs.SetString(playerPrefsPrefix + "Sprint", Sprint);
		}

		//Interact Button
		if( PlayerPrefs.HasKey( playerPrefsPrefix + "Interact" )){
			Interact = PlayerPrefs.GetString(playerPrefsPrefix + "Interact");
		}else{
			Interact = "e";
			PlayerPrefs.SetString(playerPrefsPrefix + "Interact", Interact);
		}

		//Mark Button
		if( PlayerPrefs.HasKey( playerPrefsPrefix + "Mark" )){
			Mark = PlayerPrefs.GetString(playerPrefsPrefix + "Mark");
		}else{
			Mark = "m";
			PlayerPrefs.SetString(playerPrefsPrefix + "Mark", Mark);
		}

		//Stun Button
		if( PlayerPrefs.HasKey( playerPrefsPrefix + "Stun" )){
			Stun = PlayerPrefs.GetString(playerPrefsPrefix + "Stun");
		}else{
			Stun = "f";
			PlayerPrefs.SetString(playerPrefsPrefix + "Stun", Stun);
		}

		//Cancel Button
		if( PlayerPrefs.HasKey( playerPrefsPrefix + "Cancel" )){
			Cancel = PlayerPrefs.GetString(playerPrefsPrefix + "Cancel");
		}else{
			Cancel = "space";
			PlayerPrefs.SetString(playerPrefsPrefix + "Cancel", Cancel);
		}

		UpdateBoundKeys();
	}

	public static void SetKey(string key, string newBinding){
		switch(key){
			case "Interact": 
				Interact = newBinding;
				PlayerPrefs.SetString(playerPrefsPrefix + "Interact", Interact);
				break;

			case "Mark": 
				Mark = newBinding;
				PlayerPrefs.SetString(playerPrefsPrefix + "Mark", Mark);
				break;

			case "Stun": 
				Stun = newBinding;
				PlayerPrefs.SetString(playerPrefsPrefix + "Stun", Stun);
				break;

			case "Cancel": 
				Cancel = newBinding;
				PlayerPrefs.SetString(playerPrefsPrefix + "Cancel", Cancel);
				break;

			case "Sprint":
				Sprint = newBinding;
				PlayerPrefs.SetString(playerPrefsPrefix + "Sprint", Sprint);
				break;		
			}
		PlayerPrefs.Save();
	}

	public static void SetFloat(string key, float newSetting){
		switch(key){
			case "MouseX":
				MouseSensitivityX = newSetting;
				PlayerPrefs.SetFloat(playerPrefsPrefix + "MouseSensitivityX", MouseSensitivityX);
				break;

			case "MouseY":
				MouseSensitivityY = newSetting;
				PlayerPrefs.SetFloat(playerPrefsPrefix + "MouseSensitivityY", MouseSensitivityY);
				break;

			case "MasterVolume":
				MasterVolume = newSetting;
				PlayerPrefs.SetFloat(playerPrefsPrefix + "MasterVolume", MasterVolume);
				break;

			case "GameVolume":
				GameVolume = newSetting;
				PlayerPrefs.SetFloat(playerPrefsPrefix + "GameVolume", GameVolume);
				break;

			case "AmbientVolume":
				AmbientVolume = newSetting;
				PlayerPrefs.SetFloat(playerPrefsPrefix + "AmbientVolume", AmbientVolume);
				break;
		}
	}

	public static void SetInt(string key, int newSetting){
		switch(key){
			case "QualitySetting":
				QualitySetting = newSetting;
				PlayerPrefs.SetInt(playerPrefsPrefix + "QualitySetting", QualitySetting);
				QualitySettings.SetQualityLevel(QualitySetting,true);
				break;
		}
	}

	public static void UpdateBoundKeys(){
		BoundKeys.Clear();

		BoundKeys.Add(Interact);
		BoundKeys.Add(Mark);
		BoundKeys.Add(Stun);
		BoundKeys.Add(Cancel);
		BoundKeys.Add(Sprint);
	}
	
}
