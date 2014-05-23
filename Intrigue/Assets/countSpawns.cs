using UnityEngine;
using System.Collections;

public class countSpawns : MonoBehaviour {

	// Use this for initialization
	void Start () {

		GameObject[] spawns = GameObject.FindGameObjectsWithTag("Respawn");
		Debug.Log(spawns.Length);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
