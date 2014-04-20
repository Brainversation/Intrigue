using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

namespace RBS{
	class WantToMoveRoom : Rule{
		private GameObject go;
		public WantToMoveRoom(GameObject gameObject){
			this.go = gameObject;
			this.addCondition(new IsContent(gameObject));
			this.consequence = goToRoom;
			this.antiConsequence = stopMoving;
			this.weight = 7;
		}

		private Status goToRoom(GameObject gameObject){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			GameObject curRoom = script.room.me;
			GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
			GameObject room = rooms[UnityEngine.Random.Range(0, rooms.Length)];

			//ensure next room to go to is not the same as the current room
			while(room == curRoom){
				room = rooms[UnityEngine.Random.Range(0, rooms.Length)];
			}

			// Debug.Log("Room: " + room.name);

			//Pick spot inside collider of chosen room
			Vector3 newDest;
            newDest = new Vector3(UnityEngine.Random.Range(room.GetComponent<BoxCollider>().bounds.min.x,
                                  room.GetComponent<BoxCollider>().bounds.max.x),
                                  gameObject.transform.position.y,
                                  UnityEngine.Random.Range(room.GetComponent<BoxCollider>().bounds.min.z,
                                  room.GetComponent<BoxCollider>().bounds.max.z));

			script.bored -= 20;
			script.lonely -= 20;
			script.anxiety -= 20;

            //Set BaseAI variables and run
            script.distFromDest = 5f;
            script.agent.SetDestination(newDest);
            script.anim.SetBool("Speed", true);
            return Status.Waiting;
		}

		private Status stopMoving(){
			this.go.GetComponent<BaseAI>.anim.SetBool("Speed", false);
			return Status.True;
		}
	}

	class WantToWanderRoom : Rule{
		private GameObject go;
		public WantToWanderRoom(GameObject gameObject){
			this.go = gameObject;
			this.addCondition(new IsBored(gameObject));
			this.consequence = wanderRoom;
			this.antiConsequence = stopMoving;
			this.weight = 7;
		}

		private Status wanderRoom(GameObject gameObject){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			GameObject room = script.room.me;

			//Choose random point inside curRoom collider
			Vector3 newDest;
            newDest = new Vector3(UnityEngine.Random.Range(room.GetComponent<BoxCollider>().bounds.min.x,
                                  room.GetComponent<BoxCollider>().bounds.max.x),
                                  gameObject.transform.position.y,
                                  UnityEngine.Random.Range(room.GetComponent<BoxCollider>().bounds.min.z,
                                  room.GetComponent<BoxCollider>().bounds.max.z));

            //Following ensures that chosen random point is on navMesh, currently probably not useful.
/*            
            script.agent.CalculatePath(newDest, path);

            while(path.status ==  NavMeshPathStatus.PathPartial){
            	newDest = new Vector3(UnityEngine.Random.Range(room.GetComponent<BoxCollider>().bounds.min.x,
                                      room.GetComponent<BoxCollider>().bounds.max.x),
                                      gameObject.transform.position.y,
                                      UnityEngine.Random.Range(room.GetComponent<BoxCollider>().bounds.min.z,
                                      room.GetComponent<BoxCollider>().bounds.max.z));

            	script.agent.CalculatePath(newDest, path);
            }
*/
			script.bored -= 20;
            script.anim.SetBool("Speed", true);
            script.agent.SetDestination(newDest);
            return Status.Waiting;
		}

		private Status stopMoving(){
			this.go.GetComponent<BaseAI>.anim.SetBool("Speed", false);
			return Status.True;
		}

	}

	class WantToGetDrink : Rule{
		private GameObject go;
		public WantToGetDrink(GameObject gameObject) {
			this.addCondition(new IsThirsty(gameObject));
			this.addCondition(new IsBored(gameObject));
			this.consequence = setDestRoom;
			this.antiConsequence = stopDrinking;
			this.weight = 7;
			this.go = gameObject;
		}

		private Status setDestRoom(GameObject gameObject){
			// Debug.Log("Wants a drink");
			BaseAI script = gameObject.GetComponent<BaseAI>();
			script.bored -= 10;
			script.thirst -= 25;

			//Check room for hotspot location
			if(script.room.drinkLocation != Vector3.zero){
				script.destination = script.room.drinkLocation;
			}
			//Choose random hotspot location
			else{
				GameObject[] drinkLocations = GameObject.FindGameObjectsWithTag("Drink");
				GameObject drinkLocation = drinkLocations[UnityEngine.Random.Range(0, drinkLocations.Length)];
				script.destination = drinkLocation.transform.position;
			}
			script.anim.SetBool("Speed", true);
			gameObject.GetComponent<BaseAI>().distFromDest = 10f;
			script.agent.SetDestination(script.destination);
			script.tree = new DrinkingTree(gameObject);
			Debug.DrawLine(gameObject.transform.position, script.destination, Color.red, 115f, false);
			return Status.Waiting;
		}

