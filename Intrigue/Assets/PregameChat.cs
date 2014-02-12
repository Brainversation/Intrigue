using UnityEngine;

/// <summary>
/// Very simple example of how to use a TextList with a UIInput for chat.
/// </summary>

[RequireComponent(typeof(UIInput))]
[AddComponentMenu("NGUI/Examples/Chat Input")]
public class PregameChat : MonoBehaviour
{
	public UITextList textList;
	private PhotonView photonView = null;

	UIInput mInput;

	void Start ()
	{
		this.photonView = PhotonView.Get(this);
		PhotonNetwork.isMessageQueueRunning = true;
		Debug.Log("pv"+ this.photonView);
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
			// It's a good idea to strip out all symbols as we don't want user input to alter colors, add new lines, etc
			string text = NGUIText.StripSymbols(mInput.value);

			if (!string.IsNullOrEmpty(text))
			{
				textList.Add(text);
				photonView.RPC("recieveMessage", PhotonTargets.OthersBuffered, text);
				mInput.value = "";
			}
		}
	}

	[RPC]
	public void recieveMessage(string s){
		textList.Add(s);
	}
}
