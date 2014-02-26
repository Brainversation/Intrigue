using UnityEngine;
using System.Collections;

public class AI_RoomState : MonoBehaviour{

	public Transform dl;
	public Transform cl;
	public Transform rrl;
	
	public AIRoomInfo roomInfo = new AIRoomInfo();

	public void Start(){
		roomInfo.drinkLocation = dl;
		roomInfo.converseLocation = cl;
		roomInfo.restroomLocation = rrl;
		roomInfo.roomName = gameObject.name;
		roomInfo.population = 0;
		roomInfo.me = gameObject;
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Guest")
			other.gameObject.GetComponent<BaseAI>().room = roomInfo;

		if(other.tag == "Guest" || other.tag == "Player")
			roomInfo.population++;
	}


	void OnTriggerExit(Collider other){
		if(other.tag == "Guest" || other.tag == "Player")
			roomInfo.population--;
	}
}

public struct AIRoomInfo {
	public string roomName;
	public int population;
	public Transform drinkLocation;
	public Transform converseLocation;
	public Transform restroomLocation;
	public GameObject me;
}
