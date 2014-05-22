using UnityEngine;
using System.Collections;
using BehaviorTree;

public class RelaxHotSpot : MonoBehaviour {

	[HideInInspector] public bool occupied = false;
	[HideInInspector] private GameObject currGuest;

	void OnTriggerEnter(Collider other){
		if (other.tag == "Guest" && 
			other.gameObject.GetComponent<BaseAI>().destination == gameObject.transform.position){
			currGuest = other.gameObject;
			//other.gameObject.GetComponent<BaseAI>().status = Status.Waiting;
			occupied = true;
		}
	}

	protected virtual void OnTriggerExit(Collider other){
		if(other.tag == "Guest" && other.gameObject == currGuest){				
			occupied = false;
		}
	}

}
