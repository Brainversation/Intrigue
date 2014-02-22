using UnityEngine;
using System.Collections;

public class AI_RoomState : MonoBehaviour{

	int population;

	public void Start(){
		population = 0;
	}

	void OnTriggerEnter(Collider guestCollider){
		Debug.Log("IHAVEBEENENTERED!!!");
		guestCollider.gameObject.GetComponent<TempBaseAI>().room = gameObject;
		population++;
	}


	void OnTriggerExit(Collider guestCollider){
		population--;
	}
}
