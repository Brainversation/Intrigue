﻿using UnityEngine;
using System.Collections;

public class PianoAnimScript : MonoBehaviour {

	private Animator anim;

	void Start () {
		anim = GetComponent<Animator>();
		anim.SetBool("Piano", true);
	}
}
