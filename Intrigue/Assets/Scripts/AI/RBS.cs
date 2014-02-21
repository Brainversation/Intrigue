using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

namespace RBS{
	public delegate Status ConsequenceFunction(GameObject gameObject);
	public delegate Status AntiConsequenceFunction();

	public abstract class Condition {
		protected GameState game;
		protected GameObject gameObject;

		public Condition(){
			game = GameState.Game;
		}

		public Condition(GameObject gameObject) {
			game = GameState.Game;
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

	class Bar : Condition {
		public override bool test(){
			if (game.room == "Bar") {
				return true;
			}
			return false;
		}
	}

	class Library : Condition{
		public override bool test(){
			if (game.room == "Library"){
				return true;
			}
			return false;
		}
	}

	class Party : Condition{
		public override bool test(){
			if (game.personality == "Party"){
				return true;
			}
			return false;
		}
	}

	class Thirst : Condition{
		public Thirst(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().thirst > 50){
				return true;
			}
			return false;
		}
	}

	class Bored : Condition{
		public Bored(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().bored > 50){
				return true;
			}
			return false;
		}
	}

	class DestChange : Condition{
		private Vector3 currDest;

		public DestChange(GameObject gameObject):base(gameObject){
			currDest = gameObject.transform.position;
		}

		public override bool test(){
			if (currDest != gameObject.GetComponent<BaseAI>().destination){
				currDest = gameObject.GetComponent<BaseAI>().destination;
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

	class RunJ : Condition{
		public override bool test(){
			return true;
		}
	}

	// <------------------------- Rules -------------------->

	class WantToGoToBar : Rule{
		Vector3 barLocation;
		public WantToGoToBar(GameObject gameObject) {
			this.conditions.Add(new Thirst(gameObject));
			this.conditions.Add(new Bored(gameObject));
			this.consequence = setDestRoom;
			this.barLocation = GameObject.Find("Bar").transform.position;
		}

		private Status setDestRoom(GameObject gameObject){
			gameObject.GetComponent<BaseAI>().bored = 40;
			gameObject.GetComponent<BaseAI>().destination = barLocation;
			Debug.DrawLine(gameObject.transform.position, barLocation, Color.red, 100f, false);
			return Status.True;
		}
	}

	class GoToDestination : Rule{
		public GoToDestination(GameObject gameObject) {
			this.conditions.Add(new DestChange(gameObject));
			this.consequence = go;
		}

		private Status go(GameObject gameObject){
			Vector3 dest = gameObject.GetComponent<BaseAI>().destination;
			gameObject.GetComponent<Animator>().SetFloat("Speed", .2f);
			gameObject.GetComponent<NavMeshAgent>().SetDestination(dest);
			return Status.True;
		}
	}

	class DoIdle : Rule{
		public DoIdle(GameObject gameObject){
			this.conditions.Add(new StayStill());
			this.consequence = stay;
		}

		private Status stay(GameObject gameObject){
			Debug.Log("Hello");
			gameObject.GetComponent<Animator>().CrossFade("Idle", 0f);
			return Status.Waiting;
		}
	}

	class DoRunJ : Rule{
		private GameObject go;
		public DoRunJ(GameObject gameObject){
			go = gameObject;
			this.conditions.Add(new RunJ());
			this.consequence = (new JumpGap()).run;
			this.antiConsequence = stopRun;
		}

		private Status stopRun(){
			go.GetComponent<Animator>().SetBool("Run", false);
			return Status.True;
		}
	}

	public class GameState {
		public string room = "Bar";
		public string personality = "Party";
		public bool thirst = true;
		public bool bored = true;
		
		private GameState(){}

		private static GameState gameState = null;
			public static GameState Game {
			get{
				if (gameState == null){
				gameState = new GameState();
				}
				return gameState;
			}
		}
	}

}
