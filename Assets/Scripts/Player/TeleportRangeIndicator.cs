/*A script for the player that changes the
* cursor to indicate if the cursor is in
* teleport range.

* Created by: Zac Batog
*/

using UnityEngine;
using System.Collections;

public class TeleportRangeIndicator : MonoBehaviour {
	
	//Textures for the cursor
	public Texture2D cursorInRange;
	public Texture2D cursorOutRange;
	
	private Vector3 mousePos;//where the mouse is in worldspace
	private Vector2 rayVector;//direction to cast the ray in
	private RaycastHit2D obstacleHit;//which object the ray hit
	private Vector2 offset;//centers the reticle sprite on the mouse
	
	void Start(){
		offset = new Vector2(32f, 32f);
		Cursor.SetCursor(cursorOutRange, offset, CursorMode.Auto);
	}
	
	void Update() {
		//finds mouse location in worldspace
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.Set(mousePos.x,mousePos.y,0f);
		
		//calculates direction to raycast
		rayVector = new Vector2(gameObject.transform.position.x - mousePos.x, gameObject.transform.position.y - mousePos.y);
		
		obstacleHit = Physics2D.Raycast(new Vector2(mousePos.x,mousePos.y), rayVector);
		
		if(Vector3.Distance(gameObject.transform.position, mousePos) < 10f && obstacleHit.collider.tag == "Player"){
			Cursor.SetCursor(cursorInRange, offset, CursorMode.Auto);
		}
		else{
			Cursor.SetCursor(cursorOutRange, offset, CursorMode.Auto);
		}
		
	}
}
