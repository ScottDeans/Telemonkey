/*The character controller script

* Created by: Zac Batog
*/


using UnityEngine;
using System.Collections;

public class MonkeyController : MonoBehaviour {
	
	//game mechanic constants
	private const float RUN_MAX = 10f;
	private const float JUMP_MAX = 20f;
	private const float RANGE_MAX = 10f;
	
	private bool grounded = false;//flag for if the character is on the ground
	private char powerupState = 'N';//which powerup is avtive
	private float powerupTimer;//how much time the powerup has left
	private int facing = 1;//which direction the player is facing
	private Vector2 charPos2D;//where the character is in 2D space
	private Vector3 mousePos;//mouse position in world space
	private Vector2 mousePos2D;//mouse position in 2D space
	private Vector2 rayVec;//direction to raycast
	private RaycastHit2D teleCast;//which object is hit by the raycast
	private float teleCool;//cooldown for the teleport gun
	private AudioSource soundPlayer;//plays the sound effects
	ParticleSystem powerupEffect;//shows which powerup is active
	private bool isPaused = false;//flag for if the game is paused

	public bool teleGun;//flag for if the player has the teleport gun
	//audio assets
	public AudioClip jumpSound;
	public AudioClip teleSound;
	public AudioClip shieldSound;
	//teleport cooldown and powerup timer textures
	public Texture batteryEmpty;
	public Texture teleCharge1;
	public Texture teleCharge2;
	public Texture teleCharge3;
	public Texture teleCharge4;
	public Texture teleCharge5;
	public Texture powCharge1;
	public Texture powCharge2;
	public Texture powCharge3;
	public Texture powCharge4;
	public Texture powCharge5;
	
	public GameObject gameManager;//connection to the game manager

	Animator animator;
	Rigidbody2D body;
	
	private bool isNetwork;
	void Start () {
		isNetwork = true;
		if ((Network.peerType != NetworkPeerType.Server)&&( Network.peerType != NetworkPeerType.Client))  {
			isNetwork=false;
			
			
		} else{
			if(networkView.isMine){
				isNetwork=false;
			}
		}
		animator = GetComponent<Animator>();
		body = GetComponent<Rigidbody2D>();
		teleCool = -1f;
		powerupTimer = -1f;
		powerupEffect = GetComponent<ParticleSystem>();
		powerupEffect.enableEmission = false;
		soundPlayer = GetComponent<AudioSource> ();
	}

	void OnGUI() {
		//displays the charging of the teleport gun
		if(teleGun){
			switch(Mathf.CeilToInt(teleCool)){
			case 0:
				GUI.DrawTexture(new Rect(Screen.width/2 - Screen.width/5,0f,Screen.width/10,Screen.height/5),teleCharge5);
				break;
			case 1:
				GUI.DrawTexture(new Rect(Screen.width/2 - Screen.width/5,0f,Screen.width/10,Screen.height/5),teleCharge4);
				break;
			case 2:
				GUI.DrawTexture(new Rect(Screen.width/2 - Screen.width/5,0f,Screen.width/10,Screen.height/5),teleCharge3);
				break;
			case 3:
				GUI.DrawTexture(new Rect(Screen.width/2 - Screen.width/5,0f,Screen.width/10,Screen.height/5),teleCharge2);
				break;
			case 4:
				GUI.DrawTexture(new Rect(Screen.width/2 - Screen.width/5,0f,Screen.width/10,Screen.height/5),teleCharge1);
				break;
			default:
				GUI.DrawTexture(new Rect(Screen.width/2 - Screen.width/5,0f,Screen.width/10,Screen.height/5),batteryEmpty);
				break;
			}
		}
		
		//displays time left in a powerup state
		int timeScale = 1;//adjusts for how long a powerup will last
		if(powerupState != 'N'){
			if(powerupState == 'B'){timeScale = 2;}
			switch(Mathf.CeilToInt(powerupTimer/timeScale)){
			case 0:
				GUI.DrawTexture(new Rect(Screen.width/2 + Screen.width/10,0f,Screen.width/10,Screen.height/5),batteryEmpty);
				break;
			case 1:
				GUI.DrawTexture(new Rect(Screen.width/2 + Screen.width/10,0f,Screen.width/10,Screen.height/5),powCharge1);
				break;
			case 2:
				GUI.DrawTexture(new Rect(Screen.width/2 + Screen.width/10,0f,Screen.width/10,Screen.height/5),powCharge2);
				break;
			case 3:
				GUI.DrawTexture(new Rect(Screen.width/2 + Screen.width/10,0f,Screen.width/10,Screen.height/5),powCharge3);
				break;
			case 4:
				GUI.DrawTexture(new Rect(Screen.width/2 + Screen.width/10,0f,Screen.width/10,Screen.height/5),powCharge4);
				break;
			default:
				GUI.DrawTexture(new Rect(Screen.width/2 + Screen.width/10,0f,Screen.width/10,Screen.height/5),powCharge5);
				break;
			}
		}
	}
	
