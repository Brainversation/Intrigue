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


	void Start(){
		anim = GetComponent<Animator>();
		anim.speed = 1.5f;
		room = GameObject.FindWithTag("RoomCollider");
	}

	public void Update(){
		if(!photonView.isMine){
			transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
		} else {

			// Debug.Log("Inside Guest Update");
			if(counter > 0.5f){
				moveGuest();
				// Debug.Log("Inside Guest if");
				counter = 0;
			}
			else{
				counter += Time.deltaTime;
			}
		}
	}

	public void FixedUpdate(){
			anim.SetFloat("Speed", agent.velocity.z);
			anim.SetFloat("Direction", agent.velocity.x);
	}

	void moveGuest(){
		Vector3 max;
		Vector3 min;

		Collider roomCollider = room.GetComponent<BoxCollider>();
		max = roomCollider.bounds.max;
		min = roomCollider.bounds.min;

		finalPosition = new Vector3(Random.Range(min.x, max.x), 
									room.transform.position.y,
									Random.Range(min.z, max.z));

		agent.SetDestination(finalPosition);
		// Debug.Log("At end of moveGuest()");
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if (stream.isWriting){
			// We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(agent.velocity.x);
			stream.SendNext(agent.velocity.z);

		}else{
			// Network player, receive data
			this.correctPlayerPos = (Vector3) stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion) stream.ReceiveNext();
			anim.SetFloat("Direction", (float) stream.ReceiveNext());
			anim.SetFloat("Speed", (float) stream.ReceiveNext());
		}
	}
}
