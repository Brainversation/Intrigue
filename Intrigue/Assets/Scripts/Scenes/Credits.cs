using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {
	
	void Awake(){
		GameObject menuMusic = GameObject.Find("MenuMusic");
		if(menuMusic){
			Destroy(menuMusic);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(transform.localPosition.y<2900){
			transform.Translate(new Vector3(0f,0.15f,0f) * Time.deltaTime);
		}
		else{
			Vector3 temp = new Vector3(0f,-113f,0f);
			transform.localPosition = temp;
		}
	}

	public void ReturnToMain(){
		Application.LoadLevel("MainMenu");
	}
}
