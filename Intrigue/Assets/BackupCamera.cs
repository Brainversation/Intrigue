using UnityEngine;
using System.Collections;

public class BackupCamera : MonoBehaviour {
	private GameObject[] cams;
	private bool foundActive = false;

	// Use this for initialization
	void Awake () {
		gameObject.GetComponent<Camera>().enabled = false;
		gameObject.GetComponent<AudioListener>().enabled = false;
		gameObject.GetComponentInChildren<UIRoot>().enabled = false;
	}
	
	void Update(){
			cams = GameObject.FindGameObjectsWithTag("MainCamera");
			foundActive = false;
			foreach(GameObject c in cams){
				if(c.GetComponent<Camera>().enabled){
					foundActive = true;
				}
			}
			if(!foundActive){
				gameObject.GetComponent<Camera>().enabled = true;
				gameObject.GetComponent<AudioListener>().enabled = true;
				gameObject.GetComponentInChildren<UIRoot>().enabled = true;
			}
			else{
				gameObject.GetComponent<Camera>().enabled = false;
				gameObject.GetComponent<AudioListener>().enabled = false;
				gameObject.GetComponentInChildren<UIRoot>().enabled = false;
			}
		}
}
