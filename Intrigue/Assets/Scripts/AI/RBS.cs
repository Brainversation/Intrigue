using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

namespace RBS{
	public delegate Status ConsequenceFunction(GameObject gameObject);
	public delegate Status AntiConsequenceFunction();

	public abstract class Condition {
		protected GameObject gameObject;

		public Condition(){}

		public Condition(GameObject gameObject) {
			this.gameObject = gameObject;
		}

		abstract public bool test();
	}

	public class Rule : IComparable{
		// every element in this list is a condition but the last;
		// the last element is the consequence
		public List<Condition> conditions;
		public ConsequenceFunction consequence;
		public AntiConsequenceFunction antiConsequence;
		public int weight = 0;

		public Rule(){
			this.conditions = new List<Condition>();
		}

		public Rule(List<Condition> conditions, ConsequenceFunction consequence){
			this.conditions = conditions;
			this.consequence = consequence;
		}

		public bool isFired(){
			foreach(Condition con in conditions){
				if (!con.test()){
					return false;
				}
			}
			return true;
		}

		public void addCondition(Condition con){
			conditions.Add(con);
		}

		public int CompareTo(object other){
			Rule otherRule = (Rule) other;

			if (otherRule == null || otherRule.weight < this.weight)
				return -1;
			else if (otherRule.weight > this.weight)
				return 1;
			else
				return 0;
		}
	}

	// <---------------- Conditions ------------------>

