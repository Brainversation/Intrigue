using UnityEngine;
using System.Collections;

public class PlayerList : MonoBehaviour {

	private PhotonView photonView = null;
	private Player player;
	public GameObject playerPrefab;

	// Use this for initialization
	void Start () {
		this.photonView = PhotonView.Get(this);
		PhotonNetwork.isMessageQueueRunning = true;
		player = GameObject.Find("Player").GetComponent<Player>();
		GameObject playerInfo = NGUITools.AddChild(gameObject, playerPrefab);
		UILabel label = playerInfo.GetComponent<UILabel>();
		label.text = player.Handle;
		Debug.Log("pos"+gameObject.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("pos"+gameObject.transform.position);

	}
}