	void Update () {
		if (!isNetwork) {
			//input variables
			float run = Input.GetAxis ("Horizontal");
			bool jump = Input.GetButtonDown ("Jump");
			bool shoot = Input.GetButtonDown ("Fire1");
	
			//controls the animator
			if (run != 0) {
					animator.SetBool ("Running", true);
			} else {
					animator.SetBool ("Running", false);
			}
	
			//controls the character's movement from input
			if (run > 0.1f) {
					run *= RUN_MAX;
					if (!teleGun && facing < 0) {
							body.transform.Rotate (Vector3.up * 180);
							facing = 1;
					}
			}
			if (run < -0.1f) {
					run *= RUN_MAX;
					if (!teleGun && facing > 0) {
							body.transform.Rotate (Vector3.up * 180);
							facing = -1;
					}
			}
	
	
			//controls the player's jump from input
			if (jump) {
					if (grounded) {
							soundPlayer.PlayOneShot (jumpSound);
							body.AddForce (transform.up * JUMP_MAX, ForceMode2D.Impulse);
					}
			}
	
	
	
			if (!isPaused && teleGun && teleCool <= 0 && shoot) {
					//finds the character and mouse positions 
					charPos2D = new Vector2 (transform.position.x, transform.position.y);
					mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					mousePos2D = new Vector2 (mousePos.x, mousePos.y);
					rayVec = new Vector2 (mousePos.x - charPos2D.x, mousePos.y - charPos2D.y);
					
					//raycasts to check the validity of the teleport
					gameObject.layer = 2;
					teleCast = Physics2D.Raycast (charPos2D, rayVec, RANGE_MAX);
					gameObject.layer = 10;
					
					//checks if there is an object between the player and the mouse position
					if (teleCast) {
							if (teleCast.distance > Vector2.Distance (mousePos2D, charPos2D)) {
									if (teleCast.distance > 0.5f) {
											Teleport (mousePos2D, charPos2D);
									}
							}
					} else if (!teleCast) {
							if (RANGE_MAX > Vector2.Distance (mousePos2D, charPos2D)) {
									Teleport (mousePos2D, charPos2D);
							}
					}
			}
			
			//adjusts speed when the speed powerup is on
			if (powerupState == 'S') {
					run *= 2;
			}
			
			//moves the player
			body.velocity = new Vector2 (run, body.velocity.y);
			
			//controls the teleport cooldown
			if (teleCool > 0) {
					teleCool -= Time.smoothDeltaTime;
			}
			if (teleCool < 0) {
					teleCool = 0f;
			}
			
			//controls the powerup timer
			if (powerupTimer > 0) {
					powerupTimer -= Time.smoothDeltaTime;
			} else {
					powerupState = 'N';
					powerupEffect.enableEmission = false;
			}
		}
	}
	
	//changes the direction that the character is facing
	void FaceDirection(float dir){
		if (dir > 0 && facing < 0) {
			body.transform.Rotate (Vector3.up * 180);
			facing = 1;
		}else if (dir < 0 && facing > 0) {
			body.transform.Rotate (Vector3.up * 180);
			facing = -1;
		}
	}
	
	//checks if the mouse position is within teleport range on click and teleports the player
	void Teleport(Vector2 aPos, Vector2 bPos){
		if(Vector2.Distance(aPos, bPos) < RANGE_MAX){
			aPos.x *= facing;
			bPos.x *= facing;
			soundPlayer.PlayOneShot(teleSound);
			transform.Translate(aPos - bPos);
			teleCool = 5f;
		}
	}

	void OnTriggerStay2D(Collider2D ground){
		if (ground.gameObject.tag == "Terrain") {
			grounded = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D ground){
		grounded = false;
	}
	
	//sets the powerup state
	void PowerUp(char type){
		powerupState = type;
		powerupEffect.enableEmission = true;
		switch (powerupState) {
		case 'S':
			powerupEffect.startColor = Color.green;
			powerupTimer = 5f;
			break;
		case 'B':
			powerupEffect.startColor = Color.blue;
			powerupTimer = 10f;
			break;
		case 'I':
			powerupEffect.startColor = Color.yellow;
			powerupTimer = 5f;
			break;
		case 'F':
			powerupEffect.startColor = Color.cyan;
			powerupTimer = 5f;
			break;
		}
	}
	
	void Pause(){
		isPaused = true;
	}
	void Unpause(){
		isPaused = false;
	}

	//handles when the player gets hit
	void Hit(){
		if(powerupState == 'B'){
			soundPlayer.PlayOneShot(shieldSound);
			powerupState = 'D';
			powerupTimer = 0.5f;
			powerupEffect.enableEmission = false;
		}
		else if(powerupState == 'I'){
			soundPlayer.PlayOneShot(shieldSound);
		}
		else if(powerupState != 'D'){
			Death ();
		}
	}
	//Manages what happens when the player dies
	void Death(){
		gameManager.BroadcastMessage("Death");
		gameObject.SetActive(false);
	}
}
