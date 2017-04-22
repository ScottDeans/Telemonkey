/*A script for the main camera that gets the
* Camera to follow the player.

* Created by: Zac Batog
*/

using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -1f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -1f);
	}
}
