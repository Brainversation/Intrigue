using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RBS;
using BehaviorTree;

public class GuestMovement : Photon.MonoBehaviour {

	public NavMeshAgent agent;
	private Animator anim;
	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private bool init = true;
	private Task treeRoot1;
	private Task treeRoot2;
	private List<Rule> rules;
	private Status behaving = Status.False;


	void Start(){
		anim = GetComponent<Animator>();
		anim.speed = 1.0f;
		initAI();
	}

	public void Update(){
		// if(!PhotonNetwork.isMasterClient){
		// 	transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
		// 	transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
		// } else {
		// 	//Sort the list in terms of weight
		// 	rules.Sort();

		// 	for (int i = 0; i < rules.Count; i++){
		// 		Debug.Log("Testing rules");
		// 		if (rules[i].isFired()){
		// 			Debug.Log("Rule fired");
		// 			rules[i].consequence(gameObject);
		// 			break;
		// 		}
		// 	}
		// }
		if( behaving == Status.False ){
			//Sort the list in terms of weight
			rules.Sort();

			for (int i = 0; i < rules.Count; i++){
				Debug.Log("Testing rules");
				if (rules[i].isFired()){
					Debug.Log("Rule fired");
					rules[i].weight -= 5;
					behaving = rules[i].consequence(gameObject);
					break;
				}
			}
		} else if( behaving == Status.True ){
			behaving = Status.Waiting;
			Invoke("backToRule", 3);
		}
	}

	void initAI(){
		List<Task> sequenceChildren = new List<Task>();
		sequenceChildren.Add(new Run());
		sequenceChildren.Add(new Jump());
		treeRoot1 = new Sequence(sequenceChildren);

		List<Task> children1 = new List<Task>();
		children1.Add( new Run() );
		treeRoot2 = new Selector(children1);

		rules = new List<Rule>();

		Bar bar = new Bar();
		Library lib = new Library();
		Thirst thirst = new Thirst(gameObject);
		Bored bored = new Bored(gameObject);
		Party party = new Party();

		List<Condition> conditions0 = new List<Condition>();
		conditions0.Add(bar);
		conditions0.Add(thirst);
		conditions0.Add(party);

		Rule rule0 = new Rule(conditions0, treeRoot1.run);
		rule0.weight = 7;
		rules.Add(rule0);

		List<Condition> conditions1 = new List<Condition>();
		conditions1.Add(bar);
		conditions1.Add(thirst);
		conditions1.Add(party);

		Rule rule1 = new Rule(conditions1, treeRoot2.run);
		rule1.weight = 6;
		rules.Add(rule1);
	}

	void backToRule(){
		behaving = Status.False;
	}

	// public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
	// 	if (stream.isWriting){
	// 		// We own this player: send the others our data
	// 		stream.SendNext(transform.position);
	// 		stream.SendNext(transform.rotation);
	// 		stream.SendNext(agent.velocity.x);
	// 		stream.SendNext(agent.speed);
	// 		stream.SendNext(anim.GetBool("Run"));
	// 		stream.SendNext(anim.GetBool("Jump"));

	// 	}else{
	// 		// Network player, receive data
	// 		this.correctPlayerPos = (Vector3) stream.ReceiveNext();
	// 		this.correctPlayerRot = (Quaternion) stream.ReceiveNext();
	// 		anim.SetFloat("Direction", (float) stream.ReceiveNext());
	// 		anim.SetFloat("Speed", (float) stream.ReceiveNext());
	// 		anim.SetBool("Run", (bool) stream.ReceiveNext());
	// 		anim.SetBool("Jump", (bool) stream.ReceiveNext());
	// 	}
	// }
}
