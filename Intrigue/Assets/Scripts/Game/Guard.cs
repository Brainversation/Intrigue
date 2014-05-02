using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Guard : BasePlayer{
	
	[HideInInspector] public bool stunned = false;
	public UIPanel stunUI;
	public AudioSource heartbeat;
	public AudioSource server1;
	public AudioSource server2;
	public AudioSource server3;
	public GameObject stunPrefab;
	public GameObject spyCaughtLabel;

	private GameObject accused;
	private GameObject currentStunEffect;
	private bool recentlyPlayed1 = false;
	private bool recentlyPlayed2 = false;
	private bool recentlyPlayed3 = false;
	private bool stunInstantiated = false;
	private GameObject[] guests = null;
	private GameObject[] spies = null;
	private GameObject[] servers = null;
	private bool accusing = false;
	private UIPanel accusationGUI;
	private Renderer[] renders;
	private bool nearSpy = false;
	private static List<int> markedSpies = new List<int>();
	private static List<int> markedGuests = new List<int>();

	protected override void Update () {
			base.Update();
			
			if(photonView.isMine){
			guests = GameObject.FindGameObjectsWithTag("Guest");
			spies = GameObject.FindGameObjectsWithTag("Spy");
			
			//Code to get references to necessary NGUI Objects
			/*------------------------------------------------------*/
			locateNGUIObjects();
			/*------------------------------------------------------*/

			//Check if any servers under attack
			/*------------------------------------------------------*/
			if(servers == null){
				servers = GameObject.FindGameObjectsWithTag("ObjectiveMain");
			}
			foreach(GameObject serv in servers){
				if(serv.GetComponent<ObjectiveMain>().inUse){
					//Debug.Log(serv.GetComponent<ObjectiveMain>().objectiveName + " in use");
					switch (serv.GetComponent<ObjectiveMain>().objectiveName){
						case 1: if(!server1.isPlaying && !recentlyPlayed1){
									server1.Play(); 
									recentlyPlayed1=true;
									Invoke("resetRecentlyPlayed1", 5f);} 
									break;
						case 2: if(!server2.isPlaying && !recentlyPlayed2){
									server2.Play(); 
									recentlyPlayed2=true;
									Invoke("resetRecentlyPlayed2", 5f);} 
									break;
						case 3: if(!server3.isPlaying && !recentlyPlayed3){
									server3.Play(); 
									recentlyPlayed3=true;
									Invoke("resetRecentlyPlayed3", 5f);} 
									break;
					}
				}
			}
			/*------------------------------------------------------*/



			//NGUI code for getting out
			/*------------------------------------------------------*/
			if(isOut){
				accusing = false;
				accused = null;
				if(hairHat!=null)
					hairHat.GetComponent<Renderer>().enabled = true;
				NGUITools.SetActive(outLabel, true);
			}
			else{
				NGUITools.SetActive(outLabel, false);
			}
			/*------------------------------------------------------*/



			//Code to cancel accusation state
			/*------------------------------------------------------*/
			if(Input.GetKeyUp(KeyCode.Space)){
				accusing = false;
				accused = null;
			}

			if(accused!=null && Vector3.Distance(accused.transform.position, gameObject.transform.position)>60){
				accused = null;
				accusing = false;
			}
			/*------------------------------------------------------*/



			//Updates guest/spy highlighting/marking
			/*------------------------------------------------------*/
			updateHighlighting();
			/*------------------------------------------------------*/
			


			//Highlights the currently targeted guest
			/*------------------------------------------------------*/
			if(Camera.main!=null){
				highlightTargeted();
			}
			/*------------------------------------------------------*/


		}
	}

	void resetRecentlyPlayed1(){
		recentlyPlayed1 = false;
	}

	void resetRecentlyPlayed2(){
		recentlyPlayed2 = false;
	}

	void resetRecentlyPlayed3(){
		recentlyPlayed3 = false;
	}

	void highlightTargeted(){
		RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay( screenPoint );
			if (Physics.Raycast (ray, out hit, 15)) {
				if(accused==null)
					if(hit.transform.gameObject.CompareTag("Guest")||hit.transform.gameObject.CompareTag("Spy")){
							renders = hit.transform.gameObject.GetComponentsInChildren<Renderer>();
							foreach(Renderer rend in renders){
								if(rend.gameObject.CompareTag("highLight"))
								rend.material.shader = Shader.Find("Reflect_Bump_Spec_Lightmap");
							}
					}
			}
			
			if(accusing && accused!=null){

				if(accused.CompareTag("Spy") && !accused.GetComponent<Spy>().isOut){
					accusationGUI.alpha = 1;
					if(Input.GetKeyUp(KeyCode.E)){
						accusing = false;
						testAccusation();
					}
				}
				else if(accused.CompareTag("Guest")){
					accusationGUI.alpha = 1;
					if(Input.GetKeyUp(KeyCode.E)){
						accusing = false;
						testAccusation();
					}
				}
			} else if ( Input.GetKeyUp(KeyCode.E) ){
				if ( Physics.Raycast(ray, out hit, 15) ) {
					if(hit.transform.gameObject.CompareTag("Guest") || hit.transform.gameObject.CompareTag("Spy")){
							accusing = true;
							accused = hit.transform.gameObject;
					}
				}
			} else if( Input.GetKeyUp(KeyCode.M) ){
				if ( Physics.Raycast(ray, out hit, 15) ) {
					if(hit.transform.gameObject.CompareTag("Guest")){
						photonView.RPC("markGuest", PhotonTargets.AllBuffered, hit.transform.gameObject.GetComponent<PhotonView>().viewID);
					} else if (hit.transform.gameObject.CompareTag("Spy")){
						photonView.RPC("markSpy", PhotonTargets.AllBuffered, hit.transform.gameObject.GetComponent<PhotonView>().owner.ID);
					}
				}
			} else {
				accusationGUI.alpha = 0;
			}
	}

	void updateHighlighting(){
		if(guests!=null){
			foreach (GameObject guest in guests){
				if(guest!=accused){
					renders = guest.GetComponentsInChildren<Renderer>();
					foreach(Renderer rend in renders){
						if(rend.gameObject.CompareTag("highLight")){
							if(markedGuests.Contains(rend.transform.root.gameObject.GetComponent<PhotonView>().viewID)){
								rend.material.shader = Shader.Find("Reflect_Bump_Spec_Lightmap");
								rend.material.SetColor("_ReflectColor", Color.yellow);
							} else {
								rend.material.color = Color.white;
								rend.material.shader = Shader.Find("Toon/Basic Outline");
							}
						}
					}
				}
			}
		}
		if(spies!=null){
			nearSpy = false;
			foreach (GameObject spy in spies){
				if(spy!=accused){
					renders = spy.GetComponentsInChildren<Renderer>();
					foreach(Renderer rend in renders){
						if(rend.gameObject.CompareTag("highLight")){
							if(markedSpies.Contains(rend.transform.root.gameObject.GetComponent<PhotonView>().owner.ID)){
								rend.material.color = Color.green;
							} else {
								rend.material.color = Color.white;
								rend.material.shader = Shader.Find("Toon/Basic Outline");
							}
						}
					}
				}
				if(Vector3.Distance(spy.transform.position, gameObject.transform.position)<50 && Mathf.Abs(spy.transform.position.y - gameObject.transform.position.y)<=2){
						nearSpy = true;
					if(!heartbeat.isPlaying){
						heartbeat.Play();
					}
				}
			}
			if(!nearSpy){
				heartbeat.Stop();
			}
		}
	}

	void locateNGUIObjects(){
		guiPanels = GetComponentsInChildren<UIPanel>();
		guiLabels = GetComponentsInChildren<UILabel>();
		if(outLabel == null){
			foreach(UILabel lab in guiLabels){
				if(lab.gameObject.CompareTag("OutLabel")){
					outLabel = lab.gameObject;
				}
			}
		}
		foreach(UIPanel pan in guiPanels){
			if(pan.gameObject.CompareTag("Accusations")){
				accusationGUI = pan;
			}
		}
	}

	void testAccusation(){
		if(accused != null && accused.CompareTag("Spy")){
			if(!accused.GetComponent<Spy>().isOut){
				photonView.RPC("addPlayerScore", PhotonTargets.AllBuffered, player.TeamID, 100);
				photonView.RPC("invokeSpyCaught", PhotonTargets.MasterClient);
				accused.GetComponent<PhotonView>().RPC("destroySpy", PhotonTargets.All);
				accused.GetComponent<Spy>().isOut = true;
				spyCaughtLabel.active = true;
				Invoke("removeSpyCaughtLabel", 2);
				base.newEvent("[FF2B2B]"+player.Handle+"[-] [FFCC00]has caught [-][00CCFF]" + accused.GetComponent<Spy>().localHandle + "[-][FF2B2B]![-]");
				accused = null;
			}
		}else{
			photonView.RPC("invokeGuardFailed", PhotonTargets.MasterClient );
			isOut = true;
			gameObject.GetComponent<NetworkCharacter>().isOut = true;
			accused = null;
			base.newEvent("[FF2B2B]"+player.Handle+"[-] [FFCC00]has accused a guest![-]");
		}
	}

	void removeSpyCaughtLabel(){
		spyCaughtLabel.active = false;
	}

	void stunCooldown(){
		stunned = false;
		stunInstantiated = false;
		photonView.RPC("updateStunPS", PhotonTargets.All, false);
		NGUITools.SetActive(stunUI.gameObject, false);
		GetComponentInChildren<AudioListener>().enabled = true;
		//Have to disable the mouse look on the camera as well
		Component [] mouseLooks = GetComponentsInChildren<MouseLook>();
			foreach(MouseLook ml in mouseLooks){
				ml.enabled = true;
			}	
		GetComponentInChildren<Crosshair>().enabled = true;
		GetComponent<MouseLook>().enabled = true;
		Debug.Log("STUN OVER");
	}

	void spyCaught(){
		--Intrigue.numSpiesLeft;
	}

	void guardFailed(){
	    --Intrigue.numGuardsLeft;
	}

	void toggleChatOff(){
		chatArea.GetComponentInChildren<UILabel>().alpha = 0;
	}

	void toggleChatOn(){
		chatArea.GetComponentInChildren<UILabel>().alpha = 1;
	}
	
	[RPC]
	void isStunned(){
		if(photonView.isMine){
			Debug.Log("I'm stunned");
			if(!stunInstantiated){
				photonView.RPC("updateStunPS", PhotonTargets.All, true);
				stunInstantiated = true;
			}

			NGUITools.SetActive(stunUI.gameObject, true);
			stunned = true;
			//Have to disable the mouse look on the camera as well
			Component [] mouseLooks = GetComponentsInChildren<MouseLook>();
				foreach(MouseLook ml in mouseLooks){
					ml.enabled = false;
				}

			GetComponentInChildren<AudioListener>().enabled = false;
			GetComponentInChildren<Crosshair>().enabled = false;
			GetComponent<MouseLook>().enabled = false;
			Invoke("stunCooldown", 5);
		}
	}

	[RPC]
	void updateStunPS(bool creating){
		if(creating){
			currentStunEffect = Instantiate(stunPrefab, transform.position, transform.rotation) as GameObject;
			currentStunEffect.transform.parent = transform;
			currentStunEffect.transform.localPosition = new Vector3(0, 13.25f, 0);
		}
		else{
			Destroy(currentStunEffect);
		}
	}

	[RPC]
	void invokeSpyCaught(){
		Invoke("spyCaught",5);
	}

	[RPC]
	void invokeGuardFailed(){
		Invoke("guardFailed", 5);
	}


	[RPC]
	void addPlayerScore(int teamID, int scoreToAdd){
		if(photonView.isMine){
			player.Score += scoreToAdd;
			PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Score", player.Score}});
		}
		if(teamID == this.player.TeamID){
			player.TeamScore += scoreToAdd;
		}
		else{
			player.EnemyScore += scoreToAdd;
		}
	}

	[RPC]
	void markSpy(int ID){
		if(!markedSpies.Contains(ID)){
			markedSpies.Add(ID);
		} else {
			markedSpies.Remove(ID);
		}
	}

	[RPC]
	void markGuest(int ID){
		if(!markedGuests.Contains(ID)){
			markedGuests.Add(ID);
		} else {
			markedGuests.Remove(ID);
		}
	}

	[RPC]
	void createTeleport(){
		GameObject teleportInstance = Instantiate(teleportPrefab, transform.position, Quaternion.identity) as GameObject;
		teleportInstance.transform.parent = gameObject.transform;
	}

	[RPC]
	public void receiveMessage(string s){
		toggleChatOn();
		textList.Add(s);
		CancelInvoke("toggleChatOff");
		Invoke("toggleChatOff", 5);
	}
}