		private Status stopDrinking(){
			this.go.GetComponent<Animator>().SetBool("Drink", false);
			return Status.True;
		}
	}

	class WantToConverse : Rule{
		protected int offset;

		public WantToConverse(GameObject gameObject){
			offset = ConversationHotSpot.max;
			this.addCondition( new IsLonely(gameObject) );
			this.addCondition( new IsBored(gameObject) );
			this.addCondition( new NotInConvo(gameObject) );
			this.consequence = handleConverse;
			this.weight = 4;
		}

		private Status handleConverse(GameObject gameObject){
			// Debug.Log("Wants to converse");
			Status returnStat;
			List<GameObject> conversers;
			BaseAI script = gameObject.GetComponent<BaseAI>();
			if(script.room != null)
				conversers = script.room.conversers;
			else
				return Status.False;
			script.lonely -= 20;
			script.bored -= 10;
			if(conversers.Count == 0 || conversers.Count >= offset){
				script.destination = gameObject.transform.position;
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ConversationHotSpot"), gameObject.transform.position, Quaternion.identity);
				script.tree = new IdleSelector();
				conversers.Clear();
				returnStat = Status.Tree;
			} else {
				script.destination = conversers[0].transform.position;
				script.anim.SetBool("Speed", true);
				script.agent.SetDestination(script.destination);
				returnStat = Status.Waiting;
			}
			conversers.Add(gameObject);
			Debug.DrawLine(gameObject.transform.position, script.destination, Color.red, 15f, false);
			return returnStat;
		}
	}

	class NeedToUseRestroom : Rule{
		public NeedToUseRestroom(GameObject gameObject){
			this.addCondition( new IsBursting(gameObject) );
			this.consequence = setDestRestroom;
		}

		private Status setDestRestroom(GameObject gameObject){
			// Debug.Log("needs to use restroom");
			BaseAI script = gameObject.GetComponent<BaseAI>();
			script.bladder -= 25;

			//Check if room has hotspot
			if(script.room.restroomLocation != Vector3.zero){
				script.destination = script.room.restroomLocation; //script.room.restroomLocation.position;
				script.anim.SetBool("Speed", true);
				gameObject.GetComponent<BaseAI>().distFromDest = 5f;
				script.agent.SetDestination(script.destination);
			}
			//Find random hotspot
			else{
				GameObject[] bathroomLocations = GameObject.FindGameObjectsWithTag("RestRoom");
				GameObject bathroomLocation = bathroomLocations[UnityEngine.Random.Range(0, bathroomLocations.Length)];
				script.destination = bathroomLocation.transform.position; //script.room.restroomLocation.position;
				script.anim.SetBool("Speed", true);
				gameObject.GetComponent<BaseAI>().distFromDest = 5f;
				script.agent.SetDestination(script.destination);
			}
			Debug.DrawLine(gameObject.transform.position, script.destination, Color.red, 15f, false);
			return Status.Waiting;
		}
	}

	class AdmireArt : Rule{
		public AdmireArt(GameObject gameObject){
			this.addCondition(new HasArt(gameObject));
			this.addCondition(new IsBored(gameObject));
			this.addCondition(new ConversationInRoom(gameObject));
			this.consequence = goToArt;
		}

		private Status goToArt(GameObject gameObject){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			script.bored -= 25;

			int minIndex = 0;
			for(int i = 1; i < script.room.artLocations.Count; ++i){
				if(Vector3.Distance(script.room.artLocations[i-1], gameObject.transform.position) <
						Vector3.Distance(script.room.artLocations[i], gameObject.transform.position)){
					minIndex = (i-1);
				}
			}

			script.destination = script.room.artLocations[minIndex];
			script.anim.SetBool("Speed", true);
			gameObject.GetComponent<BaseAI>().distFromDest = 5f;
			script.agent.SetDestination(script.destination);

			Debug.DrawLine(gameObject.transform.position, script.destination, Color.red, 15f, false);
			return Status.Waiting;
		}
	}

