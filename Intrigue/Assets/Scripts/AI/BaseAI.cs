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
	private float updateWants = 5f;

	protected List<Rule> rules;
	protected Status behaving = Status.False;

	// AI info
	[HideInInspector] public Vector3 destination;
	[HideInInspector] public AIRoomInfo room;
	[HideInInspector] public bool atDrink = false;
	[HideInInspector] public bool isYourTurn = false;

	// Wants, needs, and feelings 0-100 scale
	[HideInInspector] public float thirst = 0f;
	[HideInInspector] public float bored = 0f;
	[HideInInspector] public float hunger = 0f;
	[HideInInspector] public float lonely = 0f;
	[HideInInspector] public float tired = 0f;
	[HideInInspector] public float anxiety = 0f;
	[HideInInspector] public float bladder = 0f;

	void Start(){
		anim = GetComponent<Animator>();
		anim.speed = 1f;
		initAI();
		bored = 51;
		thirst = 51;
		lonely = 51;
		bladder = 40;
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
				if (rules[i].isFired()){
					currentRule = rules[i];
					rules[i].weight -= 15;
					behaving = rules[i].consequence(gameObject);
					break;
				}
			}
		} else if( behaving == Status.True ){
			behaving = Status.Waiting;
			Invoke("backToRule", 2.5f);
		}
	}

	void FixedUpdate(){
		if(updateWants < 0){
			if( thirst <= 100) thirst += 1f;
			if( bored <= 100) bored += 1f;
			if( hunger <= 100) hunger += 1f;
			if( lonely <= 100) lonely += 1f;
			if( tired <= 100) tired += 1f;
			if( anxiety <= 100) anxiety += 1f;
			if( bladder <= 100) bladder += 1f;
			updateWants = 5f;
		} else {
			updateWants -= Time.deltaTime;
		}
	}

	void OnGUI(){
		GUILayout.Label( "thirst " + thirst);
		GUILayout.Label( "bored " + bored);
		GUILayout.Label( "hunger " + hunger);
		GUILayout.Label( "lonely " + lonely);
		GUILayout.Label( "tired " + tired);
		GUILayout.Label( "anxiety " + anxiety);
		GUILayout.Label( "bladder " + bladder);
	}

	void initAI(){
		rules = new List<Rule>();

		Rule rule0 = new WantToGetDrink(gameObject);
		rule0.weight = 7;
		rules.Add(rule0);

		Rule rule2 = new ReadyToDrink(gameObject);
		rule2.weight = 5;
		rules.Add(rule2);

		Rule rule4 = new WantToConverse(gameObject);
		rule4.weight = 4;
		rules.Add(rule4);

		Rule rule5 = new NeedToUseRestroom(gameObject);
		rule5.weight = 10;
		rules.Add(rule5);
	}

	void backToRule(){
		Debug.Log("Back to rule");
		if(currentRule.antiConsequence != null)
			currentRule.antiConsequence();
		behaving = Status.False;
	}

	public void addDrink(){
		Debug.Log("adding Drink");
	}
}
