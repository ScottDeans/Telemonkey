/*----------------
 Scott Deans-1700147
 GrantMacewan University
 4th Year Computer Science
 saves names so players can have identiity, saves to computer ip address so if run on same computer will be the same
------------------*/
using UnityEngine;
using System;
using System.Collections;

public class MainMenuScript : MonoBehaviour
{

	//static var SP : MainMenu;
	private Player1Menu singleplayermenu;
	private JoinMenu joinMenuScript ;
	private GameLobby gameLobbyScript;
	private MultiplayerMenu multiplayerScript;
	Rect myWindowRect;
	private bool requirePlayerName  = false;
	private string playerNameInput  = "";
	private bool hasBeenPressed = false;
	void Awake(){
		
		playerNameInput = PlayerPrefs.GetString("playerName", "");
		requirePlayerName=true;
		
		myWindowRect = new Rect ();
		joinMenuScript = GetComponent<JoinMenu>();
		singleplayermenu = GetComponent<Player1Menu>();
		gameLobbyScript = GetComponent<GameLobby>();
		multiplayerScript = GetComponent<MultiplayerMenu>();	
	
		//OpenMenu("multiplayer");
	}

	
	
	void OnGUI(){
		if(requirePlayerName){
		
			myWindowRect =  GUILayout.Window (109, new Rect(Screen.width/2-150,Screen.height/2-100,300,100), NameMenu, "Please enter a name:");
			//myWindowRect =  GUILayout.Window (8, new Rect(Screen.width/2-150,Screen.height/2+100,200,200), NameMenu, "Please enter a name:");
			
		}
	}


	public  void OpenMenu(string newMenu ){
		Debug.Log (newMenu);// all the options on main screen when called goes to controller needed
		if(requirePlayerName){
			return;
		}
		if(newMenu=="singleplayer-new"){					
			singleplayermenu.Enablenewgame();
         }else if(newMenu=="singleplayer-load"){ 
			singleplayermenu.Enableload();		
			
		}else if(newMenu=="multiplayer-quickplay"){					
			joinMenuScript.EnableMenu(true);//quickplay=true	

		}else if(newMenu=="multiplayer-host"){ 
			gameLobbyScript.EnableLobby();		
			
		}else if(newMenu=="multiplayer-join"){ 
			joinMenuScript.EnableMenu(false);//quickplay:false
			
		}else if(newMenu=="multiplayer"){ 
			multiplayerScript.EnableMenu();
			
		}else{			
			Debug.LogError("Wrong menu:"+newMenu);	
			
		}
	}
	
	//open name entry for reconization
	void NameMenu(int id ){
		
		GUILayout.BeginVertical();
		GUILayout.Space(10);
		
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(10);
		GUILayout.Label("Please enter your name");
		GUILayout.Space(10);
		GUILayout.EndHorizontal();
		
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(10);
		playerNameInput = GUILayout.TextField(playerNameInput,15);
		GUILayout.Space(10);
		GUILayout.EndHorizontal();	
		

		
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(10);
		if(playerNameInput.Length>=1){
			if(GUILayout.Button("Save")){
				requirePlayerName=false;// eneters a playerpref name to stay consistent
				PlayerPrefs.SetString("playerName", playerNameInput);
				OpenMenu("multiplayer");
			}
		}else{
			GUILayout.Label("Enter a name to continue...");
		}
		GUILayout.Space(10);
		GUILayout.EndHorizontal();
		
		
		GUILayout.EndVertical();
		
	}
}

