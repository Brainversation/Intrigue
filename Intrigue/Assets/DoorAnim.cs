using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorAnim : MonoBehaviour 
{
	public enum eInteractiveState
	{
		Active, 	//Open
		Inactive, 	//Close
	}
	
	private eInteractiveState 	m_State;
	private string[]			m_AnimNames;
	private int curInside;
	private List<GameObject> peopleInside = new List<GameObject>();

	void Start()
	{
		m_State = eInteractiveState.Inactive;
		m_AnimNames = new string[animation.GetClipCount()];
		curInside = 0;

		int index = 0;
		foreach(AnimationState anim in animation)
		{
			m_AnimNames[index] = anim.name;
			index++;
		}
	}
	
 	public void TriggerInteraction()
	{
		if(!animation.isPlaying)
			{
				switch (m_State) 
				{
				case eInteractiveState.Active:
					animation.Play(m_AnimNames[0]);
					break;
				case eInteractiveState.Inactive:
					animation.Play(m_AnimNames[1]);
					break;
				default:
				 	break;
				}
		}
	}


	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Spy")||other.gameObject.CompareTag("Guard")||other.gameObject.CompareTag("Guest"))
		{
			if(!peopleInside.Contains(other.gameObject)){
				++curInside;
				peopleInside.Add(other.gameObject);

				if(m_State == eInteractiveState.Inactive){
					m_State = eInteractiveState.Active;
					this.TriggerInteraction();
				}
			}



		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.CompareTag("Spy")||other.gameObject.CompareTag("Guard")||other.gameObject.CompareTag("Guest"))
		{
			if(peopleInside.Contains(other.gameObject)){
				peopleInside.Remove(other.gameObject);
				--curInside;

				if(curInside == 0){
					m_State = eInteractiveState.Inactive;
					this.TriggerInteraction();
				}
			}
		}
	}
}
