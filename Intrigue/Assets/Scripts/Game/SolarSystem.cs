using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolarSystem : MonoBehaviour {

	public GameObject[] planets;
	public GameObject[] asteroids;

	private Vector3 rotationalAxis;
	private List<Vector3> rotationalTypes = new List<Vector3>();
	// Use this for initialization
	void Start () {
		rotationalTypes.Add(Vector3.up);
		rotationalTypes.Add(Vector3.down);
		rotationalTypes.Add(Vector3.left);
		rotationalTypes.Add(Vector3.right);

		gameObject.renderer.material.SetColor("_Color", new Color(255f/255f,100f/255f,47f/255f));
		planets[0].renderer.material.SetColor("_Color", new Color(183f/255f,0f/255f,100f/255f));
		planets[1].renderer.material.SetColor("_Color", new Color(0f/255f,255f/255f,100f/255f));
		planets[2].renderer.material.SetColor("_Color", new Color(255f/255f,247f/255f,100f/255f));
		planets[3].renderer.material.SetColor("_Color", new Color(0f/255f,0f/255f,200f/255f));


	}
	
	// Update is called once per frame
	void Update () {
		int i = 0;
		foreach(GameObject planet in planets){
			planet.transform.RotateAround(gameObject.transform.position, rotationalTypes[i], Random.Range(5,40) * Time.deltaTime);
			++i;
		}

		foreach(GameObject asteroid in asteroids){
			asteroid.transform.RotateAround(gameObject.transform.position, Vector3.left, Random.Range(5,40) * Time.deltaTime);
		}
	}
}
