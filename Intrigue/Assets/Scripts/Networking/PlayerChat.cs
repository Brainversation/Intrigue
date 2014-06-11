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
		//SET TO ALLOW CHAT COMMANDS
	private static bool debugMode = true;
	private Player player;
	private bool mIgnoreUp = false;
	private PhotonView photonView = null;
	public string lastMessagedPlayer = "";

	UIInput mInput;

	void Start ()
	{
		player = Player.Instance;
		mInput = GetComponent<UIInput>();
		mInput.label.maxLineCount = 1;
		photonView = PhotonView.Get(this);
		window.GetComponent<UISprite>().alpha = 0;
		gameObject.GetComponent<UIInput>().enabled = false;	
		UICamera.inputHasFocus = false;
		UICamera.selectedObject = null;
		gameObject.GetComponent<UIInput>().enabled = false;
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

		if(Input.GetKeyUp(KeyCode.Return)){
			if(window.GetComponent<UISprite>().alpha == 0){
				if(mInput){
					if(!mIgnoreUp && !UICamera.inputHasFocus)
						UICamera.selectedObject = gameObject;
					mIgnoreUp = false;
				}
				else{
					UICamera.selectedObject = gameObject;
				}
				window.GetComponent<UISprite>().alpha = 1;
				gameObject.GetComponent<UIInput>().enabled = true;
			}
			else{
				window.GetComponent<UISprite>().alpha = 0;
				gameObject.GetComponent<UIInput>().enabled = false;	
				UICamera.inputHasFocus = false;
				UICamera.selectedObject = null;
				gameObject.GetComponent<UIInput>().enabled = false;		
			}
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
			text = StringCleaner.CleanString(text);
			isCommand = testCommands(text);
			
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
				if(debugMode){
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
				}
			break;

			case "/free":
				if(debugMode){
					if(targetTest == "true" || targetTest == "True"){
						toggleUI(0);
						Intrigue.playerGO.GetComponent<NetworkCharacter>().gravityToggle = false;
						Intrigue.playerGO.GetComponent<NetworkCharacter>().infiniteRun = true;
						Renderer[] rend = Intrigue.playerGO.GetComponentsInChildren<Renderer>();
						foreach(Renderer r in rend){
							r.enabled = false;
						}
						Intrigue.playerGO.layer = 19;
						mInput.value = "";
						return true;
					}
					else if(targetTest == "false" || targetTest == "False"){
						toggleUI(1);
						Intrigue.playerGO.GetComponent<NetworkCharacter>().gravityToggle = true;
						Intrigue.playerGO.GetComponent<NetworkCharacter>().infiniteRun = false;
						if(player.Handle == "Guard"){
							Intrigue.playerGO.layer = BasePlayer.GUARD;
						} else {
							Intrigue.playerGO.layer = BasePlayer.SPY;
						}
						mInput.value = "";
						return true;
					}
				}
			break;

			case "/ui":
				if(debugMode){
					if(targetTest == "true" || targetTest == "True"){
						toggleUI(1);
						mInput.value = "";
						return true;
					}
					else if(targetTest == "false" || targetTest == "False"){
						toggleUI(0);
						mInput.value = "";
						return true;
					}
				}
			break;

			case "/gravity":
					if(debugMode){
						if(targetTest == "true" || targetTest == "True"){
								Intrigue.playerGO.GetComponent<NetworkCharacter>().gravityToggle = true;
								mInput.value = "";
								textList.Add("[FFCC00]Gravity set to true");
								return true;
							}
							else if(targetTest == "false" || targetTest == "False"){
								Intrigue.playerGO.GetComponent<NetworkCharacter>().gravityToggle = false;
								textList.Add("[FFCC00]Gravity set to false");
								mInput.value = "";
								return true;
							}
					}
			break;

			case "/supersprint":
					if(debugMode){
						if(targetTest == "true" || targetTest == "True"){
								Intrigue.playerGO.GetComponent<NetworkCharacter>().infiniteRun = true;
								mInput.value = "";
								textList.Add("[FFCC00]SuperSprint set to true");
								return true;
							}
							else if(targetTest == "false" || targetTest == "False"){
								Intrigue.playerGO.GetComponent<NetworkCharacter>().infiniteRun = false;
								textList.Add("[FFCC00]SuperSprint set to false");
								mInput.value = "";
								return true;
							}
					}
			break;

			case "/r":
				if(lastMessagedPlayer!=""){
					// Debug.Log("Messaging : " + lastMessagedPlayer);
					foreach(PhotonPlayer p in PhotonNetwork.playerList){
						if(p!= PhotonNetwork.player && lastMessagedPlayer == (string)p.customProperties["Handle"]){
							sendPrivateMessage(commandTest, message, p);
							return true;
						}
					}	
				}else{
					textList.Add("[D60004]No recent player");
				}
				break;	

			default:
				foreach(PhotonPlayer p in PhotonNetwork.playerList){
					if(p!= PhotonNetwork.player && commandTest == ("/"+(string)p.customProperties["Handle"])){
						sendPrivateMessage(commandTest, message, p);
						return true;
					}
				}
				break;

		}

		return false;
	}

	void toggleUI(int alpha){
		UIRect[] a = UIRoot.list[0].GetComponentsInChildren<UIRect>();
		foreach(UIRect r in a){
			r.alpha = alpha;
		}
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
			p.GetComponent<BasePlayer>().receiveMessage(s);
		}
	}

	[RPC]
	public void receivePrivateMessage(string s, string sender){
		foreach(GameObject p in GameObject.FindGameObjectsWithTag((string)PhotonNetwork.player.customProperties["Team"])){
			p.GetComponent<BasePlayer>().receiveMessage(s);
			p.GetComponentInChildren<PlayerChat>().lastMessagedPlayer = sender;
		}
	}
}
