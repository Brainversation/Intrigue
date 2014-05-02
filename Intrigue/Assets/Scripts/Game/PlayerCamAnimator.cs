using UnityEngine;
using System.Collections;

public class PlayerCamAnimator : MonoBehaviour {
	
	public Transform startMarker;
	public Transform endMarker;
	public float camSpeed = 2.0f;
	public float rotSpeed = .5f;
	public float smooth = 5.0F;

	private float startTime;
	private float journeyLength;
	private GameObject playerObj;
	private Quaternion startRot;
	private Quaternion endRot;
	private bool started = false;
  
	void Start() {
	  playerObj = gameObject.transform.parent.gameObject;
	}

	void Update() {
		if(playerObj.GetComponent<BasePlayer>().isOut){
			if(!started){
				started = true;
				playerObj.GetComponent<BasePlayer>().outStarted();
				startTime = Time.time;
				startRot = transform.rotation;
				endRot = endMarker.rotation;
				journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
				gameObject.GetComponent<MouseLook>().enabled = false;
				playerObj.GetComponent<MouseLook>().enabled = false;
			}

			float distCovered = (Time.time - startTime) * camSpeed;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
			transform.rotation = Quaternion.Lerp(startRot, endRot, Time.time * rotSpeed);
		}
	}
}
