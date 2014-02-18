using UnityEngine;
using System.Collections;

public class Spy : MonoBehaviour
{

	private bool isSpectating = false;
	private Player player;
	public PhotonView photonView = null;
	public GameObject allytext;
	public bool textAdded = false;
	public bool isOut = false;
	public string localHandle = "No Handle";
	public float percentComplete = 0;
	public bool doingObjective = false;
	public int remoteScore = 0;
	private GameObject timeLabel;
	private GameObject outLabel;
	private UILabel[] guiLabels;
	private UIPanel[] uiPanels;
	private UIPanel objPanel;
	private UISlider objSlider;
	private GameObject[] guards = null;
	private GameObject[] spies = null;
	private Intrigue intrigue;
	private Rect windowRect = new Rect(Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/2);
	private Vector3 screenPoint = new Vector3(Screen.width/2, Screen.height/2, 0);

	//Yield function that waits specified amount of seconds
	IEnumerator Yielder(int seconds){
		yield return new WaitForSeconds(seconds);
	}

	void Start(){
		photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();
		intrigue = GameObject.FindWithTag("Scripts").GetComponent<Intrigue>();

		if(photonView.isMine){
			Debug.Log( "Spy" );
			localHandle = player.Handle;
			remoteScore = player.Score;
			photonView.RPC("giveHandle", PhotonTargets.OthersBuffered, player.Handle);
			photonView.RPC("giveScore", PhotonTargets.OthersBuffered, player.Score);

		} else {
			Debug.Log("Spy Deactivated");
			GetComponentInChildren<Camera>().enabled = false; 
			GetComponentInChildren<AudioListener>().enabled = false;
			GetComponentInChildren<MovementController>().enabled = false;
			GetComponentInChildren<MouseLook>().enabled = false; 
			GetComponentInChildren<SpyCrosshair>().enabled = false;
			GetComponent<MouseLook>().enabled = false;
			enabled = false;
		}
	}

