using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

public class ConversationHotSpot : MonoBehaviour {

	private int population = 0;
	private List<GameObject> queue = new List<GameObject>();
	private Vector3[] spots = null;
	private float radius;
	private Vector3 center;
	private float slice;
	private int talkerIndex = 0;
	private float talkerTime = 5;

	public static int max = 5;

	void Start(){
		radius = GetComponent<NavMeshObstacle>().radius = max/2;
		GetComponent<SphereCollider>().radius = max*1.5f;
		center = transform.TransformPoint(GetComponent<SphereCollider>().center);
		slice = 360/max;
		spots = new Vector3[max];
		for(int i = 0; i < spots.Length; ++i){
			spots[i] = new Vector3(findX(i+1), center.y, findZ(i+1));
		}
	}

	void Update () {
		if(population == 0){
			Destroy(gameObject);
		} else {
			int i = 0;
			foreach(GameObject g in queue){
				if(g.GetComponent<BaseAI>().status != Status.Waiting){
					if(i == talkerIndex && queue.Count > 1){
						g.GetComponent<Animator>().SetBool("Converse", true);
						talkerTime -= Time.deltaTime;
					}

					if(talkerTime < 0){
						queue[talkerIndex].GetComponent<Animator>().SetBool("Converse", false);
						talkerIndex = Random.Range(0, queue.Count);
						talkerTime = 5;
					}
					g.transform.LookAt(transform.position);
				}
				++i;
			}
		}


	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Guest" &&
			other.gameObject.GetComponent<BaseAI>().destination == gameObject.transform.position){
			BaseAI script = other.gameObject.GetComponent<BaseAI>();
			script.agent.SetDestination(spots[queue.Count]);
			script.anim.SetBool("Speed", true);
			script.distFromDest = 3;
			script.status = Status.Waiting;
			++population;
			Debug.DrawLine(other.gameObject.transform.position, spots[queue.Count], Color.red, 15f, false);
			queue.Add(other.gameObject);
			// Debug.Log("Adding to mah pop");
			other.GetComponent<BaseAI>().isYourTurn = true;
		}

		//AKA Player
		if(other.gameObject.layer == 8){
			//Activate GUI that says enter conversation hotspot
		}
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "Guest" && other.GetComponent<BaseAI>().isYourTurn){
			--population;
			queue.Remove(other.gameObject);
			other.GetComponent<BaseAI>().isYourTurn = false;
		}
	}

	float findX(int index){
		return center.x + radius*Mathf.Cos(slice * index);
	}

	float findZ(int index){
		return center.z + radius*Mathf.Sin(slice * index);
	}
}
