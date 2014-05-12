using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Spy : BasePlayer{
	
	[HideInInspector] public float percentComplete = 0;
	[HideInInspector] public bool doingObjective = false;
	[HideInInspector] public string objectiveType;
	
	public UILabel stunsUI;
	public UIPanel objPanel;
	public UISlider objSlider;
	
	private int stuns = 3;
	private GameObject [] serverInUse; 

	void Awake(){
		stunsUI.text = "Stun Charges:\n[00FF00]3[-]";
		serverInUse = GameObject.FindGameObjectsWithTag("ObjectiveMain");
	}

	protected override void Start(){
		// Guest or Guard layer
		layerMask = (1 << 9) | (1 << 8);
		base.Start();
	}

	protected override void Update () {
		base.Update();
		if(photonView.isMine){
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
			if( Input.GetKey(KeyCode.E) ){
				if(Camera.main!=null)
					attemptInteract();
			}
			else{
				doingObjective = false;
			}

			if( Input.GetKeyUp(KeyCode.E) ){
				foreach(GameObject serv in serverInUse){
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
		if( Physics.Raycast(ray, out hit, 20f) ){
			if(hit.transform.tag == "Guard" || hit.transform.tag == "Guest"){
				if(stuns>=1){
						if(hit.transform.tag == "Guard" && !hit.transform.gameObject.GetComponent<Guard>().stunned){
							photonView.RPC("addPlayerScore", PhotonTargets.All, 50);
							hit.transform.GetComponent<PhotonView>().RPC("isStunned", PhotonTargets.All);
							hit.transform.gameObject.GetComponent<Guard>().stunned = true;
							stuns--;
							base.newEvent("[00CCFF]"+player.Handle+"[-] [FFCC00]has stunned [-][FF2B2B]" + hit.transform.gameObject.GetComponent<BasePlayer>().localHandle + "[-][FFCC00]![-]");
						}
						else if(!hit.transform.gameObject.GetComponent<BaseAI>().stunned){
							photonView.RPC("addPlayerScore", PhotonTargets.All, -50);
							hit.transform.GetComponent<PhotonView>().RPC("isStunned", PhotonTargets.All);
							stuns--;
							base.newEvent("[00CCFF]"+player.Handle+"[-] [FFCC00]has stunned a guest!");
						}
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
		toggleChatOn();
		textList.Add(s);
		CancelInvoke("toggleChatOff");
		Invoke("toggleChatOff", 5);
	}

	[RPC]
	void setLocalHandle(string handle){
		this.localHandle = handle;
	}

	[RPC]
	void sendID(int ID){
		this.photonID = ID;
	}
}