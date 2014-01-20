using UnityEngine;
using System.Collections;

public class CubeController : MonoBehaviour {

	private PhotonView photonView;

	// Use this for initialization
	void Start () {
		photonView = PhotonView.Get(this);
	}
	
	// Update is called once per frame
	void Update () {
		if( photonView.isMine ){
			if( Input.GetKey( "left" )){
				transform.Translate( new Vector3(-1, 0, 0) );
			}
			if( Input.GetKey( "right" )){
				transform.Translate( new Vector3(1, 0, 0) );
			}
		}
	}
}
