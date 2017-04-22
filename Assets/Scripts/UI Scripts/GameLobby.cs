/*----------------
 Scott Deans-1700147
 GrantMacewan University
 4th Year Computer Science
------------------*/
using UnityEngine;
using System;
using System.Collections;

public class GameLobby : MonoBehaviour {
	public int serverPort = 45671;
	public string gameName = "TeleMonkey";
	
	private bool launchingGame  = false;
	private bool showMenu  = false;
	private  ArrayList  playerList = new ArrayList();

	//private string[] playerList = new string();
	 class PlayerInfo {
		public string username ;
		public NetworkPlayer player ;

	}
	
	private int serverMaxPlayers  =4;
	private string serverTitle = "Loading..";
	private bool serverPasswordProtected  = false;
	
	private string playerName  = "";
	
	private MainMenuScript mainMenuScript ;
	
	public float lastRegTime  = -60f;

	void Awake(){
		showMenu=false;
		mainMenuScript = GetComponent<MainMenuScript>();
	}
	
	
	//void Start(){
		//mainMenuScript =  MainMenu.SP;
	//}
	
	
	public void EnableLobby(){
		playerName = PlayerPrefs.GetString("playerName");

		lastRegTime=Time.time-3600;

		launchingGame=false;
		showMenu=true;
		
		//LobbyChat chat : LobbyChat = GetComponent(LobbyChat);		
		//chat.ShowChatWindow();
	}
	
	
	void OnGUI () {
		if(!showMenu){
			return;
		}
		
		
		//Back to main menu
		if(GUI.Button(new Rect(40,10,150,20), "Back to main menu")){
			leaveLobby();
			showMenu=false;
			mainMenuScript.OpenMenu("multiplayer");
		}
		
		if(launchingGame){		
			launchingGameGUI();//starts calling game launch
			
		} else if(!Network.isServer && !Network.isClient){
			//First set player count, server name and password option			
			hostSettings();
			
		} else {
			//Show the lobby		
			showLobby();
		}
	}
	

	IEnumerator leaveLobby(){
		//Disconnect fdrom host, or shotduwn host
		showMenu=false;
		if (Network.isServer || Network.isClient){//if host is server or client to disconect will disconect from network
			if(Network.isServer){//if host however also deletes game
				MasterServer.UnregisterHost();
			}
			showMenu=false;
			Network.Disconnect();
			yield return new WaitForSeconds(0.3f);
		}	
		//showMenu=false;
	}
	
	
	private string hostSetting_title  = "Monkey Escape";
	private int hostSetting_players  = 4;
	private string hostSetting_password  = "";
	
	
	void hostSettings(){//allows host to sett the settings of the game
		
		GUI.BeginGroup (new Rect (Screen.width/2-175, Screen.height/2-75-50, 350, 150));
		GUI.Box (new Rect (0,0,350,150), "Server options");
		
		GUI.Label (new Rect (10,20,150,20), "Server title");
		hostSetting_title = GUI.TextField (new Rect (175,20,160,20), hostSetting_title);//title og game change
		
		GUI.Label (new Rect (10,40,150,20), "Max. players (1-4)");
		hostSetting_players = System.Int32.Parse(GUI.TextField (new Rect (175,40,160,20), hostSetting_players+""));//amount of players
		
		GUI.Label (new Rect (10,60,150,50), "Password\n");
		hostSetting_password = (GUI.TextField (new Rect (175,60,160,20), hostSetting_password));//if no password entered none needed
		
		
		if(GUI.Button (new Rect (100,115,150,20), "Go to lobby")){
			StartHost(hostSetting_password,(hostSetting_players), hostSetting_title);//goes to lobby hosts game
		}
		GUI.EndGroup();
	} 
	
	
	void StartHost(string password ,int players , string serverName ){
		if(players<1){
			players=1;//ensures if setting of players is not within bound defaults certain amount
		}
		if(players>=4){
			players=4;
		}
		if(password.Length==0 && password.Length==0){
			serverPasswordProtected  = true;
			Network.incomingPassword = password;
		}else{
			serverPasswordProtected  = false;
			Network.incomingPassword = "";
		}
		
		serverTitle = serverName;
		
		Network.InitializeSecurity();
		Network.InitializeServer((players-1), serverPort, true);	
	}
	
	
	public void showLobby(){
		string players = "";
		int currentPlayerCount  =0;

		foreach (PlayerInfo playerInstance  in playerList) {//lists all joined playerws
			players=playerInstance.username+"\n"+players;
			currentPlayerCount++;	
		}

		GUI.BeginGroup (new Rect (Screen.width/2-200, Screen.height/2-200, 400, 180));
	
		GUI.Box (new Rect (0,0,400,200), "Game lobby");
		
		//shows lobbby information and
		string pProtected="no";
		if(serverPasswordProtected){
			pProtected="yes";
		}
		GUI.Label (new Rect (10,20,150,20), "Password protected");
		GUI.Label (new Rect (150,20,100,100), pProtected);
		
		GUI.Label (new Rect (10,40,150,20), "Server title");
		GUI.Label (new Rect (150,40,100,100), serverTitle);
		
		GUI.Label (new Rect (10,60,150,20), "Players");
		GUI.Label (new Rect (150,60,100,100), currentPlayerCount+"/"+serverMaxPlayers);
		
		GUI.Label (new Rect (10,80,150,20), "Current players");
		GUI.Label (new Rect (150,80,100,100), players);
		
		
		if(Network.isServer){//when host hits launch launches game for both
			if(GUI.Button (new Rect (25,140,150,20), "Start the game")){
				HostLaunchGame();
			}
		}else{
			GUI.Label (new Rect (25,140,200,40), "Waiting for the server to start the game..");
		}
		
		GUI.EndGroup();
	}
	
	
	void OnConnectedToServer(){
		//Called on client
		//Send everyone this clients data
		playerList  = new ArrayList() ;
		playerName = PlayerPrefs.GetString("playerName");
		networkView.RPC("addPlayer",RPCMode.AllBuffered, Network.player, playerName);	
	}
	
