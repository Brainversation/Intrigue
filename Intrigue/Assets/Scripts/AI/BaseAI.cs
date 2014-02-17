using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RBS;
using BehaviorTree;

public class BaseAI : Photon.MonoBehaviour {

	public NavMeshAgent agent;

	private Animator anim;
	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;

	protected List<Task> behaveRoots;
	protected List<Rule> rules;

	protected Status behaving = Status.False;

	// AI info
	[HideInInspector] public Vector3 destination;

	// Wants, needs, and feelings 0-100 scale
	[HideInInspector] public int thirst;
	[HideInInspector] public int bored;
	[HideInInspector] public int hunger;
	[HideInInspector] public int lonely;
	[HideInInspector] public int tired;
	[HideInInspector] public int anxiety;
	[HideInInspector] public int bladder;

	void Start(){
		anim = GetComponent<Animator>();
		anim.speed = 1f;
		initAI();
		bored = 66;
		thirst = 67;
	}

	public void Update(){
		// if(!PhotonNetwork.isMasterClient){
		// 	transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
		// 	transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
		// } else {
			// Do updating stuff
		// }

		if(agent.hasPath && agent.remainingDistance < .5f){
			Debug.Log("HERE");
			anim.SetFloat("Speed", 0f);
			behaving = Status.False;
		}

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
			if(!agent.hasPath) Invoke("backToRule", 1);
		}
	}

	void initAI(){
		behaveRoots = new List<Task>();
		List<Task> sequenceChildren = new List<Task>();
		sequenceChildren.Add(new Run());
		sequenceChildren.Add(new Jump());
		behaveRoots.Add( new Sequence(sequenceChildren) );

		List<Task> children1 = new List<Task>();
		children1.Add( new Run() );
		behaveRoots.Add( new Selector(children1) );

		rules = new List<Rule>();

		Bar bar = new Bar();
		Library lib = new Library();
		Thirst thirst = new Thirst(gameObject);
		Bored boredCon = new Bored(gameObject);
		Party party = new Party();

		Rule rule0 = new WantToGoToBar(gameObject);
		rule0.weight = 7;
		rules.Add(rule0);

		Rule rule1 = new GoToDestination(gameObject);
		rule1.weight = 6;
		rules.Add(rule1);
	}

	void backToRule(){
		behaving = Status.False;
	}
}
