using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DanceFloorColors : MonoBehaviour {

	public Color c1;
	public Color c2;
	public Color c3;
	public Color c4;
	private List<Color> colors = new List<Color>();
	private float t;

	void Start () {
		colors.Add(c1);
		colors.Add(c2);
		colors.Add(c3);
		colors.Add(c4);
		t = 0.0f;
	}
	
	void Update () {
		t += Time.deltaTime;

		if(t > 0.5f){
			gameObject.renderer.material.SetColor("_Color", colors[Random.Range(0, 4)]);
			t = 0.0f;
		}
	}
}
