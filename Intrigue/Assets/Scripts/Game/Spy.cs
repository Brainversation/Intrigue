using UnityEngine;
using System.Collections;

public class Spy : MonoBehaviour
{
    private PhotonView photonView = null;
    RaycastHit objHit = new RaycastHit();
	Ray objRay = new Ray();


    //Yield function that waits specified amount of seconds
	IEnumerator Yielder(int seconds){
		yield return new WaitForSeconds(seconds);
	}

	void Start(){
		photonView = PhotonView.Get(this);

		if(photonView.isMine){
			Debug.Log("Spy");
		} 
		else {
			GetComponentInChildren<Camera>().enabled = false; 
			GetComponentInChildren<AudioListener>().enabled = false;
			GetComponentInChildren<MovementController>().enabled = false;
			GetComponentInChildren<MouseLook>().enabled = false; 
			GetComponent<MouseLook>().enabled = false;
			enabled = false;
		}
	}

	void OnGUI() {
		GUI.skin.label.fontSize = 20;
		GUI.color = Color.black;
		GUI.Label(new Rect((Screen.width/2)-150,Screen.height-100,300,100), string.Format("{0}", photonView.owner.Score) );
	}

	void Update () {
		
		 if (Input.GetKey("e")){
		 		objRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		        if (Physics.Raycast(objRay, out objHit, 10.0f)){
		            Debug.Log(objHit.transform.tag);
		            if(objHit.transform.tag=="Objective"){
		            	objHit.transform.GetComponent<Objective>().useObjective(gameObject);
		            }
		        }
		}
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