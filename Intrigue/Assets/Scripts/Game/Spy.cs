using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Spy : BasePlayer{
	
	public UILabel stunsUI;
	[HideInInspector]
	public float percentComplete = 0;
	[HideInInspector]
	public bool doingObjective = false;
	[HideInInspector]
	public string objectiveType;
	private int stuns = 3;
	private UIPanel objPanel;
	private UISlider objSlider;
	private GameObject [] servers; 

	void Awake(){
		stunsUI.text = "Stun Charges:\n[00FF00]3[-]";
		servers = GameObject.FindGameObjectsWithTag("ObjectiveMain");
	}


	protected override void Update () {
		base.Update();
		if(photonView.isMine){
			//Locate the necessary NGUI objects
			/*------------------------------------------------------*/
			locateNGUIObjects();
			/*------------------------------------------------------*/


			//NGUI code for doing Objectives
			/*------------------------------------------------------*/
			NGUITools.SetActive(objPanel.gameObject, doingObjective);
			if(doingObjective){
				objSlider = objPanel.GetComponentInChildren<UISlider>();
				objSlider.value = percentComplete;
			}
			/*------------------------------------------------------*/

			//NGUI code for getting out
			/*------------------------------------------------------*/
			if(isOut){
				NGUITools.SetActive(outLabel, true);
				if(hairHat!=null)
					hairHat.GetComponent<Renderer>().enabled = true;
			}
			else{
				NGUITools.SetActive(outLabel, false);
			}
			/*------------------------------------------------------*/


			//Code for interacting
			/*------------------------------------------------------*/
			if ( Input.GetKey(KeyCode.E) ){
				if(Camera.main!=null)
					attemptInteract();
			}
			else{
				doingObjective = false;
			}

			if( Input.GetKeyUp(KeyCode.E) ){
				foreach(GameObject serv in servers){
					serv.GetComponent<PhotonView>().RPC("setInUse",PhotonTargets.All, false);
				}
			}

			/*------------------------------------------------------*/

			//Code for stunning
			/*------------------------------------------------------*/
			if ( Input.GetKeyUp(KeyCode.F) ){
				if(Camera.main!=null)
					attemptStun();
			}
			/*------------------------------------------------------*/

			//Code to add [] display for active objectives
			/*------------------------------------------------------*/
			addObjectiveText();
			/*------------------------------------------------------*/

			// Objective Animations
			objAnimations();
		}
	}

	void locateNGUIObjects(){
		guiLabels = GetComponentsInChildren<UILabel>();
		guiPanels = GetComponentsInChildren<UIPanel>(true);
		if(objPanel == null){
			foreach(UIPanel uiP in guiPanels){
				if(uiP.gameObject.CompareTag("ObjectivePanel")){
					objPanel = uiP;
				}
			}
		}
		if(outLabel == null){
			foreach(UILabel lab in guiLabels){
				if(lab.gameObject.CompareTag("OutLabel")){
					outLabel = lab.gameObject;
				}
			}
		}
	}

	void addObjectiveText(){
		//Create Objective Texts
		GameObject[] objecs = GameObject.FindGameObjectsWithTag("Objective");
		foreach(GameObject objer in objecs){
				if(!objer.GetComponent<Objective>().textAdded && objer.GetComponent<Objective>().isActive){
					objer.GetComponent<Objective>().textAdded = true;
					GameObject textInstance = Instantiate(allytext, objer.transform.position, objer.transform.rotation) as GameObject;
					Vector3 temp = new Vector3(1.5f,-0.75f,0f);
					Vector3 temp2 = new Vector3(270,0,0);
					textInstance.transform.Rotate(temp2);
					textInstance.GetComponent<AllyText>().offset = temp;
					textInstance.transform.localScale += new Vector3(0.5f,0.5f,0.5f);
					textInstance.GetComponent<AllyText>().target = objer.transform;
					textInstance.transform.parent = objer.transform;
					textInstance.GetComponent<TextMesh>().text = "[ ]";
				}
				else if (!objer.GetComponent<Objective>().isActive && objer.GetComponent<Objective>().textAdded){
					objer.GetComponentInChildren<TextMesh>().text = "";
				}
		}
	}

	void attemptInteract(){
		Ray ray = Camera.main.ScreenPointToRay( screenPoint );
		RaycastHit hit;
		if( Physics.Raycast(ray, out hit, 1000f) ){
			if( hit.transform.tag == "ObjectiveMain" ){
				ObjectiveMain hitObjective = hit.transform.GetComponent<ObjectiveMain>();
				hitObjective.useObjective(gameObject);
				objectiveType = hitObjective.objectiveType;
			}
			else if((Vector3.Distance(hit.transform.position, transform.position)<7 && hit.transform.tag == "Objective")){
				Objective hitObjective = hit.transform.GetComponent<Objective>();
				hitObjective.useObjective(gameObject);
				objectiveType = hitObjective.objectiveType;				
			}
			else
				doingObjective = false;
		}
		else
				doingObjective = false;
	}

	void attemptStun(){
		Ray ray = Camera.main.ScreenPointToRay( screenPoint );
		RaycastHit hit;
		if( Physics.Raycast(ray, out hit, 10f) ){
			if(hit.transform.tag == "Guard" || hit.transform.tag == "Guest"){
				if(stuns>=1){
					hit.transform.GetComponent<PhotonView>().RPC("isStunned", PhotonTargets.All);
					if(hit.transform.tag == "Guard"){
						photonView.RPC("addPlayerScore", PhotonTargets.All, 50);
						base.newEvent("[00CCFF]"+player.Handle+"[-] [FFCC00]has stunned [-][FF2B2B]" + hit.transform.gameObject.GetComponent<BasePlayer>().localHandle + "[-][FFCC00]![-]");
					}
					else{
						photonView.RPC("addPlayerScore", PhotonTargets.All, -50);
						base.newEvent("[00CCFF]"+player.Handle+"[-] [FFCC00]has stunned a guest!");
					}
					stuns--;
					stunsUI.text = "Stun Charges:\n[FF00FF]"+stuns+"[-]";
				}
			}

		}
	}

	void toggleChatOff(){
		chatArea.GetComponentInChildren<UILabel>().alpha = 0;
	}

	void toggleChatOn(){
		chatArea.GetComponentInChildren<UILabel>().alpha = 1;
	}

	void objAnimations(){
		if(doingObjective){
			if(objectiveType=="Safe"){
				animator.SetBool("InteractSafe",true);
			}
			else if(objectiveType=="Computer"){
				animator.SetBool("InteractComp",true);
			}
			else if(objectiveType=="Server"){
				animator.SetBool("InteractServer",true);
			}
		}
		else{
			animator.SetBool("InteractSafe",false);
			animator.SetBool("InteractComp",false);
			animator.SetBool("InteractServer",false);
		}
	}

	[RPC]
	void destroySpy(){
		if( photonView.isMine){
			isOut = true;
			gameObject.GetComponent<NetworkCharacter>().isOut = true;
		}
	}

	[RPC]
	void addPlayerScore(int scoreToAdd){
		if(photonView.isMine){
			player.Score += scoreToAdd;
			PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Score", player.Score}});
		}
		//Adding to team scores
		if(player.TeamID == 1)
			player.TeamScore +=scoreToAdd;
		else
			player.EnemyScore += scoreToAdd;
	}

	[RPC]
	void createTeleport(){
		GameObject teleportInstance = Instantiate(teleportPrefab, transform.position, Quaternion.identity) as GameObject;
		teleportInstance.transform.parent = gameObject.transform;
	}
	
	[RPC]
	public void receiveMessage(string s){
		Debug.Log(PhotonNetwork.player.customProperties["Handle"] + s);
		toggleChatOn();
		textList.Add(s);
		CancelInvoke("toggleChatOff");
		Invoke("toggleChatOff", 5);
	}

	[RPC]
	void setLocalHandle(string handle){
		localHandle = handle;
	}

	[RPC]
	void sendID(int ID){
		this.photonID = ID;
	}
}