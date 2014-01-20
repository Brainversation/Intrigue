using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour
{
    
	bool accusing = false;
	GameObject accused;
	private PhotonView photonView = null;




		//Yield function that waits specified amount of seconds
		IEnumerator Yielder(int seconds){
			yield return new WaitForSeconds(seconds);
		}

		void Start(){
			
			photonView = PhotonView.Get(this);

			if(photonView.isMine){
				
				Debug.Log( "Guard" );
				
				//Highlights Teammates
				Yielder(1);				
				//Debug.Log("Number of allies to color: "+guards.Length);

					/* 
					<---------------------------------->
					NEED TO FIND WAY TO HIGHLIGHT ALLIES
					<---------------------------------->
					*/


			} else {
				
				Debug.Log("Guard Deactivated");
				GetComponentInChildren<Camera>().enabled = false; 
				GetComponentInChildren<AudioListener>().enabled = false;
				GetComponentInChildren<MovementController>().enabled = false;
				GetComponentInChildren<MouseLook>().enabled = false; 
				GetComponent<MouseLook>().enabled = false;
				enabled = false;
			
			}
		}

		void Update () {

				GameObject[] guests = GameObject.FindGameObjectsWithTag("Guest");
				GameObject[] spies = GameObject.FindGameObjectsWithTag("Spy");
				
				if(guests!=null){
					foreach (GameObject guest in guests){
						//guestAI = guest.GetComponent("GuestAi"); DEPRECATED
						//guestAI.outView();
						//<---------- NEED NEW WAY TO HIGHLIGHT GUESTS -------->
					}
				}
				if(spies!=null){
					foreach (GameObject spy in spies){
					
						/*
						spy.renderer.material.SetColor("_Color", Color.white);
						spyView = spy.GetComponent("InGuardView");
						spyView.outView(); 
						^ DEPRECATED ^
						<--------- NEED NEW WAY TO HIGHLIGHT SPIES ------> 
						*/
					}
				}

				//Highlights the currently targeted guest
				RaycastHit hit;
				Vector3 fwd = transform.TransformDirection (Vector3.forward);
						if (Physics.Raycast (transform.position, fwd, out hit, 5)) {
							if(hit.transform.gameObject.CompareTag("Guest")){
								/*
								guestAI = hit.transform.gameObject.GetComponent("GuestAi");
								guestAI.inView();
								^ DEPRECATED
								<------- NEED NEW WAY TO HIGHLIGHT GUESTS -------->
								*/
							}
							if(hit.transform.gameObject.CompareTag("Spy")){
								/*
								hit.transform.gameObject.renderer.material.SetColor("_Color", Color.red);
								spyView = hit.transform.gameObject.GetComponent("InGuardView");
								spyView.inView();
								^ DEPRECATED
								<------- NEED NEW WAY TO HIGHLIGHT SPIES -------->
								*/
							}
					}

	

				if ( Input.GetKeyUp (KeyCode.E) && !accusing ){
						if (Physics.Raycast (transform.position, fwd,out hit, 5)) {
							if(hit.transform.gameObject.CompareTag("Guest") || hit.transform.gameObject.CompareTag("Spy")){
									accusing = true;
									accused = hit.transform.gameObject;
								}
						}
				}	
			}

			void testAccusation(){
				
				//Accused Spy
		
				//Accused Guest
	
			}

			void OnNetworkInstantiate (NetworkMessageInfo info) {
					Debug.Log("New object instantiated by " + info.sender);
			}

			[RPC]
			void sendMessage(string text, NetworkMessageInfo info){
			    Debug.Log(text + " from " + info.sender);
			}

			[RPC]
			void spyCaught(){
			    --Intrigue.numSpiesLeft;
			}

			[RPC]
			void guardFailed(){
			    --Intrigue.numGuardsLeft;
			}
}