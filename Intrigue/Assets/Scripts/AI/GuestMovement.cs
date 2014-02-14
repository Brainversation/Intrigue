using UnityEngine;
using System.Collections;

public class GuestMovement : Photon.MonoBehaviour {

	public NavMeshAgent agent;
	
	private GameObject room;
	private Vector3 finalPosition;
	private float counter = 0;
	private Animator anim;
	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	//private bool isStopped = false;
	private bool init = true;


	void Start(){
		anim = GetComponent<Animator>();
		anim.speed = 1.0f;
		room = GameObject.FindWithTag("RoomCollider");
	}

	public void Update(){
		if(!PhotonNetwork.isMasterClient){
			transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
		} else {
			if(init){
				moveGuest();
				anim.SetFloat("Speed", agent.speed);
				init = false;
			}
			if(agent.remainingDistance < 5){
				moveGuest();
				anim.SetFloat("Speed", agent.speed);
			}
		}
		Debug.DrawLine(transform.position, finalPosition, Color.red, 0.0f, false);
	}

	void moveGuest(){
		//Vector3 max;
		//Vector3 min;
		int walkRadius = 300;
		Vector3 randomDirection = Random.insideUnitSphere * walkRadius;

		randomDirection += transform.position;
		NavMeshHit hit;
		NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);

		finalPosition = hit.position;
		/*
		Collider roomCollider = room.GetComponent<BoxCollider>();
		max = roomCollider.bounds.max;
		min = roomCollider.bounds.min;

		finalPosition = new Vector3(Random.Range(min.x, max.x), 
									room.transform.position.y,
									Random.Range(min.z, max.z));
									*/

		agent.SetDestination(finalPosition);
		// Debug.Log("At end of moveGuest()");
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if (stream.isWriting){
			// We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(agent.velocity.x);
			stream.SendNext(agent.speed);

		}else{
			// Network player, receive data
			this.correctPlayerPos = (Vector3) stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion) stream.ReceiveNext();
			anim.SetFloat("Direction", (float) stream.ReceiveNext());
			anim.SetFloat("Speed", (float) stream.ReceiveNext());
		}
	}
}
