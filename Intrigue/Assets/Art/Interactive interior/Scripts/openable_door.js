#pragma strict

var smooth = 2.0;
var DoorOpenAngle = 90.0;
var DoorCloseAngle = 0.0;
var open : boolean;
var enter : boolean;
var EnterLabel: GameObject;


//Main function
function Update (){


if(open == true){
var target = Quaternion.Euler (0, DoorOpenAngle, 0);
// Dampen towards the target rotation
transform.localRotation = Quaternion.Slerp(transform.localRotation, target,
Time.deltaTime * smooth);
}

if(open == false){
var target1 = Quaternion.Euler (0, DoorCloseAngle, 0);
// Dampen towards the target rotation
transform.localRotation = Quaternion.Slerp(transform.localRotation, target1,
Time.deltaTime * smooth);
}


if(enter == true){

EnterLabel.GetComponent("GUITexture").active = true;

if(Input.GetKeyDown("e")){
open = !open;
}
}
}

//Activate the Main function when player is near the door
function OnTriggerEnter (other : Collider){

if (other.gameObject.tag == "Hand") {
(enter) = true;
}
}

//Deactivate the Main function when player is go away from door
function OnTriggerExit (other : Collider){

if (other.gameObject.tag == "Hand") {
EnterLabel.GetComponent("GUITexture").active = false;
(enter) = false;
}
}
