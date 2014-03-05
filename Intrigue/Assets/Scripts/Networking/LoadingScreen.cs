using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public string levelToLoad;
	public GameObject UIRoot;
	public GameObject bg;
	public GameObject loadingBar;
	private int loadCounter = 0;
	private AsyncOperation async;
	private PhotonView photonView = null;
	private Player player;
	private float countdownDuration = 10;
	private float countdownCur = 0;


	void Start(){
		this.photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();
	}

	public void StartLoadingLevel(string levelTitle){
		StartCoroutine(levelLoader(levelTitle));
		if(gameObject.GetComponent<UIPanel>())
			gameObject.GetComponent<UIPanel>().alpha = 1;
		if(bg!=null)
			bg.GetComponent<SpriteRenderer>().enabled = false;
	}

	IEnumerator levelLoader(string levelTitle){
		if(async==null){
	 		async = Application.LoadLevelAsync(levelTitle);
	 	}

	 	if(levelTitle=="Intrigue"){
        	async.allowSceneActivation = false;
        }

        else{
        	PhotonNetwork.LoadLevel2(levelTitle);
        	if(loadingBar!=null){
		 		loadingBar.GetComponent<UISlider>().value = async.progress;
	 		}
        }

        while(async.progress<0.9f){
        	yield return null;
        }

        photonView.RPC("levelLoaded", PhotonTargets.MasterClient, player.Handle);

        while(!async.allowSceneActivation){	
	        if(PhotonNetwork.isMasterClient){
	        	Debug.Log("Waiting: " + loadCounter + "/" + (PhotonNetwork.playerList.Length));
	        	if(loadCounter == PhotonNetwork.playerList.Length){
	        		photonView.RPC("startGame", PhotonTargets.All, levelTitle);
	        	}
	        }
	        yield return null;
	    }
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
   	
   	}

   	IEnumerator Waiter() {
		loadingBar.GetComponent<UISlider>().value = countdownCur/countdownDuration;
		if(countdownCur<countdownDuration){
			countdownCur+= Time.deltaTime;
			yield return null;
		}
		else{
		 	async.allowSceneActivation = true;
		}
	}

	[RPC]
	void levelLoaded(string playerName){
		++loadCounter;
	}

	[RPC]
	void startGame(string level){
		Debug.Log("Start Game Called");
		PhotonNetwork.LoadLevel2(level);
		if(level == "Intrigue" && Application.loadedLevelName == "Intrigue")
			StartCoroutine(Waiter());
		else
			async.allowSceneActivation = true;
	}
}