using UnityEngine;
using System.Collections;

public class PokerAnimScript : MonoBehaviour {

	private Animator anim;
	private bool myTurn;

	void Start () {
		anim = GetComponent<Animator>();
		//anim.SetLayerWeight(1, 1);
		myTurn = false;
	}
	
	void Update () {
		if(!anim.GetCurrentAnimatorStateInfo(0).IsName("CardPlayerCardLook")){
			if(Random.Range(0,1000) > 900)
				StartCoroutine(turnHandler());
		}	
	}

	IEnumerator turnHandler(){
		yield return new WaitForSeconds(Random.Range(5, 15));
		myTurn = true;
		anim.SetBool("MyTurn", myTurn);
		StartCoroutine(WaitAndCallback(anim.GetCurrentAnimatorStateInfo(0).length));
	}

	IEnumerator WaitAndCallback(float waitTime){
		yield return new WaitForSeconds(waitTime);
		myTurn = false;
		anim.SetBool("MyTurn", myTurn);   
	}

}
