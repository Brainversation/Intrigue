/**************************************************************************
 **                                                                       *
 **                COPYRIGHT AND CONFIDENTIALITY NOTICE                   *
 **                                                                       *
 **    Copyright (c) 2014 Hacksaw Games. All rights reserved.             *
 **                                                                       *
 **    This software contains information confidential and proprietary    *
 **    to Hacksaw Games. It shall not be reproduced in whole or in        *
 **    part, or transferred to other documents, or disclosed to third     *
 **    parties, or used for any purpose other than that for which it was  *
 **    obtained, without the prior written consent of Hacksaw Games.      *
 **                                                                       *
 **************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RBS;
using BehaviorTree;

public class BaseAI : Photon.MonoBehaviour {

	private Vector3 nextPlayerPos;
	private Quaternion nextPlayerRot;
	private Rule currentRule;
	private float updateWants = 5f;
	private bool stunInstantiated = false;
	private GameObject currentStunEffect;

	private static bool resetGameObjects = true;

	protected static List<Rule> rules = null;

	//Audio Sources
	public AudioSource footstepL;
	public AudioSource footstepR;

	public NavMeshAgent agent;
	public Animator anim;
	//Particle Effects
	public GameObject stunPrefab;


	// AI info
	[HideInInspector] public Vector3 destination;
	[HideInInspector] public AI_RoomState room;
	[HideInInspector] public Task tree = null;
	public Status status = Status.False;
	[HideInInspector] public bool recentlyStunned = false;
	[HideInInspector] public float distFromDest = 5f;
	[HideInInspector] public float convoTime = 5f;
	[HideInInspector] public float timeInRoom = 20f;
	[HideInInspector] public float ruleTime = 0f;

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

	//Other AI Characteristics
	[HideInInspector] public bool smoker = false;
	[HideInInspector] public bool hasDrink = false;
	[HideInInspector] public bool inConvo = false;
	[HideInInspector] public bool isYourTurn = false;
	[HideInInspector] public bool inQueue = false;
	[HideInInspector] public bool stunned = false;

	// Used for offline testing
	public static bool aiTesting = false;
	public static List<GameObject> aiTestingList;
	public int AIID;

	void Awake(){
		if(aiTesting){
			if(aiTestingList == null)
				aiTestingList = new List<GameObject>();
			AIID = aiTestingList.Count;
			aiTestingList.Add(gameObject);
		}

		if(resetGameObjects){
			WantToMoveRoom.rooms = GameObject.FindGameObjectsWithTag("Room");
			WantToGetDrink.drinkLocations = GameObject.FindGameObjectsWithTag("Drink");
			NeedToUseRestroom.bathroomLocations = GameObject.FindGameObjectsWithTag("RestRoom");
			FindRoom.rooms = WantToMoveRoom.rooms;
		}
	}

	// Initializes all fields
	void Start(){
		agent.speed = NetworkCharacter.CHARSPEED;
		agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
		initAI();
	}

	public void Update(){
		// Only sets position and rotation when not master client
		if(!PhotonNetwork.isMasterClient && !BaseAI.aiTesting){
			transform.position = Vector3.Lerp(transform.position, this.nextPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp(transform.rotation, this.nextPlayerRot, Time.deltaTime * 5);
		} else {
			// Animation is based on the velocity of the Model
			anim.SetFloat("Speed", agent.velocity.magnitude);
			// Status system that updates the AI appropriately
			switch(status){
				case Status.False:
					//Sort the list in terms of weight
					rules.Sort();
					for (int i = 0; i < rules.Count; i++){
						if (rules[i].isFired(gameObject)){
							currentRule = rules[i];
							rules[i].weight -= 15;
							status = rules[i].consequence(gameObject);
							ruleTime = 2.5f;
							// Debug.Log(currentRule);
							break;
						}
					}
				break;

				case Status.True:
					if(ruleTime > 0){
						status = Status.Waiting;
					} else {
						ruleTime -= Time.deltaTime;
					}
				break;

				case Status.Tree:
					if(tree.run(gameObject) == Status.True){
						doAnti();
						tree = null;
						status = Status.Waiting;
					}
				break;

				case Status.Waiting:
					if(stunned){
						return;
					} else if (agent.pathStatus == NavMeshPathStatus.PathPartial ||
						agent.pathStatus == NavMeshPathStatus.PathInvalid){
						agent.ResetPath();
						tree = null;
						doAnti();
						status = Status.False;
					} else if(agent.hasPath){
						if(agent.remainingDistance < distFromDest){
							agent.ResetPath();
							if(tree != null){
								status = Status.Tree;
							} else if(inConvo){
								status = Status.Convo;
								convoTime = 5f;
							} else if(!inQueue){
								status = Status.False;
								doAnti();
							}
						}
					} else if(!inQueue){
						status = Status.False;
						doAnti();
					}
				break;

				case Status.Convo:
					if(convoTime < 0){
						status = Status.False;
					} else {
						convoTime -= Time.deltaTime;
					}
				break;
			}
		}

		//Left foot position
		Vector3 leftFootT = anim.GetIKPosition(AvatarIKGoal.LeftFoot);
		Quaternion leftFootQ = anim.GetIKRotation(AvatarIKGoal.LeftFoot);
		Vector3 leftFootH = new Vector3(0, -anim.leftFeetBottomHeight, 0);
		Vector3 posL = leftFootT + leftFootQ * leftFootH;
		//Right foot position
		Vector3 rightFootT = anim.GetIKPosition(AvatarIKGoal.RightFoot);
		Quaternion rightFootQ = anim.GetIKRotation(AvatarIKGoal.RightFoot);
		Vector3 rightFootH = new Vector3(0, -anim.rightFeetBottomHeight, 0);
		Vector3 posR = rightFootT + rightFootQ * rightFootH;

		float rHeight = posR.y - transform.position.y;
		float lHeight = posL.y - transform.position.y;

		if(rHeight > 0f){
			if(!footstepR.isPlaying){
				footstepR.Play();
			}
		}
		if(lHeight > 0f){
			if(!footstepL.isPlaying){
				footstepL.Play();
			}
		}

	}

	void FixedUpdate(){
		// Updates the AI's stats
		if((PhotonNetwork.isMasterClient || BaseAI.aiTesting) && updateWants < 0){
			if( thirst < 100) thirst += 2f;
			if( bored < 100) bored += 2f;
			if( hunger < 100) hunger += 2f;
			if( lonely < 100) lonely += 2f;
			if( tired < 100) tired += 2f;
			if( anxiety < 100) anxiety += 2f;
			if( bladder < 100) bladder += 2f;
			if( anger < 100) anger += 2f;
			if( happy < 100) happy += 2f;
			if( sad < 100) sad += 2f;
			if( toxicity < 100) toxicity += 2f;
			if(timeInRoom > 0){
				timeInRoom -=5f;
			}
			updateWants = 5f;
		} else {
			updateWants -= Time.deltaTime;
		}
	}

#if (UNITY_EDITOR)
	void OnGUI(){
		if(!BaseAI.aiTesting) return;
		GUI.color = Color.black;
		GUILayout.BeginArea(new Rect(100*AIID, 0, 100, 300));
			GUILayout.Label( "thirst " + thirst);
			GUILayout.Label( "bored " + bored);
			GUILayout.Label( "hunger " + hunger);
			GUILayout.Label( "lonely " + lonely);
			GUILayout.Label( "tired " + tired);
			GUILayout.Label( "anxiety " + anxiety);
			GUILayout.Label( "bladder " + bladder);
			GUILayout.Label( status.ToString() );
			if(currentRule != null)
				GUILayout.Label( currentRule.ToString() );
		GUILayout.EndArea();
	}
#endif
	
	// Sets rules and stats
	void initAI(){
		if(resetGameObjects){
			rules = new List<Rule>();
			rules.Add( new FindRoom() );
			rules.Add( new WantToGetDrink() );
			rules.Add( new WantToConverse() );
			rules.Add( new WantToWanderRoom() );
			rules.Add( new WantToMoveRoom() );
			rules.Add( new NeedToUseRestroom() );
			rules.Add( new AdmireArt() );
			rules.Add( new Smoke() );
			rules.Add( new DoIdle() );
			BaseAI.resetGameObjects = false;
			Invoke("resetGO", 10);
		}
		//<-------- Rules To Add ------->
		// Relax
		// LetOffSteam
		// ReadPoetry
		if(BaseAI.aiTesting){
			thirst = 0;
			bored = 0;
			hunger = 0;
			lonely = 0;
			tired = 0;
			anxiety = 0;
			bladder = 61;
			anger = 0;
			happy = 0;
			sad = 0;
			toxicity = 0;
			smoker = true;
		} else {
			thirst = Random.Range(20, 100);
			bored = Random.Range(20, 100);
			hunger = Random.Range(20, 100);
			lonely = Random.Range(20, 100);
			tired = Random.Range(20, 100);
			anxiety = Random.Range(20, 100);
			bladder = Random.Range(20, 100);
			anger = Random.Range(20, 100);
			happy = Random.Range(20, 100);
			sad = Random.Range(20, 100);
			toxicity = Random.Range(20, 100);
			smoker = ((Random.Range(0,100) > 50) ?  true : false);
		}
	}

	// Used so rules do not fire to quickly and so we can have anti-consequences
	void doAnti(){
		if(currentRule != null && currentRule.antiConsequence != null)
			currentRule.antiConsequence(gameObject);
	}

	void finishStun(){
		stunInstantiated = false;
		stunned = false;
		recentlyStunned = true;
		Invoke("resetRecentlyStunned", 5);
		if(!BaseAI.aiTesting){
			photonView.RPC("updateStunPS", PhotonTargets.All, false);
		}
	}

	void resetRecentlyStunned(){
		recentlyStunned = false;
	}

	public void addDrink(){
		this.thirst -= 10;
		this.bladder += 5;
		this.hasDrink = true;
	}

	public void resetGO(){
		BaseAI.resetGameObjects = true;
	}

	[RPC]
	void stunAI(){
		if(PhotonNetwork.isMasterClient){
			stunned = true;
			status = Status.Waiting;
			agent.ResetPath();
			CancelInvoke();
			Invoke("finishStun", 5);
			if(!stunInstantiated){
				photonView.RPC("updateStunPS", PhotonTargets.All, true);
				stunInstantiated = true;
			}
		}
	}

	[RPC]
	void updateStunPS(bool creating){
		if(creating){
			currentStunEffect = Instantiate(stunPrefab, transform.position, transform.rotation) as GameObject;
			currentStunEffect.transform.parent = transform;
			currentStunEffect.transform.localPosition = new Vector3(0, 13.25f, 0);
		}
		else{
			Destroy(currentStunEffect);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if(BaseAI.aiTesting) return;
		if (stream.isWriting){
			// We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(anim.GetFloat("Speed"));
			stream.SendNext(anim.GetBool("Drink"));
			stream.SendNext(anim.GetBool("Converse"));
			stream.SendNext(anim.GetBool("Smoking"));

		}else{
			// Network player, receive data
			this.nextPlayerPos = (Vector3) stream.ReceiveNext();
			this.nextPlayerRot = (Quaternion) stream.ReceiveNext();
			anim.SetFloat("Speed", (float) stream.ReceiveNext());
			anim.SetBool("Drink", (bool) stream.ReceiveNext());
			anim.SetBool("Converse", (bool) stream.ReceiveNext());
			anim.SetBool("Smoking", (bool) stream.ReceiveNext());
		}
	}
}
