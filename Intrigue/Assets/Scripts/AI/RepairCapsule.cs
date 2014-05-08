using UnityEngine;
using System.Collections;

public class RepairCapsule : QueueHotSpot {

	private bool repair = false;

	public override void Update(){
		base.Update();
		if(	queue.Count >= 1 &&
			!queue[0].GetComponent<NavMeshAgent>().hasPath &&
			queue[0].GetComponent<BaseAI>().isYourTurn){
			
			Quaternion temp = gameObject.transform.rotation;
			temp.x = 0;
			temp.z = 0;
			queue[0].transform.rotation = temp;
			queue[0].transform.Rotate(0,-90,0);
			Vector3 temp2 = gameObject.transform.position;
			temp2.y = queue[0].transform.position.y;
			queue[0].transform.position = temp2;
			if(!IsInvoking() && !repair)
				Invoke("doRepair", 2.5f);
		}
	}

	public void doRepair(){
		Debug.Log("DO REPAIR");
		repair = true;
	}

	protected override void OnTriggerExit(Collider other){
		base.OnTriggerExit(other);
		if(other.tag == "Guest" && queue.Contains(other.gameObject)){
			repair = false;
		}
	}
}
