using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

namespace TempRBS{
    public delegate Status ConsequenceFunction(GameObject gameObject);

    public abstract class Condition {
        protected GameState game;
        protected GameObject gameObject;

        public Condition(){
            game = GameState.Game;
        }

        public Condition(GameObject gameObject) {
            game = GameState.Game;
            this.gameObject = gameObject;
        }

        abstract public bool test();
    }

    public class Rule : IComparable{
        // every element in this list is a condition but the last;
        // the last element is the consequence
        public List<Condition> conditions;
        public ConsequenceFunction consequence;    
        public int weight = 0;

        public Rule(){
            this.conditions = new List<Condition>();
        }

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

    // <---------------- Conditions ------------------>

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

    class Party : Condition{
        public override bool test(){
            if (game.personality == "Party"){
                return true;
            }
            return false;
        }
    }

    class Thirst : Condition{

        public Thirst(GameObject gameObject):base(gameObject){}

        public override bool test(){
            if (gameObject.GetComponent<TempBaseAI>().thirst > 50){
                return true;
            }
            return false;
        }
    }

    class Anxious : Condition{
        public Anxious(GameObject gameObject):base(gameObject){}

        public override bool test(){
            if(gameObject.GetComponent<TempBaseAI>().anxiety> 50){
                return true;
            }
            return false;
        }
    }

    class TimeInRoom : Condition{
        public TimeInRoom(GameObject gameObject):base(gameObject){}

        public override bool test(){
            if(gameObject.GetComponent<TempBaseAI>().timeInRoom > 10f){
                return true;
            }
            return false;
        }
    }

    class RoomIsNull : Condition{
        public RoomIsNull(GameObject gameObject):base(gameObject){}

        public override bool test(){
            if(gameObject.GetComponent<TempBaseAI>().room == null){
                return true;
            }
            return false;
        }
    }

    class Bored : Condition{

        public Bored(GameObject gameObject):base(gameObject){}

        public override bool test(){
            if (gameObject.GetComponent<TempBaseAI>().bored > 50){
                return true;
            }
            return false;
        }
    }

    class NotBored : Condition{
        public NotBored(GameObject gameObject):base(gameObject){}

        public override bool test(){
            if (!(gameObject.GetComponent<TempBaseAI>().bored > 50)){
                return true;
            }
            return false;
        }
    }

    class AtDest: Condition{
        public AtDest(GameObject gameObject):base(gameObject){
        }

        public override bool test(){
            if (!gameObject.GetComponent<NavMeshAgent>().hasPath){
                return true;
            }
            return false;
        }
    }

    class DestChange : Condition{
        private Vector3 currDest;

        public DestChange(GameObject gameObject):base(gameObject){
            currDest = gameObject.transform.position;
        }

        public override bool test(){
            if (currDest != gameObject.GetComponent<TempBaseAI>().destination){
                currDest = gameObject.GetComponent<TempBaseAI>().destination;
                return true;
            }
            return false;
        }
    }

    // <------------------------- Rules -------------------->

    class WantToRandRoom : Rule{
        public WantToRandRoom(GameObject gameObject){
            this.conditions.Add(new Bored(gameObject));
            this.conditions = conditions;
            this.consequence = setDestRoom;
        }

        private Status setDestRoom(GameObject gameObject){
            Vector3 newRoomLoc;
            string newRoom;
            int roomSelect;
            roomSelect = UnityEngine.Random.Range(1,10);
            newRoom = "Room" + roomSelect.ToString();
            //Debug.Log("Chosen Room: " + newRoom);
            newRoomLoc = UnityEngine.GameObject.Find(newRoom.ToString()).transform.position;
            //Debug.Log("newRoomLoc: " + newRoomLoc);
            //gameObject.GetComponent<TempBaseAI>().room = UnityEngine.GameObject.Find(newRoom);
            gameObject.GetComponent<TempBaseAI>().destination = newRoomLoc;
            gameObject.GetComponent<TempBaseAI>().bored = 0;
            return Status.True;
        }
    }

