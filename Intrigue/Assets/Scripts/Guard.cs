using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour
{
    
	bool accusing = false;
	GameObject accused;




		//Yield function that waits specified amount of seconds
		IEnumerator Yielder(int seconds){
			yield return new WaitForSeconds(seconds);
		}

		void Start(){
			
			if(networkView.isMine){
				
				Debug.Log( "Guard" );
				
				//Highlights Teammates
				Yielder(1);
				GameObject[] guards = GameObject.FindGameObjectsWithTag( "Guard" );
				
				//Debug.Log("Number of allies to color: "+guards.Length);
				foreach( GameObject guard in guards ){
					/* 
					<---------------------------------->
					NEED TO FIND WAY TO HIGHLIGHT ALLIES
					<---------------------------------->
					*/
				}

			} else {
				
				/*
				GetComponentInChildren<Camera>().enabled = false;
				GetComponentInChildren<AudioListener>().enabled = false;
				GetComponentInChildren<MouseLook>().enabled = false; 
				GetComponentInChildren<FPSInputController>().enabled = false; 
				GetComponent<MouseLook>().enabled = false;
				GetComponent<CharacterMotor>().enabled = false;
				enabled = false;
				UNCOMMENT THIS ONCE IT IS ATTACHED TO A CHARACTER
				*/
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
				if(accused != null && accused.CompareTag("Spy")){
					print("You found a spy!");
					networkView.RPC("spyCaught", RPCMode.Server);
					if (Network.isServer){--Intrigue.numSpiesLeft;}
					NetworkView netView = accused.GetComponent<NetworkView>();
					Debug.Log("accused viewID " + netView.viewID + " owner " + netView.viewID.owner);
					Spy spyScript = accused.GetComponent<Spy>();
					spyScript.callRPC(netView.viewID);
				}
				//Accused Guest
				else {
					print("You accused a guest! You have been relieved of duty.");

					networkView.RPC("guardFailed", RPCMode.Server);
					if (Network.isServer) --Intrigue.numGuardsLeft;

					GameObject[] guards = GameObject.FindGameObjectsWithTag("Guard");
					foreach (GameObject guard in guards){
						if ((guard.GetComponent<NetworkView>().viewID) != networkView.viewID){
							Transform camRef = GetComponentInChildren<Camera>().transform;
							camRef.parent = guard.transform;
							Intrigue.isSpectating = true;
							Vector3 camHeightFix = new Vector3(0.1499996f,0.5277554f,0.0f);
							camRef.position = guard.transform.position + camHeightFix;
							camRef.rotation = guard.transform.rotation;
							break;
						}
					
					}
					Network.Destroy(this.gameObject);
				}
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