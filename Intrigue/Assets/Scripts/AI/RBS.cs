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
}
