using UnityEngine;
using System.Collections;

public class BackupCamera : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		gameObject.GetComponent<Camera>().enabled = false;
		gameObject.GetComponent<AudioListener>().enabled = false;
		gameObject.GetComponentInChildren<UIRoot>().enabled = false;
	}
	
	void Update(){
		if(GameObject.FindGameObjectsWithTag("MainCamera").Length==0){
			Debug.Log("Can't find cameras");
			gameObject.GetComponent<Camera>().enabled = true;
			gameObject.GetComponent<AudioListener>().enabled = true;
			gameObject.GetComponentInChildren<UIRoot>().enabled = true;
		}
	}
}
