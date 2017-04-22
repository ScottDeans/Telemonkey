/*A script for the invincibility powerup that tells
* the character controller to enter the invincibility
* powerup state.

* Created by: Zac Batog
*/


using UnityEngine;
using System.Collections;

public class InvincibilityPowerup : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D hit){
		if(hit.tag == "Player"){
			hit.SendMessage("PowerUp", 'I');
			gameObject.SetActive(false);
		}
	}
}
