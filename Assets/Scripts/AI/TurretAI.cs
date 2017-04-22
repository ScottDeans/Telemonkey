/*A script for the AI of the turret

* Created by: Zac Batog
*/

using UnityEngine;
using System.Collections;

public class TurretAI : MonoBehaviour {

	public Rigidbody2D shot;
	public float shotSpeed;
	public AudioClip shotSound;
	
	Transform muzzle;//location where the projectiles are instantiated
	private GameObject following;//the object that the turret is following
	private GameObject voidGO;//an empty game object that is used when no character is in range
	private int shotDelay;//the amount of frames before the next shot is fired
	private AudioSource soundPlayer;
	private bool isPaused = false, freeze = false;//flags for if the game is paused or the freeze powerup is on

	// Use this for initialization
	void Start () {
		voidGO = new GameObject();
		muzzle = GetComponentsInChildren<Transform>()[1];
		following = voidGO;
		shotDelay = 0;
		soundPlayer = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (freeze == false && isPaused == false) {
			if(following != voidGO){
				Vector3 faceFollowing;//direction that the object that the turret is following is in
				//adjust for object velocity
				Vector3 velocityAdjust = new Vector3(following.rigidbody2D.velocity.x/3f, following.rigidbody2D.velocity.y/3f);
				float angle;//angle to rotate the turret to
				faceFollowing = following.transform.position + velocityAdjust - transform.position;
				angle = Mathf.Atan2(faceFollowing.y, faceFollowing.x) * Mathf.Rad2Deg - 90f;
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * 5f);
				if(shotDelay <= 0){
					FireGun();
					shotDelay = 100;
				}
			}
			if(shotDelay > 0){
				shotDelay--;
			}
		}
	}
	
	//Track which player is closest to the turret in the range and set it to following
	void OnTriggerStay2D(Collider2D entered){
		if(entered.tag == "Player"){
			if(!entered.gameObject.activeSelf){
				following = voidGO;
			}
			else if(following.gameObject.tag != "Player"){
				following = entered.gameObject;
			}else if(following != entered.gameObject &&
			         (following.transform.position.x * following.transform.position.x) + (following.transform.position.y * following.transform.position.y) >
			         (entered.transform.position.x * entered.transform.position.x) + (entered.transform.position.y * entered.transform.position.y)){
				following = entered.gameObject;
			}
		}
	}
	
	//stop following player on exit
	void OnTriggerExit2D(Collider2D leaving){
		if (leaving.name == following.name){
			following = voidGO;
		}
	}
	
	//fires the projectiles
	void FireGun(){
		Rigidbody2D accel;
		soundPlayer.PlayOneShot(shotSound);
		accel = Instantiate(shot, new Vector3(muzzle.position.x, muzzle.position.y), transform.rotation) as Rigidbody2D;
		accel.velocity = transform.TransformDirection(Vector2.up * shotSpeed);
	}
	
	void Pause(){
		isPaused = true;
	}
	void Unpause(){
		isPaused = false;
	}
	
	void Freeze() {
		freeze = true;
		StartCoroutine ("Unfreeze");
	}
	
	IEnumerator Unfreeze() {
		yield return (new WaitForSeconds(5));
		freeze = false;
	}
}