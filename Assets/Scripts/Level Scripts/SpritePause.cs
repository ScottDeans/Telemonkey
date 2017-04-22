/*A script for the pause menu object that
* controls pausing for all sprites that have
* features that work during Time.timeScale = 0.

* Created by: Zac Batog
*/

using UnityEngine;
using System.Collections;

public class SpritePause : MonoBehaviour {

	private GameObject[] players;
	private GameObject[] ai;

	//Sets isPaused flag on all sprites that have functions that can run when Time.timeScale is set to zero.
	void PauseAll(){
		players = GameObject.FindGameObjectsWithTag("Player");
		ai = GameObject.FindGameObjectsWithTag("AI");
		foreach(GameObject player in players){
			player.BroadcastMessage("Pause", SendMessageOptions.DontRequireReceiver);
		}
		foreach(GameObject enemy in ai){
			enemy.BroadcastMessage("Pause", SendMessageOptions.DontRequireReceiver);
		}
	}
	
	//Turns off isPaused flag on all sprites that have functions that can run when Time.timeScale is set to zero.
	void UnpauseAll(){
		players = GameObject.FindGameObjectsWithTag("Player");
		ai = GameObject.FindGameObjectsWithTag("AI");
		foreach(GameObject player in players){
			player.BroadcastMessage("Unpause", SendMessageOptions.DontRequireReceiver);
		}
		foreach(GameObject enemy in ai){
			enemy.BroadcastMessage("Unpause", SendMessageOptions.DontRequireReceiver);
		}
	}
}
