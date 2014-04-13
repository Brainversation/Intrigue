using UnityEngine;
using System.Collections;

public class NetworkCharacter : Photon.MonoBehaviour {

	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private Animator anim;
	private UIPanel sprintPanel;
	private UIPanel[] guiPanels;
	private UISlider sprintSlider;
	private Spy spyRef;
	private Player player;
	private Camera cam;
	private Vector3 camStart;
	private float stamina = 100;
	private float staminaDrainSpeed;
	private float staminaRegenSpeed;
	private bool canRegen;
	public bool isOut;

	void Start() {
		//Get References to Animator and Collider
		anim = GetComponent<Animator>();
		anim.speed = 1.0f;
		spyRef = gameObject.GetComponent<Spy>();
		player = GameObject.Find("Player").GetComponent<Player>();
		if(player.Team=="Spy"){
			staminaDrainSpeed = 20;
			staminaRegenSpeed = 10;
		}
		else{
			staminaDrainSpeed = 15;
			staminaRegenSpeed = 10;
		}
	}

	public void Update(){
		if(!photonView.isMine){
			transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
		}	
	}

	public void FixedUpdate(){
		if(photonView.isMine && !isOut){
			if(cam==null){
				cam = GetComponentInChildren<Camera>();
				camStart = cam.transform.localPosition;
			}
			guiPanels = GetComponentsInChildren<UIPanel>(true);
			if(sprintPanel==null||sprintSlider==null){
				foreach(UIPanel uiP in guiPanels){
					if(uiP.gameObject.CompareTag("SprintPanel")){
						sprintPanel = uiP;
						sprintSlider = sprintPanel.GetComponentInChildren<UISlider>();
					}
				}
			}

			if(stamina<100){
				NGUITools.SetActive(sprintPanel.gameObject, true);
				sprintSlider.value = stamina/100;
			}
			else{
				NGUITools.SetActive(sprintPanel.gameObject, false);
			}

			anim.SetFloat("Speed", Input.GetAxis("Vertical"));
			if(stamina>=1 && Input.GetKey("left shift") && Input.GetAxis("Vertical")!=0){
				stamina-=staminaDrainSpeed*Time.deltaTime;
				canRegen = false;
				cam.transform.localPosition = camStart + new Vector3(0f,0f,2f);
				anim.SetBool("Run", Input.GetKey("left shift"));
			}
			else{
				cam.transform.localPosition = camStart;
				if(!canRegen){
					Invoke("StartRegen", 3);
				}
				anim.SetBool("Run", false);
				if(stamina<100 && canRegen)
					stamina+=staminaRegenSpeed*Time.deltaTime;
				if(stamina>100)
					stamina=100;
			}

			anim.SetBool("Out", false);
			
			if(player.Team=="Spy"){
				spyRef = gameObject.GetComponent<Spy>();
				if(spyRef!=null && spyRef.doingObjective){
					if(spyRef.objectiveType=="Safe"){
						anim.SetBool("InteractSafe",true);
					}
					else if(spyRef.objectiveType=="Computer"){
						anim.SetBool("InteractComp",true);
					}
				}
				else{
					anim.SetBool("InteractSafe",false);
					anim.SetBool("InteractComp",false);
				}
			}
		}
		else if(photonView.isMine && isOut){
			anim.SetFloat("Speed", 0f);
			anim.SetFloat("Direction", 0f);
			anim.SetBool("Run", false);
			anim.SetBool("Out", true);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if (stream.isWriting){
			// We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(Input.GetAxis("Vertical"));
			stream.SendNext(Input.GetAxis("Horizontal"));
			stream.SendNext(Input.GetKey("left shift"));
			stream.SendNext(anim.GetBool("InteractSafe"));
			stream.SendNext(anim.GetBool("InteractComp"));
			stream.SendNext(anim.GetBool("Out"));

		}else{
			// Network player, receive data
			this.correctPlayerPos = (Vector3) stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion) stream.ReceiveNext();
			anim.SetFloat("Speed", (float) stream.ReceiveNext());
			anim.SetFloat("Direction", (float) stream.ReceiveNext());
			anim.SetBool("Run", (bool) stream.ReceiveNext());
			anim.SetBool("InteractSafe", (bool) stream.ReceiveNext());
			anim.SetBool("InteractComp", (bool) stream.ReceiveNext());
			anim.SetBool("Out", (bool) stream.ReceiveNext());
		}
	}

	void StartRegen(){
		canRegen = true;
	}
}
