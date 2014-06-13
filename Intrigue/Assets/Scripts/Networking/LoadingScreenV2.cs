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

public class LoadingScreenV2 : MonoBehaviour {

	public GameObject UIRoot;
	public GameObject bg;
	public GameObject loadingBar;
	public GameObject loadTimer;
	public GameObject loadTitle;
	public GameObject loadResult;
	public GameObject loadNextRound;
	public GameObject loadTip;
	private AsyncOperation async;
	private Player player;
	private GameObject[] guards;
	private GameObject[] spies;
	private float countdownDuration = 5.0f;
	private float countdownCur = 0;
	private Intrigue intrigue;
	private string roundResultDisplay;
	private bool countdownStarted = false;
	private float totalCountdownWithLoading = 0;
	private PhotonView photonView = null;


	void Start(){
		photonView = PhotonView.Get(this);
		player = Player.Instance;
		intrigue = GameObject.FindWithTag("Scripts").GetComponent<Intrigue>();
		loadTimer.GetComponent<UILabel>().text = totalCountdownWithLoading+"s";

		loadTip.GetComponent<UILabel>().text = getNewTip((string)PhotonNetwork.player.customProperties["Team"]);
		guards = GameObject.FindGameObjectsWithTag("Guard");
		spies = GameObject.FindGameObjectsWithTag("Spy");
		
		foreach (GameObject guard in guards){
			guard.GetComponentInChildren<Camera>().GetComponentInChildren<MouseLook>().enabled = false;
			guard.GetComponentInChildren<MouseLook>().enabled = false;
		}

		foreach (GameObject spy in spies){
			spy.GetComponentInChildren<Camera>().GetComponentInChildren<MouseLook>().enabled = false;
			spy.GetComponentInChildren<MouseLook>().enabled = false;
		}

		if(intrigue.GetRoundsLeft == intrigue.GetRounds){
			loadTitle.GetComponent<UILabel>().text = "MATCH STARTING";
			loadResult.GetComponent<UILabel>().text = "";
		}
		else{
			if((string)PhotonNetwork.player.customProperties["Team"] == "Spy"){
				loadTitle.GetComponent<UILabel>().text = "ROUND STARTING\n[-]TEAM: [00CCFF]SPIES[-]";
			}
			else{
				loadTitle.GetComponent<UILabel>().text = "ROUND STARTING\n[-]TEAM: [FF2B2B]GUARDS[-]";
			}
			loadResult.GetComponent<UILabel>().text = player.PrevResult;
		}
	}

	void Update(){
		if(intrigue.gameStart && !Intrigue.finalRoundOver){
			transform.parent.gameObject.SetActive(false);

			foreach (GameObject guard in guards){
				guard.GetComponentInChildren<Camera>().GetComponentInChildren<MouseLook>().enabled = true;
				guard.GetComponentInChildren<MouseLook>().enabled = true;
			}

			foreach (GameObject spy in spies){
				spy.GetComponentInChildren<Camera>().GetComponentInChildren<MouseLook>().enabled = true;
				spy.GetComponentInChildren<MouseLook>().enabled = true;
			}
		}

		if(PhotonNetwork.isMasterClient){
			if(intrigue.totalGuests!=0){
			loadingBar.GetComponent<UISlider>().value = intrigue.loadedGuests/intrigue.totalGuests;
			photonView.RPC("syncLoadingBar", PhotonTargets.Others, intrigue.loadedGuests/intrigue.totalGuests);
			}
			else{
			photonView.RPC("syncLoadingBar", PhotonTargets.All, 1.0f);	
			}
		}		

		if(!intrigue.doneLoading){
			loadNextRound.GetComponent<UILabel>().text = "Loading Level";
			loadTimer.GetComponent<UILabel>().text = "";
		}


		if(intrigue.doneLoading && !countdownStarted){
			countdownStarted = true;
			loadNextRound.GetComponent<UILabel>().text = "Round starts in: ";
			StartCoroutine(Waiter());
			Invoke("beepLow",1);
			Invoke("beepLow",2);
			Invoke("beepLow",3);
			Invoke("beepHigh",4);
		}

		if(Intrigue.finalRoundOver){
			finalRoundInfo();
		}
	}

   	IEnumerator Waiter() {
		while(countdownCur<countdownDuration){
			countdownCur+= Time.deltaTime;
			int timerSec = Mathf.RoundToInt(countdownDuration-countdownCur);
			loadTimer.GetComponent<UILabel>().text = timerSec+"s";
			yield return null;
		}
		intrigue.GetComponent<PhotonView>().RPC("sendGameStart", PhotonTargets.AllBuffered);
	}

