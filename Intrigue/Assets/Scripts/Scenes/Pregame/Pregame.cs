using UnityEngine;
using System.Collections;

public class Pregame : MonoBehaviour {

	private string chatBox = "";
	private int readyCount = 0;

	// Use this for initialization
	void Start () {
		Screen.lockCursor = false;
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
