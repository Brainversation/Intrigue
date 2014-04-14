using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BehaviorTree{

	public abstract class Task {
		public List<Task> children;

		public Task(){
			children = new List<Task>();
		}

		public Task( List<Task> children ){
			this.children = children;
		}

		public void addChild( Task task ){
			children.Add(task);
		}

		abstract public Status run(GameObject gameObject);
	}

	class Selector : Task {

		public Selector(){}
		public Selector( List<Task> children ) : base(children){}

		public override Status run(GameObject gameObject){
			foreach( Task t in children ){
				if( t.run(gameObject) == Status.True ){
					return Status.True;
				}
			}

			return Status.False;
		}
	}

	class NonDeterministicSelector : Task{
		public NonDeterministicSelector(){}

		public NonDeterministicSelector( List<Task> children ) : base(children){}

		public override Status run(GameObject gameObject){
			List<Task> shuffled = children;
			shuffled.Shuffle();
			foreach( Task t in shuffled ){
				if( t.run(gameObject) == Status.True ){
					return Status.True;
				}
			}

			return Status.False;
		}
	}

	class Sequence : Task {
		public Sequence(){}
		public Sequence( List<Task> children ) : base(children){}
		
		public override Status run(GameObject gameObject){
			foreach( Task t in children ){
				if( t.run(gameObject) != Status.True ){
					return Status.False;
				}
			}

			return Status.True;
		}
	}

	class NonDeterministicSequence : Task{

		public NonDeterministicSequence( List<Task> children ) : base(children){}

		public override Status run(GameObject gameObject){
			List<Task> shuffled = children;
			shuffled.Shuffle();
			foreach( Task t in shuffled ){
				if( t.run(gameObject) != Status.True ){
					return Status.False;
				}
			}

			return Status.True;
		}
	}

	abstract class Decorator : Task {
		public Task child;

		public Decorator(){}

		public Decorator( Task child ){
			this.child = child;
		}
	}

	class Limit : Decorator{

		private int runLimit;
		private int runSoFar;

		public Limit( Task child, int runLimit ) : base(child) {
			this.runLimit = runLimit;
		}

		public override Status run(GameObject gameObject){
			if( runSoFar >= runLimit ){
				return Status.False;
			}

			++runSoFar;
			return children[0].run(gameObject);
		}
	}

	class Inverter : Decorator {
		public Inverter(){}
		public Inverter( Task child ) : base( child ){}

		public override Status run(GameObject gameObject){
			switch( child.run(gameObject) ){
				case Status.True:
					return Status.False;
				case Status.False:
					return Status.True;
				default:
					return Status.Error;
			}
		}
	}

	// <--------------- Actions -------------------->

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

	class Idle1 : Task { 
		public override Status run(GameObject gameObject){
			// gameObject.GetComponent<Animator>().SetBool("Idle1", true);
			return Status.True;
		}
	}

	class Idle2 : Task { 
		public override Status run(GameObject gameObject){
			// gameObject.GetComponent<Animator>().SetBool("Idle2", true);
			return Status.True;
		}
	}

	class GoToDestination : Task {
		public override Status run(GameObject gameObject){
			Vector3 dest = gameObject.GetComponent<BaseAI>().destination;
			gameObject.GetComponent<Animator>().SetFloat("Speed", .2f);
			gameObject.GetComponent<BaseAI>().distFromDest = 5f;
			gameObject.GetComponent<NavMeshAgent>().SetDestination(dest);
			return Status.True;
		}
	}

	class WalkAway : Task {
		public override Status run(GameObject gameObject){
			// Debug.Log("Walking away");
			Vector3 newDest;
			newDest = new Vector3(UnityEngine.Random.Range( gameObject.GetComponent<BaseAI>().room.me.GetComponent<BoxCollider>().bounds.min.x,
															gameObject.GetComponent<BaseAI>().room.me.GetComponent<BoxCollider>().bounds.max.x),
															gameObject.transform.position.y,
															UnityEngine.Random.Range(gameObject.GetComponent<BaseAI>().room.me.GetComponent<BoxCollider>().bounds.min.z,
															gameObject.GetComponent<BaseAI>().room.me.GetComponent<BoxCollider>().bounds.max.z));
			gameObject.GetComponent<BaseAI>().destination = newDest;
			gameObject.GetComponent<BaseAI>().distFromDest = 10f;
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

	class doPoetry : Task{
		public override Status run(GameObject gameObject){
			gameObject.GetComponent<Animator>().SetBool("Poetry", true);
			Debug.Log("Now reading Poetry!!");
			return Status.True;
		}
	}

	class doSmoking : Task{
		public override Status run(GameObject gameObject){
			gameObject.GetComponent<Animator>().SetBool("Smoke", true);
			Debug.Log("Now smoking");
			return Status.True;
		}
	}

	class Wait : Task{

		private float seconds = 0;

		public Wait(float seconds){
			this.seconds = seconds;
		}

		public override Status run(GameObject gameObject){
			if(seconds >= 0){
				// Debug.Log("Waiting...");
				seconds -= Time.deltaTime;
				return Status.False;
			}
			// Debug.Log("Done Waiting");
			return Status.True;
		}
	}

	// <---------------------- Behave Trees ------------------------>
	class MakeDrink : Sequence{
		public MakeDrink(){
			this.addChild( new GoToDestination() );
			this.addChild( new HoldDrink() );
			this.addChild( new CreateDrink() );
			this.addChild( new WalkAway() );
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

	class IdleSelector : NonDeterministicSelector {
		public IdleSelector(){
			this.addChild(new Idle1());
			this.addChild(new Idle2());
		}
	}

	class DrinkingTree : Sequence {
		public DrinkingTree(){
			addChild(new Sequence());
			children[children.Count-1].addChild(new Inverter( new WaitInLine() ));
			children[children.Count-1].addChild(new MakeDrink());
		}
	}

	class PoetryTree : Sequence{
		public PoetryTree(){
			addChild(new Sequence());
			//this.addChild(new Wait);
			this.addChild(new doPoetry());
		}

	}

	class SmokeTree : Sequence{
		public SmokeTree(){
			addChild(new Sequence());
			this.addChild(new doSmoking());
		}
	}

	public enum Status{
		False,
		True,
		Waiting,
		Tree,
		Error
	}

	static class ExtraMethods{
		public static void Shuffle<T>(this IList<T> list){
			int n = list.Count;
			while (n > 1) {
				n--;
				int k = Random.Range(0, n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
	}
}