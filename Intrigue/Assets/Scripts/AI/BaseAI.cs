using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RBS;
using BehaviorTree;

public class BaseAI : Photon.MonoBehaviour {

	public NavMeshAgent agent;

	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private Rule currentRule;
	private float updateWants = 5f;
	private float animationTime = 5;
	private bool stunned = false;

	protected List<Rule> rules;

	// AI info
	[HideInInspector] public Animator anim;
	[HideInInspector] public Vector3 destination;
	[HideInInspector] public AI_RoomState room;
	[HideInInspector] public Task tree = null;
	[HideInInspector] public bool isYourTurn = false;
	[HideInInspector] public Status status = Status.False;
	[HideInInspector] public float distFromDest = 5f;

	// Wants, needs, and feelings 0-100 scale
	[HideInInspector] public float thirst = 0f;
	[HideInInspector] public float bored = 0f;
	[HideInInspector] public float hunger = 0f;
	[HideInInspector] public float lonely = 0f;
	[HideInInspector] public float tired = 0f;
	[HideInInspector] public float anxiety = 0f;
	[HideInInspector] public float bladder = 0f;
	[HideInInspector] public float anger = 0f;
	[HideInInspector] public float happy = 0f;
	[HideInInspector] public float sad = 0f;
	[HideInInspector] public float toxicity = 0f;

	private bool aiTesting = false;

	void Start(){
		anim = GetComponent<Animator>();
		anim.speed = 1f;
		initAI();
		if(aiTesting){
			thirst = 0;
			bored = 60;
			hunger = 0;
			lonely = 60;
			tired = 0;
			anxiety = 0;
			bladder = 0;
		} else {
			thirst = Random.Range(0, 100);
			bored = Random.Range(0, 100);
			hunger = Random.Range(0, 100);
			lonely = Random.Range(0, 100);
			tired = Random.Range(0, 100);
			anxiety = Random.Range(0, 100);
			bladder = Random.Range(0, 100);
			anger = Random.Range(0, 100);
			happy = Random.Range(0, 100);
			sad = Random.Range(0, 100);
			toxicity = Random.Range(0, 100);
		}
	}

	public void Update(){
		if(!PhotonNetwork.isMasterClient && !aiTesting){
			transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
		} else {
			switch(status){
				case Status.False:
					//Sort the list in terms of weight
					rules.Sort();

					for (int i = 0; i < rules.Count; i++){
						if (rules[i].isFired()){
							currentRule = rules[i];
							rules[i].weight -= 15;
							status = rules[i].consequence(gameObject);
							break;
						}
					}
				break;

				case Status.True:
					status = Status.Waiting;
					Invoke("backToRule", 2.5f);
				break;

				case Status.Tree:
					if( tree.run(gameObject) == Status.True){
						Invoke("backToRule", 5f);
						tree = null;
						status = Status.Waiting;
					}
				break;

				case Status.Waiting:
					if(stunned)
						return;

					if (agent.pathStatus == NavMeshPathStatus.PathPartial ||
						agent.pathStatus == NavMeshPathStatus.PathPartial){
						Debug.Log("Path invalid or can not be reached!");
					}

					if(agent.hasPath && !agent.pathPending && agent.remainingDistance < distFromDest){
						anim.SetBool("Speed", false);
						agent.ResetPath();
						if(tree == null)
							status = Status.False;
						else
							status = Status.Tree;
					}
				break;
			}
		}
	}

	void FixedUpdate(){
		if(updateWants < 0){
			if( thirst < 100) thirst += 1f;
			if( bored < 100) bored += 1f;
			if( hunger < 100) hunger += 1f;
			if( lonely < 100) lonely += 1f;
			if( tired < 100) tired += 1f;
			if( anxiety < 100) anxiety += 1f;
			if( bladder < 100) bladder += 1f;
			if( anger < 100) anger += 1f;
			if( happy < 100) happy += 1f;
			if( sad < 100) sad += 1f;
			if( toxicity < 100) toxicity += 1f;
			updateWants = 5f;
		} else {
			updateWants -= Time.deltaTime;
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Conversation") transform.LookAt(other.transform.position);
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if(aiTesting) return;
		if (stream.isWriting){
			// We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(anim.GetBool("Speed"));

		}else{
			// Network player, receive data
			this.correctPlayerPos = (Vector3) stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion) stream.ReceiveNext();
			anim.SetBool("Speed", (bool) stream.ReceiveNext());
		}
	}

	void OnGUI(){
		if(!aiTesting) return;
		GUI.color = Color.black;
		GUILayout.BeginArea(new Rect(100, 0, 100, 200));
			GUILayout.Label( "thirst " + thirst);
			GUILayout.Label( "bored " + bored);
			GUILayout.Label( "hunger " + hunger);
			GUILayout.Label( "lonely " + lonely);
			GUILayout.Label( "tired " + tired);
			GUILayout.Label( "anxiety " + anxiety);
			GUILayout.Label( "bladder " + bladder);
		GUILayout.EndArea();
	}

	void initAI(){
		rules = new List<Rule>();

		Rule rule0 = new WantToGetDrink(gameObject);
		rule0.weight = 7;
		rules.Add(rule0);

		Rule rule1 = new WantToConverse(gameObject);
		rule1.weight = 4;
		rules.Add(rule1);


		// Rule rule2 = new NeedToUseRestroom(gameObject);
		// rule2.weight = 10;
		// rules.Add(rule2);

		// Rule rule3 = new WantToMoveRoom(gameObject);
		// rule3.weight = 1;
		// rules.Add(rule3);

		// Rule rule4 = new WantToWanderRoom(gameObject);
		// rule4.weight = 1;
		// rules.Add(rule4);
	}

	void backToRule(){
		// Debug.Log("Back to rule");
		if(currentRule.antiConsequence != null)
			currentRule.antiConsequence();
		if(!agent.hasPath) status = Status.False;
	}

	public void addDrink(){
		// Debug.Log("adding Drink");
		this.thirst -= 10;
		this.bladder += 5;
	}

	[RPC]
	void isStunned(){
		if(PhotonNetwork.isMasterClient){
			Debug.Log("Guest STUNNED");
			stunned = true;
			status = Status.Waiting;
			anim.SetBool("Speed", false);
			agent.ResetPath();
			CancelInvoke();
			Invoke("backToRule", 5);
		}
	}
}
