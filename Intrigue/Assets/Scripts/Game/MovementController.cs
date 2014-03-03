using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	[HideInInspector] public float gravity;
	[HideInInspector] public float rotSpeed;
	private Vector3 moveDirection;

	void Start(){
		gravity = 20.0f;
		rotSpeed = 90.0f;
		moveDirection = Vector3.zero;
	}

	void Update() {
		CharacterController controller = GetComponent<CharacterController>();
		transform.Rotate(0, Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime, 0); 

		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime); 
	}
	
}