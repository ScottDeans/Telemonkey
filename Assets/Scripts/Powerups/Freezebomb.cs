/*A script for the freeze bomb power up which will freeze
* all of the lethal obstacles, Tobor, Laser Wall and Turret,
* from moving, shooting and tracking the player. These enemies
* will unfreeze within 5 seconds of the power up being activated.
* It is activated when the player runs over the powerup.

* Created by: David Sollinger
*/

using UnityEngine;
using System.Collections;

public class Freezebomb : MonoBehaviour {

	/*When the player hits the trigger of the powerup, it will call this function.
	* hit is the collider that interacted with this trigger
	*/
	void OnTriggerEnter2D(Collider2D hit){
		//checks if hit is tagged as player
		if(hit.tag == "Player"){
			/*Gathers all gameObjects that have a tag of "AI" and stores them into an array.
			* The for loop goes through that array and sends the message "Freeze" to all of
			* the objects within that tag. 
			*/
			GameObject [] gos = GameObject.FindGameObjectsWithTag("AI");
			for (int i = 0; i < gos.Length; i++) {
				gos[i].SendMessage("Freeze");
			}
			//sends a message to the player script to activate the particle affect and color it cyan
			hit.SendMessage("PowerUp", 'F');
			//sets the powerup to inactive
			gameObject.SetActive(false);
		}
	}
}
