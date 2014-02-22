using UnityEngine;
using System.Collections;

public class MenuMusic : MonoBehaviour {

	void Awake() {
	    // see if we've got game music still playing
	    GameObject gameMusic = GameObject.Find("GameMusic");
	    if (gameMusic) {
	        // kill game music
	        Destroy(gameMusic);
	    }
	    // make sure we survive going to different scenes
	    DontDestroyOnLoad(gameObject);
	}
}
