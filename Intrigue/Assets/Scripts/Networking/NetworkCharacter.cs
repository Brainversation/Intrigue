﻿using UnityEngine;
using System.Collections;

public class NetworkCharacter : Photon.MonoBehaviour {

	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private Animator anim;
	private GUIStyle style = new GUIStyle();

	public bool isOut;

	void Start() {
		//Get References to Animator and Collider
		anim = GetComponent<Animator>();
		anim.speed = 1.5f;
		this.style.fontSize = 40;
		this.style.normal.textColor = Color.red;

	}

	public void Update(){
		if(!photonView.isMine){
			transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
		}
	}

	public void FixedUpdate(){
		if(photonView.isMine && !isOut){
			anim.SetFloat("Speed", Input.GetAxis("Vertical"));
			anim.SetFloat("Direction", Input.GetAxis("Horizontal"));
			anim.SetBool("Run", Input.GetKey("left shift"));
			anim.SetBool("Interact", Input.GetKey("e"));
			anim.SetBool("Out", false);
		}
		else if(photonView.isMine && isOut){
			anim.SetFloat("Speed", 0f);
			anim.SetFloat("Direction", 0f);
			anim.SetBool("Run", false);
			anim.SetBool("Interact", false);
			anim.SetBool("Out", true);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if (stream.isWriting){
			// We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(Input.GetAxis("Vertical"));
			stream.SendNext(Input.GetAxis("Horizontal"));
			stream.SendNext(Input.GetKey("left shift"));
			stream.SendNext(Input.GetKey("e"));

		}else{
			// Network player, receive data
			this.correctPlayerPos = (Vector3) stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion) stream.ReceiveNext();
			anim.SetFloat("Speed", (float) stream.ReceiveNext());
			anim.SetFloat("Direction", (float) stream.ReceiveNext());
			anim.SetBool("Run", (bool) stream.ReceiveNext());
			anim.SetBool("Interact", (bool) stream.ReceiveNext());
		}
	}

	void OnGUI(){
		if(isOut)
			GUI.Label(new Rect((Screen.width/2)-100,(Screen.height/2)-50,200,100),"YOU ARE OUT", style);
	}
}