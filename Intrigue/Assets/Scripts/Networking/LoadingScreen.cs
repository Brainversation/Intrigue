using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public string levelToLoad;
	public GameObject UIRoot;
	public GameObject bg;

	public void StartLoadingLevel(string levelTitle){
		StartCoroutine(levelLoader(levelTitle));
		gameObject.GetComponent<UIPanel>().alpha = 1;
		bg.GetComponent<SpriteRenderer>().enabled = false;
		foreach(Transform child in UIRoot.transform){
			if(!child.gameObject.CompareTag("LevelLoader")){
				NGUITools.SetActive(child.gameObject,false);
			}
		}
	}

	IEnumerator levelLoader(string levelTitle){
		PhotonNetwork.LoadLevel2(levelToLoad);
        AsyncOperation async = Application.LoadLevelAsync(levelTitle);
        while(!async.isDone){
        	Debug.Log(async.progress);
        	yield return null;
        }
	}
}