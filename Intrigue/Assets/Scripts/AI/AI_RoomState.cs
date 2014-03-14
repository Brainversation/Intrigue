using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_RoomState : MonoBehaviour{

	public string roomName = "";

	[HideInInspector] public List<GameObject> conversers;
	[HideInInspector] public int population;
	[HideInInspector] public Vector3 drinkLocation;
	[HideInInspector] public Vector3 converseLocation;
	[HideInInspector] public Vector3 restroomLocation;
	[HideInInspector] public GameObject me;

	public void Start(){
		roomName = gameObject.name;
		population = 0;
		me = gameObject;
		conversers = new List<GameObject>();
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Guest"){
			other.gameObject.GetComponent<BaseAI>().room = this;
			other.gameObject.GetComponent<BaseAI>().anxiety += population * 2;
			population++;
		}

		if(other.tag == "Guest" || other.tag == "Player") population++;

		if(other.tag == "Drink"){
			drinkLocation = other.transform.position;
		}
	}


	void OnTriggerExit(Collider other){
		if(other.tag == "Guest" || other.tag == "Player")
			population--;
	}
}
