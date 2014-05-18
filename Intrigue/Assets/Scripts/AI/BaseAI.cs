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
	private bool stunInstantiated = false;

	protected List<Rule> rules;

	//Audio Sources
	public Animator animator;
	public AudioSource footstepL;
	public AudioSource footstepR;

	//Particle Effects
	public GameObject stunPrefab;
	private GameObject currentStunEffect;

	// AI info
	[HideInInspector] public Animator anim;
	[HideInInspector] public Vector3 destination;
	[HideInInspector] public AI_RoomState room;
	[HideInInspector] public Task tree = null;
	[HideInInspector] public bool isYourTurn = false;
	[HideInInspector] public Status status = Status.False;
	[HideInInspector] public float distFromDest = 5f;
	[HideInInspector] public bool stunned = false;

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
	[HideInInspector] public float timeInRoom = 0f;

	//Other AI Characteristics
	[HideInInspector] public bool smoker = false;
	[HideInInspector] public bool hasDrink = false;
	[HideInInspector] public bool inConvo = false;

	private bool aiTesting = false;

	void Start(){
		anim = GetComponent<Animator>();
		GetComponent<NavMeshAgent>().speed = NetworkCharacter.CHARSPEED;
		initAI();
	}

	public void Update(){
		//Debug.Log(status);
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
							// Debug.Log(rules[i].ToString());
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
						agent.pathStatus == NavMeshPathStatus.PathInvalid){
						//Debug.Log("Path invalid or can not be reached!");
						anim.SetBool("Speed", false);
						agent.ResetPath();
						tree = null;
						CancelInvoke();
						Invoke("backToRule", 5f);
					}

					if(agent.hasPath && !agent.pathPending && agent.remainingDistance < distFromDest){
						anim.SetBool("Speed", false);
						agent.ResetPath();
						if(tree == null){
							status = Status.False;
							backToRule();
						} else {
							status = Status.Tree;
						}
					}
				break;
			}
			Debug.Log(status);
		}

		if(!aiTesting){
			//Left foot position
			Vector3 leftFootT = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
			Quaternion leftFootQ = animator.GetIKRotation(AvatarIKGoal.LeftFoot);
			Vector3 leftFootH = new Vector3(0, -animator.leftFeetBottomHeight, 0);
			Vector3 posL = leftFootT + leftFootQ * leftFootH;
			//Right foot position
			Vector3 rightFootT = animator.GetIKPosition(AvatarIKGoal.RightFoot);
			Quaternion rightFootQ = animator.GetIKRotation(AvatarIKGoal.RightFoot);
			Vector3 rightFootH = new Vector3(0, -animator.rightFeetBottomHeight, 0);
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

	}

	void FixedUpdate(){
		if(PhotonNetwork.isMasterClient && updateWants < 0){
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
			if(timeInRoom <= 10){
				timeInRoom +=1f;
			}
			updateWants = 5f;
		} else {
			updateWants -= Time.deltaTime;
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
		//rules.Add( new WantToGetDrink(gameObject) );
		//rules.Add( new WantToConverse(gameObject) );
		rules.Add( new FindRoom(gameObject) );
		//rules.Add( new WantToWanderRoom(gameObject) );
		rules.Add( new WantToMoveRoom(gameObject) );
		//rules.Add( new NeedToUseRestroom(gameObject) );
		//rules.Add( new AdmireArt(gameObject) );
		//<-------- Rules To Add ------->
		// Relax
		// LetOffSteam
		// Smoke
		// ReadPoetry
		// DoIdle
		if(aiTesting){
			thirst = 0;
			bored = 51;
			hunger = 0;
			lonely = 0;
			tired = 0;
			anxiety = 0;
			bladder = 51;
			anger = 0;
			happy = 0;
			sad = 0;
			toxicity = 0;
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
		}

		if(Random.Range(0,100) > 50){
			smoker = true;
		}
		else{
			smoker = false;
		}
	}

	void backToRule(){
		if(!aiTesting){
			photonView.RPC("updateStunPS", PhotonTargets.All, false);
		}
		stunInstantiated = false;
		stunned = false;
		if(currentRule != null && currentRule.antiConsequence != null)
			currentRule.antiConsequence();
		if(!agent.hasPath) status = Status.False;
	}

	public void addDrink(){
		// Debug.Log("adding Drink");
		this.thirst -= 10;
		this.bladder += 5;
		this.hasDrink = true;
	}

	[RPC]
	void isStunned(){
		if(PhotonNetwork.isMasterClient){
			stunned = true;
			status = Status.Waiting;
			anim.SetBool("Speed", false);
			agent.ResetPath();
			CancelInvoke();
			Invoke("backToRule", 5);
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
		if(aiTesting) return;
		if (stream.isWriting){
			// We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(anim.GetBool("Speed"));
			stream.SendNext(anim.GetBool("Drink"));
			stream.SendNext(anim.GetBool("Converse"));

		}else{
			// Network player, receive data
			this.correctPlayerPos = (Vector3) stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion) stream.ReceiveNext();
			anim.SetBool("Speed", (bool) stream.ReceiveNext());
			anim.SetBool("Drink", (bool) stream.ReceiveNext());
			anim.SetBool("Converse", (bool) stream.ReceiveNext());
		}
	}
}
