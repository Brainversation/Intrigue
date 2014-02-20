using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scoreboard : MonoBehaviour {
	
	public GameObject scoreboard;
	private PhotonView photonView = null;
	private Player player;
	public GameObject playerPrefab;
	public GameObject guardTable;
	public GameObject spyTable;
	public GameObject spyTeam;
	public GameObject guardTeam;
	private List<string> spiesL = new List<string>();
	private List<string> guardsL = new List<string>();
	private GameObject [] spies;
	private GameObject [] guards;
	
	// Use this for initialization
	void Start () {
		scoreboard.GetComponent<UIPanel>().alpha = 0;
		this.photonView = PhotonView.Get(this);
		player = GameObject.Find("Player").GetComponent<Player>();

		for(int i=0; i<5; i++){
			GameObject playerScoreInstance= NGUITools.AddChild(guardTable, playerPrefab);
			Vector3 temp = new Vector3(0f,(i)*30f,0f);
			playerScoreInstance.transform.localPosition -= temp;
			UILabel label = playerScoreInstance.GetComponent<UILabel>();
			label.user = "";
			label.text = "";

			GameObject playerScoreInstance2= NGUITools.AddChild(spyTable, playerPrefab);
			playerScoreInstance2.transform.localPosition -= temp;
			UILabel label2 = playerScoreInstance2.GetComponent<UILabel>();
			label2.user = "";
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


		if(player.TeamID==1){
			spyTeam.GetComponent<UILabel>().text = "Spies : " + player.TeamScore;
			guardTeam.GetComponent<UILabel>().text = "Guards: " + player.EnemyScore;
		}

		spies = GameObject.FindGameObjectsWithTag("Spy");
		guards = GameObject.FindGameObjectsWithTag("Guard");
		
		foreach(GameObject sp in spies){
			Spy spI = sp.GetComponent<Spy>();
			if(spI.localHandle!=""){
				foreach(Transform child in spyTable.transform){
					if(child.gameObject.GetComponent<UILabel>().user == spI.localHandle){
							int ping = spI.localPing;
							string pingColor = "[000000]";
								if (ping<50)
									pingColor = "[00FF00]";
								else if(ping<100)
									pingColor = "[FF9D00]";
								else
									pingColor = "[FF0000]";		
						child.gameObject.GetComponent<UILabel>().text = "[000000]" + spI.localHandle + " - Score: " + spI.remoteScore + " ("+ pingColor+ping+"[-]" + ") ms";	
					}
					else if(child.gameObject.GetComponent<UILabel>().user =="" && !spI.isAssigned){
						child.gameObject.GetComponent<UILabel>().user = spI.localHandle;
						spI.isAssigned = true;
					}
				}
			}
		}

	}


}
