/*A script for the detonator object which will
* make the wall with a dynamite stick, "Boom Wall"
* explode when the player touches the top.

* Created by: David Sollinger
*/

using UnityEngine;
using System.Collections;

public class Detonator : MonoBehaviour {
	//the wall that will be destroyed when this detonator is pushed
	public GameObject wall;
	Animator anim;
	bool exploded = false;
	
	private AudioSource soundPlayer;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		soundPlayer = GetComponent<AudioSource>();
	}
	
	/*when the player collides with the top of
	* the detonator, it will move down and destroy
	* the wall
	*/
	void OnCollisionEnter2D(Collision2D target) {
		//checks if target is tagged as player
		if (target.gameObject.tag == "Player" && exploded == false) {
			//plays its Pressed animation and destroys the wall
			anim.SetBool ("Pressed", true);
			Destroy(wall);
			soundPlayer.Play();
			exploded = true;
		}
	}
}
