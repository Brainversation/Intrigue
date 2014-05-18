using UnityEngine;
using System.Collections;

public class ServerIndicator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Camera.main!=null){
			inView();
			transform.LookAt(Camera.main.transform);
			transform.Rotate(Vector3.up, 180);
		}
	}

	void inView(){
		if(Vector3.Distance(Camera.main.transform.position, transform.position)<70){
			gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
		else{
			gameObject.GetComponent<MeshRenderer>().enabled = true;
		}
	}
}
