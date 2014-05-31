/**************************************************************************
 **                                                                       *
 **                COPYRIGHT AND CONFIDENTIALITY NOTICE                   *
 **                                                                       *
 **    Copyright (c) 2014 Hacksaw Games. All rights reserved.             *
 **                                                                       *
 **    This software contains information confidential and proprietary    *
 **    to Hacksaw Games. It shall not be reproduced in whole or in        *
 **    part, or transferred to other documents, or disclosed to third     *
 **    parties, or used for any purpose other than that for which it was  *
 **    obtained, without the prior written consent of Hacksaw Games.      *
 **                                                                       *
 **************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolarSystem : MonoBehaviour {

	public GameObject[] planets;

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
	}
}
