/* An AI script for the hazard known as "Tobor". Tobor will
* patrol back and fourth between two walls until the player
* is within his sight. Once the player is spotted, he will pause
* for 2 seconds, which allows the player to move out of his vision.
* If the player does not move out of his vision, he will chase the player.
* He is able to jump over walls and other obstacles. Once the player is 
* no longer in sight, he resumes his regular patrol pattern.
*
*3 States:
* Patrol
**	Player is not in sight and Tobor will pace back and fourth between two walls
* Searching
**	Tobor has seen the player and will pause for 2 seconds before entering the next stage
* Pursuing
**	Tobor is facing the player and is chasing after him
* An animator is used to visually indicate which state he is in. 

*Created by: David Sollinger
*/

using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour {
	
	//variables for the robot's movement
	private float speed = 5;
	private int direction;
	private const float JUMP_MAX = 11f;
	
	//creates a vector 2 for the direction to racast in
	Vector2 rayDirection;
	
	//boolean variables to handle different cases the robot uses
	private bool patrol, grounded, jump, facingRight, freeze;
	
	/*different gameObjects the robot requires to work.
	* Wall detector is the trigger for tobor to switch directions
	* Jump detector is the trigger for Tobor to jump over obstacles
	* Player allows Tobor to track the player's movement
	*/
	public GameObject wallDetector, jumpDetector, player;
	
	Rigidbody2D body;
	
	//sets Tobor's initial movement direction. True for right, false for left
	public bool moveRight;
	
	Animator animator;

	void Start () {
		animator = GetComponent <Animator> ();
		patrol = true;
		grounded = true;
		animator.SetBool ("Normal", true);
		body = GetComponent<Rigidbody2D> ();
		jump = false;
		freeze = false;
		
		//sets the movement direction based on the value of moveRight
		if (moveRight == true) {
			direction = 1;
			facingRight = true;
		} else {
			direction = -1;
			facingRight = false;
		}
		rayDirection = new Vector2 (direction, 0);
	}

	void Update () {
	
		//If Tobor does not have the freeze bomb effect on him
		if (freeze == false) {
			body.velocity = new Vector2 (speed * direction, body.velocity.y);
			
			/*if patrol is true, then Tobor is pacing back and fourth between walls
			*if patrol is false, then Tobor is pursuing the player
			*/
			if (patrol) {
				//sets wall detector to true and jump detector to false
				jumpDetector.SetActive (false);
				wallDetector.SetActive (true);
			} else {
				//sets wall detector to false and jump detector to true
				jumpDetector.SetActive (true);
				wallDetector.SetActive (false);
				
				//gets the player's object center position and sends it to the function FaceDirection
				FaceDirection(player.transform.renderer.bounds.center.x);
			}
			
			//Tobor will only be able to switch states if he is grounded
			if (grounded) {
				var rayCast = Physics2D.Raycast(transform.position, rayDirection, 10, 1 << LayerMask.NameToLayer("Player"));
				
				/*if raycast and patrol are true, then the player is spotted during the patrol phase
				* if raycast and patrol are false, then the player is no longer in sight during the pursue phase
				*/
				if (rayCast && patrol) {
					//when player is in sight, start the pursue coroutine and set patrol to false
					StopCoroutine ("Searching");
					StopCoroutine ("Patrol");
					StartCoroutine("Pursue");
					patrol = false;
				} else if (!rayCast && !patrol) {
					//when player is not in sight, start the searching coroutine
					StopCoroutine("Pursue");
					StartCoroutine("Searching");
				}
			} else {
				//deactivates the jump detector so he does not continuously jump up a wall
				jumpDetector.SetActive(false);
			}
			
			//allows Tobor to jump over obstacles
			if (jump && grounded) {
				body.AddForce(Vector2.up * JUMP_MAX, ForceMode2D.Impulse);
				jump = false;
				grounded = false;
			}
		}
	}
	
	/*ChangeDirection will change the direction that Tobor is facing. This function is
	* called from the wallDetector when it detects a wall and from the FaceDirection function. 
	*/
	void ChangeDirection () {
		direction *= -1;
		facingRight = !facingRight;
		rayDirection = -rayDirection;
		
		//flips Tobor's direction
		transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}
	
	/*Jump will set the boolean jump to true which will enable Tobor to jump. 
	* This function is called from the jumpDetector when it detects a wall 
	*/
	void Jump() {
		jump = true;
	}
	
	/*FaceDirection accepts a float and changes the direction of Tobor according to the
	* player's position. Calls ChangeDirection to change Tobor's direction to face the player.
	* If the player's center is greater than Tobor's center, he will flip to the right. If the
	* player's center is less than Tobor's center, he will flip to the left.
	*/
	void FaceDirection(float dir){
		if (dir > transform.renderer.bounds.center.x && !facingRight) {
			ChangeDirection();
		} else if (dir < transform.renderer.bounds.center.x && facingRight) {
			ChangeDirection();
		}
	}
	
	/*Pursue Coroutine is called once the player is in sight. It switches
	* to Tobor's searching animation as he stops where he is. Once two seconds have passed
	* it switches to Tobor's Pursue animation and increases his speed in order to chase the player.
	*/
	IEnumerator Pursue() {
		animator.SetBool ("Normal", false);
		animator.SetBool ("Searching", true);
		speed = 0;
		yield return (new WaitForSeconds(2));
		speed = 7;
		animator.SetBool ("Searching", false);
		animator.SetBool ("Pursuing", true);
	}
	
	/*Patrol Coroutine is called once the player is no longer in sight. It switches
	* to Tobor's searching animation as he stops where he is. Once two seconds have passed
	* it switches to Tobor's Normal animation and returns his speed to normal.
	*/
	IEnumerator Patrol() {
		animator.SetBool ("Pursuing", false);
		animator.SetBool ("Searching", true);
		speed = 0;
		yield return (new WaitForSeconds(2));
		speed = 5;
		animator.SetBool ("Searching", false);
		animator.SetBool ("Normal", true);
	}
	
	/*Searching Coroutine is called once the player is no longer in sight. It waits 4 seconds
	* before doing another raycast to check if the player is still in sight. It is used to make 
	* Tobor appear to keep track of the player for a brief moment after losing sight. For example
	* if the player was to jump over an obstacle, Tobor will still keep track of him for four seconds
	* before searching for the player again
	*/
	IEnumerator Searching() {
		yield return (new WaitForSeconds(4)); //waits four seconds before "giving up" the search
		
		/*Does one raycast at this point in the game. If the player is still in view, it continues to pursue
		* If not, then Tobor will enter his patrol phase.*/
		var rayCast = Physics2D.Raycast(transform.position, rayDirection, 10, 1 << LayerMask.NameToLayer("Player"));
		if (rayCast) {
			StartCoroutine ("Pursue");
		} else {
			patrol = true;
			StartCoroutine ("Patrol");
		}
	}
	
	//To check if Tobor is on the ground
	void OnTriggerStay2D(Collider2D ground){
		if (ground.gameObject.tag == "Terrain") {
			grounded = true;
		}
	}
	
	//To check if Tobor is in the air
	void OnTriggerExit2D(Collider2D ground){
		grounded = false;
	}
	
	/* sends a message "hit" to the Monkey Controller script
	* when the player touches Tobor.
	*/
	void OnCollisionEnter2D(Collision2D hit) {
		if (hit.gameObject.name == "Player"){
			hit.gameObject.SendMessage("Hit");
		}
	}
	
	/*Destroys Tobor. This function is called from the 
	* Deathfield script when Tobor runs off an edge into a pit
	*/
	void Death(){
		gameObject.SetActive(false);
	}
	
	/* Freezes Tobor in place. This function is called from the
	* freezebomb powerup when the player runs overtop of the powerup.
	* It begins the Unfreeze coroutine which will allow Tobor to move
	* once 5 seconds have passed
	*/
	void Freeze() {
		freeze = true;
		StartCoroutine ("Unfreeze");
	}
	
	/* Unfreeze Coroutine will unfreeze Tobor within 5 seconds of the freezebomb
	* powerup being picked up by the player. Sets his velocity to 0 and then sets freeze
	* to false once 5 seconds have passed.
	*/
	IEnumerator Unfreeze() {
		body.velocity = new Vector2 (0, 0);
		yield return (new WaitForSeconds(5));
		freeze = false;
	}
}