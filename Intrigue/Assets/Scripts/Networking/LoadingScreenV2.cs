using UnityEngine;
using System.Collections;

public class LoadingScreenV2 : MonoBehaviour {

	public string levelToLoad;
	public GameObject UIRoot;
	public GameObject bg;
	public GameObject loadingBar;
	public GameObject loadTimer;
	public GameObject loadTitle;
	public GameObject loadResult;
	private int loadCounter = 0;
	private AsyncOperation async;
	private PhotonView photonView = null;
	private Player player;
	private float countdownDuration = 10;
	private float countdownCur = 0;
	private Intrigue intrigue;
	private string roundResultDisplay;


	void Start(){
		this.photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();
		intrigue = GameObject.FindWithTag("Scripts").GetComponent<Intrigue>();

		if(intrigue.GetRoundsLeft == intrigue.GetRounds){
			loadTitle.GetComponent<UILabel>().text = "MATCH STARTING";
			loadResult.GetComponent<UILabel>().text = "";
		}
		else{
			loadTitle.GetComponent<UILabel>().text = "ROUND STARTING\n[FF0000]SWITCHING SIDES[-]";
			loadResult.GetComponent<UILabel>().text = player.PrevResult;
		}
	}

	void Update(){
		if(intrigue.gameStart){
			transform.parent.gameObject.active = false;
		}

		loadingBar.GetComponent<UISlider>().value = intrigue.loadedGuests/intrigue.totalGuests;
	}

}