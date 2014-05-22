using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_RoomState : MonoBehaviour{

	public string roomName = "";
	
	[HideInInspector] public List<GameObject> conversers;
	[HideInInspector] public List<Vector3> artLocations;
	[HideInInspector] public List<Vector3> restroomLocations;
	[HideInInspector] public int population;
	[HideInInspector] public List<GameObject> relaxLocations;
	[HideInInspector] public Vector3 drinkLocation;
	[HideInInspector] public Vector3 converseLocation;
	[HideInInspector] public Vector3 poetLocation;
	[HideInInspector] public GameObject me;
	[HideInInspector] public GameObject poet;

	public void Start(){
		roomName = gameObject.name;
		population = 0;
		me = gameObject;
		conversers = new List<GameObject>();
		artLocations = new List<Vector3>();
		restroomLocations = new List<Vector3>();
	}

	void OnTriggerEnter(Collider other){

		switch(other.tag){
			case "Guest":
				other.gameObject.GetComponent<BaseAI>().room = this;
				other.gameObject.GetComponent<BaseAI>().anxiety += population * 2;
				population++;
				break;

			case "Player":
				population++;
				break;

			case "Drink":
				drinkLocation = other.transform.position;
				//Debug.Log("drinkLocation: " + drinkLocation);
				break;

			case "RestRoom":
				restroomLocations.Add(other.transform.position);
				// Debug.Log("bathroomLocation: " + restroomLocation);
				break;

			case "poetry":
				poetLocation = other.transform.position;
				break;

			case "Relax":
				Debug.Log("Adding Relax: " + other.gameObject);
				relaxLocations.Add(other.gameObject);
				break;

			case "Art":
				artLocations.Add(other.transform.position);
				// Debug.Log("artLocations: " + other.transform.position);
				break;

			// default:
			// 	Debug.LogError("Collide not in stuff: " + other.tag);
			// 	break;
		}
	}


	void OnTriggerExit(Collider other){
		if(other.tag == "Guest" || other.tag == "Player")
			population--;
	}
}