	public void beepLow(){
		audio.pitch = 0.5f;
		audio.Play();
	}

	public void beepHigh(){
		audio.pitch = 0.65f;
		audio.Play();
	}

	string getNewTip(string team){
		int rand;
		string tip;
		if(team == "Guard"){
			rand = Random.Range(1,9);
			switch(rand){
			case 1: tip = "Tip: Use the '[FFCC00]A[-]' and '[FFCC00]D[-]' keys to rotate smoothly when rotating is on.";
				break;
			case 2: tip = "Tip: Use [FFCC00]" + Settings.Mark.ToUpper() + "[-] to mark suspicious guests!";
				break;
			case 3: tip = "Tip: Use [FFCC00]" + Settings.Interact.ToUpper() + "[-] to do accuse!";
				break;
			case 4: tip = "Tip: You will hear a heartbeat when a Spy is near!";
				break;
			case 5: tip = "Tip: Use [FFCC00]CTRL[-] to toggle strafing/smooth rotating!";
				break;
			case 6: tip = "Tip: The closer a Spy is to the server, the faster it downloads.";
				break;
			case 7: tip = "Tip: Use [FFCC00]SPACE[-] to cycle through teammates when spectating!";
				break;
			case 8: tip = "Tip: Use [FFCC00]1[-] to play a random idle animation to blend in!";
				break;
			default: tip = "Tip: Kayvan rarely showers, avoid getting too near!";
				break;
			}
		}else{
			rand = Random.Range(1,10);
			switch(rand){
			case 1: tip = "Tip: Use the '[FFCC00]A[-]' and '[FFCC00]D[-]' keys to rotate smoothly when rotating is on.";
				break;
			case 2: tip = "Tip: When you start downloading a server, it alerts the guards!\nUse this to your advantage.";
				break;
			case 3: tip = "Tip: Use [FFCC00]" + Settings.Stun.ToUpper() + "[-] to stun Guards!";
				break;
			case 4: tip = "Tip: Use [FFCC00]" + Settings.Interact.ToUpper() + "[-] to do objectives!";
				break;
			case 5: tip = "Tip: Guards hear a heartbeat when a you are near.";
				break;
			case 6: tip = "Tip: Use [FFCC00]CTRL[-] to toggle strafing/smooth rotating!";
				break;
			case 7: tip = "Tip: The closer you are to the server, the faster it downloads.";
				break;
			case 8: tip = "Tip: Use [FFCC00]SPACE[-] to cycle through teammates when spectating!";
				break;
			case 9: tip = "Tip: Use [FFCC00]1[-] to play a random idle animation to blend in!";
				break;
			default: tip = "Tip: Kayvan rarely showers, avoid getting too near!";
				break;
			}
		}
		return tip;
	}

	void finalRoundInfo(){
		Invoke("EndGame", 5f);
		foreach (GameObject guard in guards){
			guard.GetComponentInChildren<Camera>().GetComponentInChildren<MouseLook>().enabled = false;
			guard.GetComponentInChildren<MouseLook>().enabled = false;
		}
		foreach (GameObject spy in spies){
			spy.GetComponentInChildren<Camera>().GetComponentInChildren<MouseLook>().enabled = false;
			spy.GetComponentInChildren<MouseLook>().enabled = false;
		}

		NGUITools.SetActive(loadingBar, false);
		NGUITools.SetActive(loadTimer, false);
		NGUITools.SetActive(loadNextRound, false);
		NGUITools.SetActive(loadTip, false);

		if((int)PhotonNetwork.player.customProperties["Team1Score"] > (int)PhotonNetwork.player.customProperties["Team2Score"]){
			if((int)PhotonNetwork.player.customProperties["TeamID"] == 1){
				loadTitle.GetComponent<UILabel>().text = "[FFCC00]VICTORY[-]";
			}else{
				loadTitle.GetComponent<UILabel>().text = "[FF2B2B]DEFEAT[-]";
			}
		}else{
			if((int)PhotonNetwork.player.customProperties["TeamID"] == 2){
				loadTitle.GetComponent<UILabel>().text = "[FFCC00]VICTORY[-]";
			}else{
				loadTitle.GetComponent<UILabel>().text = "[FF2B2B]DEFEAT[-]";
			}
		}
		loadResult.GetComponent<UILabel>().text = player.PrevResult;

	}

	public void EndGame(){
		PhotonNetwork.LoadLevel("PostGame");
	}

	[RPC]
	void syncLoadingBar(float percentage){
		loadingBar.GetComponent<UISlider>().value = percentage;
	}
}