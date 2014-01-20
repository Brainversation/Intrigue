using UnityEngine;
using System.Collections;

public class Spy : MonoBehaviour
{
    private PhotonView photonView = null;
    //Yield function that waits specified amount of seconds
	IEnumerator Yielder(int seconds){
		yield return new WaitForSeconds(seconds);
	}

	void Start(){
		photonView = PhotonView.Get(this);

		if(photonView.isMine){
			Debug.Log("Spy");
			Yielder(1);
			GameObject[] spies = GameObject.FindGameObjectsWithTag("Spy");
			foreach(GameObject spy in spies){
				spy.renderer.material.SetColor("_Color", Color.red);
			}
		} 
		else {
			GetComponentInChildren<Camera>().enabled = false; 
			GetComponentInChildren<AudioListener>().enabled = false;
			GetComponentInChildren<MouseLook>().enabled = false; 
			GetComponent<MouseLook>().enabled = false;
			enabled = false;

			
		}
	}



	void Update () {
		
		//<-------------- HERE IS WHERE INTERACTION WITH OBJECTIVES GOES ------------->

	}

	void OnNetworkInstantiate ( NetworkMessageInfo info ) {
			Debug.Log("New object instantiated by " + info.sender);
	}

	
	[RPC]
	void sendMessage( string text, NetworkMessageInfo info ){
	    Debug.Log(text + " from " + info.sender);
	}


	void OnDestory(){
		GameObject[] spies = GameObject.FindGameObjectsWithTag("Spy");
		foreach (GameObject spy in spies){
				Transform camRef = GetComponentInChildren<Camera>().transform;
				camRef.parent = spy.transform;
				Intrigue.isSpectating = true;
				Vector3 camHeightFix = new Vector3(0.1499996f,0.5277554f,0.0f);
				camRef.position = spy.transform.position + camHeightFix;
				camRef.rotation = spy.transform.rotation;
				break;
		}

	}
}