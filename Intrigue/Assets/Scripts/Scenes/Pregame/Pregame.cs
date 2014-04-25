using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class Pregame : MonoBehaviour {

	public UITextList textList;
	public UIInput mInput;
	public GameObject readyCheck;
	public UILabel readyLabel;
	public UIToggle readyCheckToggle;
	public UISlider slider;
	public UILabel sliderLabel;
	public GameObject playerPrefab;
	public GameObject guardTable;
	public GameObject spyTable;

	private PhotonView photonView = null;
	private Player player;
	private bool isReady = false;
	private int readyCount = 0;
	private List<int> spies = new List<int>();
	private List<int> guards = new List<int>();

	void Start(){
		this.photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();

		//Sets Chat Max Line Count
		mInput.label.maxLineCount = 1;

		//Disables Controlling the Guest Slider for Clients
		if(!PhotonNetwork.isMasterClient){
			slider.enabled = false;
		}

		//Updates Ping and Score Every X Seconds
		InvokeRepeating("syncPingAndScore", 0, 2F);

		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Handle", player.Handle}});
	}

	void Update(){
		//Checks the Ready Status of All Players
		readyStatus();

		//Updates the Slider for Guest Count
		updateGuestSlider();

		//Sync Player Team
		syncPlayerTeam();
	}

	public void OnSubmit(){
		if (textList != null)
		{
			// It's a good idea to strip out all symbols as we don't want user input to alter colors, add new lines, etc
			string text = NGUIText.StripSymbols(mInput.value);

			if (!string.IsNullOrEmpty(text)){
				if(player.Team == "Spy"){
					textList.Add("[8169FF]"+player.Handle+": [-]"+text);
					photonView.RPC("recieveMessage", PhotonTargets.Others, "[8169FF]"+player.Handle+": [-]"+text);
				} else if(player.Team == "Guard") {
					textList.Add("[FF2B2B]"+player.Handle+": [-]"+text);
					photonView.RPC("recieveMessage", PhotonTargets.Others, "[FF2B2B]"+player.Handle+": [-]"+text);
				} else {
					textList.Add(player.Handle+": [-]"+text);
					photonView.RPC("recieveMessage", PhotonTargets.Others, player.Handle+": [-]"+text);
				}
				mInput.value = "";
			}
		}
	}

	void syncPlayerTeam(){
		if(player.Team=="Spy" && !spies.Contains(PhotonNetwork.player.ID)){
			photonView.RPC("addSpy", PhotonTargets.AllBuffered, player.Handle, PhotonNetwork.player.ID);
			photonView.RPC("removeName", PhotonTargets.AllBuffered, player.Handle, "Guard", PhotonNetwork.player.ID);
			player.TeamID = 1;
		}
		if(player.Team=="Guard" && !guards.Contains(PhotonNetwork.player.ID)){
			photonView.RPC("addGuard", PhotonTargets.AllBuffered, player.Handle, PhotonNetwork.player.ID);
			photonView.RPC("removeName", PhotonTargets.AllBuffered, player.Handle, "Spy", PhotonNetwork.player.ID);
			player.TeamID = 2;
		}
	}

	void syncPingAndScore(){
		photonView.RPC("editPing", PhotonTargets.All, player.Handle, player.Team, PhotonNetwork.player.ID, player.Ready, PhotonNetwork.GetPing());
	}


	void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer){
		/*Debug.Log("OPPD from Pregame: " +
					(string)photonPlayer.customProperties["Handle"] + " " +
					(string)photonPlayer.customProperties["Team"] + " " +
					photonPlayer.ID);*/
		photonView.RPC("removeName", PhotonTargets.All, photonPlayer.customProperties["Handle"], photonPlayer.customProperties["Team"], photonPlayer.ID );
	}

	void updateGuestSlider(){
		if(PhotonNetwork.isMasterClient){
			player.Guests = Mathf.RoundToInt(slider.value*80);
			sliderLabel.text = "Guest Count: " + player.Guests;
			photonView.RPC("guestCount", PhotonTargets.Others, player.Guests);
		}
		else{
			sliderLabel.text = ("Guest Count: " + player.Guests.ToString());
		}
	}

	void swapToSpy(){
		player.Team = "Spy";
		player.TeamID = 1;
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Team", "Spy"}});
	}

	void swapToGuard(){
		player.Team = "Guard";
		player.TeamID = 2;
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Team", "Guard"}});
	}

	void readyStatus(){
		//Ready Button
		if(PhotonNetwork.isMasterClient){
			if(readyCount == PhotonNetwork.playerList.Length-1 && player.Team!=""){
				readyLabel.text = "START GAME";
				readyLabel.fontSize = 28;
				player.Ready = true;
				readyCheckToggle.value = true;
			}
			else
			{	
				if(player.Team==""){
					readyLabel.text = "CHOOSE TEAM";
					readyLabel.fontSize = 28;
					player.Ready = false;
					readyCheckToggle.value = false;

				}
				else{
					readyLabel.text = "WAITING FOR OTHERS";
					readyLabel.fontSize = 24;
					player.Ready = true;
					readyCheckToggle.value = false;
				}
			}
		}
		else{
			if(player.Team!=""){
				readyLabel.text = "READY";
				readyLabel.fontSize = 40;
			}
			else{
				if(player.Team==""){
					readyLabel.text = "CHOOSE TEAM";
					readyLabel.fontSize = 28;
				}
			}
		}
	}

	void readyClick(){
		if(PhotonNetwork.isMasterClient){
			if(readyCount == PhotonNetwork.playerList.Length-1 && player.Team!=""){
				PhotonNetwork.room.visible = false;
				photonView.RPC("go", PhotonTargets.All);
			}
		}
		else{
			if(isReady){
				isReady = false;
				readyCheckToggle.value = false;
				player.Ready = false;
				photonView.RPC("ready", PhotonTargets.MasterClient, -1);
			}
			else if(!isReady){
				if(player.Team!=""){
					isReady = true;
					readyCheckToggle.value = true;
					player.Ready = true;
					photonView.RPC("ready", PhotonTargets.MasterClient, 1);
				}
			}

		}
	}
	
	void leaveLobby(){
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LoadLevel( "MainMenu" );
	}

	[RPC]
	void addSpy(string handle, int playerID){
		//Debug.Log("Added spy: " + handle + " ID: " + playerID);
		spies.Add(playerID);
		guards.Remove(playerID);
		GameObject playerInfo = NGUITools.AddChild(spyTable, playerPrefab);
		Vector3 temp = new Vector3(0f,(spies.Count-1)*0.1f,0);
		playerInfo.transform.position-=temp;
		UILabel label = playerInfo.GetComponent<UILabel>();
		label.user = playerID;
		label.text = "[FFFFFF]"+handle;
		syncPingAndScore();
	}

	[RPC]
	void addGuard(string handle, int playerID){
		//Debug.Log("Added guard: " + handle + " ID: " + playerID);
		guards.Add(playerID);
		spies.Remove(playerID);
		GameObject playerInfo = NGUITools.AddChild(guardTable, playerPrefab);
		Vector3 temp = new Vector3(0f,(guards.Count-1)*0.1f,0);
		playerInfo.transform.position-=temp;
		UILabel label = playerInfo.GetComponent<UILabel>();
		label.user = playerID;
		label.text = "[FFFFFF]"+handle;
		syncPingAndScore();
	}

	[RPC]
	void editPing(string handle, string team, int playerID, bool ready, int ping){
		string pingColor = "[000000]";
		if (ping<50)
			pingColor = "[00FF00]";
		else if(ping<100)
			pingColor = "[FF9D00]";
		else
			pingColor = "[FF0000]";
		if(team=="Spy"){
					foreach(Transform child in spyTable.transform){
						if(child.gameObject.GetComponent<UILabel>().user == playerID){
							if(ready)
								child.gameObject.GetComponent<UILabel>().text = "[FFFFFF]" + handle + "   [00FF00][READY][FFFFFF]   ("+ pingColor+ping+"[-]" + ") ms";
							else
								child.gameObject.GetComponent<UILabel>().text = "[FFFFFF]" + handle + "   [FF0000][READY][FFFFFF]   ("+ pingColor+ping+"[-]" + ") ms";
						}
					}
				}
		else{
			foreach(Transform child in guardTable.transform){
				if(child.gameObject.GetComponent<UILabel>().user == playerID){
						if(ready)
							child.gameObject.GetComponent<UILabel>().text = "[FFFFFF]" + handle + "   [00FF00][READY][FFFFFF]   ("+ pingColor+ping+"[-]" + ") ms";
						else
							child.gameObject.GetComponent<UILabel>().text = "[FFFFFF]" + handle + "   [FF0000][READY][FFFFFF]   ("+ pingColor+ping+"[-]" + ") ms";				
				}
			}
		}
	}

	[RPC]
	void removeName(string handle, string team, int playerID){
		bool removed = false;
		float removedHeight=0;
		GameObject curTable;
		if(team=="Spy")
			curTable = spyTable;
		else
			curTable = guardTable;

		foreach(Transform child in curTable.transform){
			if(child.gameObject.GetComponent<UILabel>().user == playerID){
				removedHeight = child.localPosition.y;
				NGUITools.Destroy(child.gameObject);
				removed = true;
			}
		}

		if(removed){
			foreach(Transform child in curTable.transform){
				Vector3 temp = new Vector3(0f, 0.1f,0);
				if(Mathf.RoundToInt(child.gameObject.transform.localPosition.y)<Mathf.RoundToInt(removedHeight)){
					child.gameObject.transform.position+=temp;
				}
			}
		}
		syncPingAndScore();
	}

	[RPC]
	public void go(){
		PhotonNetwork.LoadLevel("Intrigue");
	}

	[RPC]
	public void ready(int val){
		this.readyCount +=val;
	}

	[RPC]
	public void recieveMessage(string s){
		textList.Add(s);
	}

	[RPC]
	void guestCount(int guests){
		player.Guests = guests;
	}
}
