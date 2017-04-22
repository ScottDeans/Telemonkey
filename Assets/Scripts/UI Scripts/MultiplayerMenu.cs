/*----------------
 Scott Deans-1700147
 GrantMacewan University
 4th Year Computer Science
------------------*/
//This script deals with the main menu screen that will direct to apropriate options
using UnityEngine;
using System.Collections;

public class MultiplayerMenu : MonoBehaviour {
	private bool showMenu  = false;
	private Rect myWindowRect;
	private  MainMenuScript  MainMenu;

	void Awake(){
		myWindowRect  =new  Rect (Screen.width/2-150,Screen.height/2-100,300,200);	
		MainMenu = GetComponent<MainMenuScript>();
	}
	
	
	//void Start(){
		//mainMenuScript =  MainMenu.SP;
	//}
	
	
	public void EnableMenu(){
		showMenu=true;
	}
	
	void OnGUI ()
	{		
		if(!showMenu){
			return;
		}
		myWindowRect = GUILayout.Window (0, myWindowRect, windowGUI, "Multiplayer");			
	}
	
	
	void windowGUI(int id ){
		GUI.color = Color.yellow;
		GUILayout.BeginVertical();
		GUILayout.Space(10);
		GUILayout.EndVertical();
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(10);	
		GUILayout.Label("");
		GUILayout.Space(10);
		GUILayout.EndHorizontal();	
		//these statments are main interface
		if(GUI.Button(new Rect(50,20,200,20), "Single Player New Game")){
			showMenu=false;
			MainMenu.OpenMenu("singleplayer-new");//send mainmenu parameters to open single player
		}
		if(GUI.Button(new Rect(50,50,200,20), "Single Player Load Game")){
			showMenu=false;
			MainMenu.OpenMenu("singleplayer-load");//goes to available saves screen
		}
		if(GUI.Button(new Rect(50,80,200,20), "Host a game")){
			showMenu=false;
			MainMenu.OpenMenu("multiplayer-host");//goes to hosting iptions
		}
		
		if(GUI.Button(new Rect(50,110,200,20), "Select a game to join")){
			showMenu=false;
			MainMenu.OpenMenu("multiplayer-join");//allows to join available games
		}
		
		if(GUI.Button(new Rect(50,140,200,20), "Random Join")){
			showMenu=false;
			MainMenu.OpenMenu("multiplayer-quickplay");//will join first available game
		}
		
		if(GUI.Button(new Rect(50,170,200,20), "Exit game")){
			showMenu=false;//exit game option
			Application.Quit();
		}
}
}
