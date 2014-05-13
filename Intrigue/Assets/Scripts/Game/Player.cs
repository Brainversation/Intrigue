using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private string handle = "";
	private string roomName = "";
	private string team = "";
	private string result = "";
	private int teamID = 0;
	private int numberOfGuests = 0;
	private int score = 0;
	private int team1Score = 0;
	private int team2Score = 0;
	private bool ready = false;

	void Awake(){
		DontDestroyOnLoad(transform.gameObject);
	}

	public int TeamID {
		get{
			return this.teamID;
		}
		set{
			this.teamID = value;
		}
	}

	public int Team1Score {
		get{
			return this.team1Score;
		}
		set{
			this.team1Score = value;
		}
	}

	public int Team2Score {
		get{
			return this.team2Score;
		}
		set{
			this.team2Score = value;
		}
	}

	public string Handle {
		get{
			return this.handle;
		}
		set{
			this.handle = value;
		}
	}

	public string RoomName {
		get{
			return this.roomName;
		}
		set{
			this.roomName = value;
		}
	}

	public string Team {
		get{
			return this.team;
		}
		set{
			this.team = value;
		}
	}

	public int Guests {
		get{
			return this.numberOfGuests;
		}
		set{
			this.numberOfGuests = value;
		}
	}

	public bool Ready{
		get{
			return this.ready;
		}
		set{
			this.ready = value;
		}
	}

	public int Score {
		get{
			return this.score;
		}
		set{
			this.score = value;
		}
	}

	public string PrevResult {
		get{
			return this.result;
		}
		set{
			this.result = value;
		}
	}
}
