using UnityEngine;
using System.Collections;

public class PointPop : MonoBehaviour {

	private float a = 1;
	private float t = .01f;
	private Color temp;
	private MeshRenderer mr;

	void Start(){
		mr = GetComponent<MeshRenderer>();
		a = 1;
		transform.LookAt(Camera.main.transform, Vector3.up);
		transform.Rotate(0,180,0);
	}
	
	void Update () {
		if(t < 0){
			a -= (Time.deltaTime);
			transform.Translate(Vector3.up * Time.deltaTime, Space.World);
			temp = mr.material.color;
			temp.a = a;
			mr.material.color = temp;
			t = .01f;
		} else {
			t -= Time.deltaTime;
		}

		transform.LookAt(Camera.main.transform, Vector3.up);
		transform.Rotate(0,180,0);

		if(a < 0) Destroy(gameObject);
	}
}
