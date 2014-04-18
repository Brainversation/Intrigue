using UnityEngine;
using System.Collections;

public class BasePlayer : MonoBehaviour {
	
	protected Player player;
	protected Vector3 screenPoint = new Vector3(Screen.width/2, Screen.height/2, 0);
	protected Intrigue intrigue;
	protected UIPanel[] guiPanels;
	protected UILabel[] guiLabels;
	protected GameObject outLabel;

	public AudioSource footstepL;
	public AudioSource footstepR;
	public PhotonView photonView = null;
	public Animator animator;
	public GameObject timeLabel;
	public GameObject hairHat;
	public GameObject allytext;
	public GameObject teleportPrefab;
	[HideInInspector] public string localHandle = "";
	[HideInInspector] public int localPing = 0;
	[HideInInspector] public int remoteScore = 0;
	[HideInInspector] public bool textAdded = false;
	[HideInInspector] public bool isAssigned = false;
	[HideInInspector] public bool isOut = false;


	private bool roundStarted = false;

	//Yield function that waits specified amount of seconds
	IEnumerator Yielder(int seconds){
		yield return new WaitForSeconds(seconds);
	}

	void Start () {
		photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();
		intrigue = GameObject.FindWithTag("Scripts").GetComponent<Intrigue>();
		InvokeRepeating("syncPingAndScore", 1, 1F);
		
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
			GetComponentInChildren<Crosshair>().enabled = false;
			GetComponent<MouseLook>().enabled = false;
			guiPanels = GetComponentsInChildren<UIPanel>(true);
			foreach(UIPanel uiP in guiPanels){
				NGUITools.SetActive(uiP.gameObject, false);
			}
		}
	}
	
	protected virtual void Update () {
		if(photonView.isMine){
			if(!intrigue.gameStart && !roundStarted){
				GetComponentInChildren<Camera>().enabled = false;
				GetComponentInChildren<AudioListener>().enabled = false;
				GetComponentInChildren<MovementController>().enabled = false;
				GetComponentInChildren<Crosshair>().enabled = false;
			}
			else if(intrigue.gameStart && !roundStarted){
				GetComponentInChildren<Camera>().enabled = true;
				GetComponentInChildren<AudioListener>().enabled = true;
				GetComponentInChildren<MovementController>().enabled = true;
				GetComponentInChildren<Crosshair>().enabled = true;
				roundStarted = true;
			}

			//Code to create ally usernames
			/*------------------------------------------------------*/
			allyUsernames();
			/*------------------------------------------------------*/


			//Code to update time/round label
			/*------------------------------------------------------*/
			if(timeLabel!=null)
				updateTimeLabel();
			/*------------------------------------------------------*/


		}

		playFootsteps();

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
				if(player.Team=="Spy"){
					if(!ally.GetComponent<Spy>().textAdded){
						ally.GetComponent<Spy>().textAdded = true;
						GameObject textInstance = Instantiate(allytext, ally.transform.position,ally.transform.rotation) as GameObject;
						textInstance.GetComponent<AllyText>().target = ally.transform;
						textInstance.transform.parent = ally.transform;
						textInstance.GetComponent<TextMesh>().text = ally.GetComponent<Spy>().localHandle;
					}
					if((ally.GetComponentInChildren<TextMesh>().text == ""|| ally.GetComponentInChildren<TextMesh>().text == "No Handle") && ally.GetComponent<Spy>().textAdded){
						ally.GetComponentInChildren<TextMesh>().text = ally.GetComponent<Spy>().localHandle;
					}
				}
				else{
					if(!ally.GetComponent<Guard>().textAdded){
						ally.GetComponent<Guard>().textAdded = true;
						GameObject textInstance = Instantiate(allytext, ally.transform.position,ally.transform.rotation) as GameObject;
						textInstance.GetComponent<AllyText>().target = ally.transform;
						textInstance.transform.parent = ally.transform;
						textInstance.GetComponent<TextMesh>().text = ally.GetComponent<Guard>().localHandle;
					}
					if((ally.GetComponentInChildren<TextMesh>().text == "" || ally.GetComponentInChildren<TextMesh>().text == "No Handle") && ally.GetComponent<Guard>().textAdded){
						ally.GetComponentInChildren<TextMesh>().text = ally.GetComponent<Guard>().localHandle;
					}
				}
			}
		}
	}

	void syncPingAndScore(){
		localPing = PhotonNetwork.GetPing();
		photonView.RPC("givePing", PhotonTargets.All, localPing);
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
					break;
				}
			}
		}

		PhotonNetwork.Destroy(gameObject);
	}

}
