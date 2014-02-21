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
	private Rule currentRule;

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

		if(agent.hasPath && agent.remainingDistance < 1f){
		Debug.Log("Running bool" + anim.GetBool("Run"));
			anim.SetFloat("Speed", 0f);
			agent.ResetPath();
			behaving = Status.False;
		}

		if( behaving == Status.False ){
			//Sort the list in terms of weight
			rules.Sort();

			for (int i = 0; i < rules.Count; i++){
				// Debug.Log("Testing rules");
				if (rules[i].isFired()){
					// Debug.Log("Rule fired");
					currentRule = rules[i];
					rules[i].weight -= 5;
					behaving = rules[i].consequence(gameObject);
					break;
				}
			}
		} else if( behaving == Status.True ){
			behaving = Status.Waiting;
			if(!agent.hasPath) Invoke("backToRule", 3);
		}
	}

	void initAI(){
		behaveRoots = new List<Task>();
		behaveRoots.Add( new JumpGap() );

		rules = new List<Rule>();

		Rule rule0 = new WantToGoToBar(gameObject);
		rule0.weight = 7;
		rules.Add(rule0);

		Rule rule1 = new GoToDestination(gameObject);
		rule1.weight = 6;
		rules.Add(rule1);

		Rule rule2 = new DoRunJ(gameObject);
		rule2.consequence = behaveRoots[0].run;
		rule2.weight = 5;
		rules.Add(rule2);

		Rule rule3 = new DoIdle(gameObject);
		rule3.weight = 4;
		rules.Add(rule3);
	}

	void backToRule(){
		// Debug.Log("Back to rule");
		if(currentRule.antiConsequence != null)
			currentRule.antiConsequence();
		behaving = Status.False;
	}
}
