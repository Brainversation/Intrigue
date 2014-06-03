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
	public delegate Status ConsequenceFunction(GameObject gameObject);
	public delegate Status AntiConsequenceFunction();

	// An array of these are used to choose the correct rule
	public abstract class Condition {
		protected GameObject gameObject;

		public Condition(){}

		public Condition(GameObject gameObject) {
			this.gameObject = gameObject;
		}

		abstract public bool test();
	}

	// Base class for rules
	public class Rule : IComparable{
		public List<Condition> conditions;
		public ConsequenceFunction consequence;
		public AntiConsequenceFunction antiConsequence;
		public int weight = 0;

		public Rule(){
			this.conditions = new List<Condition>();
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
