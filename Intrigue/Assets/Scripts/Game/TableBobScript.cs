using UnityEngine;
using System.Collections;

public class TableBobScript : MonoBehaviour {

	private Vector3 pos1;
	private Vector3 pos2;
	private Vector3 moveTo;
	public float offset;	
	public float moveSpeed;


	// Use this for initialization
	void Start () {
		pos1 = transform.position;
		pos2 = pos1 + (offset*Vector3.down);
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position == pos1){
			moveTo = pos2;
		}

		if(transform.position == pos2){
			moveTo = pos1;
		}

		transform.position = Vector3.MoveTowards(transform.position, moveTo, moveSpeed);
	}
}
