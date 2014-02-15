using UnityEngine;
using System.Collections;

public class ReadyButton : MonoBehaviour {

	private Player player;
	private PhotonView photonView = null;
	private bool isReady = false;
	private int readyCount = 0;
	private UILabel label;
	private UISprite sprite;

	// Use this for initialization
	void Start () {

	this.photonView = PhotonView.Get(this);
	PhotonNetwork.networkingPeer.NewSceneLoaded();
	player = GameObject.Find("Player").GetComponent<Player>();
	label = gameObject.GetComponentInChildren<UILabel>();
	sprite = gameObject.GetComponent<UISprite>();

	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("Total Players: " + (PhotonNetwork.playerList.Length-1) + " Ready: " + readyCount);
		if(PhotonNetwork.isMasterClient){
			if(readyCount == PhotonNetwork.playerList.Length-1 && player.Team!=""){
				label.text = "START GAME";
				sprite.color = Color.green;
			}
			else
			{	
				if(player.Team==""){
					label.text = "MUST CHOOSE TEAM";
					sprite.color = Color.red;
				}
				else{
					label.text = "WAITING FOR READY";
					sprite.color = Color.red;
				}
			}
		}
		else{
			if(isReady && player.Team!=""){
				label.text = "READY";
				sprite.color = Color.green;
			}
			else{
				if(player.Team==""){
					label.text = "MUST CHOOSE TEAM";
					sprite.color = Color.red;
				}
				else{
					label.text = "NOT READY";
					sprite.color = Color.red;
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
				photonView.RPC("ready", PhotonTargets.MasterClient, -1);
			}
			else if(!isReady){
				if(player.Team!=""){
					isReady = true;
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
		PhotonNetwork.LoadLevel("Intrigue");
	}
}
