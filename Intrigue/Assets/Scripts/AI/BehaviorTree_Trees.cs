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

	class MakeDrink : Sequence{
		public MakeDrink(){
			this.addChild( new GoToDestination() );
			this.addChild( new AtDestination() );
			this.addChild( new CreateDrink() );
			this.addChild( new HoldDrink() );
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
			this.addChild(new IdleComeHere());
			this.addChild(new IdleWave());
			this.addChild(new IdleCough());
			this.addChild(new HeelClick());
			this.addChild(new Yawn());
			this.addChild(new FacePalm());
			this.addChild(new IdleSadHips());
			this.addChild(new BackScratch());
			rand();
		}
	}

	class IdleSad : Sequence {
		public IdleSad(){
			this.addChild(new AnimStart("IdleSad"));
			this.addChild(new Wait(6));
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

	class IdleComeHere : Sequence {
		public IdleComeHere(){
			this.addChild(new AnimStart("IdleComeHere"));
			this.addChild(new Wait(4));
			this.addChild(new AnimStop("IdleComeHere"));
		}
	}

	class IdleWave : Sequence {
		public IdleWave(){
			this.addChild(new AnimStart("IdleWave"));
			this.addChild(new Wait(5));
			this.addChild(new AnimStop("IdleWave"));
		}
	}

	class IdleCough : Sequence {
		public IdleCough(){
			this.addChild(new AnimStart("IdleCough"));
			this.addChild(new Wait(5));
			this.addChild(new AnimStop("IdleCough"));
		}
	}

	class HeelClick : Sequence {
		public HeelClick(){
			this.addChild(new AnimStart("HeelClick"));
			this.addChild(new Wait(3));
			this.addChild(new AnimStop("HeelClick"));
		}
	}

	class Yawn : Sequence {
		public Yawn(){
			this.addChild(new AnimStart("Yawn"));
			this.addChild(new Wait(4));
			this.addChild(new AnimStop("Yawn"));
		}
	}

	class FacePalm : Sequence {
		public FacePalm(){
			this.addChild(new AnimStart("FacePalm"));
			this.addChild(new Wait(4));
			this.addChild(new AnimStop("FacePalm"));
		}
	}

	class IdleSadHips : Sequence {
		public IdleSadHips(){
			this.addChild(new AnimStart("IdleSadHips"));
			this.addChild(new Wait(5));
			this.addChild(new AnimStop("IdleSadHips"));
		}
	}

	class BackScratch : Sequence {
		public BackScratch(){
			this.addChild(new AnimStart("BackScratch"));
			this.addChild(new Wait(5));
			this.addChild(new AnimStop("BackScratch"));
		}
	}

	class DrinkingTree : Sequence {
		public DrinkingTree(GameObject go){
			addChild(new Inverter( new WaitInLine() ));
			addChild(new SemaphoreGuard(new MakeDrink(), new HasDrink()));
			addChild(new Sequence());
			children[children.Count-1].addChild(new Wait(5));
			children[children.Count-1].addChild(new WalkAway());
		}
	}

	class RepairingTree : Sequence {
		public RepairingTree(GameObject go){
			addChild(new Inverter( new WaitInLine() ));
			addChild(new Sequence());
			children[children.Count-1].addChild(new Wait(10));
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