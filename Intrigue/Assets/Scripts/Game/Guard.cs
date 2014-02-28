using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour
{
	private bool accusing = false;
	private bool isSpectating = false;
	private GameObject accused;
	private PhotonView photonView = null;
	private GameObject[] guests = null;
	private GameObject[] spies = null;
	private GameObject[] guards = null;
	private Player player;
	private Intrigue intrigue;
	private Network network;
	private Vector3 screenPoint = new Vector3(Screen.width/2, Screen.height/2, 0);
	private GameObject timeLabel;
	private GameObject outLabel;
	private UIPanel[] guiPanels;
	private UILabel[] guiLabels;
	private Renderer[] renders;
	public int remoteScore = 0;
	public int localPing = 0;
	public GameObject allytext;
	public bool textAdded = false;
	public bool isOut = false;
	public bool isAssigned = false;
	public string localHandle = "";
	public GameObject hairHat;

	//Yield function that waits specified amount of seconds


	private Rect windowRect = new Rect(Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/2);

	IEnumerator Yielder(int seconds){
		yield return new WaitForSeconds(seconds);
	}

	void Start(){
		InvokeRepeating("syncPingAndScore", 1, 2F);
		intrigue = GameObject.FindWithTag("Scripts").GetComponent<Intrigue>();
		network = GameObject.FindWithTag("Scripts").GetComponent<Network>();
		player = GameObject.Find("Player").GetComponent<Player>();
		photonView = PhotonView.Get(this);

		if(photonView.isMine){
			localHandle = player.Handle;
			remoteScore = player.Score;
			photonView.RPC("giveHandle", PhotonTargets.OthersBuffered, player.Handle);
			photonView.RPC("giveScore", PhotonTargets.Others, player.Score);
			if(hairHat!=null)
				hairHat.GetComponent<Renderer>().enabled = false;
		} else {
			GetComponentInChildren<Camera>().enabled = false;
			GetComponentInChildren<AudioListener>().enabled = false;
			GetComponentInChildren<MovementController>().enabled = false;
			GetComponentInChildren<MouseLook>().enabled = false;
			GetComponentInChildren<GuardCrosshair>().enabled = false;
			GetComponent<MouseLook>().enabled = false;
			guiPanels = GetComponentsInChildren<UIPanel>(true);
			foreach(UIPanel uiP in guiPanels){
				NGUITools.SetActive(uiP.gameObject, false);
			}
			enabled = false;

		}
	}

	void syncPingAndScore(){
		remoteScore = player.Score;
		localPing = PhotonNetwork.GetPing();
		photonView.RPC("givePing", PhotonTargets.All, PhotonNetwork.GetPing());
		photonView.RPC("giveScore", PhotonTargets.All, player.Score);
	}

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

		int minutesLeft = Mathf.RoundToInt(Mathf.Floor(intrigue.GetTimeLeft/60));
		int seconds = Mathf.RoundToInt(intrigue.GetTimeLeft%60);
		int curRound = intrigue.GetRounds - intrigue.GetRoundsLeft +1;
		if(timeLabel!=null)
			timeLabel.GetComponent<UILabel>().text = minutesLeft +":" + seconds + "\nRound: " + curRound +"/" + (intrigue.GetRounds+1);
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

		if(accusing && accused!=null){
			guiPanels[1].alpha = 1;
			if(Input.GetKeyUp(KeyCode.E)){
				accusing = false;
				testAccusation();
			}
		}
		else{
			guiPanels[1].alpha= 0;
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

		if ( Input.GetKeyUp (KeyCode.E) && !accusing ){
				if ( Physics.Raycast(ray, out hit, 15) ) {
					if(accused==null)
						if(hit.transform.gameObject.CompareTag("Guest") || hit.transform.gameObject.CompareTag("Spy")){
								accusing = true;
								accused = hit.transform.gameObject;
							}
				}
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

	void testAccusation(){
		if(accused != null && accused.CompareTag("Spy")){
			photonView.RPC("addPlayerScore", PhotonTargets.AllBuffered, 100);
			photonView.RPC("addScore", PhotonTargets.AllBuffered, player.TeamID, 100);
			photonView.RPC("spyCaught", PhotonTargets.MasterClient);
			accused.GetComponent<PhotonView>().RPC("destroySpy", PhotonTargets.All);
		}
		else{
			photonView.RPC("guardFailed", PhotonTargets.MasterClient);
			isOut = true;
			gameObject.GetComponent<NetworkCharacter>().isOut = true;
		}
		accusing = false;
		accused = null;
	}
		
	public void outStarted(){
		Invoke("spectate", 5);
	}

	void spectate(){
		GetComponentInChildren<Camera>().enabled = false; 
		GameObject[] guards = GameObject.FindGameObjectsWithTag("Guard");
		if(guards.Length == 0){
			guards = GameObject.FindGameObjectsWithTag("Spy");
		}
		foreach (GameObject guard in guards){
			if(guard.gameObject != gameObject){
				guard.GetComponentInChildren<Camera>().enabled = true; 
				isSpectating = true;
				break;
			}
		}
		PhotonNetwork.Destroy(gameObject);
	}

	[RPC]
	void spyCaught(){
		--Intrigue.numSpiesLeft;
	}

	[RPC]
	void guardFailed(){
	    --Intrigue.numGuardsLeft;
	}

	[RPC]
	void giveHandle(string handle){
		localHandle = handle;
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
	void giveScore(int score){
		remoteScore = score;
	}

	[RPC]
	void givePing(int ping){
		localPing = ping;
	}

	[RPC]
	void addPlayerScore(int scoreToAdd){
		if(photonView.isMine)
			player.Score += scoreToAdd;
		else
			remoteScore += scoreToAdd;
	}
}
