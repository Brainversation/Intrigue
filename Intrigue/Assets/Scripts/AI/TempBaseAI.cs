using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TempRBS;
using BehaviorTree;

public class TempBaseAI : Photon.MonoBehaviour {

	public NavMeshAgent agent;

	private Animator anim;
	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;

	//private List<Task> behaveRoots;
	protected List<Rule> rules;

	protected Status behaving = Status.False;

	// AI info
	[HideInInspector] public Vector3 destination;

	// Wants, needs, and feelings 0-100 scale
	[HideInInspector] public int thirst;
	[HideInInspector] public int bored = 0;
	[HideInInspector] public int hunger;
	[HideInInspector] public int lonely;
	[HideInInspector] public int tired;
	[HideInInspector] public int anxiety;
	[HideInInspector] public int bladder;


	[HideInInspector] public float timeInRoom = 11f;

	//Current Room and various attributes that describe it
	//See AI_RoomState for further info
	[HideInInspector] public GameObject room;

	void Start(){
		//room = null;

		anim = GetComponent<Animator>();
		anim.speed = 1f;
		initAI();
		bored = 66;
		thirst = 67;
		timeInRoom = 11f;
	}

	public void Update(){
		// if(!PhotonNetwork.isMasterClient){
		// 	transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
		// 	transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
		// } else {
			// Do updating stuff
		// }

		if(agent.hasPath && agent.remainingDistance < 5f){
			anim.SetFloat("Speed", 0f);
			agent.ResetPath();
			behaving = Status.False;
		}

		if( behaving == Status.False ){
			//Sort the list in terms of weight
			rules.Sort();

			for (int i = 0; i < rules.Count; i++){
				//Debug.Log("Testing rules");
				if (rules[i].isFired()){
					//Debug.Log("Rule fired");
					rules[i].weight -= 5;
					behaving = rules[i].consequence(gameObject);
					break;
				}
			}
		} else if( behaving == Status.True ){
			//behaving = Status.Waiting;
			if(!agent.hasPath) Invoke("backToRule", 3);
		}

		timeInRoom+= Time.deltaTime;
		//Debug.Log(destination);
		//Debug.Log(room.name);
	}

	void initAI(){
		//behaveRoots = new List<Task>();
		//behaveRoots.Add( new JumpGap() );

		rules = new List<Rule>();

		//Rule rule0 = new WantToGoToRoom();
		//rule0.weight = 7;
		//rules.Add(rule0);

		Rule rule0 = new WantToRandRoom(gameObject);
		rule0.weight = 5;
		rules.Add(rule0);

		Rule rule1 = new TempGoToDestination(gameObject);
		rule1.weight = 6;
		rules.Add(rule1);

		Rule rule2 = new WantToInitRoom(gameObject);
		rule2.weight = 8;
		rules.Add(rule2);

		Rule rule3 = new WantToWanderRoom(gameObject);
		rule3.weight = 7;
		rules.Add(rule3);

		/*
		Rule rule2 = new Rule();
		rule2.addCondition(new Party());
		rule2.consequence = behaveRoots[0].run;
		rule2.weight = 5;
		rules.Add(rule2);
		*/
		
	}

	void backToRule(){
		//Debug.Log("Back to rule");
		behaving = Status.False;
	}
}