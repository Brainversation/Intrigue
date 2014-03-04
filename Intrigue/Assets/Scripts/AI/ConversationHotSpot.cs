using UnityEngine;
using System.Collections;

public class ConversationHotSpot : MonoBehaviour {

	[HideInInspector] public int population = 0;

	void Update () {
		if(population == 0){
			Debug.Log("Killin mahself");
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Guest" &&
			other.gameObject.GetComponent<BaseAI>().destination == gameObject.transform.position){
			++population;
			Debug.Log("Adding to mah pop");
			other.GetComponent<BaseAI>().isYourTurn = true;
		}
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "Guest" && other.GetComponent<BaseAI>().isYourTurn){
			--population;
			other.GetComponent<BaseAI>().isYourTurn = false;
		}
	}
}
