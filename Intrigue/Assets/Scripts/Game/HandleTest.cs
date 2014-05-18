using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class HandleTest : MonoBehaviour {

	public UILabel handleLabel;
	public GameObject proceedButton;
	private HashSet<char> validChars = new HashSet<char>() {'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','1','2','3','4','5','6','7','8','9','0','_'};

	void Start(){
		proceedButton.SetActive(false);
	}

	void Update(){
		if(handleLabel.text.Length>=1 && handleLabel.text.Length<=10){
			handleLabel.text = CleanString(handleLabel.text);
			handleLabel.text = StringCleaner.CleanString(handleLabel.text);
			proceedButton.SetActive(true);
		}
		else{
			proceedButton.SetActive(false);
		}
	}

	string CleanString(string s){
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		for(int i=0;i<s.Length;i++){
			if(validChars.Contains(s[i])){
				sb.Append (s[i]);
			}
        }
		return sb.ToString();
	}

}
