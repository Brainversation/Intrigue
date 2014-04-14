using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_RoomState : MonoBehaviour{

	public string roomName = "";
	public bool hasArt = false;

	[HideInInspector] public List<GameObject> conversers;
	[HideInInspector] public int population;
	[HideInInspector] public List<Vector3> artLocations;
	[HideInInspector] public Vector3 relaxLocation;
	[HideInInspector] public Vector3 drinkLocation;
	[HideInInspector] public Vector3 converseLocation;
	[HideInInspector] public Vector3 restroomLocation;
	[HideInInspector] public Vector3 poetLocation;
	[HideInInspector] public GameObject me;
	[HideInInspector] public GameObject poet;

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
			//Debug.Log("drinkLocation: " + drinkLocation);
		}

		if(other.tag == "RestRoom"){
			restroomLocation = other.transform.position;
			//Debug.Log("bathroomLocation: " + restroomLocation);
		}

		if(other.tag == "poetry"){
			poetLocation = other.transform.position;
		}

		if(other.tag == "Relax"){
			relaxLocation = other.transform.position;
		}
		/*
		if(other.tag == "Art"){
			artLocations.Add(other.transform.position);
		}
		*/
	}

	void OnTriggerStay(Collider other){
		if(other.tag == "Drink"){
			drinkLocation = other.transform.position;
			//Debug.Log("drinkLocation: " + drinkLocation);
		}

		if(other.tag == "RestRoom"){
			restroomLocation = other.transform.position;
			//Debug.Log("bathroomLocation: " + restroomLocation);
		}
	}


	void OnTriggerExit(Collider other){
		if(other.tag == "Guest" || other.tag == "Player")
			population--;
	}
}
