using UnityEngine;
using System.Collections;

public class DJDanceAnimScript : MonoBehaviour {

	private Animator anim;

	void Start () {
		anim = GetComponent<Animator>();
		anim.SetBool("DJdance", true);
	}
}
