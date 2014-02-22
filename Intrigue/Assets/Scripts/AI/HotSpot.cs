using UnityEngine;
using System.Collections;

public class HotSpot : MonoBehaviour {
	void OnTriggerEnter(Collider other){
		if(other.tag == "Guest")
			other.gameObject.GetComponent<BaseAI>().atDrink = true;
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "Guest")
			other.gameObject.GetComponent<BaseAI>().atDrink = false;
	}
}
