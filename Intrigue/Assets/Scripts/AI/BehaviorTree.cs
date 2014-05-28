using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RBS;

namespace BehaviorTree{
	// Abstract base class for all behavior tree classes
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

	// Chooses one child at "Random" and runs that until destroyed
	class RandomChildSelector : Selector {
		private int index;
		public RandomChildSelector(){}
		public RandomChildSelector( List<Task> children ) : base( children ){
			this.children = children;
			rand();
		}

		public override Status run(GameObject gameObject){
			return children[index].run(gameObject);
		}

		public void rand(){
			index = UnityEngine.Random.Range(0, children.Count);
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

	class SemaphoreGuard : Decorator {
		private Condition cond;
		public SemaphoreGuard(){}
		public SemaphoreGuard( Task child, Condition cond ) : base( child ){
			this.cond = cond;
		}

		public override Status run(GameObject gameObject){
			if( cond.test() ){
				return Status.True;
			}
			return child.run(gameObject);
		}
	}

	public enum Status{
		// No rule and task failed
		False,
		// Found rule and task passed
		True,
		// AI is waiting while walking somewhere or in line
		Waiting,
		// Behavior tree is running
		Tree,
		// Will be used for edge cases
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