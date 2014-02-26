using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public string levelToLoad;
	public GameObject UIRoot;
	public GameObject bg;

	public void StartLoadingLevel(string levelTitle){
		StartCoroutine(levelLoader(levelTitle));
		gameObject.GetComponent<UIPanel>().alpha = 1;
		if(bg!=null)
			bg.GetComponent<SpriteRenderer>().enabled = false;
		foreach(Transform child in UIRoot.transform){
			if(!child.gameObject.CompareTag("LevelLoader") && !child.gameObject.CompareTag("MainCamera")){
				NGUITools.SetActive(child.gameObject,false);
			}
		}
	}

	IEnumerator levelLoader(string levelTitle){
		Debug.Log("In LevelLoader");
		PhotonNetwork.LoadLevel2(levelToLoad);
        AsyncOperation async = Application.LoadLevelAsync(levelTitle);
        while(!async.isDone){
        	Debug.Log(async.progress);
        	yield return null;
        }
	}
}