﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SettingsManager : MonoBehaviour {

	//Panels
	public UIPanel Settings_Controls;
	public UIPanel Settings_Audio;
	public UIPanel Settings_Video;



	//Bindings
	public UILabel Binding_Interact;
	public UILabel Binding_Sprint;
	public UILabel Binding_Stun;
	public UILabel Binding_Mark;
	public UILabel Binding_Cancel;
	public UILabel Binding_MouseX;
	public UILabel Binding_MouseY;
	public UILabel Binding_NewX;
	public UILabel Binding_NewY;
	public UILabel Rebind_Handle;
	public UILabel Rebind_Current;
	public UILabel Rebind_New;
	public UILabel Handle_Current;
	public UISlider Slider_X;
	public UISlider Slider_Y;
	public GameObject RebindPanel;

	private string curKey;
	private Player player;
	private string playerPrefsPrefix;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Player>();
		Settings.Start();
		updateKeyBindings();

		//Set Panels
		Settings_Controls.alpha = 1;
		Settings_Video.alpha = 0;
		Settings_Audio.alpha = 0;


		Slider_X.value = (Settings.MouseSensitivityX-1)/14;
		Slider_Y.value = (Settings.MouseSensitivityY-1)/14;

		if (Application.isEditor)
			playerPrefsPrefix = "PlayerEditor";
		else
			playerPrefsPrefix = "Player";
	}
	
	void Update(){
		bool shift = false;
		string newKey;
		if(RebindPanel.GetComponent<UIPanel>().alpha == 1){
			if(Input.GetKeyUp("left shift")){
				shift = true;
			}
			if(Input.inputString.Length>=1 || shift){
				if(shift)
					newKey = "left shift";
				else{
					newKey = "" + Input.inputString[0];
				}
				if(newKey == " ")
					newKey = "space";
				Rebind_New.text = "New:\n[FFCC00]" + newKey;
				switch(curKey){
					case "Interact": 
					Settings.SetKey("Interact", "" + newKey);
					Invoke("hidePanel", 0.5f);
					break;

				case "Mark": 
					Settings.SetKey("Mark", "" + newKey);
					Invoke("hidePanel", 0.5f);
					break;

				case "Stun": 
					Settings.SetKey("Stun", "" + newKey);
					Invoke("hidePanel", 0.5f);
					break;

				case "Cancel": 
					Settings.SetKey("Cancel", "" + newKey);
					Invoke("hidePanel", 0.5f);
					break;
				case "Sprint":
					Settings.SetKey("Sprint", "" + newKey);
					Invoke("hidePanel", 0.5f);
					break;
				}
			}
		}
	}

	void changeSprint(){
		RebindPanel.GetComponent<UIPanel>().alpha = 1;
		Rebind_Current.text = "[FFCC00]" + Settings.Sprint;
		curKey = "Sprint";
	}

	void changeInteract(){
		RebindPanel.GetComponent<UIPanel>().alpha = 1;
		Rebind_Current.text = "[FFCC00]" + Settings.Interact;
		curKey = "Interact";
	}

	void changeCancel(){
		RebindPanel.GetComponent<UIPanel>().alpha = 1;
		Rebind_Current.text = "[FFCC00]" + Settings.Cancel;
		curKey = "Cancel";
	}

	void changeStun(){
		RebindPanel.GetComponent<UIPanel>().alpha = 1;
		Rebind_Current.text = "[FFCC00]" + Settings.Stun;
		curKey = "Stun";
	}

	void changeMark(){
		RebindPanel.GetComponent<UIPanel>().alpha = 1;
		Rebind_Current.text = "[FFCC00]" + Settings.Mark;
		curKey = "Mark";
	}

	void changeMouseX(){
		Settings.SetFloat("MouseX", Mathf.RoundToInt((Slider_X.value*14)) + 1);
		updateKeyBindings();
	}

	void changeMouseY(){
		Settings.SetFloat("MouseY", Mathf.RoundToInt((Slider_Y.value*14)) + 1);
		updateKeyBindings();
	}

	void hidePanel(){
		RebindPanel.GetComponent<UIPanel>().alpha = 0;
		updateKeyBindings();
	}

	public void onMouseXChange(){
		Binding_NewX.text = "[FFCC00]" + Mathf.RoundToInt((Slider_X.value * 14) + 1);
	}

	public void onMouseYChange(){
		Binding_NewY.text = "[FFCC00]" + Mathf.RoundToInt((Slider_Y.value *14) + 1);
	}

	public void changeHandle(){
		player.Handle = Rebind_Handle.text;
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Handle", player.Handle}});	
		PlayerPrefs.SetString(playerPrefsPrefix + "Name", player.Handle);
		updateKeyBindings();
	}

	//Activate Different Panels

	public void activateControlsPanel(){
		Settings_Controls.alpha = 1;
		Settings_Audio.alpha = 0;
		Settings_Video.alpha = 0;
	}

	public void activateAudioPanel(){
		Settings_Controls.alpha = 0;
		Settings_Audio.alpha = 1;
		Settings_Video.alpha = 0;
	}

	public void activateVideoPanel(){
		Settings_Controls.alpha = 0;
		Settings_Audio.alpha = 0;
		Settings_Video.alpha = 1;
	}





	void updateKeyBindings(){
		Binding_Sprint.text = "Sprint: [FFCC00]" + Settings.Sprint.ToUpper();
		Binding_Interact.text = "Interact: [FFCC00]" + Settings.Interact.ToUpper();
		Binding_Mark.text = "Mark: [FFCC00]" + Settings.Mark.ToUpper();
		Binding_Stun.text = "Stun: [FFCC00]" + Settings.Stun.ToUpper();
		Binding_Cancel.text = "Cancel: [FFCC00]" + Settings.Cancel.ToUpper();
		Binding_MouseX.text = "Mouse X: [FFCC00]" + Settings.MouseSensitivityX;
		Binding_MouseY.text = "Mouse Y: [FFCC00]" + Settings.MouseSensitivityY;
		Handle_Current.text = "Handle: [FFCC00]" + PlayerPrefs.GetString(playerPrefsPrefix + "Name", player.Handle);
		PlayerPrefs.Save();
	}

	void ReturnToMenu(){
		Debug.Log("Called");
		Application.LoadLevel("MainMenu");
	}
}
