using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HotSpot : MonoBehaviour {

	[HideInInspector] public int population = 0;
	[HideInInspector] public List<GameObject> queue = new List<GameObject>();

	void FixedUpdate(){
		if(!queue[0].GetComponent<BaseAI>().isYourTurn)
			queue[0].GetComponent<BaseAI>().isYourTurn = true;
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Guest" &&
			other.gameObject.GetComponent<BaseAI>().destination == gameObject.transform.position){
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
