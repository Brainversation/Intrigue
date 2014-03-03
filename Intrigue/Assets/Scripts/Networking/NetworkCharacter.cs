using UnityEngine;
using System.Collections;

public class NetworkCharacter : Photon.MonoBehaviour {

	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private Animator anim;
	private Spy spyRef;
	private Player player;
	public bool isOut;

	void Start() {
		//Get References to Animator and Collider
		anim = GetComponent<Animator>();
		anim.speed = 1.5f;
		spyRef = gameObject.GetComponent<Spy>();
		player = GameObject.Find("Player").GetComponent<Player>();

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
			anim.SetBool("Out", false);
			if(player.Team=="Spy"){
				if(spyRef.doingObjective){
					if(spyRef.objectiveType=="Safe"){
						anim.SetBool("InteractSafe",true);
					}
					else if(spyRef.objectiveType=="Computer"){
						anim.SetBool("InteractComp",true);
					}
				}
				else{
					anim.SetBool("InteractSafe",false);
					anim.SetBool("InteractComp",false);
				}
			}
		}
		else if(photonView.isMine && isOut){
			anim.SetFloat("Speed", 0f);
			anim.SetFloat("Direction", 0f);
			anim.SetBool("Run", false);
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
			stream.SendNext(anim.GetBool("InteractSafe"));
			stream.SendNext(anim.GetBool("InteractComp"));

		}else{
			// Network player, receive data
			this.correctPlayerPos = (Vector3) stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion) stream.ReceiveNext();
			anim.SetFloat("Speed", (float) stream.ReceiveNext());
			anim.SetFloat("Direction", (float) stream.ReceiveNext());
			anim.SetBool("Run", (bool) stream.ReceiveNext());
			anim.SetBool("InteractSafe", (bool) stream.ReceiveNext());
			anim.SetBool("InteractComp", (bool) stream.ReceiveNext());
		}
	}
}