	class isThirsty : Condition{
		public isThirsty(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().thirst > 50){
				return true;
			}
			return false;
		}
	}

	class isBored : Condition{
		public isBored(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().bored > 50){
				return true;
			}
			return false;
		}
	}

	class isHungry : Condition{
		public isHungry(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().hunger > 50){
				return true;
			}
			return false;
		}
	}

	class isLonely : Condition{
		public isLonely(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().lonely > 50){
				return true;
			}
			return false;
		}
	}

	class isTired : Condition{
		public isTired(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().tired > 50){
				return true;
			}
			return false;
		}
	}

	class isAnxious : Condition{
		public isAnxious(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().anxiety > 50){
				return true;
			}
			return false;
		}
	}

	class isBursting : Condition{
		public isBursting(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().bladder > 50){
				return true;
			}
			return false;
		}
	}

	class isContent : Condition{
		public isContent(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if(gameObject.GetComponent<BaseAI>().thirst < 50 &&
				gameObject.GetComponent<BaseAI>().bored < 50 &&
				//gameObject.GetComponent<BaseAI>().hunger < 50 &&
				gameObject.GetComponent<BaseAI>().lonely < 50 &&
				//gameObject.GetComponent<BaseAI>().tired < 50 &&
				gameObject.GetComponent<BaseAI>().anxiety < 50 &&
				gameObject.GetComponent<BaseAI>().bladder < 50){
				return true;
			}
			return false;
		}
	}

	class StayStill : Condition{
		public override bool test(){
			return true;
		}
	}

	// <------------------------- Rules -------------------->

	class WantToMoveRoom : Rule{
		public WantToMoveRoom(GameObject gameObject){
			this.addCondition(new isAnxious(gameObject));
			this.consequence = goToRoom;
		}

		private Status goToRoom(GameObject gameObject){
			Debug.Log("Going to Room");
			BaseAI script = gameObject.GetComponent<BaseAI>();
			GameObject curRoom = script.room.me;
			GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");

			GameObject room = rooms[UnityEngine.Random.Range(0, rooms.Length)];

			while(room == curRoom){
				room = rooms[UnityEngine.Random.Range(0, rooms.Length)];
			}

			Debug.Log("Room: " + room.name);

			Vector3 newDest;
			NavMeshPath path = new NavMeshPath();
            newDest = new Vector3(UnityEngine.Random.Range(room.GetComponent<BoxCollider>().bounds.min.x,
                                  room.GetComponent<BoxCollider>().bounds.max.x),
                                  gameObject.transform.position.y,
                                  UnityEngine.Random.Range(room.GetComponent<BoxCollider>().bounds.min.z,
                                  room.GetComponent<BoxCollider>().bounds.max.z));

            //Debug.Log("newDest: " + newDest);
            //Debug.Log("path: " + path);
            script.agent.CalculatePath(newDest, path);

            while(path.status ==  NavMeshPathStatus.PathPartial){
            	newDest = new Vector3(UnityEngine.Random.Range(room.GetComponent<BoxCollider>().bounds.min.x,
                                      room.GetComponent<BoxCollider>().bounds.max.x),
                                      gameObject.transform.position.y,
                                      UnityEngine.Random.Range(room.GetComponent<BoxCollider>().bounds.min.z,
                                      room.GetComponent<BoxCollider>().bounds.max.z));

            	script.agent.CalculatePath(newDest, path);
            }

            Debug.Log("After finding in bound path");

            script.anxiety -= 25;

            script.distFromDest = 5f;
            script.agent.SetDestination(newDest);
            script.anim.SetFloat("Speed", .2f);
            Debug.Log("After setDest in goToRoom");
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

			Vector3 newDest;
			NavMeshPath path = new NavMeshPath();
            newDest = new Vector3(UnityEngine.Random.Range(room.GetComponent<BoxCollider>().bounds.min.x,
                                  room.GetComponent<BoxCollider>().bounds.max.x),
                                  gameObject.transform.position.y,
                                  UnityEngine.Random.Range(room.GetComponent<BoxCollider>().bounds.min.z,
                                  room.GetComponent<BoxCollider>().bounds.max.z));

            //Debug.Log("newDest: " + newDest);
            //Debug.Log("path: " + path);
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
            //script.distFromDest = 2f;
            script.agent.SetDestination(newDest);
            script.anim.SetFloat("Speed", .2f);
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
		}

		private Status setDestRoom(GameObject gameObject){
			Debug.Log("Wants a drink");

			GameObject[] drinkLocations = GameObject.FindGameObjectsWithTag("Drink");

			GameObject drinkLocation = drinkLocations[UnityEngine.Random.Range(0, drinkLocations.Length)];

			this.go = gameObject;
			BaseAI script = gameObject.GetComponent<BaseAI>();
			script.bored -= 10;
			script.thirst -= 25;
			/*if(script.room.drinkLocation != null){*/
				script.destination = drinkLocation.transform.position; //script.room.drinkLocation.position;
				script.anim.SetFloat("Speed", .2f);
				gameObject.GetComponent<BaseAI>().distFromDest = 10f;
				script.agent.SetDestination(script.destination);
				Debug.Log("After Set Dest of GetDrink");
				script.tree = new DrinkingTree();
			/*}
			else
				Debug.Log("couldn't find drink location");*/
			Debug.DrawLine(gameObject.transform.position, script.destination, Color.red, 115f, false);
			return Status.Waiting;
		}

		private Status stopDrinking(){
			this.go.GetComponent<Animator>().SetBool("Drink", false);
			return Status.True;
		}
	}

	class WantToConverse : Rule{
		protected int offset = 5;

		public WantToConverse(GameObject gameObject){
			this.addCondition( new isLonely(gameObject) );
			this.addCondition( new isBored(gameObject) );
			this.consequence = handleConverse;
		}

		private Status handleConverse(GameObject gameObject){
			Debug.Log("Wants to converse");
			Status returnStat;
			List<GameObject> conversers;
			BaseAI script = gameObject.GetComponent<BaseAI>();
			if(script.room != null)
				conversers = script.room.conversers;
			else
				return Status.False;
			script.lonely -= 20;
			script.bored -= 10;
			if(conversers.Count == 0 || conversers.Count > offset){
				script.destination = gameObject.transform.position;
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ConversationHotSpot"), gameObject.transform.position, Quaternion.identity);
				script.tree = new IdleSelector();
				conversers.Clear();
				returnStat = Status.Tree;
			} else {
				script.destination = conversers[0].transform.position;
				script.anim.SetFloat("Speed", .2f);
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
			Debug.Log("needs to use restroom");
			BaseAI script = gameObject.GetComponent<BaseAI>();
			script.bladder -= 25;

			GameObject[] bathroomLocations = GameObject.FindGameObjectsWithTag("RestRoom");

			GameObject bathroomLocation = bathroomLocations[UnityEngine.Random.Range(0, bathroomLocations.Length)];
			/*if(script.room.restroomLocation != null){*/
				script.destination = bathroomLocation.transform.position; //script.room.restroomLocation.position;
				script.anim.SetFloat("Speed", .2f);
				gameObject.GetComponent<BaseAI>().distFromDest = 5f;
				script.agent.SetDestination(script.destination);
			/*}
			else
				Debug.Log("Couldn't find restroom location");*/
			Debug.DrawLine(gameObject.transform.position, script.destination, Color.red, 15f, false);
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
