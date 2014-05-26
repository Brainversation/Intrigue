using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {

	public UIPanel[] panels;
	private int curSlide = 0;

	void Start () {
		curSlide = 0;
		updatePanels();
	
	}
	
	void updatePanels(){
		int i = 0;
		foreach(UIPanel pan in panels){
			if(i == curSlide)
				pan.alpha = 1;
			else
				pan.alpha = 0;

			++i;
		}
	}

	public void Next(){
		if(curSlide<=panels.Length-2){
			++curSlide;
			updatePanels();
		}
	}

	public void Previous(){
		if(curSlide>=1){
			--curSlide;
			updatePanels();	
		}
	}

	public void Back(){
		Application.LoadLevel("MainMenu");
	}
}