	void OnServerInitialized(){
		//Called on host
		//Add hosts own data to the playerlist	
		playerList  = new ArrayList();
		networkView.RPC("addPlayer",RPCMode.AllBuffered, Network.player, playerName);
		
		
		bool pProtected  = false;
		if(Network.incomingPassword.Length!=0 && Network.incomingPassword.Length!=0){
			pProtected=true;
		}
		int maxPlayers = Network.maxConnections+1;
		
		networkView.RPC("setServerSettings",RPCMode.AllBuffered, pProtected, maxPlayers, hostSetting_title);
		
	}
	
	
	
	//float lastRegTime  = -60f;
	void Update(){
		if(Network.isServer && lastRegTime<Time.time-60){
			lastRegTime=Time.time;
			MasterServer.RegisterHost(gameName,hostSetting_title, "No description");
		}
	}
	
	
	[RPC]//sets title of server host for everyone to see
	void setServerSettings(bool password ,int  maxPlayers ,string newSrverTitle ){
		serverMaxPlayers = maxPlayers;
		serverTitle  = newSrverTitle;
		serverPasswordProtected  = password;
	}
	
	
	void OnPlayerDisconnected(NetworkPlayer player) {
		//Called on host
		//Remove player information from playerlist
		networkView.RPC("playerLeft", RPCMode.All, player);
		
		//var chat : LobbyChat = GetComponent(LobbyChat);
		//chat.addGameChatMessage("A player left the lobby");
	}
	
	
	[RPC]//adds a joined player to the remote procedure calls
	void addPlayer(NetworkPlayer player ,string username ){
		Debug.Log("got addplayer"+username);
		
		PlayerInfo playerInstance  = new PlayerInfo();
		playerInstance.player = player;
		playerInstance.username = username;		
		playerList.Add(playerInstance);

		
	}
	
	
	[RPC]//if a player left removes them from network
	void playerLeft(NetworkPlayer player ){
		
		PlayerInfo deletePlayer = new PlayerInfo();
		
		foreach (PlayerInfo playerInstance in playerList) {
			if (player == playerInstance.player) {			
				deletePlayer = playerInstance;
			}
		}
		playerList.Remove(deletePlayer); 
	
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}
	
	void HostLaunchGame(){
		if(!Network.isServer){
			return;
		}
		
		// Don't allow any more players
		Network.maxConnections = -1;
		MasterServer.UnregisterHost();	
		
		networkView.RPC("launchGame",RPCMode.All);
		
	}
	
	
	[RPC]//launches game for both players
	void launchGame(){
		Network.isMessageQueueRunning=false;
		launchingGame=true;
	}
	
	
	public void launchingGameGUI(){
		//Show loading progress, ADD LOADINGSCREEN?
		Debug.Log ("launchingGame");
		GUI.Box(new Rect(Screen.width/4+180,Screen.height/2-30,280,50), "");
		if(Application.CanStreamedLevelBeLoaded ((Application.loadedLevel+1))){
			GUI.Label(new Rect(Screen.width/4+200,Screen.height/2-25,285,150), "Loaded, starting the game!");
			Application.LoadLevel( (Application.loadedLevel+1) );

			// Allow receiving data again
			Network.isMessageQueueRunning = true;
			// Now the level has been loaded and we can start sending out data
			Network.SetSendingEnabled(0, true);

			// Notify our objects that the level and the network is ready
			//foreach (GameObject go in FindObjectsOfType( GameObject ))
			foreach (GameObject go in FindObjectsOfType( typeof(GameObject) ))
				go.SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);	
			
		}else{
			//loads game and displayes progress
			GUI.Label(new Rect(Screen.width/4+200,Screen.height/2-25,285,150), "Starting..Loading the game: "+Mathf.Floor(Application.GetStreamProgressForLevel((Application.loadedLevel+1))*100)+" %");
		}	
	}
	}