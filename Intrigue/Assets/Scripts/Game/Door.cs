using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public bool open = false;
	public bool openRight = false;
	private float speed = 8F;
	private int curInside = 0;
	private Vector3 startMarker;
	private float moveDist = 8.1f;
	private Vector3 temp;

    void Start() {
        startMarker = transform.position;
    }
    void Update() {
    	if(openRight){
	    	if(curInside>=1){
	    		open = true;
	    		if(transform.position.x<startMarker.x+moveDist){
	    			transform.Translate(Time.deltaTime*speed, 0f, 0f, Space.Self);
	    			GetComponent<SphereCollider>().center = GetComponent<SphereCollider>().center - new Vector3(Time.deltaTime*speed*2,0f,0f);
	    		}
			}
			else {
				open = false;
				if(transform.position.x>startMarker.x){
	    			transform.Translate(Time.deltaTime*-speed, 0f, 0f, Space.Self);
	    			GetComponent<SphereCollider>().center = GetComponent<SphereCollider>().center + new Vector3(Time.deltaTime*speed*2,0f,0f);
	    		}
	    	}
	    }
	    else{
	    	if(curInside>=1){
	    		open = true;
	    		if(transform.position.x>startMarker.x-moveDist){
	    			transform.Translate(Time.deltaTime*speed, 0f, 0f, Space.Self);
	    			GetComponent<SphereCollider>().center = GetComponent<SphereCollider>().center - new Vector3(Time.deltaTime*speed*2,0f,0f);
	    		}
			}
			else {
				open = false;
				if(transform.position.x<startMarker.x){
	    			transform.Translate(Time.deltaTime*-speed, 0f, 0f, Space.Self);
	    			GetComponent<SphereCollider>().center = GetComponent<SphereCollider>().center + new Vector3(Time.deltaTime*speed*2,0f,0f);
	    		}
	    	}
	    }
    }

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Spy")||other.gameObject.CompareTag("Guard")||other.gameObject.CompareTag("Guest")){
			curInside++;
		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.CompareTag("Spy")||other.gameObject.CompareTag("Guard")||other.gameObject.CompareTag("Guest")){
			curInside--;
		}
	}
}
