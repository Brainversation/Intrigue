using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

    public float speed = 6.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

	public float animSpeed = 1.5f;             // a public setting for overall animator animation speed
    public float lookSmoother = 3f;             // a smoothing setting for camera motion
    public bool useCurves;                      // a setting for teaching purposes to show use of curves

    
    private Animator anim;                          // a reference to the animator on the character
    private AnimatorStateInfo currentBaseState;         // a reference to the current state of the animator, used for base layer
    private AnimatorStateInfo layer2CurrentState;   // a reference to the current state of the animator, used for layer 2
    private CapsuleCollider col;                    // a reference to the capsule collider of the character
    

    static int idleState = Animator.StringToHash("Base Layer.Idle");    
    void Start() {
        //Get References to Animator and Collider
        anim = GetComponent<Animator>();                      
        col = GetComponent<CapsuleCollider>();  
    }    

    void FixedUpdate() {
        float h = Input.GetAxis("Horizontal");              // setup h variable as our horizontal input axis
        float v = Input.GetAxis("Vertical");                // setup v variables as our vertical input axis
        bool r = Input.GetKey("left shift");                // setup r variable as sprint input
        bool i = Input.GetKey("e");                         // setup i variable as interact input
        anim.SetFloat("Speed", v);                          // set our animator's float parameter 'Speed' equal to the vertical input axis              
        anim.SetFloat("Direction", h);                      // set our animator's float parameter 'Direction' equal to the horizontal input axis        
        anim.SetBool("Run", r);                             // set our animator's bool parameter 'Run' equal to run bool
        anim.SetBool("Interact", i);                        // set our animator's bool parameter 'Interact' equal to interact bool
        anim.speed = animSpeed;                             // set the speed of our animator to the public variable 'animSpeed'
    }
    void Update() {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded) {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed; 
        }
		
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime); 
    }
	
}