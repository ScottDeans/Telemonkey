/*A script for the barrier powerup that tells
* the character controller to enter the barrier
* powerup state.

* Created by: Zac Batog
*/

using UnityEngine;
using System.Collections;

public class BarrierPowerup : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D hit){
		if(hit.tag == "Player"){
			hit.SendMessage("PowerUp", 'B');
			gameObject.SetActive(false);
		}
	}
}
