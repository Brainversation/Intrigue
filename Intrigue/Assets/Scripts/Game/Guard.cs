using UnityEngine;
using System.Collections;

public class Guard : BasePlayer{
	
	private GameObject accused;
	private GameObject[] guests = null;
	private GameObject[] spies = null;
	private bool accusing = false;
	private UIPanel accusationGUI;
	private Renderer[] renders;

	void Update () {
		guests = GameObject.FindGameObjectsWithTag("Guest");
		spies = GameObject.FindGameObjectsWithTag("Spy");
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
		int minutesLeft = Mathf.RoundToInt(Mathf.Floor(intrigue.GetTimeLeft/60));
		int seconds = Mathf.RoundToInt(intrigue.GetTimeLeft%60);
		int curRound = intrigue.GetRounds - intrigue.GetRoundsLeft +1;
		string secondsS;
		if(seconds<10)
			secondsS = "0"+seconds.ToString();
		else
			secondsS = seconds.ToString();

		if(timeLabel!=null)
			timeLabel.GetComponent<UILabel>().text = minutesLeft +":" + secondsS + "\nRound: " + curRound +"/" + (intrigue.GetRounds+1);
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

		if(Input.GetKeyUp(KeyCode.Space)){
			accusing = false;
			accused = null;
		}

		if(accused!=null && Vector3.Distance(accused.transform.position, gameObject.transform.position)>60){
			accused = null;
			accusing = false;
		}

		if(guests!=null){
			foreach (GameObject guest in guests){
				if(guest!=accused){
					renders = guest.GetComponentsInChildren<Renderer>();
					foreach(Renderer rend in renders){
						if(rend.gameObject.CompareTag("highLight"))
							rend.material.color = Color.white;
					}
				}
			}
		}
		if(spies!=null){
			foreach (GameObject spy in spies){
				if(spy!=accused)
					renders = spy.GetComponentsInChildren<Renderer>();
					foreach(Renderer rend in renders){
						if(rend.gameObject.CompareTag("highLight"))
							rend.material.color = Color.white;
					}
			}
		}

		//Highlights the currently targeted guest
		if(Camera.main!=null){
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
				accusationGUI.alpha = 1;
				if(Input.GetKeyUp(KeyCode.E)){
					accusing = false;
					testAccusation();
				}
			} else if ( Input.GetKeyUp(KeyCode.E) ){
				if ( Physics.Raycast(ray, out hit, 15) ) {
					if(hit.transform.gameObject.CompareTag("Guest") || hit.transform.gameObject.CompareTag("Spy")){
							accusing = true;
							accused = hit.transform.gameObject;
					}
				}
			} else {
				accusationGUI.alpha = 0;
			}

			//Create Ally Texts
			GameObject[] allies = GameObject.FindGameObjectsWithTag("Guard");
			foreach(GameObject ally in allies){
				if(ally!=gameObject){
					if(!ally.GetComponent<Guard>().textAdded){
						ally.GetComponent<Guard>().textAdded = true;
						GameObject textInstance = Instantiate(allytext, ally.transform.position,ally.transform.rotation) as GameObject;
						textInstance.GetComponent<AllyText>().target = ally.transform;
						textInstance.transform.parent = ally.transform;
						textInstance.GetComponent<TextMesh>().text = ally.GetComponent<Guard>().localHandle;
					}
					if((ally.GetComponentInChildren<TextMesh>().text == "") && ally.GetComponent<Guard>().textAdded){
						ally.GetComponentInChildren<TextMesh>().text = ally.GetComponent<Guard>().localHandle;
						
					}
				}
			}
		}
	}

	void testAccusation(){
		if(accused != null && accused.CompareTag("Spy")){
			if(!accused.GetComponent<Spy>().isOut){
				Debug.Log("accuse ran");
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
}
