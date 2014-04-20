//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// This class makes it possible to activate or select something by pressing a key (such as space bar for example).
/// </summary>

[AddComponentMenu("Game/UI/Key Binding")]
public class UIKeyBinding : MonoBehaviour
{
	public enum Action
	{
		PressAndClick,
		Select,
		Toggle,
		Deselect,
	}

	public enum Modifier
	{
		None,
		Shift,
		Control,
		Alt,
	}

	/// <summary>
	/// Key that will trigger the binding.
	/// </summary>

	public KeyCode keyCode = KeyCode.None;

	/// <summary>
	/// Modifier key that must be active in order for the binding to trigger.
	/// </summary>

	public Modifier modifier = Modifier.None;

	/// <summary>
	/// Action to take with the specified key.
	/// </summary>

	public Action action = Action.PressAndClick;

	public NetworkCharacter networkController = null;

	public MovementController movementController = null;

	public GameObject Window = null;

	bool mIgnoreUp = false;
	bool mIsInput = false;

	/// <summary>
	/// If we're bound to an input field, subscribe to its Submit notification.
	/// </summary>

	void Start ()
	{
		UIInput input = GetComponent<UIInput>();
		mIsInput = (input != null);
		if (input != null) EventDelegate.Add(input.onSubmit, OnSubmit);
		if(action == Action.Toggle){
			Window.GetComponent<UISprite>().alpha = 0;
		}
	}

	/// <summary>
	/// Ignore the KeyUp message if the input field "ate" it.
	/// </summary>

	void OnSubmit () { if (UICamera.currentKey == keyCode && IsModifierActive()) mIgnoreUp = true; }

	/// <summary>
	/// Convenience function that checks whether the required modifier key is active.
	/// </summary>

	bool IsModifierActive ()
	{
		if (modifier == Modifier.None) return true;

		if (modifier == Modifier.Alt)
		{
			if (Input.GetKey(KeyCode.LeftAlt) ||
				Input.GetKey(KeyCode.RightAlt)) return true;
		}
		else if (modifier == Modifier.Control)
		{
			if (Input.GetKey(KeyCode.LeftControl) ||
				Input.GetKey(KeyCode.RightControl)) return true;
		}
		else if (modifier == Modifier.Shift)
		{
			if (Input.GetKey(KeyCode.LeftShift) ||
				Input.GetKey(KeyCode.RightShift)) return true;
		}
		return false;
	}

	/// <summary>
	/// Process the key binding.
	/// </summary>

	void Update ()
	{
		if (keyCode == KeyCode.None || !IsModifierActive()) return;

		if (action == Action.PressAndClick)
		{
			if (UICamera.inputHasFocus) return;

			UICamera.currentTouch = UICamera.controller;
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.currentTouch.current = gameObject;

			if (Input.GetKeyDown(keyCode))
			{
				UICamera.Notify(gameObject, "OnPress", true);
			}

			if (Input.GetKeyUp(keyCode))
			{
				UICamera.Notify(gameObject, "OnPress", false);
				UICamera.Notify(gameObject, "OnClick", null);
			}
			UICamera.currentTouch.current = null;
		}
		else if (action == Action.Select)
		{
			if (Input.GetKeyUp(keyCode) && IsModifierActive())
			{
				if (mIsInput)
				{
					if (!mIgnoreUp && !UICamera.inputHasFocus)
					{
						UICamera.selectedObject = gameObject;
					}
					mIgnoreUp = false;
				}
				else
				{
					UICamera.selectedObject = gameObject;
				}
			}
		}
		else if (action == Action.Deselect)
		{
			if (Input.GetKeyUp(keyCode) && !Input.GetKey(KeyCode.LeftShift))
			{					
				UICamera.inputHasFocus = false;
				UICamera.selectedObject = null;
				Debug.Log("called");
				networkController.isChatting = false;
				movementController.enabled = true;
				toggleDisappear();
			}
		}
		else if (action == Action.Toggle)
		{
			if (Input.GetKeyUp(keyCode) && IsModifierActive())
			{
				if(Window.GetComponent<UISprite>().alpha == 0)
				{
					Window.GetComponent<UISprite>().alpha = 1;
					gameObject.GetComponentInChildren<UIInput>().enabled = true;
					Debug.Log("Turn on " + Window.GetComponent<UISprite>().alpha);
					networkController.isChatting = true;
					movementController.enabled = false;
				}
				else
				{	
					gameObject.GetComponentInChildren<UIInput>().enabled = false;
					Window.GetComponent<UISprite>().alpha = 0;
					networkController.isChatting = false;
					movementController.enabled = true;
					Debug.Log("Turn off");
				}
			}
		}
	}

	void toggleDisappear(){
		gameObject.GetComponentInChildren<UIInput>().enabled = false;
		Window.GetComponent<UISprite>().alpha = 0;
	}
}
