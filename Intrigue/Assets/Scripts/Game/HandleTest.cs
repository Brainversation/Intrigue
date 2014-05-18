using UnityEngine;
using System.Collections;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class HandleTest : MonoBehaviour {

	public UILabel handleLabel;
	public GameObject proceedButton;

	void Start(){
		proceedButton.SetActive(false);
	}

	void Update(){
		if(handleLabel.text.Length>=1 && handleLabel.text.Length<=10){
			proceedButton.SetActive(true);
		}
		else{
			proceedButton.SetActive(false);
		}
	}
}
