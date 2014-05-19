using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Settings : MonoBehaviour {

	[HideInInspector] public static float MouseSensitivityX;
	[HideInInspector] public static float MouseSensitivityY;
	[HideInInspector] public static string Interact;
	[HideInInspector] public static string Mark;
	[HideInInspector] public static string Stun;

	[HideInInspector] public static float MasterVolume;
	[HideInInspector] public static float AmbientVolume;
	[HideInInspector] public static float GameVolume;

	private static string playerPrefsPrefix;


	// Use this for initialization
	public static void Start () {
		//Player Prefs
		if (Application.isEditor)
			playerPrefsPrefix = "PlayerEditor";
		else
			playerPrefsPrefix = "Player";

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
		}
	}
	
}
