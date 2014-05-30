using UnityEngine;
using System.Collections;

public class SitAnimScript : MonoBehaviour {

	private Animator anim;

	void Start () {
		anim = GetComponent<Animator>();
		anim.SetBool("SittingIdle", true);
	}
}
