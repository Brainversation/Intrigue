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
using System.Collections;
using System.Collections.Generic;
using RBS;

namespace BehaviorTree{

	class CreateDrink : Task{
		public override Status run(GameObject gameObject){
			gameObject.GetComponent<BaseAI>().addDrink();
			return Status.True;
		}
	}

	class HoldDrink : Task {
		public override Status run(GameObject gameObject){
			gameObject.GetComponent<Animator>().SetBool("Drink", true);
			return Status.True;
		}
	}

	class AtDestination : Task {
		public override Status run(GameObject gameObject){
			if(gameObject.GetComponent<NavMeshAgent>().hasPath)
				return Status.True;
			return Status.False;
		}
	}

	class AnimStart : Task {
		private string animName;
		public AnimStart(string animName){
			this.animName = animName;
		}
		public override Status run(GameObject gameObject){
			gameObject.GetComponent<Animator>().SetBool(animName, true);
			return Status.True;
		}
	}

	class AnimStop : Task {
		private string animName;
		public AnimStop(string animName){
			this.animName = animName;
		}
		public override Status run(GameObject gameObject){
			gameObject.GetComponent<Animator>().SetBool(animName, false);
			return Status.True;
		}
	}

	class GoToDestination : Task {
		public override Status run(GameObject gameObject){
			Vector3 dest = gameObject.GetComponent<BaseAI>().destination;
			gameObject.GetComponent<BaseAI>().distFromDest = 10f;
			gameObject.GetComponent<BaseAI>().status = Status.Waiting;
			gameObject.GetComponent<NavMeshAgent>().SetDestination(dest);
			return Status.True;
		}
	}

	class WalkAway : Task {
		public override Status run(GameObject gameObject){
			Vector3 newDest;
			newDest = new Vector3(UnityEngine.Random.Range( gameObject.GetComponent<BaseAI>().room.me.GetComponent<Collider>().bounds.min.x,
															gameObject.GetComponent<BaseAI>().room.me.GetComponent<Collider>().bounds.max.x),
															gameObject.transform.position.y,
															UnityEngine.Random.Range(gameObject.GetComponent<BaseAI>().room.me.GetComponent<Collider>().bounds.min.z,
															gameObject.GetComponent<BaseAI>().room.me.GetComponent<Collider>().bounds.max.z));
			gameObject.GetComponent<BaseAI>().destination = newDest;
			gameObject.GetComponent<BaseAI>().distFromDest = 1f;
			gameObject.GetComponent<BaseAI>().status = Status.Waiting;
			gameObject.GetComponent<NavMeshAgent>().SetDestination(newDest);
			return Status.True;
		}
	}

	class IsTurn : Task{
		public override Status run(GameObject gameObject){
			if(gameObject.GetComponent<BaseAI>().isYourTurn){
				return Status.True;
			}
			return Status.False;
		}
	}

	class Wait : Task{
		private float seconds = 0;

		public Wait(float seconds){
			this.seconds = seconds;
		}

		public override Status run(GameObject gameObject){
			if(seconds >= 0){
				seconds -= Time.deltaTime;
				return Status.False;
			}
			return Status.True;
		}
	}
}