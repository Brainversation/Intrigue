using UnityEngine;
using System.Collections;

public class Pregame : MonoBehaviour {

	private string chatBox = "";
	private int readyCount = 0;

	// Use this for initialization
	void Start () {
		Screen.lockCursor = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPhotonPlayerDisconnected(PhotonPlayer player){
		Debug.Log("OnPhotonPlayerDisconnected: " + player.ID );

		if (PhotonNetwork.isMasterClient){
			//Move Info towards to new master Client, but master client switches on its own
		}
	}

	[RPC]
	public void recieveMessage(string s){
		this.chatBox += s;
	}

	[RPC]
	public void ready( int val ){
		this.readyCount += val;
	}
}
