/*A script for the speed powerup that tells
* the character controller to enter the speed
* powerup state.

* Created by: Zac Batog
*/


using UnityEngine;
using System.Collections;

public class SpeedPowerup : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D hit){
		if(hit.tag == "Player"){
			hit.SendMessage("PowerUp", 'S');
			gameObject.SetActive(false);
		}
	}
}
