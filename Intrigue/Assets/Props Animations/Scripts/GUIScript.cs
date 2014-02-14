using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {

	// Use this for initialization
	public Animator animator;
	public Animator animator2;
	public Animator animator3;
	public Vector2 scrollPosition = Vector2.zero;
	public Vector2 scrollPos2 = Vector2.zero;
	public bool idleStandbool = true;
	public bool idleReadybool = false;
	public bool idleMonsterbool = false;
	public bool idleFightbool = false;
	public bool weaponStandbool = false;
	public bool pistolReadybool = false;
	public bool weaponRun = false;
	public bool oneHandSwordIdle = false;
	public bool bowIdle = false;
	public bool motorbikeIdle = false;
	public bool rollerBlade = false;
	public bool skateboard = false;
	public bool climbing = false;
	public bool office = false;
	public bool swim = false;
	public bool wand = false;
	public bool cards = false;
	public float LHandWeight = 0;
	public Transform LHandPos1;
	public Transform LHandPos2;
	public Transform LHandPos3;
	
    void OnGUI() {
    
    	
 		scrollPos2 = GUI.BeginScrollView(new Rect(10, 10, 120, 285), scrollPos2, new Rect(0, 0, 100, 375));
 		if(GUI.Button(new Rect(0, 0, 100, 20), "Idle Stand"))
 			IdleStand();
 		if(GUI.Button(new Rect(0, 25, 100, 20), "Idle Ready"))
 			IdleReady();
 		if(GUI.Button(new Rect(0, 50, 100, 20), "Idle Monster"))
 			IdleMonster();
 		if(GUI.Button(new Rect(0, 75, 100, 20), "Weapon Stand"))
 			WeaponStand();
 		if(GUI.Button(new Rect(0, 100, 100, 20), "Pistol Ready"))
 			PistolReady();
 		if(GUI.Button(new Rect(0, 125, 100, 20), "1 Hand Sword"))
 			OneHandSwordIdle();
		if(GUI.Button(new Rect(0, 150, 100, 20), "Bow"))
 			BowIdle();
		if(GUI.Button(new Rect(0, 175, 100, 20), "Motorbike"))
 			MotorbikeIdle();
		if(GUI.Button(new Rect(0, 200, 100, 20), "RollerBlade"))
			RollerBladeStand();
		if(GUI.Button(new Rect(0, 225, 100, 20), "Skateboard"))
			SkateboardIdle();
		if(GUI.Button(new Rect(0, 250, 100, 20), "Climbing"))
			ClimbIdle();
		if(GUI.Button(new Rect(0, 275, 100, 20), "Office"))
			OfficeSitting();
		if(GUI.Button(new Rect(0, 300, 100, 20), "Swimming"))
			Swim();
		if(GUI.Button(new Rect(0, 325, 100, 20), "Wand/Staff"))
			WandStand();
		if(GUI.Button(new Rect(0, 350, 100, 20), "Cards"))
			DealerIdle();
 			
 		GUI.EndScrollView();
 		
 		if(idleStandbool){
    		scrollPosition = GUI.BeginScrollView(new Rect(150, 10, 150, 285), scrollPosition, new Rect(0, 0, 120, 1275));
        	if(GUI.Button(new Rect(0, 0, 100, 20), "Idle Cheer"))
        		IdleCheer();
        	if(GUI.Button(new Rect(0, 25, 120, 20), "Idle Come Here"))
        		ComeHere();
        	if(GUI.Button(new Rect(0, 50, 120, 20), "Idle Keep Back"))
        		IdleKeepBack();
        	if(GUI.Button(new Rect(0, 75, 100, 20), "Idle Sad"))
        		IdleSad();
			if(GUI.Button(new Rect(0, 100, 100, 20), "Idle Walk"))
        		IdleWalk();
			if(GUI.Button(new Rect(0, 125, 120, 20), "Idle Strafe Right"))
        		IdleStrafeRight();
			if(GUI.Button(new Rect(0, 150, 120, 20), "Idle Strafe Left"))
        		IdleStrafeLeft();
			if(GUI.Button(new Rect(0, 175, 120, 20), "Strafe Run Left"))
        		StrafeRunLeft();
			if(GUI.Button(new Rect(0, 200, 120, 20), "Strafe Run Right"))
        		StrafeRunRight();
			if(GUI.Button(new Rect(0, 225, 120, 20), "Run Backward"))
				RunBackward();
			if(GUI.Button(new Rect(0, 250, 120, 20), "Run Back Left"))
				RunBackLeft();
			if(GUI.Button(new Rect(0, 275, 120, 20), "Run Back Right"))
				RunBackRight();
			if(GUI.Button(new Rect(0, 300, 120, 20), "Cowboy1HandDraw"))
        		Cowboy1HandDraw();
			if(GUI.Button(new Rect(0, 325, 120, 20), "Crate Push"))
        		CratePush();
			if(GUI.Button(new Rect(0, 350, 120, 20), "Crate Pull"))
        		CratePull();
			if(GUI.Button(new Rect(0, 375, 120, 20), "Idle 90 Deg Turns"))
        		IdleTurns();
			if(GUI.Button(new Rect(0, 400, 120, 20), "Idle Meditate"))
        		IdleMeditate();
			if(GUI.Button(new Rect(0, 425, 120, 20), "Idle 180"))
        		Idle180();
			if(GUI.Button(new Rect(0, 450, 120, 20), "Idle Button Press"))
        		IdleButtonPress();
			if(GUI.Button(new Rect(0, 475, 120, 20), "Idle Typing"))
        		IdleTyping();
			if(GUI.Button(new Rect(0, 500, 120, 20), "Idle Stun"))
        		IdleStun();
			if(GUI.Button(new Rect(0, 525, 120, 20), "Wood Cut"))
        		WoodCut();
			if(GUI.Button(new Rect(0, 550, 120, 20), "Worker Hammer"))
        		WorkerHammer();
			if(GUI.Button(new Rect(0, 575, 120, 20), "Worker Hammer 2"))
        		WorkerHammer2();
			if(GUI.Button(new Rect(0, 600, 120, 20), "Worker Pickaxe"))
        		WorkerPickaxe();
			if(GUI.Button(new Rect(0, 625, 120, 20), "Worker Pickaxe 2"))
        		WorkerPickaxe2();
			if(GUI.Button(new Rect(0, 650, 120, 20), "Worker Shovel"))
        		WorkerShovel();
			if(GUI.Button(new Rect(0, 675, 120, 20), "Worker Shovel 2"))
        		WorkerShovel2();
			if(GUI.Button(new Rect(0, 700, 120, 20), "Idle Spew"))
        		IdleSpew();
			if(GUI.Button(new Rect(0, 725, 120, 20), "Idle Mouth Wipe"))
        		IdleMouthWipe();
			if(GUI.Button(new Rect(0, 750, 120, 20), "Crawl Idle"))
        		CrawlIdle();
			if(GUI.Button(new Rect(0, 775, 120, 20), "Crawl Locomotion"))
        		CrawlLocomotion();
			if(GUI.Button(new Rect(0, 800, 120, 20), "Prone Idle"))
        		ProneIdle();
			if(GUI.Button(new Rect(0, 825, 120, 20), "Prone Locomotion"))
        		ProneLocomotion();
			if(GUI.Button(new Rect(0, 850, 120, 20), "Idle Feed Throw"))
				IdleFeedThrow();
			if(GUI.Button(new Rect(0, 875, 120, 20), "Idle Standing Jump"))
				IdleStandingJump();
			if(GUI.Button(new Rect(0, 900, 120, 20), "Yawn"))
				Yawn();
			if(GUI.Button(new Rect(0, 925, 120, 20), "Heel Click"))
				HeelClick();
			if(GUI.Button(new Rect(0, 950, 120, 20), "Vader Choke"))
				VaderChoke();
			if(GUI.Button(new Rect(0, 975, 120, 20), "Watering Can Idle"))
				WateringCan();
			if(GUI.Button(new Rect(0, 1000, 120, 20), "Watering Can Watering"))
				WateringCanWatering();
			if(GUI.Button(new Rect(0, 1025, 120, 20), "Up Hill Walk"))
				UpHillWalk();
			if(GUI.Button(new Rect(0, 1050, 120, 20), "UphillWalk Hand Grab"))
				UpHillWalkHandGrab();
			if(GUI.Button(new Rect(0, 1075, 120, 20), "Walk Dehydrated"))
				WalkDehydrated();
			if(GUI.Button(new Rect(0, 1100, 120, 20), "Idle Sand Cover"))
				IdleSandCover();
			if(GUI.Button(new Rect(0, 1125, 120, 20), "Battle Roar"))
				BattleRoar();
			if(GUI.Button(new Rect(0, 1150, 120, 20), "Channel Cast Directed"))
				ChannelCastDirected();
			if(GUI.Button(new Rect(0, 1175, 120, 20), "Channel Cast Omni"))
				ChannelCastOmni();
			if(GUI.Button(new Rect(0, 1200, 120, 20), "Fire Breath"))
				FireBreath();
			if(GUI.Button(new Rect(0, 1225, 120, 20), "Mutilate"))
				Mutilate();
			if(GUI.Button(new Rect(0, 1250, 120, 20), "Storm Strike"))
				StormStrike();
			        	
        	GUI.EndScrollView();
        }else if(idleReadybool){
        	scrollPosition = GUI.BeginScrollView(new Rect(150, 10, 180, 285), scrollPosition, new Rect(0, 0, 150, 750));
			if(GUI.Button(new Rect(0, 0, 100, 20), "Ready Fight") && animator.GetFloat("Curve") == 0)
				IdleFight();
			if(GUI.Button(new Rect(0, 25, 100, 20), "Ready Crouch"))
				IdleReadyCrouch();
			if(GUI.Button(new Rect(0, 50, 100, 20), "Crouch 180"))
				Crouch180();
			if(GUI.Button(new Rect(0, 75, 100, 20), "Crouch Walk"))
				CrouchWalk();
			if(GUI.Button(new Rect(0, 100, 150, 20), "Crouch Walk Backward"))
				CrouchWalkBackward();
			if(GUI.Button(new Rect(0, 125, 150, 20), "Crouch Strafe Left"))
				CrouchStrafeLeft();
			if(GUI.Button(new Rect(0, 150, 150, 20), "Crouch Strafe Right"))
				CrouchStrafeRight();
			if(GUI.Button(new Rect(0, 175, 100, 20), "Ready Look"))
				IdleReadyLook();
			if(GUI.Button(new Rect(0, 200, 150, 20), "Wizard 1 Hand Throw"))
				Wizard1HandThrow();
			if(GUI.Button(new Rect(0, 225, 150, 20), "Wizard 2 Hand Throw"))
				Wizard2HandThrow();
			if(GUI.Button(new Rect(0, 250, 150, 20), "Wizard Block"))
				WizardBlock();
			if(GUI.Button(new Rect(0, 275, 150, 20), "Wizard Overhead"))
				WizardOverhead();
			if(GUI.Button(new Rect(0, 300, 150, 20), "Wizard Power Up"))
				WizardPowerUp();
			if(GUI.Button(new Rect(0, 325, 150, 20), "Wizard Eye Beam"))
				WizardEyeBeam();
			if(GUI.Button(new Rect(0, 350, 150, 20), "Wizard Neo Block"))
				WizardNeoBlock();
			if(GUI.Button(new Rect(0, 375, 150, 20), "Idle Dodge Left"))
				IdleDodgeLeft();
			if(GUI.Button(new Rect(0, 400, 150, 20), "Idle Dodge Right"))
				IdleDodgeRight();
			if(GUI.Button(new Rect(0, 425, 150, 20), "Idle Die"))
				IdleDie();
			if(GUI.Button(new Rect(0, 450, 150, 20), "Idle Run"))
				IdleRun();
			if(GUI.Button(new Rect(0, 475, 150, 20), "Run Jump"))
				RunJump();
			if(GUI.Button(new Rect(0, 500, 150, 20), "Run Dive"))
				RunDive();
			if(GUI.Button(new Rect(0, 525, 150, 20), "Idle Fly"))
				IdleFly();
			if(GUI.Button(new Rect(0, 550, 150, 20), "Fly Forward"))
				FlyForward();
			if(GUI.Button(new Rect(0, 575, 150, 20), "Fly Backward"))
				FlyBackward();
			if(GUI.Button(new Rect(0, 600, 150, 20), "Fly Left"))
				FlyLeft();
			if(GUI.Button(new Rect(0, 625, 150, 20), "Fly Right"))
				FlyRight();
			if(GUI.Button(new Rect(0, 650, 150, 20), "Fly Up"))
				FlyUp();
			if(GUI.Button(new Rect(0, 675, 150, 20), "Fly Down"))
				FlyDown();
			if(GUI.Button(new Rect(0, 700, 120, 20), "Running Slide"))
				IdleSlide();


			
			
        	GUI.EndScrollView();
        }else if(idleFightbool){
        	scrollPosition = GUI.BeginScrollView(new Rect(150, 10, 180, 110), scrollPosition, new Rect(0, 0, 150, 150));
			if(GUI.Button(new Rect(0, 0, 150, 20), "Back to Idle Ready"))
				IdleReady();
			if(GUI.Button(new Rect(0, 25, 150, 20), "Front Kick"))
				FrontKick();
			if(GUI.Button(new Rect(0, 50, 150, 20), "Face Hit"))
				FaceHit();
			if(GUI.Button(new Rect(0, 75, 150, 20), "L Hand Punch"))
				LHandPunch();
			if(GUI.Button(new Rect(0, 100, 150, 20), "R Hand Punch"))
				RHandPunch();
			if(GUI.Button(new Rect(0, 125, 150, 20), "360 Death"))
				SpinDeath();
 					
 			GUI.EndScrollView();
        }else if(weaponStandbool){
        	scrollPosition = GUI.BeginScrollView(new Rect(150, 10, 180, 285), scrollPosition, new Rect(0, 0, 150, 350));
			if(GUI.Button(new Rect(0, 0, 150, 20), "Weapon Ready"))
				WeaponReady();
			if(GUI.Button(new Rect(0, 25, 150, 20), "Weapon Instant"))
				WeaponInstant();
			if(GUI.Button(new Rect(0, 50, 150, 20), "Weapon Fire"))
				WeaponFire();
			if(GUI.Button(new Rect(0, 75, 150, 20), "Weapon Ready Fire"))
				WeaponReadyFire();
			if(GUI.Button(new Rect(0, 100, 150, 20), "Weapon Reload"))
				WeaponReload();
			if(GUI.Button(new Rect(0, 125, 150, 20), "Shotgun Fire"))
				ShotgunFire();
			if(GUI.Button(new Rect(0, 150, 150, 20), "Shotgun Ready Fire"))
				ShotgunReadyFire();
			if(GUI.Button(new Rect(0, 175, 175, 20), "Shotgun Reload Chamber"))
				ShotgunReloadChamber();
			if(GUI.Button(new Rect(0, 200, 175, 20), "Shotgun Reload Magazine"))
				ShotgunReloadMagazine();
			if(GUI.Button(new Rect(0, 225, 150, 20), "Nade Throw"))
				NadeThrow();
			if(GUI.Button(new Rect(0, 250, 150, 20), "Weapon Run"))
				WeaponRun();
			if(GUI.Button(new Rect(0, 275, 150, 20), "Weapon Strafe Run Left"))
				WeaponStrafeRunLeft();
			if(GUI.Button(new Rect(0, 300, 150, 20), "Weapon Strafe Run Right"))
				WeaponStrafeRunRight();
			if(GUI.Button(new Rect(0, 325, 150, 20), "Weapon Run Backward"))
				WeaponRunBackward();
			
			
 					
 			GUI.EndScrollView();
        }else if(pistolReadybool){
        	scrollPosition = GUI.BeginScrollView(new Rect(150, 10, 180, 285), scrollPosition, new Rect(0, 0, 150, 100));
			if(GUI.Button(new Rect(0, 0, 150, 20), "Pistol Instant"))
				PistolInstant();
			if(GUI.Button(new Rect(0, 25, 150, 20), "Pistol Fire"))
				PistolFire();
			if(GUI.Button(new Rect(0, 50, 150, 20), "Pistol Reload"))
				PistolReload();
			if(GUI.Button(new Rect(0, 75, 150, 20), "Pistol Quick Stab"))
				PistolLeftHandStab();
 					
 			GUI.EndScrollView();
        }else if(oneHandSwordIdle){
        	scrollPosition = GUI.BeginScrollView(new Rect(150, 10, 220, 285), scrollPosition, new Rect(0, 0, 200, 325));
        		if(GUI.Button(new Rect(0, 0, 200, 20), "1 Hand Sword Ready"))
 					OneHandSwordReady();
 				if(GUI.Button(new Rect(0, 25, 200, 20), "1 Hand Sword Swing"))
 					OneHandSwordSwing();
 				if(GUI.Button(new Rect(0, 50, 200, 20), "1 Hand Sword Back Swing"))
 					OneHandSwordBackSwing();
 				if(GUI.Button(new Rect(0, 75, 200, 20), "1 Hand Sword Jab"))
 					OneHandSwordJab();
 				if(GUI.Button(new Rect(0, 100, 200, 20), "1 Hand Sword Block"))
 					OneHandSwordBlock();
			if(GUI.Button(new Rect(0, 125, 200, 20), "1 Hand Sword Shield Bash"))
 					OneHandSwordShieldBash();
			if(GUI.Button(new Rect(0, 150, 200, 20), "1 Hand Sword Charge Up"))
				OneHandSwordChargeUp();
			if(GUI.Button(new Rect(0, 175, 200, 20), "1 H Sword Charge Heavy Bash"))
				OneHandSwordChargeHeavyBash();
			if(GUI.Button(new Rect(0, 200, 200, 20), "1 Hand Sword Charge Swipe"))
				OneHandSwordChargeSwipe();
			if(GUI.Button(new Rect(0, 225, 200, 20), "1 Hand Sword Run"))
				OneHandSwordRun();
			if(GUI.Button(new Rect(0, 250, 200, 20), "1 Hand Sword Strafe Left"))
 					OneHandSwordStrafeLeft();
			if(GUI.Button(new Rect(0, 275, 200, 20), "1 Hand Sword Strafe Right"))
 					OneHandSwordStrafeRight();
			if(GUI.Button(new Rect(0, 300, 200, 20), "1 Hand Sword Roll Attack"))
 					OneHandSwordRollAttack();
    		GUI.EndScrollView();
    	}else if(bowIdle){
        	scrollPosition = GUI.BeginScrollView(new Rect(150, 10, 180, 285), scrollPosition, new Rect(0, 0, 150, 100));
			if(GUI.Button(new Rect(0, 0, 150, 20), "Bow Idle"))
 					BowIdle();
			if(GUI.Button(new Rect(0, 25, 150, 20), "Bow Ready"))
				BowReady();
			if(GUI.Button(new Rect(0, 50, 150, 20), "Bow Instant"))
				BowInstant();
			if(GUI.Button(new Rect(0, 75, 150, 20), "Bow Fire"))
				BowFire();
			if(GUI.Button(new Rect(0, 100, 150, 20), "Bow Ready2"))
				BowReady2();
			if(GUI.Button(new Rect(0, 125, 150, 20), "Bow Instant2"))
				BowInstant2();
			if(GUI.Button(new Rect(0, 150, 150, 20), "Bow Fire2"))
				BowFire2();
    		GUI.EndScrollView();
    	}else if(motorbikeIdle){
        	scrollPosition = GUI.BeginScrollView(new Rect(150, 10, 180, 285), scrollPosition, new Rect(0, 0, 150, 650));
			if(GUI.Button(new Rect(0, 0, 150, 20), "Airwalk"))
				MotorbikeAirWalk();
			if(GUI.Button(new Rect(0, 25, 150, 20), "Backward Sitting"))
				MotorbikeBackwardSitting();
			if(GUI.Button(new Rect(0, 50, 150, 20), "Backward Sitting Cheer"))
				MotorbikeBackwardSittingCheer();
			if(GUI.Button(new Rect(0, 75, 150, 20), "Backward Stand"))
				MotorbikeBackwardStand();
			if(GUI.Button(new Rect(0, 100, 150, 20), "Handlebar Sit"))
				MotorbikeHandlebarSit();
			if(GUI.Button(new Rect(0, 125, 150, 20), "Hand stand"))
				MotorbikeHandstand();
			if(GUI.Button(new Rect(0, 150, 150, 20), "Head stand"))
				MotorbikeHeadstand();
			if(GUI.Button(new Rect(0, 175, 150, 20), "Heart Attack"))
				MotorbikeHeartAttack();
			if(GUI.Button(new Rect(0, 200, 150, 20), "Look Back"))
				MotorbikeLookBack();
			if(GUI.Button(new Rect(0, 225, 150, 20), "Seat Stand"))
				MotorbikeSeatStand();
			if(GUI.Button(new Rect(0, 250, 150, 20), "Seat Stand Wheely"))
				MotorbikeSeatStandWheely();
			if(GUI.Button(new Rect(0, 275, 150, 20), "Wheely"))
				MotorbikeWheely();
			if(GUI.Button(new Rect(0, 300, 150, 20), "Wheely No Hands"))
				MotorbikeWheelyNoHands();
			if(GUI.Button(new Rect(0, 325, 150, 20), "Lasso"))
				MotorbikeLasso();
			if(GUI.Button(new Rect(0, 350, 150, 20), "Lasso Forward"))
				MotorbikeLassoFwd();
			if(GUI.Button(new Rect(0, 375, 150, 20), "Lasso Back"))
				MotorbikeLassoBack();
			if(GUI.Button(new Rect(0, 400, 150, 20), "Lasso Left"))
				MotorbikeLassoLeft();
			if(GUI.Button(new Rect(0, 425, 150, 20), "Lasso Right"))
				MotorbikeLassoRight();
			if(GUI.Button(new Rect(0, 450, 150, 20), "Shoot Back"))
				MotorbikeShootBack();
			if(GUI.Button(new Rect(0, 475, 150, 20), "Shoot Forward"))
				MotorbikeShootFwd();
			if(GUI.Button(new Rect(0, 500, 150, 20), "Shoot Left"))
				MotorbikeShootLeft();
			if(GUI.Button(new Rect(0, 525, 150, 20), "Shoot Right"))
				MotorbikeShootRight();
			if(GUI.Button(new Rect(0, 550, 150, 20), "Turn Left"))
				MotorbikeTurnLeft();
			if(GUI.Button(new Rect(0, 575, 150, 20), "Turn Right"))
				MotorbikeTurnRight();
			if(GUI.Button(new Rect(0, 600, 150, 20), "Special Flip"))
				MotorbikeSpecialFlip();
			if(GUI.Button(new Rect(0, 625, 150, 20), "Superman"))
				MotorbikeSuperman();
				
				
				
    		GUI.EndScrollView();
		}else if(rollerBlade){
			scrollPosition = GUI.BeginScrollView(new Rect(150, 10, 180, 285), scrollPosition, new Rect(0, 0, 150, 275));
			if(GUI.Button(new Rect(0, 0, 150, 20), "Roller Blade Roll"))
				RollerBladeRoll();
			if(GUI.Button(new Rect(0, 25, 150, 20), "Roller Blade Stop"))
				RollerBladeStop();
			if(GUI.Button(new Rect(0, 50, 150, 20), "Roller Blade Jump"))
				RollerBladeJump();
			if(GUI.Button(new Rect(0, 75, 150, 20), "Roller Crossover Right"))
				RollerBladeCrossoverRight();
			if(GUI.Button(new Rect(0, 100, 150, 20), "Roller Crossover Left"))
				RollerBladeCrossoverLeft();
			if(GUI.Button(new Rect(0, 125, 150, 20), "Roller Blade Back Flip"))
				RollerBladeBackFlip();
			if(GUI.Button(new Rect(0, 150, 150, 20), "Roller Blade Front Flip"))
				RollerBladeFrontFlip();
			if(GUI.Button(new Rect(0, 175, 150, 20), "Roller Blade Skate Fwd"))
				RollerBladeSkateFwd();
			if(GUI.Button(new Rect(0, 200, 150, 20), "Roller Blade Turn Left"))
				RollerBladeTurnLeft();
			if(GUI.Button(new Rect(0, 225, 150, 20), "Roller Blade Turn Right"))
				RollerBladeTurnRight();
			if(GUI.Button(new Rect(0, 250, 150, 20), "Roller Blade Grind Royale"))
				RollerBladeGrindRoyale();

			GUI.EndScrollView();
		}else if(skateboard){
			scrollPosition = GUI.BeginScrollView(new Rect(150, 10, 180, 285), scrollPosition, new Rect(0, 0, 150, 50));
			if(GUI.Button(new Rect(0, 0, 150, 20), "Skateboard Idle"))
				SkateboardIdle();
			if(GUI.Button(new Rect(0, 25, 150, 20), "SkateboardKickPush"))
				SkateboardKickPush();

			GUI.EndScrollView();

		}else if(climbing){
			scrollPosition = GUI.BeginScrollView(new Rect(150, 10, 180, 285), scrollPosition, new Rect(0, 0, 150, 100));
			if(GUI.Button(new Rect(0, 0, 150, 20), "Climbing Idle"))
				ClimbIdle();
			if(GUI.Button(new Rect(0, 25, 150, 20), "Climb Up"))
				ClimbUp();
			if(GUI.Button(new Rect(0, 50, 150, 20), "Climb Left"))
				ClimbLeft();
			if(GUI.Button(new Rect(0, 75, 150, 20), "Climb Right"))
				ClimbRight();
			
			GUI.EndScrollView();
			
		}else if(office){
			scrollPosition = GUI.BeginScrollView(new Rect(150, 10, 180, 285), scrollPosition, new Rect(0, 0, 150, 275));
			if(GUI.Button(new Rect(0, 0, 150, 20), "Sitting Idle"))
				OfficeSitting();
			if(GUI.Button(new Rect(0, 25, 150, 20), "Sitting Leg Cross"))
				OfficeSittingLegCross();
			if(GUI.Button(new Rect(0, 50, 150, 20), "Sitting 45 Degress leg"))
				OfficeSitting45DegLeg();
			if(GUI.Button(new Rect(0, 75, 150, 20), "Sitting 1 Leg Straight"))
				OfficeSitting1LegStraight();
			if(GUI.Button(new Rect(0, 100, 150, 20), "Sitting Eyes Rub"))
				OfficeSittingEyesRub();
			if(GUI.Button(new Rect(0, 125, 150, 20), "Sitting Hand Rest Finger Tap"))
				OfficeSittingHandRestFingerTap();
			if(GUI.Button(new Rect(0, 150, 150, 20), "Sitting Mouse Movement"))
				OfficeSittingMouseMovement();
			if(GUI.Button(new Rect(0, 175, 150, 20), "Sitting Reading"))
				OfficeSittingReading();
			if(GUI.Button(new Rect(0, 200, 150, 20), "Sitting Reading Lean Back"))
				OfficeSittingReadingLeanBack();
			if(GUI.Button(new Rect(0, 225, 150, 20), "Sitting Reading Page Flip"))
				OfficeSittingReadingPageFlip();
			if(GUI.Button(new Rect(0, 250, 150, 20), "Sitting Reading Coffee Sip"))
				OfficeSittingReadingCoffeeSip();
			
			GUI.EndScrollView();
		}else if(swim){
			scrollPosition = GUI.BeginScrollView(new Rect(150, 10, 180, 285), scrollPosition, new Rect(0, 0, 150, 275));
			if(GUI.Button(new Rect(0, 0, 150, 20), "Swim Idle"))
				Swim();
			if(GUI.Button(new Rect(0, 25, 150, 20), "Swim Freestyle"))
				SwimFreestyle();
			if(GUI.Button(new Rect(0, 50, 150, 20), "Swim Dog Paddle"))
				SwimDogPaddle();


			GUI.EndScrollView();
		}else if(wand){
			scrollPosition = GUI.BeginScrollView(new Rect(150, 10, 180, 285), scrollPosition, new Rect(0, 0, 150, 275));
			if(GUI.Button(new Rect(0, 0, 150, 20), "Wand Stand"))
				WandStand();
			if(GUI.Button(new Rect(0, 25, 150, 20), "Wand Attack"))
				WandAttack();
			if(GUI.Button(new Rect(0, 50, 150, 20), "Wand Attack 2"))
				WandAttack2();
			if(GUI.Button(new Rect(0, 75, 150, 20), "Staff Stand"))
				StaffStand();
			if(GUI.Button(new Rect(0, 100, 150, 20), "Staff Attack"))
				StaffAttack();
			if(GUI.Button(new Rect(0, 125, 150, 20), "Staff Heal"))
				StaffHeal();
			if(GUI.Button(new Rect(0, 150, 150, 20), "Staff Power Up"))
				StaffPowerUp();

			GUI.EndScrollView();
		}else if(cards){
			scrollPosition = GUI.BeginScrollView(new Rect(150, 10, 180, 285), scrollPosition, new Rect(0, 0, 150, 125));
			if(GUI.Button(new Rect(0, 0, 150, 20), "Dealer Idle"))
				DealerIdle();
			if(GUI.Button(new Rect(0, 25, 150, 20), "Dealer Shuffle"))
				DealerShuffle();
			if(GUI.Button(new Rect(0, 50, 150, 20), "Dealer Fan"))
				DealerFan();
			if(GUI.Button(new Rect(0, 75, 150, 20), "Card Player Idle"))
				CardPlayerIdle();
			if(GUI.Button(new Rect(0, 100, 150, 20), "Card Player Look"))
				CardPlayerLook();

			
			GUI.EndScrollView();
		}

	}

	void StormStrike(){
		IdleStand();
		animator.SetTrigger("StormStrike");
		animator2.SetTrigger("StormStrike");
		animator3.SetTrigger("StormStrike");
	}

	void Mutilate(){
		IdleStand();
		animator.SetTrigger("Mutilate");
		animator2.SetTrigger("Mutilate");
		animator3.SetTrigger("Mutilate");
	}

	void FireBreath(){
		IdleStand();
		animator.SetTrigger("FireBreath");
		animator2.SetTrigger("FireBreath");
		animator3.SetTrigger("FireBreath");
	}

	void ChannelCastOmni(){
		IdleStand();
		animator.SetBool("ChannelCastOmni", true);
		animator2.SetBool("ChannelCastOmni", true);
		animator3.SetBool("ChannelCastOmni", true);
	}

	void ChannelCastDirected(){
		IdleStand();
		animator.SetBool("ChannelCastDirected", true);
		animator2.SetBool("ChannelCastDirected", true);
		animator3.SetBool("ChannelCastDirected", true);
	}

	void BattleRoar(){
		IdleStand();
		animator.SetTrigger("BattleRoar");
		animator2.SetTrigger("BattleRoar");
		animator3.SetTrigger("BattleRoar");
	}

	void BowFire2(){
		BowInstant2();
		animator.SetTrigger("BowFire2");
		animator2.SetTrigger("BowFire2");
		animator3.SetTrigger("BowFire2");
	}

	void BowInstant2(){
		BowReady2();
		animator.SetBool("BowInstant2", true);
		animator2.SetBool("BowInstant2", true);
		animator3.SetBool("BowInstant2", true);
	}

	void BowReady2(){
		BowIdle();
		animator.SetBool("BowReady2", true);
		animator2.SetBool("BowReady2", true);
		animator3.SetBool("BowReady2", true);
	}

	void IdleSandCover(){
		IdleStand();
		animator.SetTrigger("IdleSandCover");
		animator2.SetTrigger("IdleSandCover");
		animator3.SetTrigger("IdleSandCover");
	}

	void WalkDehydrated(){
		IdleStand();
		animator.SetBool("WalkDehydrated", true);
		animator2.SetBool("WalkDehydrated", true);
		animator3.SetBool("WalkDehydrated", true);
	}

	void UpHillWalkHandGrab(){
		UpHillWalk();
		animator.SetTrigger("UpHillWalkHandGrab");
		animator2.SetTrigger("UpHillWalkHandGrab");
		animator3.SetTrigger("UpHillWalkHandGrab");
	}

	void UpHillWalk(){
		IdleStand();
		animator.SetBool("UpHillWalk", true);
		animator2.SetBool("UpHillWalk", true);
		animator3.SetBool("UpHillWalk", true);
	}

	void CardPlayerLook(){
		CardPlayerIdle();
		animator.SetTrigger("CardPlayerLook");
		animator2.SetTrigger("CardPlayerLook");
		animator3.SetTrigger("CardPlayerLook");
	}

	void CardPlayerIdle(){
		DealerIdle();
		animator.SetBool("CardPlayerIdle", true);
		animator2.SetBool("CardPlayerIdle", true);
		animator3.SetBool("CardPlayerIdle", true);
	}

	void DealerFan(){
		DealerIdle();
		animator.SetTrigger("DealerFan");
		animator2.SetTrigger("DealerFan");
		animator3.SetTrigger("DealerFan");
	}

	void DealerShuffle(){
		DealerIdle();
		animator.SetTrigger("DealerShuffle");
		animator2.SetTrigger("DealerShuffle");
		animator3.SetTrigger("DealerShuffle");
	}

	void DealerIdle(){
		IdleStand();
		cards = true;
		idleStandbool = false;
		animator.SetBool("DealerIdle", true);
		animator2.SetBool("DealerIdle", true);
		animator3.SetBool("DealerIdle", true);

	}

	void StaffPowerUp(){
		StaffStand();
		animator.SetTrigger("StaffPowerUp");
		animator2.SetTrigger("StaffPowerUp");
		animator3.SetTrigger("StaffPowerUp");
	}

	void StaffHeal(){
		StaffStand();
		animator.SetTrigger("StaffHeal");
		animator2.SetTrigger("StaffHeal");
		animator3.SetTrigger("StaffHeal");
	}

	void StaffAttack(){
		StaffStand();
		animator.SetTrigger("StaffAttack");
		animator2.SetTrigger("StaffAttack");
		animator3.SetTrigger("StaffAttack");
	}

	void StaffStand(){
		IdleStand();
		wand = true;
		idleStandbool = false;
		animator.SetBool("StaffStand", true);
		animator2.SetBool("StaffStand", true);
		animator3.SetBool("StaffStand", true);
	}

	void WandAttack(){
		WandStand();
		animator.SetTrigger("WandAttack");
		animator2.SetTrigger("WandAttack");
		animator3.SetTrigger("WandAttack");
	}

	void WandAttack2(){
		WandStand();
		animator.SetTrigger("WandAttack2");
		animator2.SetTrigger("WandAttack2");
		animator3.SetTrigger("WandAttack2");
	}

	void WandStand(){
		IdleStand();
		wand = true;
		idleStandbool = false;
		animator.SetBool("WandStand", true);
		animator2.SetBool("WandStand", true);
		animator3.SetBool("WandStand", true);
	}

	void SwimDogPaddle(){
		Swim();
		animator.SetBool("SwimDogPaddle", true);
		animator2.SetBool("SwimDogPaddle", true);
		animator3.SetBool("SwimDogPaddle", true);
	}


	void SwimFreestyle(){
		Swim();
		animator.SetBool("SwimFreestyle", true);
		animator2.SetBool("SwimFreestyle", true);
		animator3.SetBool("SwimFreestyle", true);
	}

	void Swim(){
		IdleStand();
		swim = true;
		idleStandbool = false;
		animator.SetBool("Swim", true);
		animator2.SetBool("Swim", true);
		animator3.SetBool("Swim", true);
	}

	void WateringCanWatering(){
		WateringCan();
		animator.SetBool("WateringCanWatering", true);
		animator2.SetBool("WateringCanWatering", true);
		animator3.SetBool("WateringCanWatering", true);
	}

	void WateringCan(){
		IdleStand();
		animator.SetBool("WateringCan", true);
		animator2.SetBool("WateringCan", true);
		animator3.SetBool("WateringCan", true);
	}

	void OfficeSittingReadingPageFlip(){
		OfficeSittingReading();
		animator.SetBool("OfficeSittingReadingPageFlip", true);
		animator2.SetBool("OfficeSittingReadingPageFlip", true);
		animator3.SetBool("OfficeSittingReadingPageFlip", true);
	}

	void OfficeSittingReadingLeanBack(){
		OfficeSittingReading();
		animator.SetBool("OfficeSittingReadingLeanBack", true);
		animator2.SetBool("OfficeSittingReadingLeanBack", true);
		animator3.SetBool("OfficeSittingReadingLeanBack", true);
	}

	void OfficeSittingReading(){
		OfficeSitting();
		animator.SetBool("OfficeSittingReading", true);
		animator2.SetBool("OfficeSittingReading", true);
		animator3.SetBool("OfficeSittingReading", true);
	}

	void OfficeSittingReadingCoffeeSip(){
		OfficeSittingReading();
		animator.SetBool("OfficeSittingReadingCoffeeSip", true);
		animator2.SetBool("OfficeSittingReadingCoffeeSip", true);
		animator3.SetBool("OfficeSittingReadingCoffeeSip", true);
	}

	void OfficeSittingMouseMovement(){
		OfficeSitting();
		animator.SetBool("OfficeSittingMouseMovement", true);
		animator2.SetBool("OfficeSittingMouseMovement", true);
		animator3.SetBool("OfficeSittingMouseMovement", true);
	}

	void OfficeSittingHandRestFingerTap(){
		OfficeSitting();
		animator.SetBool("OfficeSittingHandRestFingerTap", true);
		animator2.SetBool("OfficeSittingHandRestFingerTap", true);
		animator3.SetBool("OfficeSittingHandRestFingerTap", true);
	}

	void OfficeSittingEyesRub(){
		OfficeSitting();
		animator.SetBool("OfficeSittingEyesRub", true);
		animator2.SetBool("OfficeSittingEyesRub", true);
		animator3.SetBool("OfficeSittingEyesRub", true);
	}

	void OfficeSitting1LegStraight(){
		OfficeSitting();
		animator.SetBool("OfficeSitting1LegStraight", true);
		animator2.SetBool("OfficeSitting1LegStraight", true);
		animator3.SetBool("OfficeSitting1LegStraight", true);
	}

	void OfficeSitting45DegLeg(){
		OfficeSitting();
		animator.SetBool("OfficeSitting45DegLeg", true);
		animator2.SetBool("OfficeSitting45DegLeg", true);
		animator3.SetBool("OfficeSitting45DegLeg", true);
	}

	void OfficeSittingBack(){
		OfficeSitting();
		animator.SetBool("OfficeSittingBack", true);
		animator2.SetBool("OfficeSittingBack", true);
		animator3.SetBool("OfficeSittingBack", true);
	}

	void OfficeSittingLegCross(){
		OfficeSitting();
		animator.SetBool("OfficeSittingLegCross", true);
		animator2.SetBool("OfficeSittingLegCross", true);
		animator3.SetBool("OfficeSittingLegCross", true);
	}

	void OfficeSitting(){
			IdleStand();
			office = true;
			idleStandbool = false;
			animator.SetBool("OfficeSitting", true);
			animator2.SetBool("OfficeSitting", true);
			animator3.SetBool("OfficeSitting", true);
		}

	void Yawn(){
		IdleStand();
		animator.SetBool("Yawn", true);
		animator2.SetBool("Yawn", true);
		animator3.SetBool("Yawn", true);
	}

	void HeelClick(){
		IdleStand();
		animator.SetBool("HeelClick", true);
		animator2.SetBool("HeelClick", true);
		animator3.SetBool("HeelClick", true);
	}

	void VaderChoke(){
		IdleStand();
		animator.SetBool("VaderChoke", true);
		animator2.SetBool("VaderChoke", true);
		animator3.SetBool("VaderChoke", true);
	}

	void SpinDeath(){
		IdleFight();
		animator.SetBool("360SpinDeath", true);
		animator2.SetBool("360SpinDeath", true);
		animator3.SetBool("360SpinDeath", true);
	}

	void ClimbRight(){
		ClimbIdle();
		animator.SetBool("ClimbRight", true);
		animator2.SetBool("ClimbRight", true);
		animator3.SetBool("ClimbRight", true);
	}

	void ClimbLeft(){
		ClimbIdle();
		animator.SetBool("ClimbLeft", true);
		animator2.SetBool("ClimbLeft", true);
		animator3.SetBool("ClimbLeft", true);
	}

	void ClimbUp(){
		ClimbIdle();
		animator.SetBool("ClimbUp", true);
		animator2.SetBool("ClimbUp", true);
		animator3.SetBool("ClimbUp", true);
	}

	void ClimbIdle(){
		IdleStand ();
		climbing = true;
		idleStandbool = false;
		animator.SetBool("ClimbIdle", true);
		animator2.SetBool("ClimbIdle", true);
		animator3.SetBool("ClimbIdle", true);
	}

	void SkateboardKickPush(){
		SkateboardIdle();
		animator.SetBool("SkateboardKickPush", true);
		animator2.SetBool("SkateboardKickPush", true);
		animator3.SetBool("SkateboardKickPush", true);
	}

	void SkateboardIdle(){
		IdleStand();
		skateboard = true;
		idleStandbool = false;
		animator.SetBool("SkateboardIdle", true);
		animator2.SetBool("SkateboardIdle", true);
		animator3.SetBool("SkateboardIdle", true);
	}
	void IdleStandingJump(){
		IdleStand();
		animator.SetBool("IdleStandingJump", true);
		animator2.SetBool("IdleStandingJump", true);
		animator3.SetBool("IdleStandingJump", true);
	}

	void IdleFeedThrow(){
		IdleStand();
		animator.SetBool("IdleFeedThrow", true);
		animator2.SetBool("IdleFeedThrow", true);
		animator3.SetBool("IdleFeedThrow", true);
	}

	void IdleSlide(){
		IdleRun();
		animator.SetBool("IdleSlide", true);
		animator2.SetBool("IdleSlide", true);
		animator3.SetBool("IdleSlide", true);
	}

	void RollerBladeTurnRight(){
		RollerBladeRoll();
		animator.SetBool("RollerBladeTurnRight", true);
		animator2.SetBool("RollerBladeTurnRight", true);
		animator3.SetBool("RollerBladeTurnRight", true);
	}

	void RollerBladeTurnLeft(){
		RollerBladeRoll();
		animator.SetBool("RollerBladeTurnLeft", true);
		animator2.SetBool("RollerBladeTurnLeft", true);
		animator3.SetBool("RollerBladeTurnLeft", true);
	}

	void RollerBladeCrossoverLeft(){
		RollerBladeRoll();
		animator.SetBool("RollerBladeCrossoverLeft", true);
		animator2.SetBool("RollerBladeCrossoverLeft", true);
		animator3.SetBool("RollerBladeCrossoverLeft", true);
	}

	void RollerBladeSkateFwd(){
		RollerBladeRoll();
		animator.SetBool("RollerBladeSkateFwd", true);
		animator2.SetBool("RollerBladeSkateFwd", true);
		animator3.SetBool("RollerBladeSkateFwd", true);
	}

	void RollerBladeFrontFlip(){
		RollerBladeRoll();
		animator.SetBool("RollerBladeFrontFlip", true);
		animator2.SetBool("RollerBladeFrontFlip", true);
		animator3.SetBool("RollerBladeFrontFlip", true);
	}

	void RollerBladeBackFlip(){
		RollerBladeRoll();
		animator.SetBool("RollerBladeBackFlip", true);
		animator2.SetBool("RollerBladeBackFlip", true);
		animator3.SetBool("RollerBladeBackFlip", true);
	}

	void RollerBladeCrossoverRight(){
		RollerBladeRoll();
		animator.SetBool("RollerBladeCrossoverRight", true);
		animator2.SetBool("RollerBladeCrossoverRight", true);
		animator3.SetBool("RollerBladeCrossoverRight", true);
	}

	void RollerBladeGrindRoyale(){
		RollerBladeJump();
		animator.SetBool("RollerBladeGrindRoyale", true);
		animator2.SetBool("RollerBladeGrindRoyale", true);
		animator3.SetBool("RollerBladeGrindRoyale", true);
	}

	void RollerBladeJump(){
		RollerBladeRoll();
		animator.SetBool("RollerBladeJump", true);
		animator2.SetBool("RollerBladeJump", true);
		animator3.SetBool("RollerBladeJump", true);
	}

	void RollerBladeStop(){
		RollerBladeStand();
		animator.SetBool("RollerBladeStop", true);
		animator2.SetBool("RollerBladeStop", true);
		animator3.SetBool("RollerBladeStop", true);
	}

	void RollerBladeRoll(){
		RollerBladeStand();
		animator.SetBool("RollerBladeRoll", true);
		animator2.SetBool("RollerBladeRoll", true);
		animator3.SetBool("RollerBladeRoll", true);
	}

	void RollerBladeStand(){
		Falses();
		IdleStand();
		idleStandbool = false;
		rollerBlade = true;
		animator.SetBool("RollerBladeStand", true);
		animator2.SetBool("RollerBladeStand", true);
		animator3.SetBool("RollerBladeStand", true);
	}
	
	void MotorbikeSuperman(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeSuperman", true);
		animator2.SetBool("MotorbikeSuperman", true);
		animator3.SetBool("MotorbikeSuperman", true);
	}
	
	void MotorbikeSpecialFlip(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeSpecialFlip", true);
		animator2.SetBool("MotorbikeSpecialFlip", true);
		animator3.SetBool("MotorbikeSpecialFlip", true);
	}
	
	void PistolLeftHandStab(){
		PistolReady();
		animator.SetBool("PistolLeftHandStab", true);
		animator2.SetBool("PistolLeftHandStab", true);
		animator3.SetBool("PistolLeftHandStab", true);
	}
	
	void CrouchStrafeRight(){
		IdleReadyCrouch();
		animator.SetBool("CrouchStrafeRight", true);
		animator2.SetBool("CrouchStrafeRight", true);
		animator3.SetBool("CrouchStrafeRight", true);
	}
	
	void CrouchStrafeLeft(){
		IdleReadyCrouch();
		animator.SetBool("CrouchStrafeLeft", true);
		animator2.SetBool("CrouchStrafeLeft", true);
		animator3.SetBool("CrouchStrafeLeft", true);
	}
	
	void CrouchWalkBackward(){
		IdleReadyCrouch();
		animator.SetBool("CrouchWalkBackward", true);
		animator2.SetBool("CrouchWalkBackward", true);
		animator3.SetBool("CrouchWalkBackward", true);
	}
	
	void ProneLocomotion(){
		ProneIdle();
		animator.SetBool("ProneLocomotion", true);
		animator2.SetBool("ProneLocomotion", true);
		animator3.SetBool("ProneLocomotion", true);
	}
	
	void ProneIdle(){
		IdleStand();
		animator.SetBool("ProneIdle", true);
		animator2.SetBool("ProneIdle", true);
		animator3.SetBool("ProneIdle", true);
	}
	
	void CrawlLocomotion(){
		CrawlIdle();
		animator.SetBool("CrawlLocomotion", true);
		animator2.SetBool("CrawlLocomotion", true);
		animator3.SetBool("CrawlLocomotion", true);
	}
	
	void CrawlIdle(){
		IdleStand();
		animator.SetBool("CrawlIdle", true);
		animator2.SetBool("CrawlIdle", true);
		animator3.SetBool("CrawlIdle", true);
	}
	
	void IdleMouthWipe(){
		IdleStand();
		animator.SetBool("IdleMouthWipe", true);
		animator2.SetBool("IdleMouthWipe", true);
		animator3.SetBool("IdleMouthWipe", true);
	}
	
	void IdleSpew(){
		IdleStand();
		animator.SetBool("IdleSpew", true);
		animator2.SetBool("IdleSpew", true);
		animator3.SetBool("IdleSpew", true);
	}
	
	void RunBackRight(){
		RunBackward();
		animator.SetBool("RunBackRight", true);
		animator2.SetBool("RunBackRight", true);
		animator3.SetBool("RunBackRight", true);
	}
	
	void RunBackLeft(){
		RunBackward();
		animator.SetBool("RunBackLeft", true);
		animator2.SetBool("RunBackLeft", true);
		animator3.SetBool("RunBackLeft", true);
	}
	
	void WorkerShovel2(){
		IdleStand();
		animator.SetBool("WorkerShovel2", true);
		animator2.SetBool("WorkerShovel2", true);
		animator3.SetBool("WorkerShovel2", true);
	}
	
	void WorkerShovel(){
		IdleStand();
		animator.SetBool("WorkerShovel", true);
		animator2.SetBool("WorkerShovel", true);
		animator3.SetBool("WorkerShovel", true);
	}
	
	void WorkerPickaxe2(){
		IdleStand();
		animator.SetBool("WorkerPickaxe2", true);
		animator2.SetBool("WorkerPickaxe2", true);
		animator3.SetBool("WorkerPickaxe2", true);
	}
	
	void WorkerPickaxe(){
		IdleStand();
		animator.SetBool("WorkerPickaxe", true);
		animator2.SetBool("WorkerPickaxe", true);
		animator3.SetBool("WorkerPickaxe", true);
	}
	
	void WorkerHammer2(){
		IdleStand();
		animator.SetBool("WorkerHammer2", true);
		animator2.SetBool("WorkerHammer2", true);
		animator3.SetBool("WorkerHammer2", true);
	}
	
	void WorkerHammer(){
		IdleStand();
		animator.SetBool("WorkerHammer", true);
		animator2.SetBool("WorkerHammer", true);
		animator3.SetBool("WorkerHammer", true);
	}
	
	void WoodCut(){
		IdleStand();
		animator.SetBool("WoodCut", true);
		animator2.SetBool("WoodCut", true);
		animator3.SetBool("WoodCut", true);
	}
	
	void OneHandSwordRollAttack(){
		OneHandSwordReady();
		animator.SetBool("1HandSwordRollAttack", true);
		animator2.SetBool("1HandSwordRollAttack", true);
		animator3.SetBool("1HandSwordRollAttack", true);
	}
	
	void MotorbikeTurnRight(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeTurnRight", true);
		animator2.SetBool("MotorbikeTurnRight", true);
		animator3.SetBool("MotorbikeTurnRight", true);
	}
	
	void MotorbikeTurnLeft(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeTurnLeft", true);
		animator2.SetBool("MotorbikeTurnLeft", true);
		animator3.SetBool("MotorbikeTurnLeft", true);
	}
	
	void MotorbikeShootRight(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeShootRight", true);
		animator2.SetBool("MotorbikeShootRight", true);
		animator3.SetBool("MotorbikeShootRight", true);
	}
	
	void MotorbikeShootLeft(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeShootLeft", true);
		animator2.SetBool("MotorbikeShootLeft", true);
		animator3.SetBool("MotorbikeShootLeft", true);
	}
	
	void MotorbikeShootFwd(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeShootFwd", true);
		animator2.SetBool("MotorbikeShootFwd", true);
		animator3.SetBool("MotorbikeShootFwd", true);
	}
	
	void MotorbikeShootBack(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeShootBack", true);
		animator2.SetBool("MotorbikeShootBack", true);
		animator3.SetBool("MotorbikeShootBack", true);
	}
	
	void MotorbikeLassoRight(){
		MotorbikeLasso();
		animator.SetBool("MotorbikeLassoRight", true);
		animator2.SetBool("MotorbikeLassoRight", true);
		animator3.SetBool("MotorbikeLassoRight", true);
	}
	
	void MotorbikeLassoLeft(){
		MotorbikeLasso();
		animator.SetBool("MotorbikeLassoLeft", true);
		animator2.SetBool("MotorbikeLassoLeft", true);
		animator3.SetBool("MotorbikeLassoLeft", true);
	}
	
	void MotorbikeLassoBack(){
		MotorbikeLasso();
		animator.SetBool("MotorbikeLassoBack", true);
		animator2.SetBool("MotorbikeLassoBack", true);
		animator3.SetBool("MotorbikeLassoBack", true);
	}
	
	void MotorbikeLassoFwd(){
		MotorbikeLasso();
		animator.SetBool("MotorbikeLassoFwd", true);
		animator2.SetBool("MotorbikeLassoFwd", true);
		animator3.SetBool("MotorbikeLassoFwd", true);
	}
	
	void MotorbikeLasso(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeLasso", true);
		animator2.SetBool("MotorbikeLasso", true);
		animator3.SetBool("MotorbikeLasso", true);
	}
	
	void MotorbikeWheelyNoHands(){
		MotorbikeWheely();
		animator.SetBool("MotorbikeWheelyNoHands", true);
		animator2.SetBool("MotorbikeWheelyNoHands", true);
		animator3.SetBool("MotorbikeWheelyNoHands", true);
	}
	
	void MotorbikeWheely(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeWheely", true);
		animator2.SetBool("MotorbikeWheely", true);
		animator3.SetBool("MotorbikeWheely", true);
	}
	
	void MotorbikeSeatStandWheely(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeSeatStandWheely", true);
		animator2.SetBool("MotorbikeSeatStandWheely", true);
		animator3.SetBool("MotorbikeSeatStandWheely", true);
	}
	
	void MotorbikeSeatStand(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeSeatStand", true);
		animator2.SetBool("MotorbikeSeatStand", true);
		animator3.SetBool("MotorbikeSeatStand", true);
	}
	
	void MotorbikeLookBack(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeLookBack", true);
		animator2.SetBool("MotorbikeLookBack", true);
		animator3.SetBool("MotorbikeLookBack", true);
	}
	
	void MotorbikeHeartAttack(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeHeartAttack", true);
		animator2.SetBool("MotorbikeHeartAttack", true);
		animator3.SetBool("MotorbikeHeartAttack", true);
	}
	
	void MotorbikeHeadstand(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeHeadstand", true);
		animator2.SetBool("MotorbikeHeadstand", true);
		animator3.SetBool("MotorbikeHeadstand", true);
	}
	
	void MotorbikeHandstand(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeHandstand", true);
		animator2.SetBool("MotorbikeHandstand", true);
		animator3.SetBool("MotorbikeHandstand", true);
	}
	
	void MotorbikeHandlebarSit(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeHandlebarSit", true);
		animator2.SetBool("MotorbikeHandlebarSit", true);
		animator3.SetBool("MotorbikeHandlebarSit", true);
		
	}
	
	void MotorbikeBackwardStand(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeBackwardStand", true);
		animator2.SetBool("MotorbikeBackwardStand", true);
		animator3.SetBool("MotorbikeBackwardStand", true);
	}
	
	void MotorbikeBackwardSittingCheer(){
		MotorbikeBackwardSitting();
		animator.SetBool("MotorbikeBackwardSittingCheer", true);
		animator2.SetBool("MotorbikeBackwardSittingCheer", true);
		animator3.SetBool("MotorbikeBackwardSittingCheer", true);
	}
	
	void MotorbikeBackwardSitting(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeBackwardSitting", true);
		animator2.SetBool("MotorbikeBackwardSitting", true);
		animator3.SetBool("MotorbikeBackwardSitting", true);
	}
	
	void MotorbikeAirWalk(){
		MotorbikeIdle();
		animator.SetBool("MotorbikeAirWalk", true);
		animator2.SetBool("MotorbikeAirWalk", true);
		animator3.SetBool("MotorbikeAirWalk", true);
	}
	
	void MotorbikeIdle(){
		Falses();
		IdleStand();
		idleStandbool = false;
		motorbikeIdle = true;
		animator.SetBool("MotorbikeIdle", true);
		animator2.SetBool("MotorbikeIdle", true);
		animator3.SetBool("MotorbikeIdle", true);
	}
	
	void WeaponReadyFire(){
		WeaponReady();
		animator.SetBool("WeaponReadyFire", true);
		animator2.SetBool("WeaponReadyFire", true);
		animator3.SetBool("WeaponReadyFire", true);
	}
	
	void ShotgunReadyFire(){
		WeaponReady();
		animator.SetBool("ShotgunReadyFire", true);
		animator2.SetBool("ShotgunReadyFire", true);
		animator3.SetBool("ShotgunReadyFire", true);
	}
	
	void IdleStun(){
		IdleStand();
		animator.SetBool("IdleStun", true);
		animator2.SetBool("IdleStun", true);
		animator3.SetBool("IdleStun", true);
	}
	
	void OneHandSwordChargeSwipe(){
		OneHandSwordChargeUp();
		animator.SetBool("1HandSwordChargeSwipe", true);
		animator2.SetBool("1HandSwordChargeSwipe", true);
		animator3.SetBool("1HandSwordChargeSwipe", true);
	}
	
	void OneHandSwordChargeHeavyBash(){
		OneHandSwordChargeUp();
		animator.SetBool("1HandSwordChargeHeavyBash", true);
		animator2.SetBool("1HandSwordChargeHeavyBash", true);
		animator3.SetBool("1HandSwordChargeHeavyBash", true);
	}
	
	void OneHandSwordChargeUp(){
		OneHandSwordReady();
		animator.SetBool("1HandSwordChargeUp", true);
		animator2.SetBool("1HandSwordChargeUp", true);
		animator3.SetBool("1HandSwordChargeUp", true);
	}
	
	void WeaponRunBackward(){
		WeaponReady();
		animator.SetBool("WeaponRunBackward", true);
		animator2.SetBool("WeaponRunBackward", true);
		animator3.SetBool("WeaponRunBackward", true);
	}
	
	void RunBackward(){
		IdleStand();
		animator.SetBool("RunBackward", true);
		animator2.SetBool("RunBackward", true);
		animator3.SetBool("RunBackward", true);
	}
	
	void OneHandSwordShieldBash(){
		OneHandSwordReady();
		animator.SetBool("1HandSwordShieldBash", true);
		animator2.SetBool("1HandSwordShieldBash", true);
		animator3.SetBool("1HandSwordShieldBash", true);
	}
	
	void OneHandSwordStrafeLeft(){
		OneHandSwordReady();
		animator.SetBool("1HSwordStrafeRunLeft", true);
		animator2.SetBool("1HSwordStrafeRunLeft", true);
		animator3.SetBool("1HSwordStrafeRunLeft", true);
	}
	
	void OneHandSwordStrafeRight(){
		OneHandSwordReady();
		animator.SetBool("1HSwordStrafeRunRight", true);
		animator2.SetBool("1HSwordStrafeRunRight", true);
		animator3.SetBool("1HSwordStrafeRunRight", true);
	}
	
	void WeaponStrafeRunLeft(){
		WeaponReady();
		animator.SetBool("WeaponStrafeRunLeft", true);
		animator2.SetBool("WeaponStrafeRunLeft", true);
		animator3.SetBool("WeaponStrafeRunLeft", true);
	}
	
	void WeaponStrafeRunRight(){
		WeaponReady();
		animator.SetBool("WeaponStrafeRunRight", true);
		animator2.SetBool("WeaponStrafeRunRight", true);
		animator3.SetBool("WeaponStrafeRunRight", true);
	}
	
	void StrafeRunLeft(){
		IdleStand();
		animator.SetBool("StrafeRunLeft", true);
		animator2.SetBool("StrafeRunLeft", true);
		animator3.SetBool("StrafeRunLeft", true);
	}
	
	void StrafeRunRight(){
		IdleStand();
		animator.SetBool("StrafeRunRight", true);
		animator2.SetBool("StrafeRunRight", true);
		animator3.SetBool("StrafeRunRight", true);
	}
	
	void IdleTyping(){
		IdleStand();
		animator.SetBool("IdleTyping", true);
		animator2.SetBool("IdleTyping", true);
		animator3.SetBool("IdleTyping", true);
	}
	
	void IdleButtonPress(){
		IdleStand();
		animator.SetBool("IdleButtonPress", true);
		animator2.SetBool("IdleButtonPress", true);
		animator3.SetBool("IdleButtonPress", true);
	}
	
	void Idle180(){
		IdleStand();
		animator.SetBool("Idle180", true);
    	animator2.SetBool("Idle180", true);
    	animator3.SetBool("Idle180", true);
	}
	
	void CrouchWalk(){
		IdleReadyCrouch();
		animator.SetBool("CrouchWalk", true);
    	animator2.SetBool("CrouchWalk", true);
    	animator3.SetBool("CrouchWalk", true);
	}
	
	void Crouch180(){
		IdleReadyCrouch();
		animator.SetBool("Crouch180", true);
    	animator2.SetBool("Crouch180", true);
    	animator3.SetBool("Crouch180", true);
	}
	
	void FlyDown(){
		IdleFly();
		animator.SetBool("FlyDown", true);
    	animator2.SetBool("FlyDown", true);
    	animator3.SetBool("FlyDown", true);
	}
	
	void FlyUp(){
		IdleFly();
		animator.SetBool("FlyUp", true);
    	animator2.SetBool("FlyUp", true);
    	animator3.SetBool("FlyUp", true);
	}
	
	void FlyRight(){
		IdleFly();
		animator.SetBool("FlyRight", true);
    	animator2.SetBool("FlyRight", true);
    	animator3.SetBool("FlyRight", true);
	}
	
	void FlyLeft(){
		IdleFly();
		animator.SetBool("FlyLeft", true);
    	animator2.SetBool("FlyLeft", true);
    	animator3.SetBool("FlyLeft", true);
	}
	
	void FlyBackward(){
		IdleFly();
		animator.SetBool("FlyBackward", true);
    	animator2.SetBool("FlyBackward", true);
    	animator3.SetBool("FlyBackward", true);
	}
	
	void FlyForward(){
		IdleFly();
		animator.SetBool("FlyForward", true);
    	animator2.SetBool("FlyForward", true);
    	animator3.SetBool("FlyForward", true);
	}
	
	void IdleFly(){
		IdleReady();
		animator.SetBool("IdleFly", true);
    	animator2.SetBool("IdleFly", true);
    	animator3.SetBool("IdleFly", true);
	}
	
	void WizardNeoBlock(){
		IdleReady();
		animator.SetBool("WizardNeoBlock", true);
    	animator2.SetBool("WizardNeoBlock", true);
    	animator3.SetBool("WizardNeoBlock", true);
	}
	
	void WizardEyeBeam(){
		IdleReady();
		animator.SetBool("WizardEyeBeam", true);
    	animator2.SetBool("WizardEyeBeam", true);
    	animator3.SetBool("WizardEyeBeam", true);
	}
	
	void IdleMeditate(){
		IdleStand();
		animator.SetBool("IdleMeditate", true);
    	animator2.SetBool("IdleMeditate", true);
    	animator3.SetBool("IdleMeditate", true);
	}
	
	void IdleDodgeLeft(){
		IdleReady();
		animator.SetBool("IdleDodgeLeft", true);
    	animator2.SetBool("IdleDodgeLeft", true);
    	animator3.SetBool("IdleDodgeLeft", true);
	}
	
	void IdleDodgeRight(){
		IdleReady();
		animator.SetBool("IdleDodgeRight", true);
    	animator2.SetBool("IdleDodgeRight", true);
    	animator3.SetBool("IdleDodgeRight", true);
	}
	
	void RunDive(){
		IdleRun();
		animator.SetBool("RunDive", true);
    	animator2.SetBool("RunDive", true);
    	animator3.SetBool("RunDive", true);
	}
	
	void RunJump(){
		IdleRun();
		animator.SetBool("RunJump", true);
    	animator2.SetBool("RunJump", true);
    	animator3.SetBool("RunJump", true);
	}
	
	void Cowboy1HandDraw(){
		IdleStand();
    	animator.SetBool("Cowboy1HandDraw", true);
    	animator2.SetBool("Cowboy1HandDraw", true);
    	animator3.SetBool("Cowboy1HandDraw", true);
	}
	
	void BowReady(){
		BowIdle();
		animator.SetBool("BowReady", true);
    	animator2.SetBool("BowReady", true);
    	animator3.SetBool("BowReady", true);
	}
	
	void BowInstant(){
		BowReady();
		animator.SetBool("BowInstant", true);
    	animator2.SetBool("BowInstant", true);
    	animator3.SetBool("BowInstant", true);
	}
	
	void BowFire(){
		BowInstant();
		animator.SetBool("BowFire", true);
    	animator2.SetBool("BowFire", true);
    	animator3.SetBool("BowFire", true);
	}
	
	
	void BowIdle(){
		Falses();
		bowIdle = true;
		animator.SetBool("BowIdle", true);
    	animator2.SetBool("BowIdle", true);
    	animator3.SetBool("BowIdle", true);
	}
    
    void OneHandSwordRun(){
    	Falses();
    	OneHandSwordReady();
    	animator.SetBool("OneHandSwordRun", true);
    	animator2.SetBool("OneHandSwordRun", true);
    	animator3.SetBool("OneHandSwordRun", true);
    }
    
    void OneHandSwordBlock(){
    	OneHandSwordReady();
    	animator.SetBool("OneHandSwordBlock", true);
    	animator2.SetBool("OneHandSwordBlock", true);
    	animator3.SetBool("OneHandSwordBlock", true);
    }
    
    void OneHandSwordJab(){
    	OneHandSwordReady();
    	animator.SetBool("OneHandSwordJab", true);
    	animator2.SetBool("OneHandSwordJab", true);
    	animator3.SetBool("OneHandSwordJab", true);
    }
    
    void OneHandSwordBackSwing(){
    	OneHandSwordReady();
    	animator.SetBool("OneHandSwordBackSwing", true);
    	animator2.SetBool("OneHandSwordBackSwing", true);
    	animator3.SetBool("OneHandSwordBackSwing", true);
    }
    
    void OneHandSwordSwing(){
    	OneHandSwordReady();
    	animator.SetBool("OneHandSwordSwing", true);
    	animator2.SetBool("OneHandSwordSwing", true);
    	animator3.SetBool("OneHandSwordSwing", true);
    }
    
    void OneHandSwordReady(){
    	OneHandSwordIdle();
    	animator.SetBool("OneHandSwordReady", true);
    	animator2.SetBool("OneHandSwordReady", true);
    	animator3.SetBool("OneHandSwordReady", true);
    }
    
    void OneHandSwordIdle(){
    	Falses();
    	oneHandSwordIdle = true;
    	animator.SetBool("OneHandSwordIdle", true);
    	animator2.SetBool("OneHandSwordIdle", true);
    	animator3.SetBool("OneHandSwordIdle", true);
    }
    
    void WeaponRun(){
    	WeaponReady();
    	
    	weaponRun = true;
    	animator.SetBool("WeaponRun", true);
    	animator2.SetBool("WeaponRun", true);
    	animator3.SetBool("WeaponRun", true);
    }
    
    void PistolReload(){
    	PistolReady();
    	animator.SetBool("PistolReload", true);
    	animator2.SetBool("PistolReload", true);
    	animator3.SetBool("PistolReload", true);
    }
    
    void PistolFire(){
    	PistolInstant();
    	animator.SetBool("PistolFire", true);
    	animator2.SetBool("PistolFire", true);
    	animator3.SetBool("PistolFire", true);
    }
    
    void PistolInstant(){
    	animator.SetBool("PistolInstant", true);
    	animator2.SetBool("PistolInstant", true);
    	animator3.SetBool("PistolInstant", true);
    }
    
    void PistolReady(){
    	Falses();
    	pistolReadybool = true;
    	animator.SetBool("PistolReady", true);
    	animator2.SetBool("PistolReady", true);
    	animator3.SetBool("PistolReady", true);
    }
    
    void WeaponReload(){
    	WeaponReady();
    	animator.SetBool("WeaponReload", true);
    	animator2.SetBool("WeaponReload", true);
    	animator3.SetBool("WeaponReload", true);
    }
    
    void WeaponFire(){
    	WeaponInstant();
    	animator.SetBool("WeaponFire", true);
    	animator2.SetBool("WeaponFire", true);
    	animator3.SetBool("WeaponFire", true);
    }
	
	void ShotgunFire(){
		WeaponInstant();
    	animator.SetBool("ShotgunFire", true);
    	animator2.SetBool("ShotgunFire", true);
    	animator3.SetBool("ShotgunFire", true);
	}
	
	void ShotgunReloadChamber(){
		WeaponInstant();
    	animator.SetBool("ShotgunReloadChamber", true);
    	animator2.SetBool("ShotgunReloadChamber", true);
    	animator3.SetBool("ShotgunReloadChamber", true);
		animator.SetBool("ShotgunReloadMagazine", true);
    	animator2.SetBool("ShotgunReloadMagazine", true);
    	animator3.SetBool("ShotgunReloadMagazine", true);
	}
	
	void ShotgunReloadMagazine(){
		WeaponInstant();
    	animator.SetBool("ShotgunReloadMagazine", true);
    	animator2.SetBool("ShotgunReloadMagazine", true);
    	animator3.SetBool("ShotgunReloadMagazine", true);
	}
    
    void WeaponInstant(){
    	WeaponReady();
    	animator.SetBool("WeaponInstant", true);
    	animator2.SetBool("WeaponInstant", true);
    	animator3.SetBool("WeaponInstant", true);
    }
	
	void NadeThrow(){
		WeaponInstant();
		animator.SetBool("NadeThrow", true);
    	animator2.SetBool("NadeThrow", true);
    	animator3.SetBool("NadeThrow", true);
	}
    
    void WeaponReady(){
    	Falses();
    	weaponStandbool = true;
    	animator.SetBool("WeaponReady", true);
    	animator2.SetBool("WeaponReady", true);
    	animator3.SetBool("WeaponReady", true);
    }
    
    void WeaponStand(){
    	Falses();
    	weaponStandbool = true;
    	animator.SetBool("WeaponStand", true);
    	animator2.SetBool("WeaponStand", true);
    	animator3.SetBool("WeaponStand", true);
    }
    
    void RHandPunch(){
    	animator.SetBool("RHandPunch", true);
    	animator2.SetBool("RHandPunch", true);
    	animator3.SetBool("RHandPunch", true);
    }
    
    void LHandPunch(){
    	animator.SetBool("LHandPunch", true);
    	animator2.SetBool("LHandPunch", true);
    	animator3.SetBool("LHandPunch", true);
    }
    
    void FaceHit(){
    	animator.SetBool("FaceHit", true);
    	animator2.SetBool("FaceHit", true);
    	animator3.SetBool("FaceHit", true);
    }
    
    void FrontKick(){
    	animator.SetBool("FrontKick", true);
    	animator2.SetBool("FrontKick", true);
    	animator3.SetBool("FrontKick", true);
    }
    
    void IdleStand(){
    	Falses();
    	idleStandbool = true;
    	animator.SetBool("IdleStand", true);
    	animator2.SetBool("IdleStand", true);
    	animator3.SetBool("IdleStand", true);
    }
    
    void IdleReady(){
    	Falses();
    	idleReadybool = true;
    	animator.SetBool("IdleReady", true);
    	animator2.SetBool("IdleReady", true);
    	animator3.SetBool("IdleReady", true);
    }
    
    void IdleMonster(){
    	Falses();
    	idleMonsterbool = true;
    	animator.SetBool("IdleMonster", true);
    	animator2.SetBool("IdleMonster", true);
    	animator3.SetBool("IdleMonster", true);
    	
    }
    
    void IdleCheer(){
		IdleStand();
    	animator.SetBool("IdleCheer", true);
    	animator2.SetBool("IdleCheer", true);
    	animator3.SetBool("IdleCheer", true);
    }
	
	void IdleWalk(){
		IdleStand();
		animator.SetBool("IdleWalk", true);
    	animator2.SetBool("IdleWalk", true);
    	animator3.SetBool("IdleWalk", true);
	}
	
	void CratePush(){
		IdleStand();
		animator.SetBool("CratePush", true);
    	animator2.SetBool("CratePush", true);
    	animator3.SetBool("CratePush", true);
	}
	
	void CratePull(){
		IdleStand();
		animator.SetBool("CratePull", true);
    	animator2.SetBool("CratePull", true);
    	animator3.SetBool("CratePull", true);
	}
	
	void IdleStrafeRight(){
		IdleStand();
		animator.SetBool("IdleStrafeRight", true);
    	animator2.SetBool("IdleStrafeRight", true);
    	animator3.SetBool("IdleStrafeRight", true);
	}
	
	void IdleStrafeLeft(){
		IdleStand();
		animator.SetBool("IdleStrafeLeft", true);
    	animator2.SetBool("IdleStrafeLeft", true);
    	animator3.SetBool("IdleStrafeLeft", true);
	}
	
	void IdleRun(){
		IdleReady();
		animator.SetBool("IdleRun", true);
    	animator2.SetBool("IdleRun", true);
    	animator3.SetBool("IdleRun", true);
	}
    
    void ComeHere(){
		IdleStand();
    	animator.SetBool("ComeHere", true);
    	animator2.SetBool("ComeHere", true);
    	animator3.SetBool("ComeHere", true);
    }
    
    void IdleKeepBack(){
		IdleStand();
    	animator.SetBool("IdleKeepBack", true);
    	animator2.SetBool("IdleKeepBack", true);
    	animator3.SetBool("IdleKeepBack", true);
    }
    
    void IdleFight(){
    	Falses();
    	idleFightbool = true;
    	animator.SetBool("IdleFight", true);
    	animator2.SetBool("IdleFight", true);
    	animator3.SetBool("IdleFight", true);
    }
    
    void IdleReadyCrouch(){
		IdleReady();
    	animator.SetBool("IdleReadyCrouch", true);
    	animator2.SetBool("IdleReadyCrouch", true);
    	animator3.SetBool("IdleReadyCrouch", true);
    }
    
    void IdleReadyLook(){
		IdleReady();
    	animator.SetBool("IdleReadyLook", true);
    	animator2.SetBool("IdleReadyLook", true);
    	animator3.SetBool("IdleReadyLook", true);
    }
    
    void IdleSad(){
		IdleStand();
    	animator.SetBool("IdleSad", true);
    	animator2.SetBool("IdleSad", true);
    	animator3.SetBool("IdleSad", true);
    }
	
	void IdleTurns(){
		IdleStand();
    	animator.SetBool("IdleTurns", true);
    	animator2.SetBool("IdleTurns", true);
    	animator3.SetBool("IdleTurns", true);
	}
    
    void Wizard1HandThrow(){
		IdleReady();
    	animator.SetBool("Wizard1HandThrow", true);
    	animator2.SetBool("Wizard1HandThrow", true);
    	animator3.SetBool("Wizard1HandThrow", true);
    }
    
    void Wizard2HandThrow(){
		IdleReady();
    	animator.SetBool("Wizard2HandThrow", true);
    	animator2.SetBool("Wizard2HandThrow", true);
    	animator3.SetBool("Wizard2HandThrow", true);
    }
    
    void WizardBlock(){
		IdleReady();
    	animator.SetBool("WizardBlock", true);
    	animator2.SetBool("WizardBlock", true);
    	animator3.SetBool("WizardBlock", true);
    }
    
    void WizardOverhead(){
		IdleReady();
    	animator.SetBool("WizardOverhead", true);
    	animator2.SetBool("WizardOverhead", true);
    	animator3.SetBool("WizardOverhead", true);
    }
    
    void WizardPowerUp(){
		IdleReady();
    	animator.SetBool("WizardPowerUp", true);
    	animator2.SetBool("WizardPowerUp", true);
    	animator3.SetBool("WizardPowerUp", true);
    }
	
	void IdleDie(){
		IdleReady();
		animator.SetBool("IdleDie", true);
    	animator2.SetBool("IdleDie", true);
    	animator3.SetBool("IdleDie", true);
	}
    
	void Falses(){
    	weaponStandbool = false;
    	idleStandbool = false;
    	idleReadybool = false;
    	idleMonsterbool = false;
    	idleFightbool = false;
    	weaponRun = false;
    	oneHandSwordIdle = false;
    	pistolReadybool = false;
		bowIdle = false;
		motorbikeIdle = false;
		rollerBlade = false;
		skateboard = false;
		climbing = false;
		office = false;
		swim = false;
		wand = false;
		cards = false;

		animator.SetBool("ChannelCastOmni", false);
		animator2.SetBool("ChannelCastOmni", false);
		animator3.SetBool("ChannelCastOmni", false);
		animator.SetBool("ChannelCastDirected", false);
		animator2.SetBool("ChannelCastDirected", false);
		animator3.SetBool("ChannelCastDirected", false);
		animator.SetBool("BowInstant2", false);
		animator2.SetBool("BowInstant2", false);
		animator3.SetBool("BowInstant2", false);
		animator.SetBool("BowReady2", false);
		animator2.SetBool("BowReady2", false);
		animator3.SetBool("BowReady2", false);
		animator.SetBool("WalkDehydrated", false);
		animator2.SetBool("WalkDehydrated", false);
		animator3.SetBool("WalkDehydrated", false);
		animator.SetBool("UpHillWalk", false);
		animator2.SetBool("UpHillWalk", false);
		animator3.SetBool("UpHillWalk", false);
		animator.SetBool("CardPlayerIdle", false);
		animator2.SetBool("CardPlayerIdle", false);
		animator3.SetBool("CardPlayerIdle", false);
		animator.SetBool("DealerIdle", false);
		animator2.SetBool("DealerIdle", false);
		animator3.SetBool("DealerIdle", false);
		animator.SetBool("StaffStand", false);
		animator2.SetBool("StaffStand", false);
		animator3.SetBool("StaffStand", false);
		animator.SetBool("WandStand", false);
		animator2.SetBool("WandStand", false);
		animator3.SetBool("WandStand", false);
		animator.SetBool("SwimDogPaddle", false);
		animator2.SetBool("SwimDogPaddle", false);
		animator3.SetBool("SwimDogPaddle", false);
		animator.SetBool("SwimFreestyle", false);
		animator2.SetBool("SwimFreestyle", false);
		animator3.SetBool("SwimFreestyle", false);
		animator.SetBool("Swim", false);
		animator2.SetBool("Swim", false);
		animator3.SetBool("Swim", false);
		animator.SetBool("WateringCan", false);
		animator2.SetBool("WateringCan", false);
		animator3.SetBool("WateringCan", false);
		animator.SetBool("OfficeSittingReadingLeanBack", false);
		animator2.SetBool("OfficeSittingReadingLeanBack", false);
		animator3.SetBool("OfficeSittingReadingLeanBack", false);
		animator.SetBool("OfficeSittingReading", false);
		animator2.SetBool("OfficeSittingReading", false);
		animator3.SetBool("OfficeSittingReading", false);
		animator.SetBool("OfficeSittingLegCross", false);
		animator2.SetBool("OfficeSittingLegCross", false);
		animator3.SetBool("OfficeSittingLegCross", false);;
		animator.SetBool("OfficeSittingBack", false);
		animator2.SetBool("OfficeSittingBack", false);
		animator3.SetBool("OfficeSittingBack", false);
		animator.SetBool("OfficeSitting45DegLeg", false);
		animator2.SetBool("OfficeSitting45DegLeg", false);
		animator3.SetBool("OfficeSitting45DegLeg", false);
		animator.SetBool("OfficeSitting1LegStraight", false);
		animator2.SetBool("OfficeSitting1LegStraight", false);
		animator3.SetBool("OfficeSitting1LegStraight", false);
		animator.SetBool("OfficeSitting", false);
		animator2.SetBool("OfficeSitting", false);
		animator3.SetBool("OfficeSitting", false);
		animator.SetBool("ClimbUp", false);
		animator2.SetBool("ClimbUp", false);
		animator3.SetBool("ClimbUp", false);
		animator.SetBool("ClimbIdle", false);
		animator2.SetBool("ClimbIdle", false);
		animator3.SetBool("ClimbIdle", false);
		animator.SetBool("SkateboardIdle", false);
		animator2.SetBool("SkateboardIdle", false);
		animator3.SetBool("SkateboardIdle", false);
		animator.SetBool("IdleFeedThrow", false);
		animator2.SetBool("IdleFeedThrow", false);
		animator3.SetBool("IdleFeedThrow", false);
		animator.SetBool("RollerBladeCrossoverLeft", false);
		animator2.SetBool("RollerBladeCrossoverLeft", false);
		animator3.SetBool("RollerBladeCrossoverLeft", false);
		animator.SetBool("RollerBladeTurnLeft", false);
		animator2.SetBool("RollerBladeTurnLeft", false);
		animator3.SetBool("RollerBladeTurnLeft", false);
		animator.SetBool("RollerBladeTurnRight", false);
		animator2.SetBool("RollerBladeTurnRight", false);
		animator3.SetBool("RollerBladeTurnRight", false);
		animator.SetBool("RollerBladeSkateFwd", false);
		animator2.SetBool("RollerBladeSkateFwd", false);
		animator3.SetBool("RollerBladeSkateFwd", false);
		animator.SetBool("RollerBladeCrossoverRight", false);
		animator2.SetBool("RollerBladeCrossoverRight", false);
		animator3.SetBool("RollerBladeCrossoverRight", false);
		animator.SetBool("RollerBladeGrindRoyale", false);
		animator2.SetBool("RollerBladeGrindRoyale", false);
		animator3.SetBool("RollerBladeGrindRoyale", false);
		animator.SetBool("RollerBladeRoll", false);
		animator2.SetBool("RollerBladeRoll", false);
		animator3.SetBool("RollerBladeRoll", false);
		animator.SetBool("RollerBladeStand", false);
		animator2.SetBool("RollerBladeStand", false);
		animator3.SetBool("RollerBladeStand", false);
		animator.SetBool("CrouchStrafeLeft", false);
		animator2.SetBool("CrouchStrafeLeft", false);
		animator3.SetBool("CrouchStrafeLeft", false);
		animator.SetBool("CrouchStrafeRight", false);
		animator2.SetBool("CrouchStrafeRight", false);
		animator3.SetBool("CrouchStrafeRight", false);
		animator.SetBool("CrouchWalkBackward", false);
		animator2.SetBool("CrouchWalkBackward", false);
		animator3.SetBool("CrouchWalkBackward", false);
		animator.SetBool("ProneLocomotion", false);
		animator2.SetBool("ProneLocomotion", false);
		animator3.SetBool("ProneLocomotion", false);
		animator.SetBool("ProneIdle", false);
		animator2.SetBool("ProneIdle", false);
		animator3.SetBool("ProneIdle", false);
		animator.SetBool("CrawlLocomotion", false);
		animator2.SetBool("CrawlLocomotion", false);
		animator3.SetBool("CrawlLocomotion", false);
		animator.SetBool("CrawlIdle", false);
		animator2.SetBool("CrawlIdle", false);
		animator3.SetBool("CrawlIdle", false);
		animator.SetBool("RunBackRight", false);
		animator2.SetBool("RunBackRight", false);
		animator3.SetBool("RunBackRight", false);
		animator.SetBool("RunBackLeft", false);
		animator2.SetBool("RunBackLeft", false);
		animator3.SetBool("RunBackLeft", false);
		animator.SetBool("WorkerShovel2", false);
		animator2.SetBool("WorkerShovel2", false);
		animator3.SetBool("WorkerShovel2", false);
		animator.SetBool("WorkerShovel", false);
		animator2.SetBool("WorkerShovel", false);
		animator3.SetBool("WorkerShovel", false);
		animator.SetBool("WorkerPickaxe", false);
		animator2.SetBool("WorkerPickaxe", false);
		animator3.SetBool("WorkerPickaxe", false);
		animator.SetBool("WorkerPickaxe2", false);
		animator2.SetBool("WorkerPickaxe2", false);
		animator3.SetBool("WorkerPickaxe2", false);
		animator.SetBool("WorkerHammer2", false);
		animator2.SetBool("WorkerHammer2", false);
		animator3.SetBool("WorkerHammer2", false);
		animator.SetBool("WorkerHammer", false);
		animator2.SetBool("WorkerHammer", false);
		animator3.SetBool("WorkerHammer", false);
		animator.SetBool("WoodCut", false);
		animator2.SetBool("WoodCut", false);
		animator3.SetBool("WoodCut", false);
		animator.SetBool("MotorbikeLasso", false);
		animator2.SetBool("MotorbikeLasso", false);
		animator3.SetBool("MotorbikeLasso", false);
		animator.SetBool("MotorbikeWheelyNoHands", false);
		animator2.SetBool("MotorbikeWheelyNoHands", false);
		animator3.SetBool("MotorbikeWheelyNoHands", false);
		animator.SetBool("MotorbikeWheely", false);
		animator2.SetBool("MotorbikeWheely", false);
		animator3.SetBool("MotorbikeWheely", false);
		animator.SetBool("MotorbikeSeatStandWheely", false);
		animator2.SetBool("MotorbikeSeatStandWheely", false);
		animator3.SetBool("MotorbikeSeatStandWheely", false);
		animator.SetBool("MotorbikeSeatStand", false);
		animator2.SetBool("MotorbikeSeatStand", false);
		animator3.SetBool("MotorbikeSeatStand", false);
		animator.SetBool("MotorbikeLookBack", false);
		animator2.SetBool("MotorbikeLookBack", false);
		animator3.SetBool("MotorbikeLookBack", false);
		animator.SetBool("MotorbikeHeartAttack", false);
		animator2.SetBool("MotorbikeHeartAttack", false);
		animator3.SetBool("MotorbikeHeartAttack", false);
		animator.SetBool("MotorbikeHeadstand", false);
		animator2.SetBool("MotorbikeHeadstand", false);
		animator3.SetBool("MotorbikeHeadstand", false);
		animator.SetBool("MotorbikeHandstand", false);
		animator2.SetBool("MotorbikeHandstand", false);
		animator3.SetBool("MotorbikeHandstand", false);
		animator.SetBool("MotorbikeHandlebarSit", false);
		animator2.SetBool("MotorbikeHandlebarSit", false);
		animator3.SetBool("MotorbikeHandlebarSit", false);
		animator.SetBool("MotorbikeBackwardStand", false);
		animator2.SetBool("MotorbikeBackwardStand", false);
		animator3.SetBool("MotorbikeBackwardStand", false);
		animator.SetBool("MotorbikeBackwardSitting", false);
		animator2.SetBool("MotorbikeBackwardSitting", false);
		animator3.SetBool("MotorbikeBackwardSitting", false);
		animator.SetBool("MotorbikeAirWalk", false);
		animator2.SetBool("MotorbikeAirWalk", false);
		animator3.SetBool("MotorbikeAirWalk", false);
		animator.SetBool("MotorbikeIdle", false);
		animator2.SetBool("MotorbikeIdle", false);
		animator3.SetBool("MotorbikeIdle", false);
		animator.SetBool("IdleStun", false);
		animator2.SetBool("IdleStun", false);
		animator3.SetBool("IdleStun", false);
		animator.SetBool("1HandSwordChargeUp", false);
		animator2.SetBool("1HandSwordChargeUp", false);
		animator3.SetBool("1HandSwordChargeUp", false);
		animator.SetBool("WeaponRunBackward", false);
		animator2.SetBool("WeaponRunBackward", false);
		animator3.SetBool("WeaponRunBackward", false);
		animator.SetBool("RunBackward", false);
		animator2.SetBool("RunBackward", false);
		animator3.SetBool("RunBackward", false);
		animator.SetBool("1HSwordStrafeRunRight", false);
		animator2.SetBool("1HSwordStrafeRunRight", false);
		animator3.SetBool("1HSwordStrafeRunRight", false);
		animator.SetBool("1HSwordStrafeRunLeft", false);
		animator2.SetBool("1HSwordStrafeRunLeft", false);
		animator3.SetBool("1HSwordStrafeRunLeft", false);
		animator.SetBool("WeaponStrafeRunRight", false);
		animator2.SetBool("WeaponStrafeRunRight", false);
		animator3.SetBool("WeaponStrafeRunRight", false);
		animator.SetBool("WeaponStrafeRunLeft", false);
		animator2.SetBool("WeaponStrafeRunLeft", false);
		animator3.SetBool("WeaponStrafeRunLeft", false);
		animator.SetBool("StrafeRunRight", false);
		animator2.SetBool("StrafeRunRight", false);
		animator3.SetBool("StrafeRunRight", false);
		animator.SetBool("StrafeRunLeft", false);
		animator2.SetBool("StrafeRunLeft", false);
		animator3.SetBool("StrafeRunLeft", false);
		animator.SetBool("FlyUp", false);
    	animator2.SetBool("FlyUp", false);
    	animator3.SetBool("FlyUp", false);
		animator.SetBool("FlyDown", false);
    	animator2.SetBool("FlyDown", false);
    	animator3.SetBool("FlyDown", false);
		animator.SetBool("FlyRight", false);
    	animator2.SetBool("FlyRight", false);
    	animator3.SetBool("FlyRight", false);
		animator.SetBool("FlyLeft", false);
    	animator2.SetBool("FlyLeft", false);
    	animator3.SetBool("FlyLeft", false);
		animator.SetBool("FlyBackward", false);
    	animator2.SetBool("FlyBackward", false);
    	animator3.SetBool("FlyBackward", false);
		animator.SetBool("FlyForward", false);
    	animator2.SetBool("FlyForward", false);
    	animator3.SetBool("FlyForward", false);
		animator.SetBool("IdleFly", false);
    	animator2.SetBool("IdleFly", false);
    	animator3.SetBool("IdleFly", false);
		animator.SetBool("IdleMeditate", false);
    	animator2.SetBool("IdleMeditate", false);
    	animator3.SetBool("IdleMeditate", false);
		animator.SetBool("ShotgunReloadMagazine", false);
    	animator2.SetBool("ShotgunReloadMagazine", false);
    	animator3.SetBool("ShotgunReloadMagazine", false);
		animator.SetBool("BowReady", false);
    	animator2.SetBool("BowReady", false);
    	animator3.SetBool("BowReady", false);
		animator.SetBool("BowInstant", false);
    	animator2.SetBool("BowInstant", false);
    	animator3.SetBool("BowInstant", false);
		animator.SetBool("BowFire", false);
    	animator2.SetBool("BowFire", false);
    	animator3.SetBool("BowFire", false);
		animator.SetBool("IdleStrafeLeft", false);
    	animator2.SetBool("IdleStrafeLeft", false);
    	animator3.SetBool("IdleStrafeLeft", false);
		animator.SetBool("IdleStrafeRight", false);
    	animator2.SetBool("IdleStrafeRight", false);
    	animator3.SetBool("IdleStrafeRight", false);
		animator.SetBool("CratePull", false);
    	animator2.SetBool("CratePull", false);
    	animator3.SetBool("CratePull", false);
		animator.SetBool("CratePush", false);
    	animator2.SetBool("CratePush", false);
    	animator3.SetBool("CratePush", false);
		animator.SetBool("IdleWalk", false);
		animator2.SetBool("IdleWalk", false);
		animator3.SetBool("IdleWalk", false);
    	animator.SetBool("WeaponRun", false);
    	animator2.SetBool("WeaponRun", false);
    	animator3.SetBool("WeaponRun", false);
    	animator.SetBool("WeaponStand", false);
    	animator2.SetBool("WeaponStand", false);
    	animator3.SetBool("WeaponStand", false);
    	animator.SetBool("IdleReady", false);
    	animator2.SetBool("IdleReady", false);
    	animator3.SetBool("IdleReady", false);
    	animator.SetBool("IdleStand", false);
    	animator2.SetBool("IdleStand", false);
    	animator3.SetBool("IdleStand", false);
    	animator.SetBool("IdleMonster", false);
    	animator2.SetBool("IdleMonster", false);
    	animator3.SetBool("IdleMonster", false);
    	animator.SetBool("WeaponReady", false);
    	animator2.SetBool("WeaponReady", false);
    	animator3.SetBool("WeaponReady", false);
    	animator.SetBool("WeaponInstant", false);
    	animator2.SetBool("WeaponInstant", false);
    	animator3.SetBool("WeaponInstant", false);
    	animator.SetBool("IdleFight", false);
    	animator2.SetBool("IdleFight", false);
    	animator3.SetBool("IdleFight", false);
    	animator.SetBool("PistolReady", false);
    	animator2.SetBool("PistolReady", false);
    	animator3.SetBool("PistolReady", false);
    	animator.SetBool("PistolInstant", false);
    	animator2.SetBool("PistolInstant", false);
    	animator3.SetBool("PistolInstant", false);
    	animator.SetBool("OneHandSwordIdle", false);
    	animator2.SetBool("OneHandSwordIdle", false);
    	animator3.SetBool("OneHandSwordIdle", false);
    	animator.SetBool("OneHandSwordReady", false);
    	animator2.SetBool("OneHandSwordReady", false);
    	animator3.SetBool("OneHandSwordReady", false);
    	animator.SetBool("OneHandSwordRun", false);
    	animator2.SetBool("OneHandSwordRun", false);
    	animator3.SetBool("OneHandSwordRun", false);
		animator.SetBool("IdleRun", false);
    	animator2.SetBool("IdleRun", false);
    	animator3.SetBool("IdleRun", false);
		animator.SetBool("BowIdle", false);
    	animator2.SetBool("BowIdle", false);
    	animator3.SetBool("BowIdle", false);
		animator.SetBool("IdleReadyCrouch", false);
    	animator2.SetBool("IdleReadyCrouch", false);
    	animator3.SetBool("IdleReadyCrouch", false);
		animator.SetBool("CrouchWalk", false);
    	animator2.SetBool("CrouchWalk", false);
    	animator3.SetBool("CrouchWalk", false);
    }
    
    
    void Update(){
    	
    	
    	if(animator.GetFloat("Curve") > 0.1f){
			if(animator.GetBool("BowFire")){
				BowReady();
			}


			animator.SetBool("WateringCanWatering", false);
			animator2.SetBool("WateringCanWatering", false);
			animator3.SetBool("WateringCanWatering", false);
			animator.SetBool("OfficeSittingReadingPageFlip", false);
			animator2.SetBool("OfficeSittingReadingPageFlip", false);
			animator3.SetBool("OfficeSittingReadingPageFlip", false);
			animator.SetBool("OfficeSittingEyesRub", false);
			animator2.SetBool("OfficeSittingEyesRub", false);
			animator3.SetBool("OfficeSittingEyesRub", false);
			animator.SetBool("OfficeSittingHandRestFingerTap", false);
			animator2.SetBool("OfficeSittingHandRestFingerTap", false);
			animator3.SetBool("OfficeSittingHandRestFingerTap", false);
			animator.SetBool("OfficeSittingMouseMovement", false);
			animator2.SetBool("OfficeSittingMouseMovement", false);
			animator3.SetBool("OfficeSittingMouseMovement", false);
			animator.SetBool("OfficeSittingReadingCoffeeSip", false);
			animator2.SetBool("OfficeSittingReadingCoffeeSip", false);
			animator3.SetBool("OfficeSittingReadingCoffeeSip", false);
			animator.SetBool("VaderChoke", false);
			animator2.SetBool("VaderChoke", false);
			animator3.SetBool("VaderChoke", false);
			animator.SetBool("HeelClick", false);
			animator2.SetBool("HeelClick", false);
			animator3.SetBool("HeelClick", false);
			animator.SetBool("Yawn", false);
			animator2.SetBool("Yawn", false);
			animator3.SetBool("Yawn", false);
			animator.SetBool("360SpinDeath", false);
			animator2.SetBool("360SpinDeath", false);
			animator3.SetBool("360SpinDeath", false);
			animator.SetBool("ClimbLeft", false);
			animator2.SetBool("ClimbLeft", false);
			animator3.SetBool("ClimbLeft", false);
			animator.SetBool("ClimbRight", false);
			animator2.SetBool("ClimbRight", false);
			animator3.SetBool("ClimbRight", false);
			animator.SetBool("SkateboardKickPush", false);
			animator2.SetBool("SkateboardKickPush", false);
			animator3.SetBool("SkateboardKickPush", false);
			animator.SetBool("IdleStandingJump", false);
			animator2.SetBool("IdleStandingJump", false);
			animator3.SetBool("IdleStandingJump", false);
			animator.SetBool("IdleSlide", false);
			animator2.SetBool("IdleSlide", false);
			animator3.SetBool("IdleSlide", false);
			animator.SetBool("RollerBladeFrontFlip", false);
			animator2.SetBool("RollerBladeFrontFlip", false);
			animator3.SetBool("RollerBladeFrontFlip", false);
			animator.SetBool("RollerBladeBackFlip", false);
			animator2.SetBool("RollerBladeBackFlip", false);
			animator3.SetBool("RollerBladeBackFlip", false);
			animator.SetBool("RollerBladeStop", false);
			animator2.SetBool("RollerBladeStop", false);
			animator3.SetBool("RollerBladeStop", false);
			animator.SetBool("RollerBladeJump", false);
			animator2.SetBool("RollerBladeJump", false);
			animator3.SetBool("RollerBladeJump", false);
			animator.SetBool("MotorbikeSuperman", false);
			animator2.SetBool("MotorbikeSuperman", false);
			animator3.SetBool("MotorbikeSuperman", false);
			animator.SetBool("MotorbikeSpecialFlip", false);
			animator2.SetBool("MotorbikeSpecialFlip", false);
			animator3.SetBool("MotorbikeSpecialFlip", false);
			animator.SetBool("PistolLeftHandStab", false);
			animator2.SetBool("PistolLeftHandStab", false);
			animator3.SetBool("PistolLeftHandStab", false);
			animator.SetBool("IdleMouthWipe", false);
			animator2.SetBool("IdleMouthWipe", false);
			animator3.SetBool("IdleMouthWipe", false);
			animator.SetBool("IdleSpew", false);
			animator2.SetBool("IdleSpew", false);
			animator3.SetBool("IdleSpew", false);
			animator.SetBool("1HandSwordRollAttack", false);
			animator2.SetBool("1HandSwordRollAttack", false);
			animator3.SetBool("1HandSwordRollAttack", false);
			animator.SetBool("MotorbikeTurnRight", false);
			animator2.SetBool("MotorbikeTurnRight", false);
			animator3.SetBool("MotorbikeTurnRight", false);
			animator.SetBool("MotorbikeTurnLeft", false);
			animator2.SetBool("MotorbikeTurnLeft", false);
			animator3.SetBool("MotorbikeTurnLeft", false);
			animator.SetBool("MotorbikeShootLeft", false);
			animator2.SetBool("MotorbikeShootLeft", false);
			animator3.SetBool("MotorbikeShootLeft", false);
			animator.SetBool("MotorbikeShootRight", false);
			animator2.SetBool("MotorbikeShootRight", false);
			animator3.SetBool("MotorbikeShootRight", false);
			animator.SetBool("MotorbikeShootFwd", false);
			animator2.SetBool("MotorbikeShootFwd", false);
			animator3.SetBool("MotorbikeShootFwd", false);
			animator.SetBool("MotorbikeShootBack", false);
			animator2.SetBool("MotorbikeShootBack", false);
			animator3.SetBool("MotorbikeShootBack", false);
			animator.SetBool("MotorbikeLassoRight", false);
			animator2.SetBool("MotorbikeLassoRight", false);
			animator3.SetBool("MotorbikeLassoRight", false);
			animator.SetBool("MotorbikeLassoLeft", false);
			animator2.SetBool("MotorbikeLassoLeft", false);
			animator3.SetBool("MotorbikeLassoLeft", false);
			animator.SetBool("MotorbikeLassoBack", false);
			animator2.SetBool("MotorbikeLassoBack", false);
			animator3.SetBool("MotorbikeLassoBack", false);
			animator.SetBool("MotorbikeLassoFwd", false);
			animator2.SetBool("MotorbikeLassoFwd", false);
			animator3.SetBool("MotorbikeLassoFwd", false);
			animator.SetBool("MotorbikeBackwardSittingCheer", false);
			animator2.SetBool("MotorbikeBackwardSittingCheer", false);
			animator3.SetBool("MotorbikeBackwardSittingCheer", false);
			animator.SetBool("WeaponReadyFire", false);
			animator2.SetBool("WeaponReadyFire", false);
			animator3.SetBool("WeaponReadyFire", false);
			animator.SetBool("ShotgunReadyFire", false);
			animator2.SetBool("ShotgunReadyFire", false);
			animator3.SetBool("ShotgunReadyFire", false);
			animator.SetBool("1HandSwordChargeUp", false);
			animator2.SetBool("1HandSwordChargeUp", false);
			animator3.SetBool("1HandSwordChargeUp", false);
			animator.SetBool("1HandSwordChargeSwipe", false);
			animator2.SetBool("1HandSwordChargeSwipe", false);
			animator3.SetBool("1HandSwordChargeSwipe", false);
			animator.SetBool("1HandSwordChargeHeavyBash", false);
			animator2.SetBool("1HandSwordChargeHeavyBash", false);
			animator3.SetBool("1HandSwordChargeHeavyBash", false);
			animator.SetBool("1HandSwordShieldBash", false);
			animator2.SetBool("1HandSwordShieldBash", false);
			animator3.SetBool("1HandSwordShieldBash", false);
			animator.SetBool("Crouch180", false);
			animator2.SetBool("Crouch180", false);
			animator3.SetBool("Crouch180", false);
			animator.SetBool("WizardNeoBlock", false);
			animator2.SetBool("WizardNeoBlock", false);
			animator3.SetBool("WizardNeoBlock", false);
			animator.SetBool("WizardEyeBeam", false);
			animator2.SetBool("WizardEyeBeam", false);
			animator3.SetBool("WizardEyeBeam", false);
			animator.SetBool("ShotgunFire", false);
			animator2.SetBool("ShotgunFire", false);
			animator3.SetBool("ShotgunFire", false);
			animator.SetBool("IdleDodgeLeft", false);
			animator2.SetBool("IdleDodgeLeft", false);
			animator3.SetBool("IdleDodgeLeft", false);
			animator.SetBool("IdleDodgeRight", false);
			animator2.SetBool("IdleDodgeRight", false);
			animator3.SetBool("IdleDodgeRight", false);
			animator.SetBool("RunDive", false);
			animator2.SetBool("RunDive", false);
			animator3.SetBool("RunDive", false);
			animator.SetBool("RunJump", false);
			animator2.SetBool("RunJump", false);
			animator3.SetBool("RunJump", false);
			animator.SetBool("Cowboy1HandDraw", false);
			animator2.SetBool("Cowboy1HandDraw", false);
			animator3.SetBool("Cowboy1HandDraw", false);
    		animator.SetBool("WeaponReload", false);
    		animator2.SetBool("WeaponReload", false);
    		animator3.SetBool("WeaponReload", false);
    		animator.SetBool("WeaponFire", false);
    		animator2.SetBool("WeaponFire", false);
    		animator3.SetBool("WeaponFire", false);
    		animator.SetBool("RHandPunch", false);
    		animator2.SetBool("RHandPunch", false);
    		animator3.SetBool("RHandPunch", false);
    		animator.SetBool("LHandPunch", false);
    		animator2.SetBool("LHandPunch", false);
    		animator3.SetBool("LHandPunch", false);
    		animator.SetBool("FaceHit", false);
    		animator2.SetBool("FaceHit", false);
    		animator3.SetBool("FaceHit", false);
    		animator.SetBool("FrontKick", false);
    		animator2.SetBool("FrontKick", false);
    		animator3.SetBool("FrontKick", false);
    		animator.SetBool("IdleCheer", false);
    		animator2.SetBool("IdleCheer", false);
    		animator3.SetBool("IdleCheer", false);
    		animator.SetBool("ComeHere", false);
    		animator2.SetBool("ComeHere", false);
    		animator3.SetBool("ComeHere", false);
    		animator.SetBool("IdleKeepBack", false);
    		animator2.SetBool("IdleKeepBack", false);
    		animator3.SetBool("IdleKeepBack", false);
    		animator.SetBool("IdleReadyLook", false);
    		animator2.SetBool("IdleReadyLook", false);
    		animator3.SetBool("IdleReadyLook", false);
    		animator.SetBool("IdleSad", false);
    		animator2.SetBool("IdleSad", false);
    		animator3.SetBool("IdleSad", false);
    		animator.SetBool("Wizard1HandThrow", false);
    		animator2.SetBool("Wizard1HandThrow", false);
    		animator3.SetBool("Wizard1HandThrow", false);
    		animator.SetBool("Wizard2HandThrow", false);
    		animator2.SetBool("Wizard2HandThrow", false);
    		animator3.SetBool("Wizard2HandThrow", false);
    		animator.SetBool("WizardBlock", false);
    		animator2.SetBool("WizardBlock", false);
    		animator3.SetBool("WizardBlock", false);
    		animator.SetBool("WizardOverhead", false);
    		animator2.SetBool("WizardOverhead", false);
    		animator3.SetBool("WizardOverhead", false);
    		animator.SetBool("WizardPowerUp", false);
    		animator2.SetBool("WizardPowerUp", false);
    		animator3.SetBool("WizardPowerUp", false);
    		animator.SetBool("PistolFire", false);
    		animator2.SetBool("PistolFire", false);
    		animator3.SetBool("PistolFire", false);
    		animator.SetBool("PistolReload", false);
    		animator2.SetBool("PistolReload", false);
    		animator3.SetBool("PistolReload", false);
    		animator.SetBool("OneHandSwordSwing", false);
    		animator2.SetBool("OneHandSwordSwing", false);
    		animator3.SetBool("OneHandSwordSwing", false);
    		animator.SetBool("OneHandSwordBackSwing", false);
    		animator2.SetBool("OneHandSwordBackSwing", false);
    		animator3.SetBool("OneHandSwordBackSwing", false);
    		animator.SetBool("OneHandSwordJab", false);
    		animator2.SetBool("OneHandSwordJab", false);
    		animator3.SetBool("OneHandSwordJab", false);
    		animator.SetBool("OneHandSwordBlock", false);
    		animator2.SetBool("OneHandSwordBlock", false);
    		animator3.SetBool("OneHandSwordBlock", false);
			animator.SetBool("IdleDie", false);
			animator2.SetBool("IdleDie", false);
			animator3.SetBool("IdleDie", false);
			animator.SetBool("IdleTurns", false);
			animator2.SetBool("IdleTurns", false);
			animator3.SetBool("IdleTurns", false);
			animator.SetBool("ShotgunReloadChamber", false);
			animator2.SetBool("ShotgunReloadChamber", false);
			animator3.SetBool("ShotgunReloadChamber", false);
			animator.SetBool("NadeThrow", false);
			animator2.SetBool("NadeThrow", false);
			animator3.SetBool("NadeThrow", false);
			animator.SetBool("Idle180", false);
			animator2.SetBool("Idle180", false);
			animator3.SetBool("Idle180", false);
			animator.SetBool("IdleButtonPress", false);
			animator2.SetBool("IdleButtonPress", false);
			animator3.SetBool("IdleButtonPress", false);
			animator.SetBool("IdleTyping", false);
			animator2.SetBool("IdleTyping", false);
			animator3.SetBool("IdleTyping", false);
    		
    	}
    	
    }
}
