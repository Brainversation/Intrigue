using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

public class HotSpot : MonoBehaviour {

	[HideInInspector] public int population = 0;
	[HideInInspector] public List<GameObject> queue = new List<GameObject>();
	public Transform actionDestination;

	void Update(){
		if(queue.Count >= 1 && !queue[0].GetComponent<BaseAI>().isYourTurn){
			queue[0].GetComponent<BaseAI>().isYourTurn = true;
			queue[0].GetComponent<BaseAI>().destination = actionDestination.position;
			queue[0].GetComponent<BaseAI>().distFromDest = 1f;
			queue[0].GetComponent<NavMeshAgent>().SetDestination(actionDestination.position);
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Guest" &&
			other.gameObject.GetComponent<BaseAI>().destination == gameObject.transform.position){
			other.gameObject.GetComponent<BaseAI>().status = Status.Waiting;
			queue.Add(other.gameObject);
			++population;
		}
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "Guest"){
			queue.Remove(other.gameObject);
			other.gameObject.GetComponent<BaseAI>().isYourTurn = false;
			--population;
		}
	}
}
