using UnityEngine;
using System.Collections;

public class AI_RoomState : MonoBehaviour{

	int population;

	public void Start(){
		population = 0;
	}

	void OnTriggerEnter(Collider guestCollider){
		//Debug.Log("IHAVEBEENENTERED!!!");
		if(guestCollider.tag == "Guest"){
			guestCollider.gameObject.GetComponent<TempBaseAI>().room = gameObject;
			population++;
		}
	}


	void OnTriggerExit(Collider guestCollider){
		if(guestCollider.tag == "Guest"){
			population--;
		}
	}
}
