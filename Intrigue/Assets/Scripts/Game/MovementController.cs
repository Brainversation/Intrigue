using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

    public float charSpeed;
    public float gravity;
    private Vector3 moveDirection;

    void Start(){
        charSpeed = 8.0f;
        gravity = 20.0f;
        moveDirection = Vector3.zero;
    }

    void Update() {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded) {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= charSpeed; 
        }
		
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime); 
    }
	
}