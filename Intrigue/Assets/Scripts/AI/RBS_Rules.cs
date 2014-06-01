/**************************************************************************
 **                                                                       *
 **                COPYRIGHT AND CONFIDENTIALITY NOTICE                   *
 **                                                                       *
 **    Copyright (c) 2014 Hacksaw Games. All rights reserved.             *
 **                                                                       *
 **    This software contains information confidential and proprietary    *
 **    to Hacksaw Games. It shall not be reproduced in whole or in        *
 **    part, or transferred to other documents, or disclosed to third     *
 **    parties, or used for any purpose other than that for which it was  *
 **    obtained, without the prior written consent of Hacksaw Games.      *
 **                                                                       *
 **************************************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

namespace RBS{
	class WantToMoveRoom : Rule{
		private static GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
		private GameObject go;
		public WantToMoveRoom(GameObject gameObject){
			this.addCondition(new TimeToMove(gameObject));
			this.consequence = goToRoom;
			this.antiConsequence = atRoom;
			this.weight = 10;
			this.go = gameObject;
		}

		private Status goToRoom(GameObject gameObject){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			GameObject curRoom = script.room.me;
			GameObject room;

			//ensure next room to go to is not the same as the current room
			if(rooms.Length != 0){
				do{
					int i = UnityEngine.Random.Range(0, rooms.Length);
					room = rooms[i];
				}while(room == curRoom);
			} else {
				return Status.False;
			}

			//Pick spot inside collider of chosen room
			Vector3 newDest;
            newDest = new Vector3(UnityEngine.Random.Range(room.GetComponent<Collider>().bounds.min.x,
                                  room.GetComponent<Collider>().bounds.max.x),
                                  gameObject.transform.position.y,
                                  UnityEngine.Random.Range(room.GetComponent<Collider>().bounds.min.z,
                                  room.GetComponent<Collider>().bounds.max.z));

			//script.bored -= 20;
			//script.lonely -= 20;
			//script.anxiety -= 20;

            //Set BaseAI variables and run
            script.distFromDest = 5f;
            script.agent.SetDestination(newDest);
            script.anim.SetBool("Speed", true);
            return Status.Waiting;
		}

		private Status atRoom(){
			go.GetComponent<BaseAI>().timeInRoom = 0f;
			return Status.True;
		}
	}

	class WantToWanderRoom : Rule{
		public WantToWanderRoom(GameObject gameObject){
			this.addCondition(new HalfRoomTime(gameObject));
			this.consequence = wanderRoom;
			this.weight = 6;
		}

		private Status wanderRoom(GameObject gameObject){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			GameObject room = script.room.me;

			//Choose random point inside curRoom collider
			Vector3 newDest;
            newDest = new Vector3(UnityEngine.Random.Range(room.GetComponent<Collider>().bounds.min.x,
                                  room.GetComponent<Collider>().bounds.max.x),
                                  gameObject.transform.position.y,
                                  UnityEngine.Random.Range(room.GetComponent<Collider>().bounds.min.z,
                                  room.GetComponent<Collider>().bounds.max.z));

            //Following ensures that chosen random point is on navMesh, currently probably not useful.
           
            // script.agent.CalculatePath(newDest, path);

            // while(path.status ==  NavMeshPathStatus.PathPartial){
            // 	newDest = new Vector3(UnityEngine.Random.Range(room.GetComponent<Collider>().bounds.min.x,
            //                           room.GetComponent<Collider>().bounds.max.x),
            //                           gameObject.transform.position.y,
            //                           UnityEngine.Random.Range(room.GetComponent<Collider>().bounds.min.z,
            //                           room.GetComponent<Collider>().bounds.max.z));

            // 	script.agent.CalculatePath(newDest, path);
            // }
            script.anim.SetBool("Speed", true);
            script.agent.SetDestination(newDest);
            return Status.Waiting;
		}
	}

	class WantToGetDrink : Rule{
		private GameObject go;
		public static GameObject[] drinkLocations = GameObject.FindGameObjectsWithTag("Drink");
		public WantToGetDrink(GameObject gameObject) {
			this.addCondition(new IsThirsty(gameObject));
			this.addCondition(new IsBored(gameObject));
			this.consequence = setDestRoom;
			this.antiConsequence = stopDrinking;
			this.weight = 7;
			this.go = gameObject;
		}

		private Status setDestRoom(GameObject gameObject){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			script.bored -= 10;
			script.thirst -= 25;

			//Check room for hotspot location
			if(script.room.drinkLocation != Vector3.zero){
				script.destination = script.room.drinkLocation;
			}
			//Choose random hotspot location
			else{
				GameObject drinkLocation = drinkLocations[UnityEngine.Random.Range(0, drinkLocations.Length)];
				script.destination = drinkLocation.transform.position;
			}
			script.anim.SetBool("Speed", true);
			script.distFromDest = 10f;
			script.agent.SetDestination(script.destination);
			script.tree = new DrinkingTree(gameObject);
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
			this.addCondition( new RoomHasPeople(gameObject) );
			this.consequence = handleConverse;
			this.weight = 7;
		}

		private Status handleConverse(GameObject gameObject){
			Status returnStat;
			List<GameObject> conversers;
			BaseAI script = gameObject.GetComponent<BaseAI>();
			if(script.room != null)
				conversers = script.room.conversers;
			else
				return Status.False;

			script.lonely -= 30;
			script.bored -= 20;

			if(conversers.Count == 0 || conversers.Count >= offset){
				script.destination = gameObject.transform.position;
				if(BaseAI.aiTesting)
					UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ConversationHotSpot"), gameObject.transform.position, Quaternion.identity);
				else
					PhotonNetwork.InstantiateSceneObject("ConversationHotSpot", gameObject.transform.position, Quaternion.identity, 0, null);
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
			return returnStat;
		}
	}

	class NeedToUseRestroom : Rule{
		private static GameObject[] bathroomLocations = GameObject.FindGameObjectsWithTag("RestRoom");
		public NeedToUseRestroom(GameObject gameObject){
			this.addCondition( new IsBursting(gameObject) );
			this.consequence = setDestRestroom;
			this.weight = 10;
		}

		private Status setDestRestroom(GameObject gameObject){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			script.bladder -= 75;

			//Check if room has hotspot
			if(script.room.restroomLocations.Count > 0){
				script.destination = script.room.restroomLocations[UnityEngine.Random.Range(0, script.room.restroomLocations.Count)];
			}
			//Find random hotspot
			else{
				GameObject bathroomLocation = bathroomLocations[UnityEngine.Random.Range(0, bathroomLocations.Length)];
				script.destination = bathroomLocation.transform.position;
			}
			script.distFromDest = 10f;
			script.anim.SetBool("Speed", true);
			script.tree = new RepairingTree(gameObject);
			script.agent.SetDestination(script.destination);
			return Status.Waiting;
		}
	}

	class AdmireArt : Rule{
		public AdmireArt(GameObject gameObject){
			this.addCondition(new HasArt(gameObject));
			this.addCondition(new IsBored(gameObject));
			this.consequence = goToArt;
			this.weight = 6;
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
			script.distFromDest = 10f;
			script.agent.SetDestination(script.destination);
			// Make then add ArtTree handleConverse
			// script.tree = new ArtTree(gameObject);

			return Status.Waiting;
		}
	}

	class FindRoom : Rule{
		private static GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
		public FindRoom(GameObject gameObject){
			this.addCondition(new NotInRoom(gameObject));
			this.consequence = goToRoom;
			this.weight = 1000;
		}

		private Status goToRoom(GameObject gameObject){
			BaseAI script = gameObject.GetComponent<BaseAI>();

			int minIndex = 0;
			for(int i = 1; i < rooms.Length; ++i){
				if(Vector3.Distance(rooms[i-1].transform.position, gameObject.transform.position) <
						Vector3.Distance(rooms[i].transform.position, gameObject.transform.position)){
					minIndex = (i-1);
				}
			}

			script.destination = rooms[minIndex].transform.position;
			script.anim.SetBool("Speed", true);
			script.distFromDest = 5f;
			script.agent.SetDestination(script.destination);

			return Status.Waiting;
		}
	}

	// class Relax : Rule{
	// 	public Relax(GameObject gameObject){
	// 		this.addCondition(new IsAnxious(gameObject));
	// 		this.addCondition(new NotBored(gameObject));
	// 		this.addCondition(new IsTired(gameObject));
	// 		this.consequence = goRelax;
	// 	}

	// 	private Status goRelax(GameObject gameObject){
	// 		BaseAI script = gameObject.GetComponent<BaseAI>();
	// 		script.anxiety -= 15;
	// 		script.tired -= 15;
	// 		script.bored += 15;

	// 		//Check if room has hotspot
	// 		if(script.room.relaxLocation != Vector3.zero){
	// 			script.destination = script.room.relaxLocation;
	// 			script.anim.SetBool("Speed", true);
	// 			script.distFromDest = 5f;
	// 			script.agent.SetDestination(script.destination);
	// 		}
	// 		//Find couch hotspot somewhere
	// 		else{
	// 			Debug.Log("Find a relaxLocation");
	// 		}

	// 		return Status.Waiting;
	// 	}
	// }
	
	// class LetOffSteam : Rule{
	// 	public LetOffSteam(GameObject gameObject){
	// 		this.addCondition(new IsAnxious(gameObject));
	// 		this.addCondition(new NotBored(gameObject));
	// 		this.addCondition(new IsAngry(gameObject));
	// 		this.consequence = coolDown;
	// 	}

	// 	private Status coolDown(GameObject gameObject){
	// 		BaseAI script = gameObject.GetComponent<BaseAI>();
	// 		script.anxiety -= 15;
	// 		script.tired -= 15;
	// 		script.bored += 15;

	// 		//Check if room has hotspot
	// 		if(script.room.relaxLocation != Vector3.zero){
	// 			script.destination = script.room.relaxLocation;
	// 			script.anim.SetBool("Speed", true);
	// 			script.distFromDest = 5f;
	// 			script.agent.SetDestination(script.destination);
	// 		} else{
	// 			//Find couch hotspot somewhere
	// 			Debug.Log("Find a relaxLocation");
	// 		}

	// 		return Status.Waiting;
	// 	}
	// }

	class Smoke : Rule{
		public Smoke(GameObject gameObject){
			this.addCondition(new IsAnxious(gameObject));
			this.addCondition(new IsBored(gameObject));
			this.addCondition(new IsSmoker(gameObject));
			this.consequence = goSmoke;
		}

		private Status goSmoke(GameObject gameObject){
			bool roomPicked = false;
			BaseAI script = gameObject.GetComponent<BaseAI>();

			GameObject[] relaxLocations = GameObject.FindGameObjectsWithTag("relax");

			Debug.Log("After finding locations");

			foreach(GameObject relaxLocation in relaxLocations){
				Debug.Log("Inside smoke foreach");
				if(!relaxLocation.GetComponent<RelaxHotSpot>().occupied){
					script.destination = relaxLocation.transform.position;
					roomPicked = true;
					break;
				}
			}

			if(roomPicked){
				script.anim.SetBool("Speed", true);
				script.distFromDest = 5f;
				script.agent.SetDestination(script.destination);
				script.tree = new SmokeTree(gameObject);
				script.anxiety -= 20;
				script.bored -= 10;
				return Status.Waiting;
			}
			else{
				script.bored += 10;
				return Status.False;
			}
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
			script.distFromDest = 5f;
			script.agent.SetDestination(script.destination);

			return Status.Waiting;
		}
	}

	class DoIdle : Rule{
		public DoIdle(GameObject gameObject){
			this.addCondition(new StayStill());
			this.consequence = stay;
			this.weight = 1;
		}

		private Status stay(GameObject gameObject){
			gameObject.GetComponent<BaseAI>().tree = new IdleSelector();
			return Status.Tree;
		}
	}
}