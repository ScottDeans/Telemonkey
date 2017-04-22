/*A script for the game manager that handles
* passing messages to the GUI object, and
* what happens when the player dies.

* Created by: Zac Batog
*/

using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	public float levelTime;//Time allotted to complete the level
	
	//sprites for displaying the time remaining
	public Texture[] digitGUI;
	public Texture tColon;
	public string nextLevel;
	
	
	public GameObject gui;//object that contains the pause menu

	private bool isPaused = false;
	private float levelTimer;//Time remaining to complete the level
	
	// Use this for initialization
	void Start () {
		levelTimer = levelTime;
		Time.timeScale = 1f;
	}

	//displays time remaining
	void OnGUI() {
		GUI.DrawTexture(new Rect(Screen.width/2 - Screen.width/50- Screen.width/20,0f,Screen.width/10,Screen.height/10),tColon);
		
		//minutes
		GUI.DrawTexture(new Rect(Screen.width/2 - 3*Screen.width/50 - Screen.width/20,0f,Screen.width/10,Screen.height/10),digitGUI[(int)(levelTimer / 60)]);
		
		//seconds (tens)
		GUI.DrawTexture(new Rect(Screen.width/2 + Screen.width/50- Screen.width/20,0f,Screen.width/10,Screen.height/10),digitGUI[(int)(levelTimer % 60)/10]);
		
		//seconds (ones)
		GUI.DrawTexture(new Rect(Screen.width/2 + 3*Screen.width/50- Screen.width/20,0f,Screen.width/10,Screen.height/10),digitGUI[(int)levelTimer % 10]);
	}

	// Update is called once per frame
	void Update () {
		//stops timer if the level is paused
		if(!isPaused){
			levelTimer -= Time.smoothDeltaTime;
		}
		
		//ends game if timer runs out
		if(levelTimer < 0){
			BroadcastMessage("GameOver");
		}
	}
	
	//handles what happens when the level is completed
	void LevelComplete(){
		//sends the message to show the level complete menu
		isPaused = true;
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		gui.SendMessage("SetNextLevel", nextLevel);
		gui.SendMessage("LevelComplete", levelTime - levelTimer);
	}
	
	//handles what happens when the player dies
	void Death(){
		GameOver();
	}

	//handles what hapens when the player fails a level
	void GameOver(){
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		gui.SendMessage("GameOver");
		Time.timeScale = 0;
	}
}
