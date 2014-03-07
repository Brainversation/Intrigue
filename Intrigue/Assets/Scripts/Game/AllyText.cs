using UnityEngine;
using System.Collections;

public class AllyText : MonoBehaviour {
	public Transform target;		// Object that this label should follow
	public Vector3 offset;	// Units in world space to offset; 1 unit above object by default
	public bool clampToScreen = false;	// If true, label will be visible even if object is off screen
	public float clampBorderSize = 0.05f;	// How much viewport space to leave at the borders when a label is being clamped
	public bool useMainCamera = true;	// Use the camera tagged MainCamera
	public Camera cameraToUse;	// Only use this if useMainCamera is false
	private Transform thisTransform;

	// Use this for initialization
	void Start () {
		thisTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
			thisTransform.position = target.position + offset;
			if(Camera.main!=null && thisTransform!=null){
				//thisTransform.LookAt(Camera.main.transform, Vector3.up);
			}
			//thisTransform.Rotate(Vector3.up, 180);
	}
}
