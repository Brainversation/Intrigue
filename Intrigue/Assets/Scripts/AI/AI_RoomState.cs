using UnityEngine;
using System.Collections;

public class AI_RoomState : MonoBehaviour{
	[HideInInspector] public string roomName;
	[HideInInspector] public int population;
	public Transform drinkLocation;
	public Transform converseLocation;

	public void Start(){
		roomName = gameObject.name;
		population = 0;
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Guest")
			other.gameObject.GetComponent<BaseAI>().room = gameObject;

		if(other.tag == "Guest" || other.tag == "Player")
			population++;
		Debug.Log(population);
	}


	void OnTriggerExit(Collider other){
		if(other.tag == "Guest" || other.tag == "Player")
			population--;
	}
}
