/*A script for the turret projectile that
* sends a message to damage the player upon hit

* Created by: Zac Batog
*/

using UnityEngine;
using System.Collections;

public class TurretProjectile : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D hit){
		if(hit.tag == "Terrain"){
			Object.Destroy(gameObject);
		}else if (hit.tag == "Player"){
			hit.gameObject.SendMessage("Hit");
			Object.Destroy(gameObject);
		}
	}
}
