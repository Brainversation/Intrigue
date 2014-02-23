using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BehaviorTree{

	public abstract class Task {
		protected List<Task> children;

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
			// Debug.Log("Selecting Task");
			foreach( Task t in children ){
				if( t.run(gameObject) == Status.True ){
					return Status.True;
				}
			}

			return Status.False;
		}
	}

	class NonDeterministicSelector : Task{

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
			// Debug.Log("Going through Sequence");
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
		protected Task child;

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

	class UntilFail : Decorator {

		public UntilFail( Task child ) : base(child){}

		public override Status run(GameObject gameObject){
			while(true){
				if( child.run(gameObject) == Status.False ) return Status.True;
			}
		}
	}

	class Inverter : Decorator {
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

	// <---------------------- Behave Trees ------------------------>
	class MakeDrink : Sequence{
		public MakeDrink(){
			this.addChild( new HoldDrink() );
			this.addChild( new CreateDrink() );
		}

		public override Status run(GameObject gameObject){
			gameObject.GetComponent<BaseAI>().thirst -= 10;
			gameObject.GetComponent<BaseAI>().bladder += 5;
			gameObject.GetComponent<BaseAI>().atDrink = false;
			return base.run(gameObject);
		}
	}

	public enum Status{
		False,
		True,
		Waiting,
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