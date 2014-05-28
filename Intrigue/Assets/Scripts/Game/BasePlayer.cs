using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class BasePlayer : MonoBehaviour {
	
	protected Player player;
	protected Vector3 screenPoint = new Vector3(Screen.width/2, Screen.height/2, 0);
	protected Intrigue intrigue;
	protected UILabel[] guiLabels;
	protected int photonID = -1;
	protected Renderer[] renders;
	protected RaycastHit hit;
	
	protected static Dictionary<int, bool> markedOther = new Dictionary<int, bool>();
	protected static Dictionary<int, bool> markedGuests = new Dictionary<int, bool>();

	public AudioSource footstepL;
	public AudioSource footstepR;
	public PhotonView photonView = null;
	public Animator animator;
	public GameObject outLabel;
	public GameObject timeLabel;
	public GameObject hairHat;
	public GameObject allytext;
	public GameObject teleportPrefab;
	public GameObject chatArea;
	public GameObject RoundGuardsIcon;
	public GameObject RoundSpiesIcon;
	public GameObject RoundResultBase;
	public GameObject UIRoot;
	public GameObject scoreboard;
	public GameObject pointPop;
	public UITextList textList;
	public UIPanel conversationGUI;
	public UILabel inConvoGUI;
	public UISprite chatWindow;
	public UISprite server1GUI;
	public UISprite server2GUI;
	public UISprite server3GUI;
	public Shader blueStaticShader;
	public Shader staticShader;
	public Shader toonShader;

	[HideInInspector] public int layerMask;
	[HideInInspector] public string localHandle = "";
	[HideInInspector] public int localPing = 0;
	[HideInInspector] public int remoteScore = 0;
	[HideInInspector] public bool textAdded = false;
	[HideInInspector] public bool isAssigned = false;
	[HideInInspector] public bool isOut = false;
	[HideInInspector] public bool isChatting = false;
	[HideInInspector] public bool isSpectated = false;

	public static bool isSpectating = false;
	public static int spectatingIndex = 0;
	public static GameObject[] spectators;

	private bool roundStarted = false;
	private bool beStrong = false;
	private GameObject[] guardsList;
	private GameObject[] spiesList;
	private GameObject[] servers;
	private List<GameObject> allPlayers = new List<GameObject>();
	private Rect windowRect = new Rect(Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/2);

	private static bool menuFlag = false;
	private bool areYouSure = false;

	protected virtual void Start (){
		// Set conversationGUI so spy and guard can use
		conversationGUI.alpha = 0;
		inConvoGUI.alpha = 0;

		servers = GameObject.FindGameObjectsWithTag("ObjectiveMain");
		photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();
		intrigue = GameObject.FindWithTag("Scripts").GetComponent<Intrigue>();
		Camera cam = GetComponentInChildren<Camera>();

		if(photonView.isMine){
			photonView.RPC("setLocalHandle", PhotonTargets.AllBuffered, player.Handle);
			photonView.RPC("sendID", PhotonTargets.AllBuffered, PhotonNetwork.player.ID);
			InvokeRepeating("syncPing", 1, 2F);
			updateRoundResults();
			textList.Add("[FF2B2B]Press [-]Enter[FF2B2B] to chat![-]");
			cam.farClipPlane = 140.0f;
			Debug.Log("Occlusion Culling : " + cam.useOcclusionCulling);
			if(hairHat!=null)
				hairHat.GetComponent<Renderer>().enabled = false;

		} else {
			cam.enabled = false; 
			cam.GetComponentInChildren<MouseLook>().enabled = false;
			GetComponentInChildren<AudioListener>().enabled = false;
			GetComponentInChildren<Crosshair>().enabled = false;
			GetComponent<MouseLook>().enabled = false;
			foreach(UIPanel uiP in GetComponentsInChildren<UIPanel>(true)){
				NGUITools.SetActive(uiP.gameObject, false);
			}
		}

		Invoke("setTeam", 2);

	}
	
	void updateRoundResults(){
		int cur = 0;
		GameObject resultInstance;
		foreach(int result in Intrigue.roundResults){
			if(result == 1)
				resultInstance = instantiateRoundResultInstance(RoundResultBase, RoundSpiesIcon);
			else
				resultInstance = instantiateRoundResultInstance(RoundResultBase, RoundGuardsIcon);
			Vector3 temp = new Vector3(cur*0.095f,0f,0f);
			resultInstance.transform.localPosition += temp;
			++cur;
		}
	}

	GameObject instantiateRoundResultInstance(GameObject parent, GameObject prefab){
        GameObject go = GameObject.Instantiate(prefab) as GameObject;
		Transform t = go.transform;
		t.parent = parent.transform;
		t.localPosition = Vector3.zero;
		t.localRotation = Quaternion.identity;
		t.localScale = new Vector3(0.003487f,0.003487f,0.003487f);
		go.layer = parent.layer;
		return go;
	}

	void setTeam(){
		PhotonNetwork.player.SetCustomProperties(new Hashtable{{"Team", player.Team}});
	}

	void getAllPlayers(){
		spiesList = GameObject.FindGameObjectsWithTag("Spy");
		guardsList = GameObject.FindGameObjectsWithTag("Guard");

		foreach(GameObject spIn in spiesList){
			allPlayers.Add(spIn);
		}

		foreach(GameObject guIn in guardsList){
			allPlayers.Add(guIn);
		}
	}

	protected virtual void Update () {
		if(photonView.isMine){
			if(!intrigue.gameStart && !roundStarted){
				GetComponentInChildren<Camera>().enabled = false;
				GetComponentInChildren<AudioListener>().enabled = false;
				GetComponentInChildren<Crosshair>().enabled = false;
			} else if(intrigue.gameStart && !roundStarted){
				GetComponentInChildren<Camera>().enabled = true;
				GetComponentInChildren<AudioListener>().enabled = true;
				GetComponentInChildren<Crosshair>().enabled = true;
				getAllPlayers();
				roundStarted = true;
			}

			//Code to create ally usernames
			/*------------------------------------------------------*/
			allyUsernames();
			/*------------------------------------------------------*/


			if(chatWindow.alpha == 1){
				isChatting = true;
				chatArea.GetComponentInChildren<UILabel>().alpha = 1;
			}
			else{ 
				isChatting = false;
			}
			/*------------------------------------------------------*/

			
			//Highlights the currently targeted guest
			/*------------------------------------------------------*/
			if(Camera.main!=null){
				highlightTargeted();
			}
			/*------------------------------------------------------*/
			
			if(intrigue.GetTimeLeft<410){
				if(!beStrong && player.Team == "Guard" && Intrigue.numGuardsLeft == 1){
					beStrong = true;
					textList.Add("[FFCC00]You are the last Guard left [-]" + player.Handle + "[FFCC00]. Be strong for mother.[-]");
				}
				if(!beStrong && player.Team == "Spy" && Intrigue.numSpiesLeft == 1){
					beStrong = true;
					textList.Add("[FFCC00]You are the last Spy left [-]" + player.Handle + "[FFCC00]. Be strong for mother.[-]");
				}	
			}
			
		}

		playFootsteps();

		//Updates Server GUI
		foreach(GameObject serv in servers){
			int curServ = serv.GetComponent<ObjectiveMain>().objectiveName-1;
			UISprite curSprite = null;
			switch(curServ){
				case 0:	curSprite = server1GUI;
					break;
				case 1: curSprite = server2GUI;
					break;
				case 2: curSprite = server3GUI;
					break;
			}
			if(curSprite){
				curSprite.fillAmount = (float)serv.GetComponent<ObjectiveMain>().completionPercentage/100f;
			}
		}

		//Code to update time/round label
		/*------------------------------------------------------*/
		if(timeLabel!=null)
			updateTimeLabel();
		/*------------------------------------------------------*/


		//Adding toggle to chat with Z and making chat visible
		/*------------------------------------------------------*/
		if(Input.GetKeyUp(KeyCode.Z)){
			if(chatArea.GetComponentInChildren<UILabel>().alpha == 1)
				chatArea.GetComponentInChildren<UILabel>().alpha = 0;
			else
				chatArea.GetComponentInChildren<UILabel>().alpha = 1;
		}

		if(BasePlayer.isSpectating && isSpectated && Input.GetKeyUp(KeyCode.Space)){
			switchSpectate();
		}

		if(Input.GetKeyDown(KeyCode.Escape)){
			Screen.lockCursor = !Screen.lockCursor;
			BasePlayer.menuFlag = !BasePlayer.menuFlag;
		}
		
	}

	void OnGUI(){
		if(BasePlayer.menuFlag){
			windowRect = GUILayout.Window(0, windowRect, doWindow, "");
		}
	}

	void doWindow(int windowID){
		GUILayout.Label("Status: " + PhotonNetwork.connectionStateDetailed.ToString());
		if(!areYouSure){
			if(GUILayout.Button("Leave Match")){
			areYouSure = true;		
			}
		}
		else{
			if(GUILayout.Button("Are you sure?\nLeaving will hurt your team you heartless meany face.")){
					PhotonNetwork.LeaveRoom();
					PhotonNetwork.LoadLevel( "MainMenu" );
			}	
		}
	}

	void updateTimeLabel(){
		int minutesLeft = Mathf.RoundToInt(Mathf.Floor(intrigue.GetTimeLeft/60));
		int seconds = Mathf.RoundToInt(intrigue.GetTimeLeft%60);
		string secondsS;
		if(seconds<10)
			secondsS = "0"+seconds.ToString();
		else
			secondsS = seconds.ToString();
		if(intrigue.GetTimeLeft>60){
			timeLabel.GetComponent<UILabel>().text = minutesLeft +":" + 
											secondsS;
		}
		else{
			timeLabel.GetComponent<UILabel>().text = "[FF0000]" + minutesLeft +":" + secondsS;
		}

	}


	void allyUsernames(){
		//Puts ally usernames above their head
		GameObject[] allies = GameObject.FindGameObjectsWithTag(player.Team);

		foreach(GameObject ally in allies){
			if(ally != null && ally!=gameObject){
				if(!ally.GetComponent<BasePlayer>().textAdded){
					ally.GetComponent<BasePlayer>().textAdded = true;
					GameObject textInstance = Instantiate(allytext, ally.transform.position,ally.transform.rotation) as GameObject;
					textInstance.GetComponent<AllyText>().target = ally.transform;
					textInstance.transform.parent = ally.transform;
					textInstance.GetComponent<TextMesh>().text = (string)PhotonPlayer.Find(ally.GetComponent<BasePlayer>().photonID).customProperties["Handle"];
				}

				if((ally.GetComponentInChildren<TextMesh>().text == "") && ally.GetComponent<BasePlayer>().textAdded){
					ally.GetComponentInChildren<TextMesh>().text = (string)PhotonPlayer.Find(ally.GetComponent<BasePlayer>().photonID).customProperties["Handle"];
				}
			}
		}
	}

	void syncPing(){
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Ping", PhotonNetwork.GetPing()}});
	}

	public void newEvent(string eventMessage){
		BasePlayer bp;
		foreach(GameObject playerInstance in allPlayers){
			bp = playerInstance.GetComponent<BasePlayer>();
			bp.GetComponent<PhotonView>().RPC("receiveMessage", PhotonPlayer.Find(bp.photonID), eventMessage);
		}
	}

	void playFootsteps(){
		//Left foot position
		Vector3 leftFootT = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
		Quaternion leftFootQ = animator.GetIKRotation(AvatarIKGoal.LeftFoot);
		Vector3 leftFootH = new Vector3(0, -animator.leftFeetBottomHeight, 0);
		Vector3 posL = leftFootT + leftFootQ * leftFootH;
		//Right foot position
		Vector3 rightFootT = animator.GetIKPosition(AvatarIKGoal.RightFoot);
		Quaternion rightFootQ = animator.GetIKRotation(AvatarIKGoal.RightFoot);
		Vector3 rightFootH = new Vector3(0, -animator.rightFeetBottomHeight, 0);
		Vector3 posR = rightFootT + rightFootQ * rightFootH;

		float rHeight = posR.y - transform.position.y;
		float lHeight = posL.y - transform.position.y;

		if(rHeight > 0f){
			if(!footstepR.isPlaying){
				footstepR.Play();
			}
		}
		if(lHeight > 0f){
			if(!footstepL.isPlaying){
				footstepL.Play();
			}
		}
	}

	public void outStarted(){
		Invoke("spectate", 5);
		Invoke("startTeleport", 4);
	}

	void startTeleport(){
		photonView.RPC("createTeleport", PhotonTargets.All);
	}

	void spectate(){
		if(!Intrigue.gameOverFlag){
			BasePlayer.isSpectating = true;
			switchSpectate();
			PhotonNetwork.Destroy(gameObject);
		}
	}

	private void switchSpectate(){

		BasePlayer.spectators = GameObject.FindGameObjectsWithTag(player.Team);
		BasePlayer.spectatingIndex = BasePlayer.spectatingIndex %
												BasePlayer.spectators.Length;
		GameObject teamMate;
		int count = 0;
		do{
			teamMate = BasePlayer.spectators[BasePlayer.spectatingIndex];
			BasePlayer.spectatingIndex = (++BasePlayer.spectatingIndex) %
												BasePlayer.spectators.Length;
			++count;
		}while(count <= BasePlayer.spectators.Length && (teamMate == null || teamMate == gameObject));

		if(Intrigue.gameOverFlag){
			return;
		}

		BasePlayer bp = teamMate.GetComponent<BasePlayer>();
		this.isSpectated = false;
		bp.Invoke("flipIsSpectated", 2);

		this.GetComponentInChildren<Camera>().enabled = false;
		NGUITools.SetActive(this.chatArea, false);
		NGUITools.SetActiveChildren(this.chatArea, false);
		NGUITools.SetActive(this.scoreboard, false);
		NGUITools.SetActiveChildren(this.scoreboard, false);
		NGUITools.SetActive(this.timeLabel, false);
		NGUITools.SetActive(this.UIRoot, false);

		teamMate.GetComponentInChildren<Camera>().enabled = true;
		NGUITools.SetActive(bp.chatArea, true);
		NGUITools.SetActiveChildren(bp.chatArea, true);
		NGUITools.SetActive(bp.scoreboard, true);
		NGUITools.SetActiveChildren(bp.scoreboard, true);
		NGUITools.SetActive(bp.timeLabel, true);
		NGUITools.SetActive(bp.UIRoot, true);
	}

	protected virtual void highlightTargeted(){
		Ray ray = Camera.main.ScreenPointToRay( screenPoint );
		if(hit.transform != null){
			renders = hit.transform.gameObject.GetComponentsInChildren<Renderer>();
			foreach(Renderer rend in renders){
				if(rend.gameObject.CompareTag("highLight")){
					if(markedGuests.ContainsKey(rend.transform.root.gameObject.GetComponent<PhotonView>().viewID) ||
						markedOther.ContainsKey(rend.transform.root.gameObject.GetComponent<PhotonView>().viewID)){
						Debug.Log("Material highlighted: " + rend.material);
						foreach(Material mat in rend.materials){
							mat.shader = staticShader;
						}
						rend.material.SetColor("_ReflectColor", Color.yellow);
					} else {
						rend.material.color = Color.white;	
						rend.material.SetColor("_ReflectColor", Color.red);
						foreach(Material mat in rend.materials){
							mat.shader = toonShader;
						}
					}
				}
			}
		}

		if (Physics.Raycast(ray, out hit, 15, layerMask)) {
			string tag = hit.transform.gameObject.tag;
			if((tag == "Spy" && !hit.transform.GetComponent<BasePlayer>().isOut) || 
				(tag == "Guard" && !hit.transform.GetComponent<BasePlayer>().isOut && !hit.transform.GetComponent<Guard>().stunned) ||
				(tag == "Guest" && !hit.transform.GetComponent<BaseAI>().stunned)){

				renders = hit.transform.gameObject.GetComponentsInChildren<Renderer>();
				foreach(Renderer rend in renders){
					if(rend.gameObject.CompareTag("highLight")){
						foreach(Material mat in rend.materials){
							Debug.Log("Mat name = " + mat);
							if(mat.name.Contains("Mat"))
								mat.shader = blueStaticShader;
							else
								mat.shader = staticShader;
						}
					}
				}

			}
		}
	}

	public void flipIsSpectated(){
		this.isSpectated = true;
	}

}