    class WantToWanderRoom : Rule{
        public WantToWanderRoom(GameObject gameObject){
            this.conditions.Add(new AtDest(gameObject));
            this.conditions.Add(new NotBored(gameObject));
            this.conditions = conditions;
            this.consequence = setDestInRoom;
        }

        private Status setDestInRoom(GameObject gameObject){
            Vector3 newDest;
            newDest = new Vector3(UnityEngine.Random.Range(gameObject.GetComponent<TempBaseAI>().room.GetComponent<BoxCollider>().bounds.min.x,
                                                            gameObject.GetComponent<TempBaseAI>().room.GetComponent<BoxCollider>().bounds.max.x),
                                                            gameObject.transform.position.y,
                                                            UnityEngine.Random.Range(gameObject.GetComponent<TempBaseAI>().room.GetComponent<BoxCollider>().bounds.min.z,
                                                            gameObject.GetComponent<TempBaseAI>().room.GetComponent<BoxCollider>().bounds.max.z));
            gameObject.GetComponent<TempBaseAI>().bored += 10;
            gameObject.GetComponent<TempBaseAI>().destination = newDest;
            return Status.True;
        }
    }

    class WantToInitRoom : Rule{
        public WantToInitRoom(GameObject gameObject){
            this.conditions.Add(new RoomIsNull(gameObject));
            this.conditions = conditions;
            this.consequence = setDestRoom;
        }

        private Status setDestRoom(GameObject gameObject){
            Vector3 newRoomLoc;
            string newRoom;
            int roomSelect;
            roomSelect = UnityEngine.Random.Range(1,10);
            newRoom = "Room" + roomSelect.ToString();
            //newRoom = "Room1";
            //Debug.Log("Init Room: " + newRoom);
            newRoomLoc = UnityEngine.GameObject.Find(newRoom.ToString()).transform.position;
            //Debug.Log("InitRoom Pos: " + newRoomLoc);
            gameObject.GetComponent<TempBaseAI>().room = UnityEngine.GameObject.Find(newRoom);
            gameObject.GetComponent<TempBaseAI>().destination = newRoomLoc;
            return Status.True;
        }
    }

    class WantToGoToBar : Rule{
        Vector3 barLocation;
        public WantToGoToBar(GameObject gameObject) {
            this.conditions.Add(new Thirst(gameObject));
            this.conditions.Add(new Bored(gameObject));
            this.conditions = conditions;
            this.consequence = setDestRoom;
            this.barLocation = GameObject.Find("Bar").transform.position;
        }

        private Status setDestRoom(GameObject gameObject){
            Debug.Log("Set Dest");
            gameObject.GetComponent<TempBaseAI>().timeInRoom = 0f;
            gameObject.GetComponent<Animator>().SetFloat("Speed", .2f);
            // Vector3 temp = barLocation;
            // NavMeshHit hit;
            // do{
            //     temp[0] += UnityEngine.Random.Range(0, 20);
            //     temp[2] += UnityEngine.Random.Range(0, 20);
            // } while(NavMesh.SamplePosition(temp, out hit, 10, 1));
            gameObject.GetComponent<TempBaseAI>().destination = barLocation;
            return Status.True;
        }
    }

    class GoToDestination : Rule{
        public GoToDestination(GameObject gameObject) {
            this.conditions.Add(new DestChange(gameObject));
            this.conditions = conditions;
            this.consequence = go;
        }

        private Status go(GameObject gameObject){
            Debug.Log("go to Dest");
            Vector3 dest = gameObject.GetComponent<TempBaseAI>().destination;
            gameObject.GetComponent<Animator>().SetFloat("Speed", .2f);
            gameObject.GetComponent<NavMeshAgent>().SetDestination(dest);
            return Status.True;
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