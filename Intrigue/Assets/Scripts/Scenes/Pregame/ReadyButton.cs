using UnityEngine;
using System.Collections;

public class ReadyButton : MonoBehaviour {

	private Player player;
	private PhotonView photonView = null;
	private bool isReady = false;
	private int readyCount = 0;
	private UILabel label;
	private UIToggle readyCheckToggle;
	public GameObject readyCheck;

	void Start () {
		this.photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();
		label = gameObject.GetComponentInChildren<UILabel>();
		readyCheckToggle = readyCheck.GetComponentInChildren<UIToggle>();
	}
	
	void Update () {
		if(PhotonNetwork.isMasterClient){
			if(readyCount == PhotonNetwork.playerList.Length-1 && player.Team!=""){
				label.text = "START GAME";
				label.fontSize = 28;
				player.Ready = true;
				readyCheckToggle.value = true;
			}
			else
			{	
				if(player.Team==""){
					label.text = "CHOOSE TEAM";
					label.fontSize = 28;
					player.Ready = false;
					readyCheckToggle.value = false;

				}
				else{
					label.text = "WAITING FOR OTHERS";
					label.fontSize = 24;
					player.Ready = true;
					readyCheckToggle.value = false;
				}
			}
		}
		else{
			if(player.Team!=""){
				label.text = "READY";
				label.fontSize = 40;
			}
			else{
				if(player.Team==""){
					label.text = "CHOOSE TEAM";
					label.fontSize = 28;
				}
			}
		}
	}

	void OnClick(){
		if(PhotonNetwork.isMasterClient){
			if(readyCount == PhotonNetwork.playerList.Length-1 && player.Team!=""){
				PhotonNetwork.room.open = false;
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
	
	[RPC]
	public void go(){
		PhotonNetwork.LoadLevel("Intrigue");
	}

	[RPC]
	public void ready(int val){
		this.readyCount +=val;
	}
}
