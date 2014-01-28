using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	string handle = "";
	string roomName = "";
	string team = "";
	int numberOfGuests = 0;
	int score = 0;

	void Awake(){
		DontDestroyOnLoad(transform.gameObject);
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
