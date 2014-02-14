using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private string handle = "";
	private string roomName = "";
	private string team = "";
	private int teamID = 0;
	private int numberOfGuests = 0;
	private int score = 0;
	private int teamScore = 0;
	private int enemyScore = 0;

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

	public int TeamScore {
		get{
			return this.teamScore;
		}
		set{
			this.teamScore = value;
		}
	}

	public int EnemyScore {
		get{
			return this.enemyScore;
		}
		set{
			this.enemyScore = value;
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

	public int Score {
		get{
			return this.score;
		}
		set{
			this.score = value;
		}
	}
}
