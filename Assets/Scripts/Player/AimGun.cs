/*A script that points the player's arm at the cursor

* Created by: Zac Batog
*/

using UnityEngine;
using System.Collections;

public class AimGun : MonoBehaviour {

	private Vector3 bodyPos;//what direction the cursor is in relation to the character
	private float deltaAngle;//difference in angle from where the arm is facing to where the cursor is
	private Quaternion rot;//used to rotate the arm
	private bool isPaused = false;//flag for if the game is paused

	Rigidbody2D body;//character's rigidbody
	public bool networkthere;
	void Start() {
		networkthere = true;
		/*if ((Network.peerType != NetworkPeerType.Server) && (Network.peerType != NetworkPeerType.Client)) {
			
			networkthere = true;
			
		} else {
			if (!networkView.isMine) {
				networkthere = false;
			}
		}*/
		body = GetComponentInParent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update() {
		/*if(!networkthere){
			return;
		}*/
		
		//rotates the arm to the cursor when the game is not paused, and flips the body to face the x direction of the cursor
		if(!isPaused){
			bodyPos = Input.mousePosition - Camera.main.WorldToScreenPoint (body.transform.position);
			SendMessageUpwards("FaceDirection", bodyPos.x);
			deltaAngle = Mathf.Atan2 (bodyPos.y, bodyPos.x) * Mathf.Rad2Deg;
			if (bodyPos.x < -1f) {
				rot = Quaternion.AngleAxis (deltaAngle - 180f, Vector3.forward) * Quaternion.AngleAxis (180f, Vector3.up);
				transform.rotation = rot;
			}else if(bodyPos.x > 1f){
				rot = Quaternion.AngleAxis (deltaAngle, Vector3.forward);
				transform.rotation = rot;
			}
		}
	}
	
	void Pause(){
		isPaused = true;
	}
	void Unpause(){
		isPaused = false;
	}
}
