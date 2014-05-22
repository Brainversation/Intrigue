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

	void Update(){
		GameObject[] musics = GameObject.FindGameObjectsWithTag("Music");
		if(musics.Length>=2){
			Destroy(musics[1]);
		}
	}
}
