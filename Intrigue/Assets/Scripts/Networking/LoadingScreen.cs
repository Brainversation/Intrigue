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
		if(UIRoot!=null){
			foreach(Transform child in UIRoot.transform){
				if(!child.gameObject.CompareTag("LevelLoader") && !child.gameObject.CompareTag("MainCamera") && !child.gameObject.CompareTag("LoaderMain")){
					NGUITools.SetActive(child.gameObject,false);
				}
			}
		}
	}

	IEnumerator levelLoader(string levelTitle){
	 	async = Application.LoadLevelAsync(levelTitle);
	 	if(loadingBar!=null){
		 	if(levelTitle=="Intrigue")
		 		loadingBar.GetComponent<UISlider>().value = loadCounter/PhotonNetwork.playerList.Length;
		 	else
		 		loadingBar.GetComponent<UISlider>().value = async.progress;
	 	}

	 	if(levelTitle=="Intrigue"){
        	async.allowSceneActivation = false;
        }
        else{
        	PhotonNetwork.LoadLevel2(levelToLoad);
        }
        while(async.progress<0.9f){
        	yield return null;
        }

        Debug.Log("LevelLoaded" + async.progress);
        photonView.RPC("levelLoaded", PhotonTargets.MasterClient, player.Handle);

        while(!async.allowSceneActivation){	
	        if(PhotonNetwork.isMasterClient){
	        	Debug.Log("Waiting: " + loadCounter + "/" + (PhotonNetwork.playerList.Length));
	        	if(loadCounter == PhotonNetwork.playerList.Length){
	        		photonView.RPC("startGame", PhotonTargets.All);
	        	}
	        }
	        yield return null;
	    }
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
   	
   	}

	[RPC]
	void levelLoaded(string playerName){
		++loadCounter;
	}

	[RPC]
	void startGame(){
		Debug.Log("Start Game Called");
		PhotonNetwork.LoadLevel2(levelToLoad);
		async.allowSceneActivation = true;
	}
}