	void OnGUI() {
		GUI.skin.label.fontSize = 20;
		GUI.color = Color.white;
		if( isSpectating ) GUI.Label(new Rect((Screen.width/2)-150,Screen.height-50,300,100), "Spectating!" );


		if( Input.GetKey(KeyCode.Tab) ){
			guards = GameObject.FindGameObjectsWithTag("Guard");
			spies = GameObject.FindGameObjectsWithTag("Spy");
			windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "Scoreboard");
		}
	}

	void DoMyWindow(int windowID) {
		GUILayout.Label("Guards: " + player.EnemyScore);

		foreach(GameObject g in guards){
			GUILayout.Label(g.GetComponent<Guard>().localHandle + " " + g.GetComponent<Guard>().remoteScore);
		}

		GUILayout.Label("Spies: " + player.TeamScore);

		foreach(GameObject s in spies){
			if(s!= gameObject)
				GUILayout.Label(s.GetComponent<Spy>().localHandle + " " + s.GetComponent<Spy>().remoteScore);
			else
				GUILayout.Label( player.Handle + " " + player.Score);
		}

	}

	void Update () {
		int minutesLeft = Mathf.RoundToInt(Mathf.Floor(intrigue.GetTimeLeft/60));
		int seconds = Mathf.RoundToInt(intrigue.GetTimeLeft%60);
		int curRound = intrigue.GetRounds - intrigue.GetRoundsLeft +1;
		guiLabels = GetComponentsInChildren<UILabel>();
		uiPanels = GetComponentsInChildren<UIPanel>(true);
		if(uiPanels[0].gameObject.CompareTag("ObjPanel"))
			objPanel = uiPanels[0];
		else
			objPanel = uiPanels[1];

		NGUITools.SetActive(objPanel.gameObject, doingObjective);
		if(doingObjective){
			objSlider = objPanel.GetComponentInChildren<UISlider>();
			Debug.Log("PC:"+percentComplete);
			objSlider.value = percentComplete;
		}

		foreach(UILabel lab in guiLabels){
			if(lab.gameObject.CompareTag("TimeLabel")){
				timeLabel = lab.gameObject;
			}
			else if(lab.gameObject.CompareTag("OutLabel")){
				outLabel = lab.gameObject;
			}
		}

		if(isOut){
			NGUITools.SetActive(outLabel, true);
		}
		else{
			NGUITools.SetActive(outLabel, false);
		}

		if(timeLabel!=null)
			timeLabel.GetComponent<UILabel>().text = minutesLeft +":" + seconds + "\nRound: " + curRound +"/" + (intrigue.GetRounds+1);

		//Interact Raycasts
		Ray ray = Camera.main.ScreenPointToRay( screenPoint );
		if (Input.GetKey("e")){
			RaycastHit hit;
			Debug.DrawRay(ray.origin,ray.direction*15f,Color.green);
			if( Physics.Raycast(ray, out hit, 15.0f) ){
				if( hit.transform.tag == "Objective" ){
					Objective hitObjective = hit.transform.GetComponent<Objective>();
					hitObjective.useObjective(gameObject);
				}
				else
					doingObjective = false;
			}
			else
					doingObjective = false;
		}
		else
			doingObjective = false;

		//Create Ally and Objective Texts
		GameObject[] allies = GameObject.FindGameObjectsWithTag("Spy");
		GameObject[] objecs = GameObject.FindGameObjectsWithTag("Objective");
		foreach(GameObject ally in allies){
			if(ally!=gameObject){
				if(!ally.GetComponent<Spy>().textAdded){
					//Debug.Log("creating ally text");
					ally.GetComponent<Spy>().textAdded = true;
					GameObject textInstance = Instantiate(allytext, ally.transform.position,ally.transform.rotation) as GameObject;
					textInstance.GetComponent<AllyText>().target = ally.transform;
					textInstance.transform.parent = ally.transform;
					textInstance.GetComponent<TextMesh>().text = ally.GetComponent<Spy>().localHandle;
				}
				if((ally.GetComponentInChildren<TextMesh>().text == "No Handle") && ally.GetComponent<Spy>().textAdded){
					//Debug.Log("Changing Handle from: " + ally.GetComponentInChildren<TextMesh>().text + " to:" + ally.GetComponent<Spy>().localHandle);
					ally.GetComponentInChildren<TextMesh>().text = ally.GetComponent<Spy>().localHandle;
					
				}
			}
		}

		foreach(GameObject objer in objecs){
				if(!objer.GetComponent<Objective>().textAdded && objer.GetComponent<Objective>().active){
					objer.GetComponent<Objective>().textAdded = true;
					GameObject textInstance = Instantiate(allytext, objer.transform.position, objer.transform.rotation) as GameObject;
					Vector3 temp = new Vector3(0,1,0);
					textInstance.GetComponent<AllyText>().offset = temp;
					textInstance.transform.localScale += new Vector3(0.5f,0.5f,0.5f);
					textInstance.GetComponent<AllyText>().target = objer.transform;
					textInstance.transform.parent = objer.transform;
					textInstance.GetComponent<TextMesh>().text = "[ACTIVE]";
				}
				else if (!objer.GetComponent<Objective>().active && objer.GetComponent<Objective>().textAdded){
					objer.GetComponentInChildren<TextMesh>().text = "";
				}
		}


	}

	public void outStarted(){
		Invoke("spectate", 5);
	}

	void spectate(){
		Debug.Log("Trying to Spectate");
		GetComponentInChildren<Camera>().enabled = false; 
		GameObject[] spies = GameObject.FindGameObjectsWithTag("Spy");
		if(spies.Length == 0){
			spies = GameObject.FindGameObjectsWithTag("Guard");
		}
		foreach (GameObject spy in spies){
			if(spy.gameObject != gameObject){
				spy.GetComponentInChildren<Camera>().enabled = true; 
				isSpectating = true;
				Debug.Log("In For loop enabled a Camera");
				break;
			}
		}

		PhotonNetwork.Destroy(gameObject);
	}

	[RPC]
	void destroySpy(){
		if( photonView.isMine){
			isOut = true;
			gameObject.GetComponent<NetworkCharacter>().isOut = true;
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
	void addPlayerScore(int scoreToAdd){
		if(photonView.isMine)
			player.Score += scoreToAdd;
		else
			remoteScore += scoreToAdd;
		//photonView.RPC("giveScore", PhotonTargets.OthersBuffered, player.Score);
	}
}
