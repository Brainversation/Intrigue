using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Guard : BasePlayer{
	

	[HideInInspector]
	public bool stunned = false;
	public UIPanel stunUI;

	private GameObject accused;
	private GameObject[] guests = null;
	private GameObject[] spies = null;
	private bool accusing = false;
	private UIPanel accusationGUI;
	private Renderer[] renders;
	private bool nearSpy = false;
	private static List<int> markedSpies = new List<int>();
	private static List<int> markedGuests = new List<int>();

	protected override void Update () {
		base.Update();
		guests = GameObject.FindGameObjectsWithTag("Guest");
		spies = GameObject.FindGameObjectsWithTag("Spy");
		
		//Code to get references to necessary NGUI Objects
		/*------------------------------------------------------*/
		locateNGUIObjects();
		/*------------------------------------------------------*/



		//Code to update time/round label
		/*------------------------------------------------------*/
		if(timeLabel!=null)
			updateTimeLabel();
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

	void highlightTargeted(){
		RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay( screenPoint );
			if (Physics.Raycast (ray, out hit, 15)) {
				if(accused==null)
					if(hit.transform.gameObject.CompareTag("Guest")||hit.transform.gameObject.CompareTag("Spy")){
							renders = hit.transform.gameObject.GetComponentsInChildren<Renderer>();
							foreach(Renderer rend in renders){
								if(rend.gameObject.CompareTag("highLight"))
								rend.material.color = Color.red;
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
								rend.material.color = Color.green;
							} else {
								rend.material.color = Color.white;
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
							}
						}
					}
				}
				if(Vector3.Distance(spy.transform.position, gameObject.transform.position)<50){
						nearSpy = true;
					if(!audio.isPlaying){
						audio.Play();
					}
				}
			}
			if(!nearSpy){
				audio.Stop();
			}
		}
	}
	void updateTimeLabel(){
		int minutesLeft = Mathf.RoundToInt(Mathf.Floor(intrigue.GetTimeLeft/60));
		int seconds = Mathf.RoundToInt(intrigue.GetTimeLeft%60);
		int curRound = intrigue.GetRounds - intrigue.GetRoundsLeft +1;
		string secondsS;
		if(seconds<10)
			secondsS = "0"+seconds.ToString();
		else
			secondsS = seconds.ToString();
		timeLabel.GetComponent<UILabel>().text = minutesLeft +":" + 
													secondsS + "\nRound: " + 
													curRound +"/" + (intrigue.GetRounds+1);
	}

	void locateNGUIObjects(){
		guiPanels = GetComponentsInChildren<UIPanel>();
		guiLabels = GetComponentsInChildren<UILabel>();
		foreach(UILabel lab in guiLabels){
			if(lab.gameObject.CompareTag("TimeLabel")){
				timeLabel = lab.gameObject;
			}
			else if(lab.gameObject.CompareTag("OutLabel")){
				outLabel = lab.gameObject;
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
				photonView.RPC("addPlayerScore", PhotonTargets.AllBuffered, 100);
				photonView.RPC("addScore", PhotonTargets.AllBuffered, player.TeamID, 100);
				photonView.RPC("spyCaught", PhotonTargets.MasterClient);
				accused.GetComponent<PhotonView>().RPC("destroySpy", PhotonTargets.All);
				accused.GetComponent<Spy>().isOut = true;
				accused = null;
			}
		}else{
			photonView.RPC("guardFailed", PhotonTargets.MasterClient);
			isOut = true;
			gameObject.GetComponent<NetworkCharacter>().isOut = true;
			accused = null;
		}
		accusing = false;
		accused = null;
	}

	void stunCooldown(){
		stunned = false;
		NGUITools.SetActive(stunUI.gameObject, false);
		GetComponentInChildren<MovementController>().enabled = true;
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

	[RPC]
	void isStunned(){
		if(photonView.isMine){
			NGUITools.SetActive(stunUI.gameObject, true);
			stunned = true;
			//Have to disable the mouse look on the camera as well
			Component [] mouseLooks = GetComponentsInChildren<MouseLook>();
				foreach(MouseLook ml in mouseLooks){
					ml.enabled = false;
				}

			GetComponentInChildren<MovementController>().enabled = false;
			GetComponentInChildren<AudioListener>().enabled = false;
			GetComponentInChildren<Crosshair>().enabled = false;
			GetComponent<MouseLook>().enabled = false;
			Invoke("stunCooldown", 5);
		}
	}

	[RPC]
	void spyCaught(){
		Debug.Log("spy removed");
		--Intrigue.numSpiesLeft;
	}

	[RPC]
	void guardFailed(){
		Debug.Log("guard removed");
	    --Intrigue.numGuardsLeft;
	}

	[RPC]
	void addScore(int teamID, int scoreToAdd){
		if(teamID == this.player.TeamID){
			player.TeamScore += scoreToAdd;
		}
		else{
			player.EnemyScore += scoreToAdd;
		}
	}

	[RPC]
	void giveHandle(string handle){
		localHandle = handle;
	}

	[RPC]
	void giveScore(int score){
		remoteScore = score;
	}

	[RPC]
	void givePing(int ping){
		localPing = ping;
	}

	[RPC]
	void addPlayerScore(int scoreToAdd){
		if(photonView.isMine){
			player.Score += scoreToAdd;
			photonView.RPC("giveScore", PhotonTargets.All, player.Score);
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
}
