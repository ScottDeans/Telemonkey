/*This script has two uses; it can be used to instantly kill both the player and Tobor
* if they collide with the trigger or will send a "Hit" message to the player. When it
* sends the hit message to the player, it checks if the player has any
* powerups that block damage. If the player has no powerups, then this script 
* will kill the player.

* Created by: David Sollinger 
*/

using UnityEngine;
using System.Collections;

public class Deathfield : MonoBehaviour {

	//boolean function to check if its a death field or a damage field
	public bool death;
	
	
	/*When the player's collider hits this trigger, it will 
	* call this function.
	* Target is the collider that interacted with the trigger
	*/
	void OnTriggerStay2D(Collider2D target) {
		if (target.gameObject.tag == "Player" && death == false) {
			//sends the message "Hit" to the player script
			target.gameObject.SendMessage("Hit");
		} else if (death == true) {
			//sends the message "Death" to target's script
			target.gameObject.SendMessage("Death");
		}
	}
}
