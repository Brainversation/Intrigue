using UnityEngine;
using System.Collections;

public class Spy : MonoBehaviour
{
    
    private PhotonView photonView = null;
    RaycastHit[] objHit;
	Ray objRay = new Ray();
	private Player player;

    //Yield function that waits specified amount of seconds
	IEnumerator Yielder(int seconds){
		yield return new WaitForSeconds(seconds);
	}

	void Start(){
		photonView = PhotonView.Get(this);

		if(photonView.isMine){
			Debug.Log( "Spy" );
			player = GameObject.Find("Player").GetComponent<Player>();
		} else {
			Debug.Log("Spy Deactivated");
			GetComponentInChildren<Camera>().enabled = false; 
			GetComponentInChildren<AudioListener>().enabled = false;
			GetComponentInChildren<MovementController>().enabled = false;
			GetComponentInChildren<MouseLook>().enabled = false; 
			GetComponent<MouseLook>().enabled = false;
			enabled = false;
		}
	}

	void OnGUI() {
		GUILayout.Label( "Team: "+ player.Team );
		GUI.skin.label.fontSize = 20;
		GUI.color = Color.black;
		GUI.Label(new Rect((Screen.width/2)-150,Screen.height-100,300,100), string.Format("{0}", player.Score) );
	}

	void Update () {
	 	if (Input.GetKey("e")){
	 		int i = 0;
	 		objRay = Camera.main.ScreenPointToRay(Input.mousePosition);
	        objHit = Physics.RaycastAll(objRay, 10.0f);
	        Debug.Log("ObjHit len: " + objHit.Length);
	        while (i < objHit.Length) {
		            RaycastHit hit = objHit[i];
		            Debug.Log("Hit: " + hit);
		            Debug.Log("HitTag: " + hit.transform.tag);
			    if(hit.transform.tag=="Objective"){
			   		Objective hitObjective = hit.transform.GetComponent<Objective>();
			   		//if(!hitObjective.inUse){
			   			Debug.Log("Hit Objective");
	            	//	hitObjective.inUse = true;
	            		hitObjective.useObjective(gameObject);
	            	//}
	            }
	            i++; 
			}
		}
	}


	void OnDestory(){
		Debug.Log("Getting Destroyed");
		/*GameObject[] spies = GameObject.FindGameObjectsWithTag("Spy");
		foreach (GameObject spy in spies){
				Transform camRef = GetComponentInChildren<Camera>().transform;
				camRef.parent = spy.transform;
				Intrigue.isSpectating = true;
				Vector3 camHeightFix = new Vector3(0.1499996f,0.5277554f,0.0f);
				camRef.position = spy.transform.position + camHeightFix;
				camRef.rotation = spy.transform.rotation;
				break;
		}*/

	}

	
	[RPC]
	void sendMessage( string text, NetworkMessageInfo info ){
	    Debug.Log(text + " from " + info.sender);
	}

	[RPC]
	void destroySpy(){
		Debug.Log("IS Mine: " + photonView.isMine);
		if( photonView.isMine)
			PhotonNetwork.Destroy(photonView);
	}
}