using UnityEngine;
using System.Collections;

public class OptionsSwitch : MonoBehaviour {

	private GameObject mainButtons;
	private GameObject optionsButtons;

	// Use this for initialization
	void Start () {
		mainButtons = GameObject.Find("MainMenuButtons");
		optionsButtons = GameObject.Find("OptionsButtons");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick(){
		mainButtons.SetActive(false);
		NGUITools.SetActive(mainButtons, false);
		optionsButtons.SetActive(true);
		NGUITools.SetActive(optionsButtons, true);
	}
}
