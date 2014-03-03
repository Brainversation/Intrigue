using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerList : MonoBehaviour {

	private PhotonView photonView = null;
	private Player player;
	public GameObject playerPrefab;
	public GameObject guardTable;
	private List<string> spies = new List<string>();
	private List<string> guards = new List<string>();

	// Use this for initialization
	void Start(){
		this.photonView = PhotonView.Get(this);
		PhotonNetwork.networkingPeer.NewSceneLoaded();
		player = GameObject.Find("Player").GetComponent<Player>();
		InvokeRepeating("syncPingAndScore", 0, 2F);
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
		label.text = "[FFFFFF]"+handle;
		syncPingAndScore();
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
								child.gameObject.GetComponent<UILabel>().text = "[FFFFFF]" + handle + "   [00FF00][READY][FFFFFF]   ("+ pingColor+ping+"[-]" + ") ms";
							else
								child.gameObject.GetComponent<UILabel>().text = "[FFFFFF]" + handle + "   [FF0000][READY][FFFFFF]   ("+ pingColor+ping+"[-]" + ") ms";
						}
					}
				}
		else{
			foreach(Transform child in guardTable.transform){
				if(child.gameObject.GetComponent<UILabel>().user == handle){
						if(ready)
							child.gameObject.GetComponent<UILabel>().text = "[FFFFFF]" + handle + "   [00FF00][READY][FFFFFF]   ("+ pingColor+ping+"[-]" + ") ms";
						else
							child.gameObject.GetComponent<UILabel>().text = "[FFFFFF]" + handle + "   [FF0000][READY][FFFFFF]   ("+ pingColor+ping+"[-]" + ") ms";				
				}
			}
		}
	}

	void syncPingAndScore(){
		photonView.RPC("editPing", PhotonTargets.All, player.Handle, player.Team, player.Ready, PhotonNetwork.GetPing());
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
		label.text = "[FFFFFF]"+handle;
		syncPingAndScore();
	}

	[RPC]
	void removeName(string handle, string team){
	bool removed = false;
		if(team=="Spy"){
			foreach(Transform child in transform){
				if(child.gameObject.GetComponent<UILabel>().user == handle){
					NGUITools.Destroy(child.gameObject);
					removed = true;
				}
			}
			if(removed){
				foreach(Transform child in transform){
					Vector3 temp = new Vector3(0f, 0.1f,0);
					if(Mathf.RoundToInt(child.gameObject.transform.localPosition.y)!=0)
						child.gameObject.transform.position+=temp;
				}
			}
		}
		else{
			foreach(Transform child in guardTable.transform){
				if(child.gameObject.GetComponent<UILabel>().user == handle){
					NGUITools.Destroy(child.gameObject);
					removed = true;
				}
			}
			if(removed){
				foreach(Transform child in guardTable.transform){
					Vector3 temp = new Vector3(0f, 0.1f,0);
					if(Mathf.RoundToInt(child.gameObject.transform.localPosition.y)!=0)
						child.gameObject.transform.position+=temp;
				}
			}
		}
		syncPingAndScore();
	}
}