	class FindRoom : Rule{
		public FindRoom(GameObject gameObject){
			this.addCondition(new NotInRoom(gameObject));
			this.consequence = goToRoom;
			this.weight = 1000;
		}

		private Status goToRoom(GameObject gameObject){
			BaseAI script = gameObject.GetComponent<BaseAI>();

			int minIndex = 0;
			GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
			for(int i = 1; i < rooms.Length; ++i){
				if(Vector3.Distance(rooms[i-1].transform.position, gameObject.transform.position) <
						Vector3.Distance(rooms[i].transform.position, gameObject.transform.position)){
					minIndex = (i-1);
				}
			}

			script.destination = rooms[minIndex].transform.position;
			script.anim.SetBool("Speed", true);
			gameObject.GetComponent<BaseAI>().distFromDest = 5f;
			script.agent.SetDestination(script.destination);

			Debug.DrawLine(gameObject.transform.position, script.destination, Color.red, 15f, false);
			return Status.Waiting;
		}
	}

	class Relax : Rule{
		public Relax(GameObject gameObject){
			this.addCondition(new IsAnxious(gameObject));
			this.addCondition(new NotBored(gameObject));
			this.addCondition(new IsTired(gameObject));
			this.consequence = goRelax;
		}

		private Status goRelax(GameObject gameObject){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			script.anxiety -= 15;
			script.tired -= 15;
			script.bored += 15;

			//Check if room has hotspot
			if(script.room.relaxLocation != Vector3.zero){
				script.destination = script.room.relaxLocation;
				script.anim.SetBool("Speed", true);
				gameObject.GetComponent<BaseAI>().distFromDest = 5f;
				script.agent.SetDestination(script.destination);
			}
			//Find couch hotspot somewhere
			else{
				Debug.Log("Find a relaxLocation");
			}

			Debug.DrawLine(gameObject.transform.position, script.destination, Color.red, 15f, false);
			return Status.Waiting;
		}
	}

	class LetOffSteam : Rule{
		public LetOffSteam(GameObject gameObject){
			this.addCondition(new IsAnxious(gameObject));
			this.addCondition(new NotBored(gameObject));
			this.addCondition(new IsAngry(gameObject));
			this.consequence = coolDown;
		}

		private Status coolDown(GameObject gameObject){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			script.anxiety -= 15;
			script.tired -= 15;
			script.bored += 15;

			//Check if room has hotspot
			if(script.room.relaxLocation != Vector3.zero){
				script.destination = script.room.relaxLocation;
				script.anim.SetBool("Speed", true);
				gameObject.GetComponent<BaseAI>().distFromDest = 5f;
				script.agent.SetDestination(script.destination);
			} else{
				//Find couch hotspot somewhere
				Debug.Log("Find a relaxLocation");
			}

			Debug.DrawLine(gameObject.transform.position, script.destination, Color.red, 15f, false);
			return Status.Waiting;
		}
	}

	class Smoke : Rule{
		public Smoke(GameObject gameObject){
			this.addCondition(new IsAnxious(gameObject));
			this.addCondition(new IsBored(gameObject));
			this.addCondition(new IsSmoker(gameObject));
		}

		private Status goSmoke(GameObject gameObject){
			BaseAI script = gameObject.GetComponent<BaseAI>();

			if(script.room.relaxLocation != Vector3.zero){
				script.destination = script.room.relaxLocation;
				script.anim.SetBool("Speed", true);
				gameObject.GetComponent<BaseAI>().distFromDest = 5f;
				script.agent.SetDestination(script.destination);
			}
			return Status.Waiting;
		}
	}

	class ReadPoetry : Rule{
		public ReadPoetry(GameObject gameObject){
			this.addCondition(new IsHappy(gameObject));
			this.addCondition(new IsNotAnxious(gameObject));
			this.addCondition(new IsNoPoet(gameObject));
		}

		private Status doPoetry(GameObject gameObject){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			script.bored -= 15;
			script.tired += 15;

			script.destination = script.room.poetLocation;
			script.anim.SetBool("Speed", true);
			gameObject.GetComponent<BaseAI>().distFromDest = 5f;
			script.agent.SetDestination(script.destination);

			return Status.Waiting;
		}
	}

	class DoIdle : Rule{
		public DoIdle(GameObject gameObject){
			this.addCondition(new StayStill());
			this.consequence = stay;
		}

		private Status stay(GameObject gameObject){
			gameObject.GetComponent<Animator>().CrossFade("Idle", 1f);
			return Status.True;
		}
	}
}