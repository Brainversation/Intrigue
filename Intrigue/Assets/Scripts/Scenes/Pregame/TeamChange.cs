using UnityEngine;
using System.Collections;

public class TeamChange : MonoBehaviour {

	private Player player;
	private PhotonView photonView = null;
	// Use this for initialization
	void Start () {
	this.photonView = PhotonView.Get(this);
	PhotonNetwork.networkingPeer.NewSceneLoaded();
	player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	void OnClick(){
		if(gameObject.tag=="Spy"){
			player.Team = "Spy";
			player.TeamID = 1;
		}
		else{
			player.Team = "Guard";
			player.TeamID = 2;
		}
	}
}
