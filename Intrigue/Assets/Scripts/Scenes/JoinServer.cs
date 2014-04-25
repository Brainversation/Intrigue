using UnityEngine;
using System.Collections;

public class JoinServer : MonoBehaviour {

	private UILabel[] roomNames;
	private string roomName;
	private Player player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Player>();
	}

	void OnClick(){
		roomNames = gameObject.GetComponentsInChildren<UILabel>();
		roomName = (string) roomNames[0].text;
		player.RoomName = roomName;
		PhotonNetwork.JoinRoom(roomName);
	}
}
