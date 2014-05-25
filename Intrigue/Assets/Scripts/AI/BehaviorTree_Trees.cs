using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RBS;

namespace BehaviorTree{

	class MakeDrink : Sequence{
		public MakeDrink(){
			this.addChild( new GoToDestination() );
			this.addChild( new AtDestination() );
			this.addChild( new CreateDrink() );
			this.addChild( new HoldDrink() );
		}

		public override Status run(GameObject gameObject){
			return base.run(gameObject);
		}
	}

	class WaitInLine : Sequence { 
		public WaitInLine(){
			this.addChild(new Inverter(new IsTurn()));
			this.addChild(new IdleSelector());
		}
	}

	class IdleSelector : RandomChildSelector {
		public IdleSelector(){
			this.addChild(new IdleSad());
			this.addChild(new IdleHover());
			rand();
		}
	}

	class IdleSad : Sequence {
		public IdleSad(){
			this.addChild(new AnimStart("IdleSad"));
			this.addChild(new Wait(5));
			this.addChild(new AnimStop("IdleSad"));
		}
	}

	class IdleHover : Sequence {
		public IdleHover(){
			this.addChild(new AnimStart("IdleHover"));
			this.addChild(new Wait(7));
			this.addChild(new AnimStop("IdleHover"));
		}
	}

	class DrinkingTree : Sequence {
		public DrinkingTree(GameObject go){
			addChild(new Inverter( new WaitInLine() ));
			addChild(new SemaphoreGuard(new MakeDrink(), new HasDrink(go)));
			addChild(new Sequence());
			children[children.Count-1].addChild(new Wait(5));
			children[children.Count-1].addChild(new WalkAway());
		}
	}

	class RepairingTree : Sequence {
		public RepairingTree(GameObject go){
			addChild(new Inverter( new WaitInLine() ));
			addChild(new Sequence());
			children[children.Count-1].addChild(new Wait(5));
			children[children.Count-1].addChild(new WalkAway());
		}
	}

	class SmokeTree : Sequence {
		public SmokeTree(GameObject go){
			addChild(new AnimStart("Smoking"));
			addChild(new Wait(10));
			addChild(new AnimStop("Smoking"));
		}
	}
}