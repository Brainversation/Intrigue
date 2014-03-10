using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_RoomState : MonoBehaviour{

	public string roomName = "";

	[HideInInspector] public List<GameObject> conversers;
	[HideInInspector] public int population;
	[HideInInspector] public Transform drinkLocation;
	[HideInInspector] public Transform converseLocation;
	[HideInInspector] public Transform restroomLocation;
	[HideInInspector] public GameObject me;

	public void Start(){
		roomName = gameObject.name;
		population = 0;
		me = gameObject;
		conversers = new List<GameObject>();
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Guest")
			other.gameObject.GetComponent<BaseAI>().room = this;

		if(other.tag == "Guest" || other.tag == "Player")
			population++;
	}


	void OnTriggerExit(Collider other){
		if(other.tag == "Guest" || other.tag == "Player")
			population--;
	}
}
