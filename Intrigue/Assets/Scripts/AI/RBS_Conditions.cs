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
	class IsThirsty : Condition{
		public IsThirsty(){}

		public override bool test(GameObject gameObject){
			if (gameObject.GetComponent<BaseAI>().thirst > 50){
				return true;
			}
			return false;
		}
	}

	class IsBored : Condition{
		public IsBored(){}

		public override bool test(GameObject gameObject){
			if (gameObject.GetComponent<BaseAI>().bored > 50){
				return true;
			}
			return false;
		}
	}

	class NotBored : Condition{
		public NotBored(){}

		public override bool test(GameObject gameObject){
			if (gameObject.GetComponent<BaseAI>().bored > 50){
				return false;
			}
			return true;
		}
	}

	class IsHungry : Condition{
		public IsHungry(){}

		public override bool test(GameObject gameObject){
			if (gameObject.GetComponent<BaseAI>().hunger > 50){
				return true;
			}
			return false;
		}
	}

	class IsLonely : Condition{
		public IsLonely(){}

		public override bool test(GameObject gameObject){
			if (gameObject.GetComponent<BaseAI>().lonely > 50){
				return true;
			}
			return false;
		}
	}

	class IsTired : Condition{
		public IsTired(){}

		public override bool test(GameObject gameObject){
			if (gameObject.GetComponent<BaseAI>().tired > 50){
				return true;
			}
			return false;
		}
	}

	class IsAnxious : Condition{
		public IsAnxious(){}

		public override bool test(GameObject gameObject){
			if (gameObject.GetComponent<BaseAI>().anxiety > 50){
				return true;
			}
			return false;
		}
	}

	class IsNotAnxious : Condition{
		public IsNotAnxious(){}

		public override bool test(GameObject gameObject){
			if(gameObject.GetComponent<BaseAI>().anxiety < 50){
				return true;
			}
			return false;
		}
	}

	class IsBursting : Condition{
		public IsBursting(){}

		public override bool test(GameObject gameObject){
			if (gameObject.GetComponent<BaseAI>().bladder > 60){
				return true;
			}
			return false;
		}
	}

	class IsAngry : Condition{
		public IsAngry(){}

		public override bool test(GameObject gameObject){
			if (gameObject.GetComponent<BaseAI>().anger > 50){
				return true;
			}
			return false;
		}
	}

	class IsHappy : Condition{
		public IsHappy(){}

		public override bool test(GameObject gameObject){
			if (gameObject.GetComponent<BaseAI>().happy > 50){
				return true;
			}
			return false;
		}
	}

	class IsSad : Condition{
		public IsSad(){}

		public override bool test(GameObject gameObject){
			if (gameObject.GetComponent<BaseAI>().sad > 50){
				return true;
			}
			return false;
		}
	}

	class IsToxic : Condition{
		public IsToxic(){}

		public override bool test(GameObject gameObject){
			if (gameObject.GetComponent<BaseAI>().toxicity > 50){
				return true;
			}
			return false;
		}
	}

	class IsContent : Condition{
		public IsContent(){}

		public override bool test(GameObject gameObject){
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
		public HasDrink(){}

		public override bool test(GameObject gameObject){
			return gameObject.GetComponent<BaseAI>().hasDrink;
		}
	}

	class HasArt : Condition{
		public HasArt(){}

		public override bool test(GameObject gameObject){
			return gameObject.GetComponent<BaseAI>().room.artLocations.Count > 0;
		}
	}

	class NotInRoom : Condition{
		public NotInRoom(){}

		public override bool test(GameObject gameObject){
			return (gameObject.GetComponent<BaseAI>().room == null);
		}
	}

	class RoomHasPeople : Condition{
		public RoomHasPeople(){}

		public override bool test(GameObject gameObject){
			return (gameObject.GetComponent<BaseAI>().room.population > 0);
		}
	}

	class StayStill : Condition{
		public override bool test(GameObject gameObject){
			return true;
		}
	}

	class IsNoPoet : Condition{
		public IsNoPoet(){}

		public override bool test(GameObject gameObject){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			if(script.room.name == "smoking" && script.room.poet != null){
				return true;
			}
			return false;
		}
	}

	class IsSmoker : Condition{
		public IsSmoker(){}

		public override bool test(GameObject gameObject){
			return gameObject.GetComponent<BaseAI>().smoker;
		}
	}

	class NotInConvo : Condition{
		public NotInConvo(){}

		public override bool test(GameObject gameObject){
			return !gameObject.GetComponent<BaseAI>().inConvo;
		}
	}

	class TimeToMove: Condition{
		private float threshold = 10;
		public TimeToMove(){
			threshold = UnityEngine.Random.Range(0, 10);
		}

		public override bool test(GameObject gameObject){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			if(script.timeInRoom <= threshold){
				return true;
			}
			return false;
		}
	}

	class HalfRoomTime:Condition{
		private static float threshold = 10;
		public HalfRoomTime(){}
		
		public override bool test(GameObject gameObject){
			BaseAI script = gameObject.GetComponent<BaseAI>();
			if(script.timeInRoom <= threshold){
				return true;
			}
			return false;
		}
	}

	class alwaysTrue : Condition{
		public alwaysTrue(){}

		public override bool test(GameObject gameObject){
			return true;
		}
	}

	class alwaysFalse : Condition{
		public alwaysFalse(){}

		public override bool test(GameObject gameObject){
			return false;
		}
	}
}