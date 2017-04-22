/*----------------
 Scott Deans-1700147
 GrantMacewan University
 4th Year Computer Science
------------------*/
using UnityEngine;
using System.Collections;

public class Player1Menu : MonoBehaviour {
	string errorMessage="";
	bool showMenu=false;
	private  MainMenuScript  MainMenu;

	private Rect windowRect1;
	private Rect windowRect2;
	public void awake(){
		windowRect1 =new Rect (Screen.width/2-305,Screen.height/2-140,380,280);
		windowRect2 =new Rect (Screen.width/2+110,Screen.height/2-140,220,100);
		MainMenu = GetComponent<MainMenuScript>();
		}
	public void Enablenewgame(){
		Application.LoadLevel("SLevel1");


	}

	public void Enableload(){
		showMenu = true;
	
	}
	void OnGUI ()
	{		
		if(!showMenu){
			return;
		}
		if(GUI.Button(new Rect(40,10,150,20), "Back to main menu")){
			showMenu=false;
			MainMenu = GetComponent<MainMenuScript>();
			MainMenu.OpenMenu("multiplayer");
		}
		GUILayout.BeginArea( new Rect((Screen.width - 200) / 2, (Screen.height - 200) / 2, 200, 200));
		//GUILayout.BeginArea(new Rect(0,0,Screen.width, Screen.height));
		SaveLoad.Load();
		
		GUI.Box(new Rect (0, 0, 400, 350),"Select Save File");
		GUILayout.Space(10);
		GUILayout.Space (25);
		GUILayout.BeginHorizontal ();
		GUILayout.Space (25);
		GUILayout.EndHorizontal ();
		foreach(Game g in SaveLoad.savedGames) {//displays list of cliacable saved games saved
			if(GUILayout.Button(g.SaveName+" - " + g.Level )) {
				//Move on to game...
				Application.LoadLevel(g.Level);
			}
			
		}
		GUILayout.EndArea();

		//Back to main menu


		
	}
}