using UnityEngine;
using System.Collections;

public class GuestSlider : MonoBehaviour {

	private Player player;
	private PhotonView photonView = null;
	private UISlider slider;
	private UILabel label;

	// Use this for initialization
	void Start () {
			this.photonView = PhotonView.Get(this);
			player = GameObject.Find("Player").GetComponent<Player>();
			slider = gameObject.GetComponent<UISlider>();
			label = gameObject.GetComponentInChildren<UILabel>();
				
			if(!PhotonNetwork.isMasterClient){
				slider.enabled = false;
			}

	}
	
	// Update is called once per frame
	void Update () {
		if(PhotonNetwork.isMasterClient){
			player.Guests = Mathf.RoundToInt(slider.value*80);
			label.text = "Guest Count: " + player.Guests;
			photonView.RPC("guestCount", PhotonTargets.Others, player.Guests);
		}
		else{
			label.text = ("Guest Count: " + player.Guests.ToString());
		}
	}

	[RPC]
	void guestCount(int guests){
		player.Guests = guests;
	}
}
