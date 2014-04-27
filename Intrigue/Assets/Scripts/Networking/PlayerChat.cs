using UnityEngine;

/// <summary>
/// Very simple example of how to use a TextList with a UIInput for chat.
/// </summary>

[RequireComponent(typeof(UIInput))]
[AddComponentMenu("NGUI/Examples/Chat Input")]
public class PlayerChat : MonoBehaviour
{
	public UITextList textList;
	// private PhotonView photonView = null;
	private Player player;
	private GameObject[] team;

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

	public void OnSubmit ()
	{
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

			if (!string.IsNullOrEmpty(text)){
				if(player.Team == "Spy"){
					foreach(GameObject gu in team){
						gu.GetComponent<Spy>().photonView.RPC("recieveMessage", PhotonTargets.All, "[FF2B2B]"+player.Handle+": [-]"+text);
					}

				} else if(player.Team == "Guard") {
					foreach(GameObject gu in team){
						gu.GetComponent<Guard>().photonView.RPC("recieveMessage", PhotonTargets.All, "[FF2B2B]"+player.Handle+": [-]"+text);
					}
				} 
				mInput.value = "";
			}
		}
	}

	[RPC]
	public void recieveMessage(string s){
		textList.Add(s);
	}
}
