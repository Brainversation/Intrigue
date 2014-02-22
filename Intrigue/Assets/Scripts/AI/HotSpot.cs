using UnityEngine;
using System.Collections;

public class HotSpot : MonoBehaviour {

	[HideInInspector] public int population = 0;

	void OnTriggerEnter(Collider other){
		if(other.tag == "Guest"){
			other.gameObject.GetComponent<BaseAI>().atDrink = true;
			++population;
		}
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "Guest"){
			other.gameObject.GetComponent<BaseAI>().atDrink = false;
			--population;
		}
	}
}
