using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scoreboard : MonoBehaviour {
	
	public GameObject scoreboard;
	//private PhotonView photonView = null;
	private Player player;
	public GameObject playerPrefab;
	public GameObject guardTable;
	public GameObject spyTable;
	public GameObject spyTeam;
	public GameObject guardTeam;
	public GameObject serverStats;
	private GameObject [] spies;
	private GameObject [] guards;
	private GameObject [] servers;
	private int [] serverCompletions = new int [] {0,0,0};

	// Use this for initialization
	void Start () {
		scoreboard.GetComponent<UIPanel>().alpha = 0;
		//this.photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();
		servers = GameObject.FindGameObjectsWithTag("ObjectiveMain");

		for(int i=0; i<5; i++){
			GameObject playerScoreInstance= NGUITools.AddChild(guardTable, playerPrefab);
			Vector3 temp = new Vector3(0f,(i)*30f,0f);
			playerScoreInstance.transform.localPosition -= temp;
			UILabel label = playerScoreInstance.GetComponent<UILabel>();
			label.user = -1;
			label.text = "";

			GameObject playerScoreInstance2= NGUITools.AddChild(spyTable, playerPrefab);
			playerScoreInstance2.transform.localPosition -= temp;
			UILabel label2 = playerScoreInstance2.GetComponent<UILabel>();
			label2.user = -1;
			label2.text = "";
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKey(KeyCode.Tab) ){
			scoreboard.GetComponent<UIPanel>().alpha = 1;
		}
		else
			scoreboard.GetComponent<UIPanel>().alpha = 0;

		if(player.Team=="Spy"){
			spyTeam.GetComponent<UILabel>().text = "Spies : " + player.TeamScore;
			guardTeam.GetComponent<UILabel>().text = "Guards : " + player.EnemyScore;
		}
		else{
			spyTeam.GetComponent<UILabel>().text = "Spies : " + player.EnemyScore;
			guardTeam.GetComponent<UILabel>().text = "Guards : " + player.TeamScore;
		}

		spies = GameObject.FindGameObjectsWithTag("Spy");
		guards = GameObject.FindGameObjectsWithTag("Guard");
		
		//SERVER STATS
		foreach(GameObject serv in servers){
			int curServ = serv.GetComponent<ObjectiveMain>().objectiveName-1;
			serverCompletions[curServ] = serv.GetComponent<ObjectiveMain>().completionPercentage;
		}

		serverStats.GetComponent<UILabel>().text = "SERVERS:\n1: [FF0000]" + serverCompletions[0] + "%[-] 2: [FF0000]" + serverCompletions[1] + "%[-] 3:[FF0000] " + serverCompletions[2] + "%[-]";






		foreach(GameObject sp in spies){
			Spy spI = sp.GetComponent<Spy>();
			if(spI.localHandle!=""){
				foreach(Transform child in spyTable.transform){
					if(child.gameObject.GetComponent<UILabel>().labelHandle == spI.localHandle){
							int ping = spI.localPing;
							string pingColor = "[000000]";
								if (ping<50)
									pingColor = "[00FF00]";
								else if(ping<100)
									pingColor = "[FF9D00]";
								else
									pingColor = "[FF0000]";		
						child.gameObject.GetComponent<UILabel>().text = "[FFFFFF]" + spI.localHandle + "[-] - " + spI.remoteScore + "pts - ping("+ pingColor+ping+"[-]" + ")";	
					}
					else if(child.gameObject.GetComponent<UILabel>().labelHandle == "" && !spI.isAssigned){
						child.gameObject.GetComponent<UILabel>().labelHandle = spI.localHandle;
						spI.isAssigned = true;
					}
				}
			}
		}

		foreach(GameObject gu in guards){
			Guard guI = gu.GetComponent<Guard>();
			if(guI.localHandle!="" && guI.localHandle!="No Handle"){
				foreach(Transform child in guardTable.transform){
					if(child.gameObject.GetComponent<UILabel>().labelHandle == guI.localHandle){
							int ping = guI.localPing;
							string pingColor = "[000000]";
								if (ping<50)
									pingColor = "[00FF00]";
								else if(ping<100)
									pingColor = "[FF9D00]";
								else
									pingColor = "[FF0000]";		
						child.gameObject.GetComponent<UILabel>().text = "[FFFFFF]" + guI.localHandle + "[-] - " + guI.remoteScore + "pts - ping ("+ pingColor+ping+"[-]" + ") ms";	
					}
					else if(child.gameObject.GetComponent<UILabel>().labelHandle == "" && !guI.isAssigned){
						child.gameObject.GetComponent<UILabel>().labelHandle = guI.localHandle;
						guI.isAssigned = true;
					}
				}
			}
		}

	}

	[RPC]
	void removeName(string handle, string team, int playerID){
		// Debug.Log("Removing: " + handle);
		bool removed = false;
		float removedHeight=0;
		GameObject curTable;
		if(team=="Spy")
			curTable = spyTable;
		else
			curTable = guardTable;

		foreach(Transform child in curTable.transform){
			if(child.gameObject.GetComponent<UILabel>().user == playerID){
				removedHeight = child.localPosition.y;
				NGUITools.Destroy(child.gameObject);
				removed = true;
			}
		}
		if(removed){
			foreach(Transform child in curTable.transform){
				Vector3 temp = new Vector3(0f, 0.1f,0);
				if(Mathf.RoundToInt(child.gameObject.transform.localPosition.y)<Mathf.RoundToInt(removedHeight)){
					child.gameObject.transform.position+=temp;
				}
			}
		}
	}


}
