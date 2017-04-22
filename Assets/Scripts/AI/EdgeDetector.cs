/*A script for moving AI that detects when
* the AI is on the edge of a platform, and
* passes the proper command.

* Created by: Zac Batog
*/

using UnityEngine;
using System.Collections;

public class EdgeDetector : MonoBehaviour {

	CircleCollider2D edgeDetect;//trigger that detects the edge

	// Use this for initialization
	void Start () {
		edgeDetect = GetComponent<CircleCollider2D> ();
	}
	
	void OnTriggerExit2D(Collider2D target){
		if(target.tag != "Player"){
			SendMessageUpwards ("ChangeDirection");
		}
	}
}
