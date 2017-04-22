/*A script for the Audio Source

* Created by: Zac Batog
*/

using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	public AudioClip deathSound;//Sound that plays when the player dies

	private AudioSource soundPlayer;

	// Use this for initialization
	void Start () {
		soundPlayer = GetComponent<AudioSource> ();
		soundPlayer.Play();
	}
	
	//Stops music on Game Over.
	void GameOver(){
		soundPlayer.Stop();
	}
	
	//Plays death sound when the player dies
	void Death(){
		soundPlayer.Stop();
		soundPlayer.PlayOneShot(deathSound);
	}
}
