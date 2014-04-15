using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

namespace RBS{
	class WantToMoveRoom : Rule{
		public WantToMoveRoom(GameObject gameObject){
			this.addCondition(new isAnxious(gameObject));
			this.consequence = goToRoom;
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

            script.anxiety -= 25;

            //Set BaseAI variables and run
            script.distFromDest = 5f;
            script.agent.SetDestination(newDest);
            script.anim.SetBool("Speed", true);
            return Status.Waiting;
		}
	}

	class WantToWanderRoom : Rule{
		public WantToWanderRoom(GameObject gameObject){
			this.addCondition(new isContent(gameObject));
			this.consequence = wanderRoom;
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
            script.agent.SetDestination(newDest);
            script.anim.SetBool("Speed", true);
            return Status.Waiting;
		}

	}

	class WantToGetDrink : Rule{
		private GameObject go;
		public WantToGetDrink(GameObject gameObject) {
			this.addCondition(new isThirsty(gameObject));
			this.addCondition(new isBored(gameObject));
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
			if(script.room.drinkLocation != script.room.nullLocation){
				script.destination = script.room.drinkLocation; //script.room.drinkLocation.position;
				script.anim.SetBool("Speed", true);
				gameObject.GetComponent<BaseAI>().distFromDest = 10f;
				script.agent.SetDestination(script.destination);
				script.tree = new DrinkingTree();
			}
			//Choose random hotspot location
			else{

				GameObject[] drinkLocations = GameObject.FindGameObjectsWithTag("Drink");
				GameObject drinkLocation = drinkLocations[UnityEngine.Random.Range(0, drinkLocations.Length)];
				script.destination = drinkLocation.transform.position; //script.room.drinkLocation.position;
				script.anim.SetBool("Speed", true);
				gameObject.GetComponent<BaseAI>().distFromDest = 10f;
				script.agent.SetDestination(script.destination);
				script.tree = new DrinkingTree();
			}
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
			this.addCondition( new isLonely(gameObject) );
			this.addCondition( new isBored(gameObject) );
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
			this.addCondition( new isBursting(gameObject) );
			this.consequence = setDestRestroom;
		}

		private Status setDestRestroom(GameObject gameObject){
			// Debug.Log("needs to use restroom");
			BaseAI script = gameObject.GetComponent<BaseAI>();
			script.bladder -= 25;

			//Check if room has hotspot
			if(script.room.restroomLocation != script.room.nullLocation){
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
			this.addCondition(new hasArt(gameObject));
			this.addCondition(new isBored(gameObject));
			this.addCondition(new hasConversation(gameObject));
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
			this.addCondition(new notInRoom(gameObject));
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
			this.addCondition(new isAnxious(gameObject));
			this.addCondition(new notBored(gameObject));
			this.addCondition(new isTired(gameObject));
			this.consequence = goRelax;
		}

		private Status goRelax(GameObject gameObject){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			script.anxiety -= 15;
			script.tired -= 15;
			script.bored += 15;

			//Check if room has hotspot
			if(script.room.relaxLocation != null){
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
			this.addCondition(new isAnxious(gameObject));
			this.addCondition(new notBored(gameObject));
			this.addCondition(new isAngry(gameObject));
			this.consequence = coolDown;
		}

		private Status coolDown(GameObject gameObject){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			script.anxiety -= 15;
			script.tired -= 15;
			script.bored += 15;

			//Check if room has hotspot
			if(script.room.relaxLocation != null){
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

	class smoke : Rule{
		public smoke(GameObject gameObject){
			this.addCondition(new isAnxious(gameObject));
			this.addCondition(new isBored(gameObject));
			this.addCondition(new isSmoker(gameObject));
		}

		private Status goSmoke(GameObject gameObject){
			BaseAI script = gameObject.GetComponent<BaseAI>();

			if(script.room.relaxLocation != null){
				script.destination = script.room.relaxLocation;
				script.anim.SetBool("Speed", true);
				gameObject.GetComponent<BaseAI>().distFromDest = 5f;
				script.agent.SetDestination(script.destination);
			}
			return Status.Waiting;
		}
	}

	class readPoetry : Rule{
		public readPoetry(GameObject gameObject){
			this.addCondition(new isHappy(gameObject));
			this.addCondition(new isNotAnxious(gameObject));
			this.addCondition(new isNoPoet(gameObject));
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