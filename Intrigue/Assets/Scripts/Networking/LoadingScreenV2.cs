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
	private float countdownDuration = 5.0f;
	private float countdownCur = 0;
	private Intrigue intrigue;
	private string roundResultDisplay;
	private bool countdownStarted = false;
	private float totalCountdownWithLoading;


	void Start(){
		this.photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();
		intrigue = GameObject.FindWithTag("Scripts").GetComponent<Intrigue>();
		totalCountdownWithLoading = countdownDuration + (intrigue.totalGuests*0.1f);
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
			transform.parent.gameObject.active = false;
		}
		Debug.Log(intrigue.loadedGuests + "/"+intrigue.totalGuests);
		loadingBar.GetComponent<UISlider>().value = intrigue.loadedGuests/intrigue.totalGuests;
		if(!intrigue.doneLoading)
			loadTimer.GetComponent<UILabel>().text = Mathf.RoundToInt(totalCountdownWithLoading - (0.1f*intrigue.loadedGuests)) + "s";

		if(intrigue.doneLoading && !countdownStarted){
			countdownStarted = true;
			StartCoroutine(Waiter());
		}
	}

   	IEnumerator Waiter() {
		while(countdownCur<countdownDuration){
			countdownCur+= Time.deltaTime;
			loadTimer.GetComponent<UILabel>().text = Mathf.RoundToInt(countdownDuration-countdownCur)+"s";
			yield return null;
		}
		intrigue.GetComponent<PhotonView>().RPC("sendGameStart", PhotonTargets.AllBuffered);
	}

}