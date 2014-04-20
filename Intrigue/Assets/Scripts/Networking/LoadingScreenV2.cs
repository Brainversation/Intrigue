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
	public GameObject loadNextRound;
	private AsyncOperation async;
	private Player player;
	private float countdownDuration = 5.0f;
	private float countdownCur = 0;
	private Intrigue intrigue;
	private string roundResultDisplay;
	private bool countdownStarted = false;
	private float totalCountdownWithLoading = 0;
	private PhotonView photonView = null;


	void Start(){
		photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();
		intrigue = GameObject.FindWithTag("Scripts").GetComponent<Intrigue>();
		loadTimer.GetComponent<UILabel>().text = totalCountdownWithLoading+"s";

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
			transform.parent.gameObject.SetActive(false);
		}

		if(PhotonNetwork.isMasterClient){
			loadingBar.GetComponent<UISlider>().value = intrigue.loadedGuests/intrigue.totalGuests;
			photonView.RPC("syncLoadingBar", PhotonTargets.Others, intrigue.loadedGuests/intrigue.totalGuests);
		}		

		if(!intrigue.doneLoading){
			loadNextRound.GetComponent<UILabel>().text = "Loading Level";
			loadTimer.GetComponent<UILabel>().text = "";
		}


		if(intrigue.doneLoading && !countdownStarted){
			countdownStarted = true;
			loadNextRound.GetComponent<UILabel>().text = "Round starts in: ";
			StartCoroutine(Waiter());
			Invoke("beepLow",1);
			Invoke("beepLow",2);
			Invoke("beepLow",3);
			Invoke("beepHigh",4);
		}
	}

   	IEnumerator Waiter() {
		while(countdownCur<countdownDuration){
			countdownCur+= Time.deltaTime;
			int timerSec = Mathf.RoundToInt(countdownDuration-countdownCur);
			loadTimer.GetComponent<UILabel>().text = timerSec+"s";
			yield return null;
		}
		intrigue.GetComponent<PhotonView>().RPC("sendGameStart", PhotonTargets.AllBuffered);
	}

	void beepLow(){
		audio.pitch = 0.5f;
		audio.Play();
	}

	void beepHigh(){
		audio.pitch = 0.65f;
		audio.Play();
	}

	[RPC]
	void syncLoadingBar(float percentage){
		loadingBar.GetComponent<UISlider>().value = percentage;
	}
}