using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

namespace RBS{
    public delegate Status ConsequenceFunction();

    public abstract class Condition {
        protected GameState game;

        public Condition() {
            game = GameState.Game;
        }

        abstract public bool test();
    }

    public class Rule : IComparable{
        // every element in this list is a condition but the last;
        // the last element is the consequence
        public List<Condition> conditions;
        public ConsequenceFunction consequence;    
        public int weight = 0;

        public Rule(List<Condition> conditions, ConsequenceFunction consequence){
            this.conditions = conditions;
            this.consequence = consequence;
        }

        public bool isFired(){
            foreach(Condition con in conditions){
                if (!con.test()){
                    return false;
                }
            }
            return true;
        }

        public int CompareTo(object other){
            Rule otherRule = other as Rule;

            if (otherRule == null) return -1;

            if (otherRule.weight < this.weight){
                return -1;
            }
            else if (otherRule.weight > this.weight){
                return 1;
            }
            else{
                return 0;
            }


        }
    }

    class Bar : Condition {
        public override bool test(){
            if (game.room == "Bar") {
                return true;
            }
            return false;
        }
    }

    class Library : Condition{
        public override bool test(){
            if (game.room == "Library"){
                return true;
            }
            return false;
        }
    }

    class Thirst : Condition{
        public override bool test(){
            if (game.thirst){
                return true;
            }
            return false;
        }
    }

    class Party : Condition{
        public override bool test(){
            if (game.personality == "Party"){
                return true;
            }
            return false;
        }
    }

    public class GameState {
        // Testing Game State
        private GameState(){}

        private static GameState gameState = null;
        public static GameState Game {
            get {
                if (gameState == null){
                    gameState = new GameState();
                }
                return gameState;
            }
        }

        //////////////////////////
        // Fields ///////////////
        /////////////////////////

        public string room = "Bar";
        public string personality = "Party";
        public bool thirst = true;
        public bool bored = true;
    }

}
