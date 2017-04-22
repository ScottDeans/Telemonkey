/* A script for the acid barrel in order to cause 
* damage to the player. Sends a "Hit" message to the player
* to check if the player has any powerups that will block hits. If the
* player has no powerups that allow him to aviod damage, then this script
* will kill the player. 
* 
* Created by: David Sollinger
*/

using UnityEngine;
using System.Collections;

public class Acidbarrel : MonoBehaviour {

	/*When the player's collider hits this trigger, it will 
	* call this function.
	* Target is the collider that interacted with the trigger
	*/
	void OnTriggerEnter2D(Collider2D hit){
		//checks if the gameObject is tagged as player
		if (hit.gameObject.tag == "Player") {
			//sends the message "Hit" to the player script
			hit.gameObject.SendMessage("Hit");
		}
	}
}
