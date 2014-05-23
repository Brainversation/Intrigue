using UnityEngine;
using System.Collections;

public class AnimateBoss : MonoBehaviour {

	private bool done = true;
	private Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(done){
			done = false;
			StartCoroutine(StartPointing(Random.Range(5,10)));
		}
	}

	IEnumerator StartPointing(float time){
		yield return new WaitForSeconds(time);
		anim.SetBool("Pointing", true);
		StartCoroutine(EndPointing(anim.GetCurrentAnimatorStateInfo(0).length));
	}

	IEnumerator EndPointing(float time){
		yield return new WaitForSeconds(time);
		anim.SetBool("Pointing", false);
		done = true;
	}
}
