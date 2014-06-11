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

public class ClothesChanger : MonoBehaviour {

	public modelType mType;
	public GameObject[] objectsToChange;
	public enum modelType{
		Blue, Red, Yellow, Puce,
	}

	private PhotonView photonView;
	private List<Color> colors = new List<Color>();
	private Material[] mats;

	// Use this for initialization
	void Start () {
		photonView = PhotonView.Get(this);
		if(photonView.isMine){
			AddColors();
			switch(mType){
				case modelType.Blue:
					Color newColor1 = colors[Random.Range(0,colors.Count)];
					Color newColor2 = colors[Random.Range(0,colors.Count)];

					photonView.RPC("colorBlue", PhotonTargets.All, colorToVector3(newColor1), colorToVector3(newColor2));
					break;

				case modelType.Yellow:
					Color newColor = colors[Random.Range(0,colors.Count)];
					photonView.RPC("colorYellow", PhotonTargets.All, colorToVector3(newColor));
					break;
			}
		}
	}

	void AddColors(){
		//Blues
		colors.Add(new Color(36f/255f,25f/255f,178f/255f));
		colors.Add(new Color(53f/255f,47f/255f,133f/255f));
		colors.Add(new Color(16f/255f,8f/255f,115f/255f));
		
		//Purple
		colors.Add(new Color(163f/255f,41f/255f,92f/255f));

		//Maroon
		colors.Add(new Color(141f/255f,0f/255f,59f/255f));

		//Green
		colors.Add(new Color(59f/255f,130f/255f,61f/255f));

		//Blue/Green
		colors.Add(new Color(53f/255f,140f/255f,121f/255f));

		//Brown
		colors.Add(new Color(135f/255f,59f/255f,43f/255f));

		//Grey
		colors.Add(new Color(50f/255f, 50f/255f, 50f/255f));

		//White-Grey
		colors.Add(new Color(229f/255f, 229f/255f, 229f/255f));

		//White-Yellow
		colors.Add(new Color(229f/255f, 229f/255f, 134f/255f));

		//Light Red
		colors.Add(new Color(159f/255f, 87f/255f, 88f/255f));
	}

	private Vector3 colorToVector3(Color c){
		return new Vector3(c.r, c.g, c.b);
	}

	private Color vector3ToColor(Vector3 v){
		return new Color(v.x, v.y, v.z, 1);
	}

	[RPC]
	void colorBlue(Vector3 c1, Vector3 c2){
		Color newColor1 = vector3ToColor(c1);
		Color newColor2 = vector3ToColor(c2);
		mats = gameObject.renderer.materials;
		int i = 0;
		foreach(Material mat in mats){
			if(i>=5){
				if(i == 6)
					mat.SetColor("_Color", newColor1);
				else
					mat.SetColor("_Color", newColor2);
			}
			++i;
		}
	}

	[RPC]
	void colorYellow(Vector3 c){
		Color newColor = vector3ToColor(c);
		foreach(GameObject obj in objectsToChange){
			foreach(Material mat in obj.renderer.materials){
				mat.SetColor("_Color", newColor);
			}
		}
	}
}
