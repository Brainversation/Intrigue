using UnityEngine;
using System.Collections;

public class JoinServer : MonoBehaviour {

	private UILabel[] roomNames;
	private string roomName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick(){
		roomNames = gameObject.GetComponentsInChildren<UILabel>();
		roomName = roomNames[0].text;
		PhotonNetwork.JoinRoom(roomName);
		//Debug.Log("Clicked room: "+roomName);
	}
}
