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

	class StayStill : Condition{
		public override bool test(){
			return true;
		}
	}

	// <------------------------- Rules -------------------->

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
			this.go = gameObject;
			BaseAI script = gameObject.GetComponent<BaseAI>();
			script.bored -= 10;
			if(script.room.drinkLocation != null){
				script.destination = script.room.drinkLocation.position;
				script.atDest = false;
				gameObject.GetComponent<Animator>().SetFloat("Speed", .2f);
				gameObject.GetComponent<NavMeshAgent>().SetDestination(script.destination);
				script.tree = new DrinkingTree();
			}
			else
				Debug.Log("couldn't find drink location");
			Debug.DrawLine(gameObject.transform.position, script.destination, Color.red, 115f, false);
			return Status.Waiting;
		}

		private Status stopDrinking(){
			this.go.GetComponent<Animator>().SetBool("Drink", false);
			return Status.True;
		}
	}
/*
	class ReadyToDrink : Rule{
		GameObject go;
		public ReadyToDrink(GameObject gameObject){
			go = gameObject;
			this.addCondition( new AtDrink(gameObject) );
			this.consequence = (new MakeDrink() ).run;
			this.antiConsequence = stop;
		}

		private Status stop(){
			Debug.Log("ready to drink");
			go.GetComponent<Animator>().SetBool("Drink", false);
			return Status.Waiting;
		}
	}*/

	class WantToConverse : Rule{
		GameObject go;
		public WantToConverse(GameObject gameObject){
			go = gameObject;
			this.addCondition( new isLonely(gameObject) );
			this.consequence = setDestConverse;
		}

		private Status setDestConverse(GameObject gameObject){
			Debug.Log("Wants to converse");
			BaseAI script = gameObject.GetComponent<BaseAI>();
			script.lonely -= 10;
			if(script.room.converseLocation != null){
				script.destination = script.room.converseLocation.position;
				script.atDest = false;
				gameObject.GetComponent<Animator>().SetFloat("Speed", .2f);
				gameObject.GetComponent<NavMeshAgent>().SetDestination(script.destination);
			}
			else
				Debug.Log("couldn't find drink location");
			Debug.DrawLine(gameObject.transform.position, script.destination, Color.red, 15f, false);
			return Status.Waiting;
		}
	}

	class NeedToUseRestroom : Rule{
		GameObject go;
		public NeedToUseRestroom(GameObject gameObject){
			go = gameObject;
			this.addCondition( new isBursting(gameObject) );
			this.consequence = setDestRestroom;
		}

		private Status setDestRestroom(GameObject gameObject){
			Debug.Log("needs to use restroom");
			BaseAI script = gameObject.GetComponent<BaseAI>();
			script.bladder -= 25;
			if(script.room.restroomLocation != null){
				script.destination = script.room.restroomLocation.position;
				script.atDest = false;
				gameObject.GetComponent<Animator>().SetFloat("Speed", .2f);
				gameObject.GetComponent<NavMeshAgent>().SetDestination(script.destination);
			}
			else
				Debug.Log("Couldn't find restroom location");
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
