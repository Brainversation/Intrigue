using UnityEngine;
using System.Collections;

public class JoinServer : MonoBehaviour {

	private MainMenu mainMenu;
	public UILabel roomName;

	void Start(){
		mainMenu = GameObject.Find("UI Root").GetComponent<MainMenu>();
	}

	void OnClick(){
		if(!mainMenu.isBanned()){
			PhotonNetwork.JoinRoom(roomName.text);
		}
		else{
			mainMenu.SendMessage("showBanInfo");
		}
	}
}
