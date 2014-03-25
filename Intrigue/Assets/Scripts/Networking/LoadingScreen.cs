using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

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
		if(Application.loadedLevelName=="Intrigue")
			intrigue = GameObject.Find("Scripts").GetComponent<Intrigue>();
	}

	public void StartLoadingLevel(string levelTitle){
		levelToLoad = levelTitle;
		async = Application.LoadLevelAsync(levelToLoad);
		StartCoroutine(levelLoader());
		if(gameObject.GetComponent<UIPanel>())
			gameObject.GetComponent<UIPanel>().alpha = 1;
		if(bg!=null)
			bg.GetComponent<SpriteRenderer>().enabled = false;

		if(levelTitle=="Intrigue"){
			if(Application.loadedLevelName == "Intrigue"){
				if(levelToLoad=="PostGame")
					loadTitle.GetComponent<UILabel>().text = "GAME OVER";
				else
					loadTitle.GetComponent<UILabel>().text = "ROUND OVER\n[FF0000]SWITCHING SIDES[-]";
				loadResult.GetComponent<UILabel>().text = intrigue.roundResult;
				}
				else
					loadTitle.GetComponent<UILabel>().text = "GAME STARTING";
		}

	}

	IEnumerator levelLoader(){
	 	if(levelToLoad=="Intrigue"){
        	async.allowSceneActivation = false;
        }
        else{
        	// PhotonNetwork.LoadLevel2(levelToLoad);
		 	loadingBar.GetComponent<UISlider>().value = async.progress;
		 	loadTitle.GetComponent<UILabel>().text = "Intrigue";
		 	loadTimer.GetComponent<UILabel>().text = "";
        }

        while(async.progress<0.9f){
        	yield return null;
        }

        photonView.RPC("levelLoaded", PhotonTargets.MasterClient, player.Handle);

        while(!async.allowSceneActivation){	
	        if(PhotonNetwork.isMasterClient){
	        	Debug.Log("Waiting: " + loadCounter + "/" + (PhotonNetwork.playerList.Length));
	        	if(loadCounter == PhotonNetwork.playerList.Length){
	        		photonView.RPC("startGame", PhotonTargets.AllBuffered);
	        		yield break;
	        	}
	        }
	        yield return null;
	    }
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
   	
   	}

   	IEnumerator Waiter() {
		while(countdownCur<countdownDuration){
			countdownCur+= Time.deltaTime;
			loadingBar.GetComponent<UISlider>().value = countdownCur/countdownDuration;
			loadTimer.GetComponent<UILabel>().text = Mathf.RoundToInt(countdownDuration-countdownCur)+"s";
			yield return null;
		}
		// PhotonNetwork.LoadLevel2(levelToLoad);
		async.allowSceneActivation = true;
	}

	void callStart(){
		photonView.RPC("startGame", PhotonTargets.All);
	}

	[RPC]
	void levelLoaded(string playerName){
		++loadCounter;
	}

	[RPC]
	void startGame(){
		Debug.Log("Start Game Called");
		StartCoroutine(Waiter());
	}
}