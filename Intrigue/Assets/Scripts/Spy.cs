using UnityEngine;
using System.Collections;

public class Spy : MonoBehaviour
{
    
    //Yield function that waits specified amount of seconds
	IEnumerator Yielder(int seconds){
		yield return new WaitForSeconds(seconds);
	}

	void Start(){
		if(networkView.isMine){
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
			GetComponentInChildren<FPSInputController>().enabled = false; 
			GetComponent<MouseLook>().enabled = false;
			GetComponent<CharacterMotor>().enabled = false;
			enabled = false;
		}
	}



	void Update () {
		
		//<-------------- HERE IS WHERE INTERACTION WITH OBJECTIVES GOES ------------->

	}

	void OnNetworkInstantiate ( NetworkMessageInfo info ) {
			Debug.Log("New object instantiated by " + info.sender);
	}

	public void callRPC( NetworkViewID viewID2){
		Debug.Log("callRPC viewID " + viewID2 + " owner " + viewID2.owner);
		networkView.RPC("moveCamera", viewID2.owner, viewID2);
	}

	[RPC]
	void sendMessage( string text, NetworkMessageInfo info ){
	    Debug.Log(text + " from " + info.sender);
	}

	[RPC]
	void moveCamera( NetworkViewID viewID2 ){
		Debug.Log("moveCamera viewID " + viewID2 + " owner " + viewID2.owner);
		if (viewID2 != networkView.viewID) return;
		GameObject[] spies = GameObject.FindGameObjectsWithTag("Spy");
		foreach (GameObject spy in spies){
			if ( (spy.GetComponent<NetworkView>().viewID) != networkView.viewID ){
				Camera camRef = GetComponentInChildren<Camera>().transform;
				camRef.parent = spy.transform;
				Intrigue.isSpectating = true;
				camRef.position = spy.transform.position + Vector3(0.1499996,0.5277554,0);
				camRef.rotation = spy.transform.rotation;
				break;
				}
		}

		Network.Destroy(this.gameObject);
	}
}