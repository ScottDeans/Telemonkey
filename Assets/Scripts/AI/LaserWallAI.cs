/*A script for the AI of the laser wall

* Created by: Zac Batog
*/

using UnityEngine;
using System.Collections;

public class LaserWallAI : MonoBehaviour {
	
	private const float MOVE_SPEED = 8f;
	private int direction = 1;//which direction the wall is moving
	public Vector2 laserDirection;//which direction the laser is pointing

	private RaycastHit2D laserCast;//detects if the player gets hit
	private bool freeze;//stops movement when the player has the freeze powerup

	Rigidbody2D body;//laser wall's rigidbody

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
		freeze = false;
	}
	
	// Update is called once per frame
	void Update () {
		//controls movement of the laser wall
		if (freeze == false) {
			body.velocity = new Vector2 (MOVE_SPEED * direction * laserDirection.y, MOVE_SPEED * direction * laserDirection.x);
		}

		//check to see if the laser hit the player
		laserCast = Physics2D.Raycast (new Vector2(body.position.x + laserDirection.x, body.position.y + laserDirection.y), laserDirection);
		if (laserCast.collider.tag == "Player") {
			laserCast.collider.gameObject.SendMessage("Hit");
		}
	}
	
	//reverse direction of movement
	void ChangeDirection(){
		direction *= -1;
	}
	
	void Freeze() {
		freeze = true;
		StartCoroutine ("Unfreeze");
	}
	
	IEnumerator Unfreeze() {
		body.velocity = new Vector2 (0, 0);
		yield return (new WaitForSeconds(5));
		freeze = false;
	}
}
