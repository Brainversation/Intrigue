using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

namespace RBS{
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

	class notBored : Condition{
		public notBored(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().bored > 50){
				return false;
			}
			return true;
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

	class isNotAnxious : Condition{
		public isNotAnxious(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if(gameObject.GetComponent<BaseAI>().anxiety < 50){
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

	class isAngry : Condition{
		public isAngry(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().anger > 50){
				return true;
			}
			return false;
		}
	}

	class isHappy : Condition{
		public isHappy(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().happy > 50){
				return true;
			}
			return false;
		}
	}

	class isSad : Condition{
		public isSad(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().sad > 50){
				return true;
			}
			return false;
		}
	}

	class isToxic : Condition{
		public isToxic(GameObject gameObject):base(gameObject){}

		public override bool test(){
			if (gameObject.GetComponent<BaseAI>().toxicity > 50){
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

	class hasDrink : Condition{
		public hasDrink(GameObject gameObject):base(gameObject){}

		public override bool test(){
			return gameObject.GetComponent<BaseAI>().hasDrink;
		}
	}

	class hasArt : Condition{
		public hasArt(GameObject gameObject):base(gameObject){}

		public override bool test(){
			return gameObject.GetComponent<BaseAI>().room.hasArt;
		}
	}

	class hasConversation : Condition{
		public hasConversation(GameObject gameObject):base(gameObject){}

		public override bool test(){
			return (gameObject.GetComponent<BaseAI>().room.conversers.Count > 0);
		}
	}

	class notInRoom : Condition{
		public notInRoom(GameObject gameObject):base(gameObject){}

		public override bool test(){
			return (gameObject.GetComponent<BaseAI>().room == null);
		}
	}

	class StayStill : Condition{
		public override bool test(){
			return true;
		}
	}

	class isNoPoet : Condition{
		public isNoPoet(GameObject gameObject):base(gameObject){}

		public override bool test(){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			if(script.room.name == "smoking" && script.room.poet != null){
				return true;
			}
			return false;
		}
	}

	class isSmoker : Condition{
		public isSmoker(GameObject gameObject):base(gameObject){}

		public override bool test(){
			BaseAI script = gameObject.GetComponent<BaseAI>();

			return script.smoker;
		}
	}
}