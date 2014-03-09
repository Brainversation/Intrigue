using UnityEngine;
using System.Collections;

public class Spy : BasePlayer{
	
	private UIPanel objPanel;
	private UISlider objSlider;
	public float percentComplete = 0;
	public bool doingObjective = false;
	public string objectiveType;

	void Update () {
		int minutesLeft = Mathf.RoundToInt(Mathf.Floor(intrigue.GetTimeLeft/60));
		int seconds = Mathf.RoundToInt(intrigue.GetTimeLeft%60);
		string secondsS;
		if(seconds<10)
			secondsS = "0"+seconds.ToString();
		else
			secondsS = seconds.ToString();
		int curRound = intrigue.GetRounds - intrigue.GetRoundsLeft +1;
		guiLabels = GetComponentsInChildren<UILabel>();
		guiPanels = GetComponentsInChildren<UIPanel>(true);
		foreach(UIPanel uiP in guiPanels){
			if(uiP.gameObject.CompareTag("ObjectivePanel")){
				objPanel = uiP;
			}
		}

		NGUITools.SetActive(objPanel.gameObject, doingObjective);
		if(doingObjective){
			objSlider = objPanel.GetComponentInChildren<UISlider>();
			objSlider.value = percentComplete;
		}

		foreach(UILabel lab in guiLabels){
			if(lab.gameObject.CompareTag("TimeLabel")){
				timeLabel = lab.gameObject;
			}
			else if(lab.gameObject.CompareTag("OutLabel")){
				outLabel = lab.gameObject;
			}
		}

		if(isOut){
			NGUITools.SetActive(outLabel, true);
			if(hairHat!=null)
				hairHat.GetComponent<Renderer>().enabled = true;
		}
		else{
			NGUITools.SetActive(outLabel, false);
		}

		if(timeLabel!=null)
			timeLabel.GetComponent<UILabel>().text = minutesLeft +":" + secondsS + "\nRound: " + curRound +"/" + (intrigue.GetRounds+1);

		//Interact Raycasts
		if(Camera.main!=null){
			Ray ray = Camera.main.ScreenPointToRay( screenPoint );
			if (Input.GetKey("e")){
				RaycastHit hit;
				Debug.DrawRay(ray.origin,ray.direction*15f,Color.green);
				if( Physics.Raycast(ray, out hit, 15.0f) ){
					if( hit.transform.tag == "Objective" ){
						Objective hitObjective = hit.transform.GetComponent<Objective>();
						hitObjective.useObjective(gameObject);
						objectiveType = hitObjective.objectiveType;
					}
					else
						doingObjective = false;
				}
				else
						doingObjective = false;
			}
			else
				doingObjective = false;
		}

		//Create Ally and Objective Texts
		GameObject[] allies = GameObject.FindGameObjectsWithTag("Spy");
		GameObject[] objecs = GameObject.FindGameObjectsWithTag("Objective");
		foreach(GameObject ally in allies){
			if(ally!=gameObject){
				if(!ally.GetComponent<Spy>().textAdded){
					//Debug.Log("creating ally text");
					ally.GetComponent<Spy>().textAdded = true;
					GameObject textInstance = Instantiate(allytext, ally.transform.position,ally.transform.rotation) as GameObject;
					textInstance.GetComponent<AllyText>().target = ally.transform;
					textInstance.transform.parent = ally.transform;
					textInstance.GetComponent<TextMesh>().text = ally.GetComponent<Spy>().localHandle;
				}
				if((ally.GetComponentInChildren<TextMesh>().text == "") && ally.GetComponent<Spy>().textAdded){
					//Debug.Log("Changing Handle from: " + ally.GetComponentInChildren<TextMesh>().text + " to:" + ally.GetComponent<Spy>().localHandle);
					ally.GetComponentInChildren<TextMesh>().text = ally.GetComponent<Spy>().localHandle;
					
				}
			}
		}

		foreach(GameObject objer in objecs){
				if(!objer.GetComponent<Objective>().textAdded && objer.GetComponent<Objective>().isActive){
					objer.GetComponent<Objective>().textAdded = true;
					GameObject textInstance = Instantiate(allytext, objer.transform.position, objer.transform.rotation) as GameObject;
					Vector3 temp = new Vector3(1.5f,-0.75f,0f);
					Vector3 temp2 = new Vector3(270,0,0);
					textInstance.transform.Rotate(temp2);
					textInstance.GetComponent<AllyText>().offset = temp;
					textInstance.transform.localScale += new Vector3(0.5f,0.5f,0.5f);
					textInstance.GetComponent<AllyText>().target = objer.transform;
					textInstance.transform.parent = objer.transform;
					textInstance.GetComponent<TextMesh>().text = "[ ]";
				}
				else if (!objer.GetComponent<Objective>().isActive && objer.GetComponent<Objective>().textAdded){
					objer.GetComponentInChildren<TextMesh>().text = "";
				}
		}
	}

	[RPC]
	void destroySpy(){
		if( photonView.isMine){
			isOut = true;
			gameObject.GetComponent<NetworkCharacter>().isOut = true;
		}
	}

		[RPC]
	void giveHandle(string handle){
		localHandle = handle;
	}

	[RPC]
	void giveScore(int score){
		remoteScore = score;
	}

	[RPC]
	void givePing(int ping){
		localPing = ping;
	}

	[RPC]
	void addPlayerScore(int scoreToAdd){
		if(photonView.isMine){
			player.Score += scoreToAdd;
			photonView.RPC("giveScore", PhotonTargets.All, player.Score);
		}
	}
}