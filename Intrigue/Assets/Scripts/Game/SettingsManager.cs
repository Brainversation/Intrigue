using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SettingsManager : MonoBehaviour {

	public UILabel Binding_Interact;
	public UILabel Binding_Stun;
	public UILabel Binding_Mark;
	public UILabel Binding_MouseX;
	public UILabel Binding_MouseY;
	public UILabel Rebind_Current;
	public UILabel Rebind_New;
	public GameObject RebindPanel;

	private string curKey;


	// Use this for initialization
	void Start () {
		Settings.Start();
		updateKeyBindings();
	}
	
	void Update(){
		if(RebindPanel.GetComponent<UIPanel>().alpha == 1){
			if(Input.inputString.Length>=1){
				char newKey = Input.inputString[0];
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
				}
			}
		}
	}

	void changeInteract(){
		Debug.Log("Clicked");
		RebindPanel.GetComponent<UIPanel>().alpha = 1;
		Rebind_Current.text = "[FFCC00]" + Settings.Interact;
		curKey = "Interact";
	}

	void hidePanel(){
		RebindPanel.GetComponent<UIPanel>().alpha = 0;
		updateKeyBindings();
	}

	void updateKeyBindings(){
		Binding_Interact.text = "Interact: [FFCC00]" + Settings.Interact.ToUpper();
		Binding_Mark.text = "Mark: [FFCC00]" + Settings.Mark.ToUpper();
		Binding_Stun.text = "Stun: [FFCC00]" + Settings.Stun.ToUpper();
		Binding_MouseX.text = "Mouse X Sensitivity: [FFCC00]" + Settings.MouseSensitivityX;
		Binding_MouseY.text = "Mouse Y Sensitivity: [FFCC00]" + Settings.MouseSensitivityY;
	}

	void ReturnToMenu(){
		Debug.Log("Called");
		Application.LoadLevel("MainMenu");
	}
}
