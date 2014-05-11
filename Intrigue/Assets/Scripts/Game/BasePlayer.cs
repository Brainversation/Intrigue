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
	protected int layerMask;
	protected Renderer[] renders;
	protected RaycastHit hit;
	protected Shader staticShader;
	protected Shader toonShader;
	
	protected static List<int> markedOther = new List<int>();
	protected static List<int> markedGuests = new List<int>();

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
	public UITextList textList;
	public UIPanel conversationGUI;
	public UISprite chatWindow;
	public UISprite server1GUI;
	public UISprite server2GUI;
	public UISprite server3GUI;

	[HideInInspector] public string localHandle = "";
	[HideInInspector] public int localPing = 0;
	[HideInInspector] public int remoteScore = 0;
	[HideInInspector] public bool textAdded = false;
	[HideInInspector] public bool isAssigned = false;
	[HideInInspector] public bool isOut = false;


	private bool roundStarted = false;
	private GameObject[] guardsList;
	private GameObject[] spiesList;
	private GameObject[] servers;
	private List<GameObject> allPlayers = new List<GameObject>();

	protected virtual void Start (){
		// Set conversationGUI so spy and guard can use
		conversationGUI.alpha = 0;

		staticShader = Shader.Find("Reflect_Bump_Spec_Lightmap");
		toonShader = Shader.Find("Toon/Basic Outline");
		servers = GameObject.FindGameObjectsWithTag("ObjectiveMain");
		photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();
		intrigue = GameObject.FindWithTag("Scripts").GetComponent<Intrigue>();

		if(photonView.isMine){
			photonView.RPC("setLocalHandle", PhotonTargets.AllBuffered, player.Handle);
			photonView.RPC("sendID", PhotonTargets.AllBuffered, PhotonNetwork.player.ID);
			InvokeRepeating("syncPing", 1, 2F);
			PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Team", player.Team},{"Ping", PhotonNetwork.GetPing()}});	
			textList.Add("[FF2B2B]Press [-]Shift [FF2B2B]+[-] Enter[FF2B2B] to chat![-]");
			if(hairHat!=null)
				hairHat.GetComponent<Renderer>().enabled = false;
		} else {
			Camera cam = GetComponentInChildren<Camera>();
			cam.enabled = false; 
			cam.GetComponentInChildren<MouseLook>().enabled = false;
			GetComponentInChildren<AudioListener>().enabled = false;
			GetComponentInChildren<Crosshair>().enabled = false;
			GetComponent<MouseLook>().enabled = false;
			foreach(UIPanel uiP in GetComponentsInChildren<UIPanel>(true)){
				NGUITools.SetActive(uiP.gameObject, false);
			}
		}
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
			}
			else if(intrigue.gameStart && !roundStarted){
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
				chatArea.GetComponentInChildren<UILabel>().alpha = 1;
			}
			/*------------------------------------------------------*/

			//Code to update time/round label
			/*------------------------------------------------------*/
			if(timeLabel!=null)
				updateTimeLabel();
			/*------------------------------------------------------*/
			
			//Highlights the currently targeted guest
			/*------------------------------------------------------*/
			if(Camera.main!=null){
				highlightTargeted();
			}
			/*------------------------------------------------------*/
	
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

		//Adding toggle to chat with Z and making chat visible
		/*------------------------------------------------------*/
		if(Input.GetKeyUp(KeyCode.Z)){
			if(chatArea.GetComponentInChildren<UILabel>().alpha == 1)
				chatArea.GetComponentInChildren<UILabel>().alpha = 0;
			else
				chatArea.GetComponentInChildren<UILabel>().alpha = 1;
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
		if(intrigue.GetTimeLeft>60){
			timeLabel.GetComponent<UILabel>().text = minutesLeft +":" + 
											secondsS + "\nRound: " + 
											curRound +"/" + (intrigue.GetRounds+1);
		}
		else{
			timeLabel.GetComponent<UILabel>().text = "[FF0000]" + minutesLeft +":" + 
								secondsS + "[-]\nRound: " + 
								curRound +"/" + (intrigue.GetRounds+1);
		}

	}


	void allyUsernames(){
		//Puts ally usernames above their head
		GameObject[] allies = GameObject.FindGameObjectsWithTag(player.Team);

		foreach(GameObject ally in allies){
			if(ally!=gameObject){
				if(!ally.GetComponent<BasePlayer>().textAdded){
					ally.GetComponent<BasePlayer>().textAdded = true;
					GameObject textInstance = Instantiate(allytext, ally.transform.position,ally.transform.rotation) as GameObject;
					textInstance.GetComponent<AllyText>().target = ally.transform;
					textInstance.transform.parent = ally.transform;
					textInstance.GetComponent<TextMesh>().text = (string)PhotonPlayer.Find(ally.GetComponent<BasePlayer>().photonID).customProperties["Handle"];
				}

				if((ally.GetComponentInChildren<TextMesh>().text == "" || ally.GetComponentInChildren<TextMesh>().text == "No Handle") && ally.GetComponent<Spy>().textAdded){
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
		GetComponentInChildren<Camera>().enabled = false;
		if(!intrigue.gameOverFlag){
			foreach (GameObject teamMates in GameObject.FindGameObjectsWithTag(player.Team)){
				if(teamMates.gameObject != gameObject){
					teamMates.GetComponentInChildren<Camera>().enabled = true;
		 			foreach(UIPanel uiP in teamMates.GetComponentsInChildren<UIPanel>(true)){
						if(uiP.gameObject.CompareTag("ChatArea") ||
						   uiP.gameObject.CompareTag("Scoreboard") ||
							uiP.gameObject.CompareTag("TimeLabel") ||
							uiP.gameObject.CompareTag("UIRoot")){
							NGUITools.SetActive(uiP.gameObject, true);
							if(uiP.gameObject.CompareTag("Scoreboard")){
								NGUITools.SetActiveChildren(uiP.gameObject, true);
							}
						}
					}
					break;
				}
			}
		}

		PhotonNetwork.Destroy(gameObject);
	}

	protected virtual void highlightTargeted(){
		Ray ray = Camera.main.ScreenPointToRay( screenPoint );
		if(hit.transform != null){
			renders = hit.transform.gameObject.GetComponentsInChildren<Renderer>();
			foreach(Renderer rend in renders){
				if(rend.gameObject.CompareTag("highLight")){
					if(markedGuests.Contains(rend.transform.root.gameObject.GetComponent<PhotonView>().viewID) ||
						markedOther.Contains(rend.transform.root.gameObject.GetComponent<PhotonView>().viewID)){
						rend.material.shader = staticShader;
						rend.material.SetColor("_ReflectColor", Color.yellow);
					} else {
						rend.material.color = Color.white;	
						rend.material.SetColor("_ReflectColor", Color.red);
						rend.material.shader = toonShader;
					}
				}
			}
		}

		if (Physics.Raycast(ray, out hit, 25, layerMask)) {
			renders = hit.transform.gameObject.GetComponentsInChildren<Renderer>();
			foreach(Renderer rend in renders){
				if(rend.gameObject.CompareTag("highLight"))
					rend.material.shader = staticShader;
			}
		}
	}

}
