using UnityEngine;
using System.Collections;

public class BartendAnimScript : MonoBehaviour {

	private bool done = true;
	private Animator anim;

	void Start () {
		anim = GetComponent<Animator>();
	}
	
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
