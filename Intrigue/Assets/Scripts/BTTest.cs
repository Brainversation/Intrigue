using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using BehaviorTree;

public class BTTest : MonoBehaviour {

	Task root;

	// Use this for initialization
	void Start () {
		List<Task> children = new List<Task>();
		children.Add( new Jump() );
		List<Task> sequenceChildren = new List<Task>();
		sequenceChildren.Add(new Run());
		sequenceChildren.Add(new Leap());
		children.Add(new Sequence(sequenceChildren));
		root = new Selector(children);
		Debug.Log( root.run() );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
