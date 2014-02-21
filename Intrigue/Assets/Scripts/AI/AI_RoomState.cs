using UnityEngine;
using System.Collections;

public class AI_RoomState : MonoBehaviour{

	public struct RoomState{
		public string name;
		public int population;
	}

	RoomState currState;

	public void Start(){
		currState = new RoomState();
		currState.name = gameObject.name;
		currState.population = 0;
	}

	void OnCollisionEnter(Collision guestCollider){
		guestCollider.gameObject.GetComponent<TempBaseAI>().room = gameObject;
		currState.population++;
	}


	void OnCollisionExit(Collision guestCollider){
		currState.population--;
	}
}
