using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public string levelToLoad;
	public GameObject UIRoot;
	public GameObject bg;
	public GameObject loadingBar;
	private int loadCounter=0;
	private AsyncOperation async;
	private PhotonView photonView = null;


	void Start(){
		this.photonView = PhotonView.Get(this);
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
		 		loadingBar.GetComponent<UISlider>().value = this.loadCounter/PhotonNetwork.playerList.Length;
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
        photonView.RPC("levelLoaded", PhotonTargets.All);

        while(!async.allowSceneActivation){
        	Debug.Log("Waiting: " + this.loadCounter + "/" + (PhotonNetwork.playerList.Length));
	        if(PhotonNetwork.isMasterClient){
	        	if(this.loadCounter == PhotonNetwork.playerList.Length){
	        		photonView.RPC("startGame", PhotonTargets.All);
	        	}
	        }
	        yield return null;
	    }
	}

	[RPC]
	void levelLoaded(){
		Debug.Log("loaded called");
		this.loadCounter++;
	}

	[RPC]
	void startGame(){
		Debug.Log("Start Game Called");
		PhotonNetwork.LoadLevel2(levelToLoad);
		async.allowSceneActivation = true;
	}
}