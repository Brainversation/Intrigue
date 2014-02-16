using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RBS;
using BehaviorTree;

public class BTTest : MonoBehaviour {

	Task treeRoot1;
	Task treeRoot2;

	// Use this for initialization
	void Start () {
		List<Task> sequenceChildren = new List<Task>();
		sequenceChildren.Add(new Run());
		sequenceChildren.Add(new Leap());
		treeRoot1 = new Sequence(sequenceChildren);

		List<Task> children1 = new List<Task>();
		children1.Add( new Jump() );
		children1.Add( new Run() );
		treeRoot2 = new Selector(children1);
        
        List<Rule> rules = new List<Rule>();

        Bar bar = new Bar();
        Library lib = new Library();
        Thirst thirst = new Thirst();
        Party party = new Party();

        List<Condition> conditions0 = new List<Condition>();
        conditions0.Add(bar);
        conditions0.Add(thirst);
        conditions0.Add(party);

        Rule rule0 = new Rule(conditions0, treeRoot1.run);
        rule0.weight = 5;
        rules.Add(rule0);

        List<Condition> conditions1 = new List<Condition>();
        conditions1.Add(bar);
        conditions1.Add(thirst);
        conditions1.Add(party);

        Rule rule1 = new Rule(conditions1, treeRoot2.run);
        rule1.weight = 6;
        rules.Add(rule1);

        //Sort the list in terms of weight
        rules.Sort();

        for (int i = 0; i < rules.Count; i++){
        	Debug.Log("Testing rules");
            if (rules[i].isFired()){
	        	Debug.Log("Rule fired");
                rules[i].consequence();
                break;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
