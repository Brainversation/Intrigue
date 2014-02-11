using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RBS;
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
		//Debug.Log( root.run() );
        
        List<Rule> rules = new List<Rule>();

        Bar bar = new Bar();
        Library lib = new Library();
        Thirst thirst = new Thirst();
        Party party = new Party();

        List<Condition> conditions0 = new List<Condition>();
        conditions0.Add(bar);
        conditions0.Add(thirst);
        conditions0.Add(party);

        Rule rule0 = new Rule(conditions0, root.run);
        rule0.weight = 2;
        rules.Add(rule0);

        List<Condition> conditions1 = new List<Condition>();
        conditions1.Add(bar);
        conditions1.Add(thirst);
        conditions1.Add(party);

        Rule rule1 = new Rule(conditions1, root.run);
        rule1.weight = 3;
        rules.Add(rule1);

        //Sort the list in terms of weight
        rules.Sort();

        for (int i = 0; i < rules.Count; i++){
            if (rules[i].isFired()){
                rules[i].consequence();
                break;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
