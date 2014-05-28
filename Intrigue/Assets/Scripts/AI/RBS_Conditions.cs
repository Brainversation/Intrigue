using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

namespace RBS{
	class IsThirsty : Condition{
		public IsThirsty(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().thirst > 50){
				return true;
			}
			return false;
		}
	}

	class IsBored : Condition{
		public IsBored(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().bored > 50){
				return true;
			}
			return false;
		}
	}

	class NotBored : Condition{
		public NotBored(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().bored > 50){
				return false;
			}
			return true;
		}
	}

	class IsHungry : Condition{
		public IsHungry(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().hunger > 50){
				return true;
			}
			return false;
		}
	}

	class IsLonely : Condition{
		public IsLonely(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().lonely > 50){
				return true;
			}
			return false;
		}
	}

	class IsTired : Condition{
		public IsTired(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().tired > 50){
				return true;
			}
			return false;
		}
	}

	class IsAnxious : Condition{
		public IsAnxious(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().anxiety > 50){
				return true;
			}
			return false;
		}
	}

	class IsNotAnxious : Condition{
		public IsNotAnxious(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if(gameObject.GetComponent<BaseAI>().anxiety < 50){
				return true;
			}
			return false;
		}
	}

	class IsBursting : Condition{
		public IsBursting(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().bladder > 80){
				return true;
			}
			return false;
		}
	}

	class IsAngry : Condition{
		public IsAngry(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().anger > 50){
				return true;
			}
			return false;
		}
	}

	class IsHappy : Condition{
		public IsHappy(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().happy > 50){
				return true;
			}
			return false;
		}
	}

	class IsSad : Condition{
		public IsSad(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().sad > 50){
				return true;
			}
			return false;
		}
	}

	class IsToxic : Condition{
		public IsToxic(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().toxicity > 50){
				return true;
			}
			return false;
		}
	}

	class IsContent : Condition{
		public IsContent(GameObject gameObject):base(gameObject){}

		public override bool test(){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			if(	script.thirst < 50 && script.bored < 50 &&
				script.hunger < 50 && script.lonely < 50 &&
				script.anxiety < 50 && script.bladder < 50){
				return true;
			}
			return false;
		}
	}

	class HasDrink : Condition{
		public HasDrink(GameObject gameObject):base(gameObject){}

		public override bool test(){
			return gameObject.GetComponent<BaseAI>().hasDrink;
		}
	}

	class HasArt : Condition{
		public HasArt(GameObject gameObject):base(gameObject){}

		public override bool test(){
			return gameObject.GetComponent<BaseAI>().room.artLocations.Count > 0;
		}
	}

	class NotInRoom : Condition{
		public NotInRoom(GameObject gameObject):base(gameObject){}

		public override bool test(){
			return (gameObject.GetComponent<BaseAI>().room == null);
		}
	}

	class StayStill : Condition{
		public override bool test(){
			return true;
		}
	}

	class IsNoPoet : Condition{
		public IsNoPoet(GameObject gameObject):base(gameObject){}

		public override bool test(){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			if(script.room.name == "smoking" && script.room.poet != null){
				return true;
			}
			return false;
		}
	}

	class IsSmoker : Condition{
		public IsSmoker(GameObject gameObject):base(gameObject){}

		public override bool test(){
			return gameObject.GetComponent<BaseAI>().smoker;
		}
	}

	class NotInConvo : Condition{
		public NotInConvo(GameObject gameObject):base(gameObject){}

		public override bool test(){
			return !gameObject.GetComponent<BaseAI>().inConvo;
		}
	}

	class TimeToMove: Condition{
		private float threshold = 15;
		public TimeToMove(GameObject gameObject):base(gameObject){
			threshold = UnityEngine.Random.Range(5, 25);
		}

		public override bool test(){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			if(script.timeInRoom >= threshold){
				return true;
			}
			return false;
		}
	}

	class HalfRoomTime:Condition{
		private float threshold = 10;
		public HalfRoomTime(GameObject gameObject):base(gameObject){
			threshold = UnityEngine.Random.Range(5, 25);
		}
		
		public override bool test(){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			if(script.timeInRoom >= threshold){
				return true;
			}
			return false;
		}
	}

	class alwaysTrue : Condition{
		public alwaysTrue(GameObject gameObject):base(gameObject){}

		public override bool test(){
			return true;
		}
	}

	class alwaysFalse : Condition{
		public alwaysFalse(GameObject gameObject):base(gameObject){}

		public override bool test(){
			return false;
		}
	}
}