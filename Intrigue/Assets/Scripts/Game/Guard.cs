/**************************************************************************
 **                                                                       *
 **                COPYRIGHT AND CONFIDENTIALITY NOTICE                   *
 **                                                                       *
 **    Copyright (c) 2014 Hacksaw Games. All rights reserved.             *
 **                                                                       *
 **    This software contains information confidential and proprietary    *
 **    to Hacksaw Games. It shall not be reproduced in whole or in        *
 **    part, or transferred to other documents, or disclosed to third     *
 **    parties, or used for any purpose other than that for which it was  *
 **    obtained, without the prior written consent of Hacksaw Games.      *
 **                                                                       *
 **************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Guard : BasePlayer{
	
	[HideInInspector] public bool stunned = false;
	[HideInInspector] public bool recentlyStunned = false;
	public UIPanel stunUI;
	public UIPanel accusationGUI;
	public AudioSource heartbeat;
	public AudioSource server1;
	public AudioSource server2;
	public AudioSource server3;
	public GameObject stunPrefab;
	public GameObject spyCaughtLabel;
	public UILabel accuseUIAccuse;
	public UILabel accuseUICancel;

	private GameObject accused;
	private GameObject currentStunEffect;
	private bool recentlyPlayed1 = false;
	private bool recentlyPlayed2 = false;
	private bool recentlyPlayed3 = false;
	private bool stunInstantiated = false;
	private GameObject[] spies = null;
	private GameObject[] serverAlerts = null;
	private bool accusing = false;
	private bool nearSpy = false;

	protected override void Start(){
		layerMask = (1 << GUEST) | (1 << SPY) |
					(1 << Objective.OBJECTIVE) |
					(1 << ObjectiveMain.OBJECTIVEMAIN) |
					(1 << WALL);
		PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Team", "Guard"},{"Ping", PhotonNetwork.GetPing()}, {"isOut", false}});	
		base.Start();
		accuseUICancel.text = "[FFCC00]" + Settings.Cancel.ToUpper();
		accuseUIAccuse.text = "[FFCC00]" + Settings.Interact.ToUpper();
	}

	protected override void Update (){
		base.Update();
		if(photonView.isMine){
			spies = GameObject.FindGameObjectsWithTag("Spy");
			
			//Heartbeat
			if(spies!=null){
				nearSpy = false;
				foreach (GameObject spy in spies){
					if(Vector3.Distance(spy.transform.position, gameObject.transform.position)<50 && Mathf.Abs(spy.transform.position.y - gameObject.transform.position.y)<=2){
						nearSpy = true;
						if(!heartbeat.isPlaying){
							heartbeat.Play();
						}
					}
				}
			}
			if(!nearSpy){
				heartbeat.Stop();
			}


			//Check if any serverAlerts under attack
			/*------------------------------------------------------*/
			if(serverAlerts == null){
				serverAlerts = GameObject.FindGameObjectsWithTag("ObjectiveMain");
			}

			foreach(GameObject serv in serverAlerts){
				if(serv.GetComponent<ObjectiveMain>().inUse){
					switch (serv.GetComponent<ObjectiveMain>().objectiveName){
						case 1: if(!server1.isPlaying && !recentlyPlayed1){
									server1.Play(); 
									recentlyPlayed1=true;
									Invoke("resetRecentlyPlayed1", 10f);} 
									break;
						case 2: if(!server2.isPlaying && !recentlyPlayed2){
									server2.Play(); 
									recentlyPlayed2=true;
									Invoke("resetRecentlyPlayed2", 10f);} 
									break;
						case 3: if(!server3.isPlaying && !recentlyPlayed3){
									server3.Play(); 
									recentlyPlayed3=true;
									Invoke("resetRecentlyPlayed3", 10f);} 
									break;
					}
				}
			}
			/*------------------------------------------------------*/



			//NGUI code for getting out
			/*------------------------------------------------------*/
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
			/*------------------------------------------------------*/



			//Code to cancel accusation state
			/*------------------------------------------------------*/
			if(Input.GetKeyUp(Settings.Cancel)){
				accusing = false;
				accused = null;
				accusationGUI.alpha = 0;
			}

			if(accused!=null && Vector3.Distance(accused.transform.position, gameObject.transform.position)>60){
				accused = null;
				accusing = false;
			}
			/*------------------------------------------------------*/
		}
	}

	void resetRecentlyPlayed1(){
		recentlyPlayed1 = false;
	}

	void resetRecentlyPlayed2(){
		recentlyPlayed2 = false;
	}

	void resetRecentlyPlayed3(){
		recentlyPlayed3 = false;
	}

	protected override void highlightTargeted(){
		base.highlightTargeted();

		if(accusing && accused!=null){
			accusationGUI.alpha = 1;
			if(Input.GetKeyUp(Settings.Interact) && !isChatting){
				accusing = false;
				testAccusation();
			}
		} else if(hit.transform != null ){
			if( Input.GetKeyUp(Settings.Interact) && !isChatting){
				if(hit.transform.gameObject.layer == BasePlayer.SPY && !(hit.transform.gameObject.GetComponent<BasePlayer>().isOut)||
					(hit.transform.gameObject.layer == BasePlayer.GUEST)){
					accusing = true;
					accused = hit.transform.gameObject;
				}

			} else if( Input.GetKeyUp(Settings.Mark) ){
				if(hit.transform.gameObject.CompareTag("Guest")){
					photonView.RPC("markGuest", PhotonTargets.AllBuffered, hit.transform.gameObject.GetComponent<PhotonView>().viewID);
				} else if (hit.transform.gameObject.CompareTag("Spy")){
					photonView.RPC("markSpy", PhotonTargets.AllBuffered, hit.transform.gameObject.GetComponent<PhotonView>().viewID);
				}
			} 
		}else {
			accusationGUI.alpha = 0;
		}
	}


	void testAccusation(){
		if(accused != null && accused.CompareTag("Spy") && !accused.GetComponent<BasePlayer>().isOut){
			photonView.RPC("addPlayerScore", PhotonTargets.AllBuffered, player.TeamID, 100);
			pointPop.GetComponent<TextMesh>().text = "+100";
			Instantiate(pointPop, accused.transform.position + (Vector3.up * accused.GetComponent<Collider>().bounds.size.y), accused.transform.rotation);
			photonView.RPC("invokeSpyCaught", PhotonTargets.MasterClient);
			accused.GetComponent<PhotonView>().RPC("destroySpy", PhotonTargets.MasterClient);
			accused.GetComponent<BasePlayer>().isOut = true;
			spyCaughtLabel.SetActive(true);
			Invoke("removeSpyCaughtLabel", 2);
			base.newEvent("[FF2B2B]"+player.Handle+"[-] [FFCC00]has caught [-][00CCFF]" + accused.GetComponent<BasePlayer>().localHandle + "[-][FFCC00]![-]");
			accused = null;
		}else{
			photonView.RPC("invokeGuardFailed", PhotonTargets.MasterClient );
			isOut = true;
			PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"isOut", true}});
			gameObject.GetComponent<NetworkCharacter>().isOut = true;
			accused = null;
			base.newEvent("[FF2B2B]"+player.Handle+"[-] [FFCC00]has accused a guest![-]");
		}
	}

	void removeSpyCaughtLabel(){
		spyCaughtLabel.SetActive(false);
	}

	void stunCooldown(){
		GetComponent<NetworkCharacter>().isStunned = stunned = false;
		recentlyStunned = true;
		Invoke("resetRecentlyStunned", 5);
		stunInstantiated = false;
		photonView.RPC("updateStunPS", PhotonTargets.All, false);
		NGUITools.SetActive(stunUI.gameObject, false);
		GetComponentInChildren<AudioListener>().enabled = true;
		//Have to disable the mouse look on the camera as well
		Component [] mouseLooks = GetComponentsInChildren<MouseLook>();
		foreach(MouseLook ml in mouseLooks){
			ml.enabled = true;
		}
		GetComponentInChildren<Crosshair>().enabled = true;
		GetComponent<MouseLook>().enabled = true;
	}

	void resetRecentlyStunned(){
		recentlyStunned = false;
	}

	void spyCaught(){
		--Intrigue.numSpiesLeft;
	}

	void guardFailed(){
		Debug.Log("die guard die");
	    --Intrigue.numGuardsLeft;
	}

	void toggleChatOff(){
		if(chatArea!=null){
			chatArea.GetComponentInChildren<UILabel>().alpha = 0;
			chatArea.GetComponentInChildren<UISprite>().alpha = 0;
		}
	}

	void toggleChatOn(){
		if(chatArea!=null){
			chatArea.GetComponentInChildren<UILabel>().alpha = 1;
			chatArea.GetComponentInChildren<UISprite>().alpha = 1;
		}
	}
	
	[RPC]
	void stunGuard(){
		if(photonView.isMine){
			if(!stunInstantiated){
				photonView.RPC("updateStunPS", PhotonTargets.All, true);
				stunInstantiated = true;
			}

			accusing = false;
			accused = null;
			stunned = GetComponent<NetworkCharacter>().isStunned = true;
			NGUITools.SetActive(stunUI.gameObject, true);
			//Have to disable the mouse look on the camera as well
			Component [] mouseLooks = GetComponentsInChildren<MouseLook>();
			foreach(MouseLook ml in mouseLooks){
				ml.enabled = false;
			}

			GetComponentInChildren<AudioListener>().enabled = false;
			GetComponentInChildren<Crosshair>().enabled = false;
			GetComponent<MouseLook>().enabled = false;
			Invoke("stunCooldown", 5);
		}
	}

	[RPC]
	void updateStunPS(bool creating){
		stunned = creating;
		if(creating){
			currentStunEffect = Instantiate(stunPrefab, transform.position, transform.rotation) as GameObject;
			currentStunEffect.transform.parent = transform;
			currentStunEffect.transform.localPosition = new Vector3(0, 13.25f, 0);
		}
		else{
			Destroy(currentStunEffect);
		}
	}

	[RPC]
	void invokeSpyCaught(){
		Invoke("spyCaught",5);
	}

	[RPC]
	void invokeGuardFailed(){
		Invoke("guardFailed", 5);
	}


	[RPC]
	void addPlayerScore(int teamID, int scoreToAdd){
		if(photonView.isMine){
			player.Score += scoreToAdd;
			PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Score", player.Score}});
		}
		if(teamID == 1){
			player.Team1Score += scoreToAdd;
			PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Team1Score", player.Team1Score}});
		}
		else{
			player.Team2Score += scoreToAdd;
			PhotonNetwork.player.SetCustomProperties(new Hashtable(){{"Team2Score", player.Team2Score}});
		}
	}

	[RPC]
	void markSpy(int ID){
		if(!markedOther.ContainsKey(ID)){
			markedOther.Add(ID, true);
		} else {
			markedOther.Remove(ID);
		}
	}

	[RPC]
	void markGuest(int ID){
		if(!markedGuests.ContainsKey(ID)){
			markedGuests.Add(ID, true);
		} else {
			markedGuests.Remove(ID);
		}
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
