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
	private GameObject[] team;
	private bool mIgnoreUp = false;

	UIInput mInput;

	void Start ()
	{
		//this.photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();
		mInput = GetComponent<UIInput>();
		mInput.label.maxLineCount = 1;
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
		if(UICamera.currentKey == KeyCode.Return);
			mIgnoreUp = true;

		if (textList != null)
		{	
			if(player.Team == "Spy"){
				team = GameObject.FindGameObjectsWithTag("Spy");
			}
			else{
				team = GameObject.FindGameObjectsWithTag("Guard");
			}
			// It's a good idea to strip out all symbols as we don't want user input to alter colors, add new lines, etc
			string text = NGUIText.StripSymbols(mInput.value);
			text = StringCleaner.CleanString(text);

			if (!string.IsNullOrEmpty(text) && text.Length>=1){
				if(player.Team == "Spy"){
					foreach(GameObject gu in team){
						gu.GetComponent<Spy>().photonView.RPC("receiveMessage", PhotonTargets.All, "[00CCFF]"+player.Handle+": [-]"+text);
					}

				} else if(player.Team == "Guard") {
					foreach(GameObject gu in team){
						gu.GetComponent<Guard>().photonView.RPC("receiveMessage", PhotonTargets.All, "[FF2B2B]"+player.Handle+": [-]"+text);
					}
				} 
				mInput.value = "";
			}
		}
	}

	[RPC]
	public void receiveMessage(string s){
		textList.Add(s);
	}
}
