using UnityEngine;
using System.Collections;

public class BasePlayer : MonoBehaviour {
	
	protected Player player;
	protected Vector3 screenPoint = new Vector3(Screen.width/2, Screen.height/2, 0);
	protected Intrigue intrigue;
	protected UIPanel[] guiPanels;
	protected UILabel[] guiLabels;
	protected GameObject timeLabel;
	protected GameObject outLabel;

	public PhotonView photonView = null;
	public string localHandle = "";
	public int localPing = 0;
	public int remoteScore = 0;
	public bool textAdded = false;
	public bool isAssigned = false;
	public bool isOut = false;
	public GameObject hairHat;
	public GameObject allytext;

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
			enabled = false;
		}
	}
	
	protected virtual void Update () {
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

	public void outStarted(){
		Invoke("spectate", 5);
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
