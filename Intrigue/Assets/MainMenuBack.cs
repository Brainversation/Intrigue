using UnityEngine;
using System.Collections;

public class MainMenuBack : MonoBehaviour {

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
		mainButtons.SetActive(true);
		NGUITools.SetActive(mainButtons, true);
		optionsButtons.SetActive(false);
		NGUITools.SetActive(optionsButtons, false);

	}
}
