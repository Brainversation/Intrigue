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

public class NetworkCharacter : Photon.MonoBehaviour {
	
	public const float CHARSPEED = 10;

	public UIPanel sprintPanel;
	public UISprite sprintSprite;
	public Camera cam;
	[HideInInspector] public bool isOut = false;
	[HideInInspector] public bool isStunned = false;
	

	private float speedMult = 1;
	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private Animator anim;
	private Player player;
	private Vector3 camStart;
	private float stamina = 100;
	private float staminaDrainSpeed;
	private float staminaRegenSpeed;
	private bool canRegen;
	private bool strafeToggle;
	private int curRandomAnim;
	private bool isIdleAnimating;

	void Start() {
		//Get References to Animator and Collider
		anim = GetComponent<Animator>();
		anim.speed = 1.0f;
		player = Player.Instance;
		if(player.Team=="Spy"){
			staminaDrainSpeed = 20;
			staminaRegenSpeed = 10;
		}
		else{
			staminaDrainSpeed = 15;
			staminaRegenSpeed = 10;
		}
		camStart = cam.transform.localPosition;
		strafeToggle = true;
		anim.SetBool("StrafeToggle", strafeToggle);
		photonView.RPC("toggleOtherAnims", PhotonTargets.Others, "StrafeToggle", strafeToggle);
	}

	public void Update(){
		if(!photonView.isMine){
			transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
		}
		else{
			if(Input.GetKeyUp(KeyCode.LeftControl)){
				strafeToggle = !strafeToggle;
				anim.SetBool("StrafeToggle", strafeToggle);
				photonView.RPC("toggleOtherAnims", PhotonTargets.Others, "StrafeToggle", strafeToggle);
			}
			if(Input.GetKeyDown(KeyCode.Alpha1) && !isIdleAnimating){
				playRandomIdle();
			}
			else if(Input.GetKeyUp(KeyCode.Alpha1)){
				stopRandomIdle();
			}
		}
	}

	public void FixedUpdate(){
		if(photonView.isMine){
			if((player.Team != "Spy" || !Intrigue.playerGO.GetComponent<Spy>().doingObjective) &&
				!isOut && !gameObject.GetComponent<BasePlayer>().isChatting && !isStunned && !Intrigue.finalRoundOver){
				// Stamina functionality
				doStamina();

				//Rotating Character and Gravity
				charControl();
			} else if(isOut){
				anim.SetFloat("Speed", 0f);
				anim.SetFloat("Direction", 0f);
				anim.SetBool("Run", false);
				anim.SetBool("Out", true);
				photonView.RPC("toggleOtherAnims", PhotonTargets.Others, "Out", true);
			}
			else if(player.Team == "Spy" && Intrigue.playerGO.GetComponent<Spy>().doingObjective){
				anim.SetFloat("Speed", 0f);
				anim.SetFloat("Direction", 0f);
				anim.SetBool("Run", false);
			}
		}
	}

	void charControl(){
		Vector3 moveDirection = Vector3.zero;
		//Strafe
		if(strafeToggle){
			moveDirection.x += Input.GetAxis("Horizontal") * CHARSPEED * speedMult;
		} //Rotate
		else {
			transform.Rotate(0, Input.GetAxis("Horizontal") * 90 * Time.deltaTime, 0);
		}

		anim.SetFloat("Speed", Input.GetAxis("Vertical"));
		anim.SetFloat("Direction", Input.GetAxis("Horizontal"));
		moveDirection.z += Input.GetAxis("Vertical") * CHARSPEED * speedMult;

		moveDirection = transform.TransformDirection(moveDirection);
		// For gravity
		moveDirection.y -= 1000 * Time.deltaTime;
		GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime); 
	}

	void playRandomIdle(){
		int animIndex = Random.Range(1,12);
		anim.SetBool("Idle" + animIndex, true);
		photonView.RPC("toggleIdleAnim", PhotonTargets.Others, curRandomAnim, true);
		curRandomAnim = animIndex;
	}

	void stopRandomIdle(){
		anim.SetBool("Idle" + curRandomAnim, false);
		photonView.RPC("toggleIdleAnim", PhotonTargets.Others, curRandomAnim, false);
		isIdleAnimating = false;
	}

	void doStamina(){
		bool canMove = true;
		if(player.Team == "Spy" && Intrigue.playerGO.GetComponent<Spy>().doingObjective)
			canMove = false;

		if(canMove && stamina>=1 && Input.GetKey(Settings.Sprint) && Input.GetAxis("Vertical")!=0){
			stamina-=staminaDrainSpeed*Time.deltaTime;
			canRegen = false;
			cam.transform.localPosition = camStart + new Vector3(0f,0f,2f);
			anim.SetBool("Run", Input.GetKey(Settings.Sprint));
			speedMult = Mathf.PI;
		} 
		else {
			speedMult = 1;
			cam.transform.localPosition = camStart;
			if(!canRegen){
				Invoke("StartRegen", 3);
			}
			anim.SetBool("Run", false);
			if(stamina<100 && canRegen)
				stamina+=staminaRegenSpeed*Time.deltaTime;
			if(stamina>100)
				stamina=100;
		}

		if(sprintPanel != null){
			if(stamina<100){
				NGUITools.SetActive(sprintPanel.gameObject, true);
				if(sprintSprite != null)
					sprintSprite.fillAmount = stamina/100;
			}else{
				NGUITools.SetActive(sprintPanel.gameObject, false);
			}
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if (stream.isWriting){
			// We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(Input.GetAxis("Vertical"));
			stream.SendNext(Input.GetAxis("Horizontal"));
			stream.SendNext(Input.GetKey(Settings.Sprint));
			stream.SendNext(anim.GetBool("InteractSafe"));
			stream.SendNext(anim.GetBool("InteractComp"));
			stream.SendNext(anim.GetBool("InteractServer"));
			stream.SendNext(anim.GetBool("Converse"));

		}else{
			// Network player, receive data
			this.correctPlayerPos = (Vector3) stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion) stream.ReceiveNext();
			anim.SetFloat("Speed", (float) stream.ReceiveNext());
			anim.SetFloat("Direction", (float) stream.ReceiveNext());
			anim.SetBool("Run", (bool) stream.ReceiveNext());
			anim.SetBool("InteractSafe", (bool) stream.ReceiveNext());
			anim.SetBool("InteractComp", (bool) stream.ReceiveNext());
			anim.SetBool("InteractServer",(bool) stream.ReceiveNext());
			anim.SetBool("Converse", (bool) stream.ReceiveNext());
		}
	}

	[RPC]
	void toggleIdleAnim(int animID, bool status){
		anim.SetBool("Idle" + animID, status);
	}

	[RPC]
	void toggleOtherAnims(string animName, bool status){
		anim.SetBool(animName, status);
	}

	void StartRegen(){
		canRegen = true;
	}
}
