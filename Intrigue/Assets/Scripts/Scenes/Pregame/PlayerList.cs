using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerList : MonoBehaviour {

	private PhotonView photonView = null;
	private Player player;
	public GameObject playerPrefab;
	public GameObject guardTable;
	private float totSpies = 0;
	private float totGuards = 0;
	private List<string> spies = new List<string>();
	private List<string> guards = new List<string>();

	// Use this for initialization
	void Start(){
		this.photonView = PhotonView.Get(this);
		PhotonNetwork.networkingPeer.NewSceneLoaded();
		player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update(){
		if(player.Team=="Spy" && !spies.Contains(player.Handle)){
			photonView.RPC("addSpy", PhotonTargets.AllBuffered, player.Handle);
			photonView.RPC("removeName", PhotonTargets.AllBuffered, player.Handle, "Guard");
			player.TeamID = 1;
		}
		if(player.Team=="Guard" && !guards.Contains(player.Handle)){
			photonView.RPC("addGuard", PhotonTargets.AllBuffered, player.Handle);
			photonView.RPC("removeName", PhotonTargets.AllBuffered, player.Handle, "Spy");
			player.TeamID = 2;
		}

		if(player.Team =="Spy" || player.Team =="Guard")
			photonView.RPC("editPing", PhotonTargets.AllBuffered, player.Handle, player.Team, player.Ready, PhotonNetwork.networkingPeer.RoundTripTime);
	}

	[RPC]
	void addSpy(string handle){
		spies.Add(handle);
		guards.Remove(handle);
		GameObject playerInfo = NGUITools.AddChild(gameObject, playerPrefab);
		Vector3 temp = new Vector3(0f,(spies.Count-1)*0.1f,0);
		playerInfo.transform.position-=temp;
		UILabel label = playerInfo.GetComponent<UILabel>();
		label.user = handle;
		label.text = "[0000FF]"+handle;
	}

	[RPC]
	void editPing(string handle, string team, bool ready, int ping){
		string pingColor = "[000000]";
		if (ping<50)
			pingColor = "[00FF00]";
		else if(ping<100)
			pingColor = "[FF9D00]";
		else
			pingColor = "[FF0000]";


		if(team=="Spy"){
					foreach(Transform child in transform){
						if(child.gameObject.GetComponent<UILabel>().user == handle){

							if(ready)
								child.gameObject.GetComponent<UILabel>().text = "[0000FF]" + handle + "   [00FF00][READY][000000]   ("+ pingColor+ping+"[-]" + ") ms";
							else
								child.gameObject.GetComponent<UILabel>().text = "[0000FF]" + handle + "   [FF0000][READY][000000]   ("+ pingColor+ping+"[-]" + ") ms";
						}
					}
				}
		else{
			foreach(Transform child in guardTable.transform){
				if(child.gameObject.GetComponent<UILabel>().user == handle){
						if(ready)
							child.gameObject.GetComponent<UILabel>().text = "[0000FF]" + handle + "   [00FF00][READY][000000]   ("+ pingColor+ping+"[-]" + ") ms";
						else
							child.gameObject.GetComponent<UILabel>().text = "[0000FF]" + handle + "   [FF0000][READY][000000]   ("+ pingColor+ping+"[-]" + ") ms";				
				}
			}
		}
	}

	[RPC]
	void addGuard(string handle){
		guards.Add(handle);
		spies.Remove(handle);
		GameObject playerInfo = NGUITools.AddChild(guardTable, playerPrefab);
		Vector3 temp = new Vector3(0f,(guards.Count-1)*0.1f,0);
		playerInfo.transform.position-=temp;
		UILabel label = playerInfo.GetComponent<UILabel>();
		label.user = handle;
		label.text = "[FF0000]"+handle;
	}

	[RPC]
	void removeName(string handle, string team){
		if(team=="Spy"){
			foreach(Transform child in transform){
				if(child.gameObject.GetComponent<UILabel>().user == handle){
					NGUITools.Destroy(child.gameObject);
				}
			}
		}
		else{
			foreach(Transform child in guardTable.transform){
				if(child.gameObject.GetComponent<UILabel>().user == handle){
					NGUITools.Destroy(child.gameObject);
				}
			}
		}
	}
}
