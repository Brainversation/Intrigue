using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SettingsManager : MonoBehaviour {

	//Panels
	public UIPanel Settings_Controls;
	public UIPanel Settings_Audio;
	public UIPanel Settings_Video;

	//Audio Controller
	public AudioControl audioControl;


	//Controls
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

	//Video
	public UIPopupList ResolutionList;
	public UILabel CurrentResolution;
	public UIToggle FullScreenToggle;
	public UIPopupList QualityList;
	public UILabel CurrentQuality;

	//Audio
	public UILabel MasterVolumeLabel;
	public UISlider MasterVolumeSlider;
	public UILabel AmbientVolumeLabel;
	public UISlider AmbientVolumeSlider;
	public UILabel GameVolumeLabel;
	public UISlider GameVolumeSlider;

	//Other
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

		//Set Initial Values to NGUI Objects

		//Controls
		Slider_X.value = (Settings.MouseSensitivityX-1)/14;
		Slider_Y.value = (Settings.MouseSensitivityY-1)/14;

		//Video
		FullScreenToggle.startsActive = FullScreenToggle.value = Screen.fullScreen;

		//Audio
		MasterVolumeSlider.value = Mathf.RoundToInt(Settings.MasterVolume * 100);
		AmbientVolumeSlider.value = Mathf.RoundToInt(Settings.AmbientVolume * 100);
		GameVolumeSlider.value = Mathf.RoundToInt(Settings.GameVolume * 100);

		//Resolutions
		ResolutionList.items.Clear();
		CurrentResolution.text = Screen.currentResolution.width + "x" + Screen.currentResolution.height;
		foreach(Resolution res in Screen.resolutions){
			if(res.width/res.height == (4/3)){
				ResolutionList.items.Add(res.width + "x" + res.height);
			}
		}

		//Quality Settings
		QualityList.items.Clear();
		CurrentQuality.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
		foreach(string name in QualitySettings.names){
			QualityList.items.Add(name);
		}

		if (Application.isEditor)
			playerPrefsPrefix = "PlayerEditor";
		else
			playerPrefsPrefix = "Player";
	}
	
	void Update(){
		bool shift = false;
		string newKey;
		if(RebindPanel.GetComponent<UIPanel>().alpha == 1){
			
			//Check Shift
			if(Input.GetKeyUp("left shift")){
				shift = true;
			}

			//Check Character Press
			if(Input.inputString.Length>=1 || shift){
				if(shift)
					newKey = "left shift";
				else{
					newKey = "" + Input.inputString[0];
				}

				//Check Space Pressed
				if(newKey == " ")
					newKey = "space";

				Settings.UpdateBoundKeys();
				if(!Settings.BoundKeys.Contains(newKey)){
					Rebind_New.text = "[FFCC00]" + newKey.ToUpper();
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
					}else{
						Rebind_New.text = "[FF2B2B]ALREADY IN USE";
					}
			}
		}
	}

	void changeSprint(){
		RebindPanel.GetComponent<UIPanel>().alpha = 1;
		Rebind_Current.text = "[FFCC00]" + Settings.Sprint.ToUpper();
		Rebind_New.text = "";
		curKey = "Sprint";
	}

	void changeInteract(){
		RebindPanel.GetComponent<UIPanel>().alpha = 1;
		Rebind_Current.text = "[FFCC00]" + Settings.Interact.ToUpper();
		Rebind_New.text = "";
		curKey = "Interact";
	}

	void changeCancel(){
		RebindPanel.GetComponent<UIPanel>().alpha = 1;
		Rebind_Current.text = "[FFCC00]" + Settings.Cancel.ToUpper();
		Rebind_New.text = "";
		curKey = "Cancel";
	}

	void changeStun(){
		RebindPanel.GetComponent<UIPanel>().alpha = 1;
		Rebind_Current.text = "[FFCC00]" + Settings.Stun.ToUpper();
		Rebind_New.text = "";
		curKey = "Stun";
	}

	void changeMark(){
		RebindPanel.GetComponent<UIPanel>().alpha = 1;
		Rebind_Current.text = "[FFCC00]" + Settings.Mark.ToUpper();
		Rebind_New.text = "";
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

	//Functions Called Through NGUI

	//Mouse Sensitivity Slider Changes
	public void onMouseXChange(){
		Binding_NewX.text = "[FFCC00]" + Mathf.RoundToInt((Slider_X.value * 14) + 1);
	}

	public void onMouseYChange(){
		Binding_NewY.text = "[FFCC00]" + Mathf.RoundToInt((Slider_Y.value *14) + 1);
	}

	//Volume Slider Changes
	public void onMasterVolumeChange(){
		Settings.SetFloat("MasterVolume", MasterVolumeSlider.value);
		updateVolumeDisplays();
	}

	//Volume Slider Changes
	public void onMusicVolumeChange(){
		Settings.SetFloat("AmbientVolume", AmbientVolumeSlider.value);
		updateVolumeDisplays();
	}

	//Volume Slider Changes
	public void onGameVolumeChange(){
		Settings.SetFloat("GameVolume", GameVolumeSlider.value);
		updateVolumeDisplays();
	}

	public void changeHandle(){
		player.Handle = Rebind_Handle.text;
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Handle", player.Handle}});	
		PlayerPrefs.SetString(playerPrefsPrefix + "Name", player.Handle);
		updateKeyBindings();
	}

	public void updateFullScreen(){
		FullScreenToggle.value = !FullScreenToggle.value;
		Screen.fullScreen = FullScreenToggle.value;
	}

	public void updateResolution(){
		string[] resolution = ResolutionList.value.Split('x');
		int width = int.Parse(resolution[0]);
		int height = int.Parse(resolution[1]);
		Screen.SetResolution(width,height,Screen.fullScreen);
	}

	public void updateQualitySettings(){
		int i = 0;
		string[] names = QualitySettings.names;
		foreach(string name in names){
			if(name == QualityList.value){
				QualitySettings.SetQualityLevel(i,true);
				break;
			}else{
				++i;
			}
		}
		PlayerPrefs.Save();
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

	void updateVolumeDisplays(){
		audioControl.SetAudio();
		MasterVolumeLabel.text = "[FFCC00]" + Mathf.RoundToInt(Settings.MasterVolume * 100);
		AmbientVolumeLabel.text = "[FFCC00]" + Mathf.RoundToInt(Settings.AmbientVolume * 100);
		GameVolumeLabel.text = "[FFCC00]" + Mathf.RoundToInt(Settings.GameVolume * 100);
		PlayerPrefs.Save();
	}

	void ReturnToMenu(){
		Debug.Log("Called");
		Application.LoadLevel("MainMenu");
	}
}
