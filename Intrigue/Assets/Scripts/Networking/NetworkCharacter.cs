using UnityEngine;
using System.Collections;

public class NetworkCharacter : Photon.MonoBehaviour {

	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;

	public float animSpeed = 1.5f;             // a public setting for overall animator animation speed
    public float lookSmoother = 3f;             // a smoothing setting for camera motion
    public bool useCurves;                      // a setting for teaching purposes to show use of curves

    
    private Animator anim;                          // a reference to the animator on the character
    private AnimatorStateInfo currentBaseState;         // a reference to the current state of the animator, used for base layer
    private AnimatorStateInfo layer2CurrentState;   // a reference to the current state of the animator, used for layer 2
    private CapsuleCollider col;                    // a reference to the capsule collider of the character
    
	void Start() {
        //Get References to Animator and Collider
        anim = GetComponent<Animator>();                      
        col = GetComponent<CapsuleCollider>();  
    }    

	public void Update(){
		if(!photonView.isMine){
			transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
		}
	}

	 void FixedUpdate() {
	 	if(photonView.isMine && gameObject.tag != "Guest"){
	        float h = Input.GetAxis("Horizontal");              // setup h variable as our horizontal input axis
	        float v = Input.GetAxis("Vertical");                // setup v variables as our vertical input axis
	        bool r = Input.GetKey("left shift");                // setup r variable as sprint input
	        bool i = Input.GetKey("e");                         // setup i variable as interact input
	        anim.SetFloat("Speed", v);                          // set our animator's float parameter 'Speed' equal to the vertical input axis              
	        anim.SetFloat("Direction", h);                      // set our animator's float parameter 'Direction' equal to the horizontal input axis        
	        anim.SetBool("Run", r);                             // set our animator's bool parameter 'Run' equal to run bool
	        anim.SetBool("Interact", i);                        // set our animator's bool parameter 'Interact' equal to interact bool
	        anim.speed = animSpeed;                             // set the speed of our animator to the public variable 'animSpeed'

	        photonView.RPC("sendAnimFloat",PhotonTargets.All,"Speed", v);
	        photonView.RPC("sendAnimFloat",PhotonTargets.All,"Direction", h);
	        photonView.RPC("sendAnimBool",PhotonTargets.All,"Run", r);
	        photonView.RPC("sendAnimBool",PhotonTargets.All,"Interact", i);
        } else {
        	Debug.Log("Guest!!");
        }
    }

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if (stream.isWriting){
			// We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		}else{
			// Network player, receive data
			this.correctPlayerPos = (Vector3) stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion) stream.ReceiveNext();
		}
	}

	[RPC]
	void sendAnimFloat(string name, float value){
		anim.SetFloat(name,value);
	}

	[RPC]
	void sendAnimBool(string name, bool value){
		anim.SetBool(name,value);
	}
}