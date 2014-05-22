using UnityEngine;
using System.Collections;

public class TableBobScript : MonoBehaviour {

	private Vector3 pos1;
	private Vector3 pos2;
	private Vector3 offset;	
	private Vector3 moveTo;
	private float moveSpeed;


	// Use this for initialization
	void Start () {
		offset = Vector3.down;
		offset.y += .9f;
		pos1 = transform.position;
		pos2 = pos1 + offset;
		moveSpeed = 0.001f;
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
