using UnityEngine;
using System.Collections;

public class Spy : MonoBehaviour
{

	private PhotonView photonView = null;
	private bool isSpectating = false;
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
		GUI.skin.label.fontSize = 20;
		GUI.color = Color.black;
		GUI.Label(new Rect((Screen.width/2)-150,Screen.height-100,300,100), string.Format("{0}", player.Score) );
		if( isSpectating ) GUI.Label(new Rect((Screen.width/2)-150,Screen.height-50,300,100), "Spectating!" );
	}

	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Input.GetKey("e")){
			RaycastHit hit;
			Debug.DrawRay(ray.origin,ray.direction*10f,Color.green);
			if( Physics.Raycast(ray, out hit, 10.0f) ){
				if( hit.transform.tag == "Objective" ){
					Objective hitObjective = hit.transform.GetComponent<Objective>();
					Debug.Log("Hit Objective");
					hitObjective.useObjective(gameObject);
				}
			}
		}
	}


	void spectate(){
		Debug.Log("Spy Getting Destroyed");
		GameObject[] spies = GameObject.FindGameObjectsWithTag("Spy");
		foreach (GameObject spy in spies){
			spy.GetComponentInChildren<Camera>().enabled = true;
			isSpectating = true;
			break;
		}
	}

	[RPC]
	void destroySpy(){
		if( photonView.isMine){
			spectate();
			PhotonNetwork.Destroy(gameObject);
		}
	}
}
