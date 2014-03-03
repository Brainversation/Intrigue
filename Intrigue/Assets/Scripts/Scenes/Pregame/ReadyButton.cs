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
	public LoadingScreen loadingScreen;
	// Use this for initialization
	void Start () {

	this.photonView = PhotonView.Get(this);
	PhotonNetwork.networkingPeer.NewSceneLoaded();
	player = GameObject.Find("Player").GetComponent<Player>();
	label = gameObject.GetComponentInChildren<UILabel>();
	readyCheckToggle = readyCheck.GetComponentInChildren<UIToggle>();

	}
	
	// Update is called once per frame
	void Update () {
		if(PhotonNetwork.isMasterClient){
			if(readyCount == PhotonNetwork.playerList.Length-1 && player.Team!=""){
				label.text = "START GAME";
				player.Ready = true;
				readyCheckToggle.value = true;
			}
			else
			{	
				if(player.Team==""){
					label.text = "CHOOSE TEAM";
					player.Ready = false;
					readyCheckToggle.value = false;

				}
				else{
					label.text = "WAITING FOR OTHERS";
					player.Ready = true;
					readyCheckToggle.value = false;
				}
			}
		}
		else{
			if(player.Team!=""){
				label.text = "READY";
			}
			else{
				if(player.Team==""){
					label.text = "CHOOSE TEAM";
				}
			}
		}
	}

	void OnClick(){
		if(PhotonNetwork.isMasterClient && readyCount == PhotonNetwork.playerList.Length-1 && player.Team!=""){
			photonView.RPC("go", PhotonTargets.AllBuffered);
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
	public void ready(int val){
		this.readyCount +=val;
	}

	[RPC]
	public void go(){
		//PhotonNetwork.LoadLevel("Intrigue");
		loadingScreen.StartLoadingLevel("Intrigue");
	}
}
