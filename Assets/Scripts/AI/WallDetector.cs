/*A script for moving AI that detects when
* the AI hits a terrain tagged object and
* passes the proper command.

* Created by: Zac Batog & David Sollinger
*/

using UnityEngine;
using System.Collections;

public class WallDetector : MonoBehaviour {
	
	public bool jump;
	
	void OnTriggerEnter2D(Collider2D target){
		if(target.tag == "Terrain" && jump == false){
			SendMessageUpwards("ChangeDirection");
		} else if (target.tag == "Terrain" && jump == true) {
			SendMessageUpwards("Jump");
		}
	}
}
