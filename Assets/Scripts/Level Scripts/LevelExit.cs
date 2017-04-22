/*A script for the level exit that communicates with
* the game manager to bring up the GUI menu.

* Created by: Zac Batog
*/

using UnityEngine;
using System.Collections;

public class LevelExit : MonoBehaviour {

	public GameObject gameManager;
	public int playerCount; //the number of players in the level
	
	int finishedPlayers = 0;//the number of players to finish the level

	void OnTriggerEnter2D(Collider2D collide){
		if(collide.tag == "Player"){
			collide.gameObject.SetActive(false);
			finishedPlayers++;
		}
		//Detects if both players have finished
		if(finishedPlayers == playerCount){
			gameManager.BroadcastMessage("LevelComplete");
		}
	}
}
