/**************************************************************************
 **                                                                       *
 **                COPYRIGHT AND CONFIDENTIALITY NOTICE                   *
 **                                                                       *
 **    Copyright (c) 2014 Hacksaw Games. All rights reserved.             *
 **                                                                       *
 **    This software contains information confidential and proprietary    *
 **    to Hacksaw Games. It shall not be reproduced in whole or in        *
 **    part, or transferred to other documents, or disclosed to third     *
 **    parties, or used for any purpose other than that for which it was  *
 **    obtained, without the prior written consent of Hacksaw Games.      *
 **                                                                       *
 **************************************************************************/

using UnityEngine;

/// <summary>
/// Very simple example of how to use a TextList with a UIInput for chat.
/// </summary>

[RequireComponent(typeof(UIInput))]
[AddComponentMenu("NGUI/Examples/Chat Input")]
public class PlayerChat : MonoBehaviour
{
	public UITextList textList;
	public GameObject window;
	// private PhotonView photonView = null;
	private Player player;
	private bool mIgnoreUp = false;
	private PhotonView photonView = null;
	private static bool debugMode = true;
	public string lastMessagedPlayer = "";

	UIInput mInput;

	void Start ()
	{
		//this.photonView = PhotonView.Get(this);
		player = Player.Instance;
		mInput = GetComponent<UIInput>();
		mInput.label.maxLineCount = 1;
		photonView = PhotonView.Get(this);
	}

	/// <summary>
	/// Submit notification is sent by UIInput when 'enter' is pressed or iOS/Android keyboard finalizes input.
	/// </summary>

	void Update(){
		if (window.GetComponent<UISprite>().alpha == 1){
			mInput.enabled = true;
			if (mInput){
				if (!mIgnoreUp && !UICamera.inputHasFocus){
					UICamera.selectedObject = gameObject;
				}
				mIgnoreUp = false;
			}
			else{
				UICamera.selectedObject = gameObject;
			}
		} else if(window.GetComponent<UISprite>().alpha == 0){					
			UICamera.inputHasFocus = false;
			UICamera.selectedObject = null;
			mInput.enabled = false;			
		}
	}

	public void OnSubmit (){
		if(UICamera.currentKey == KeyCode.Return)
			mIgnoreUp = true;

		if (textList != null)
		{	

			// It's a good idea to strip out all symbols as we don't want user input to alter colors, add new lines, etc
			string text = NGUIText.StripSymbols(mInput.value);
			bool isCommand = false;
			if(debugMode)
				isCommand = testCommands(text);
			text = StringCleaner.CleanString(text);

			if (!string.IsNullOrEmpty(text) && text.Length>=2 && !isCommand){
				foreach(PhotonPlayer p in PhotonNetwork.playerList){
					if((string)p.customProperties["Team"] == (string)PhotonNetwork.player.customProperties["Team"]){
						photonView.RPC("receiveMessage", p, "[00CCFF]"+player.Handle+": [-]"+text);
					}
				}
				mInput.value = "";
			}
		}
	}

	bool testCommands(string message){
		if(!message.Contains(" "))
			return false;
		string commandTest = message.Substring(0, message.IndexOf(" "));
		string targetTest = message.Substring(message.IndexOf(" ") + 1);

		switch(commandTest){
			case "/gameover":
					if(targetTest == "true" || targetTest == "True"){
						Intrigue.wantGameOver = true;
						mInput.value = "";
						textList.Add("[FFCC00]GameOver set to true");
						return true;
					}
					else if(targetTest == "false" || targetTest == "False"){
						Intrigue.wantGameOver = false;
						textList.Add("[FFCC00]GameOver set to false");
						mInput.value = "";
						return true;
					}
				break;

			case "/r":
				if(lastMessagedPlayer!=""){
					Debug.Log("Messaging : " + lastMessagedPlayer);
					foreach(PhotonPlayer p in PhotonNetwork.playerList){
						if(p!= PhotonNetwork.player && lastMessagedPlayer == (string)p.customProperties["Handle"]){
							sendPrivateMessage(commandTest, message, p);
							return true;
						}
					}	
				}else{
					Debug.Log("No recent player");
				}
				break;	

			default:
				foreach(PhotonPlayer p in PhotonNetwork.playerList){
					if(p!= PhotonNetwork.player && commandTest == ("/"+(string)p.customProperties["Handle"])){
						sendPrivateMessage(commandTest, message, p);
						return true;
					}
				}
				textList.Add("[FF0000]Error: [-][FFCC00]Could not find player: [-]" + commandTest.Substring(1, commandTest.Length-1));
				mInput.value = "";
				return true;
		}

		return false;
	}

	void sendPrivateMessage(string commandTest, string message, PhotonPlayer p){
		string newMessage = message.Substring(commandTest.Length+1, (message.Length-1)-commandTest.Length);
		photonView.RPC("receivePrivateMessage", p, "[FFCC00]["+(string)PhotonNetwork.player.customProperties["Handle"]+"]: " + newMessage + "[-]", PhotonNetwork.player.customProperties["Handle"]);
		textList.Add("[FFCC00][To] " + (string)p.customProperties["Handle"] + ": " + newMessage + "[-]");
		mInput.value = "";
	}

	[RPC]
	public void receiveMessage(string s){
		foreach(GameObject p in GameObject.FindGameObjectsWithTag((string)PhotonNetwork.player.customProperties["Team"])){
			p.GetComponentInChildren<PlayerChat>().textList.Add(s);
		}
	}

	[RPC]
	public void receivePrivateMessage(string s, string sender){
		foreach(GameObject p in GameObject.FindGameObjectsWithTag((string)PhotonNetwork.player.customProperties["Team"])){
			p.GetComponentInChildren<PlayerChat>().textList.Add(s);
			p.GetComponentInChildren<PlayerChat>().lastMessagedPlayer = sender;
		}
	}
}
