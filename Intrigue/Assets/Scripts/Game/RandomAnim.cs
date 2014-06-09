using UnityEngine;
using System.Collections;

public class RandomAnim : MonoBehaviour {

	private Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		anim.enabled = false;
		StartCoroutine(StartAnims(Random.Range(1,3)));
	}

	IEnumerator StartAnims(float time){
		yield return new WaitForSeconds(time);
		anim.enabled = true;
	}
}
