/*----------------
 Scott Deans-1700147
 GrantMacewan University
 4th Year Computer Science
 deals with joining a menu from player
------------------*/
using UnityEngine;
using System.Collections;

public class JoinMenu : MonoBehaviour {


	public GUISkin skin;
	
	public string gameName = "TeleMonkey";
	public int serverPort = 45671;
	
	private float timeoutHostList = 0.0f;
	private float lastHostListRequest = -1000.0f;
	private float hostListRefreshTimeout = 60.0f;
	
	private Rect windowRect1;
	private Rect windowRect2;
	
	static bool quickplayMode  = false;
	static float quickplayModeStarted   = 0.0f;
	private bool tryingToConnectquickplay  = false;
	private int tryingToConnectquickplayNumber = 0;
	private bool readyToConnect  = false;
	
	private int[] remotePort = new int[3];
	private string[] remoteIP  = new string[3];
	private string connectionInfo  = "";
	
	private bool lastMSConnectionAttemptForcedNat = false;
	private bool NAToptionWasSwitchedForTesting  = false;
	static public bool officialNATstatus  = false;//Network.useNat;
	private string errorMessage  = "";
	private float lastquickplayConnectionTime;
	

	private float lastQuickPlayConnectionTime;
	
	public GUIStyle customButton;
	
	private bool showMenu = false;
	public string password = "";
	private  MainMenuScript  MainMenu;
	
	/////////////////////////////

	void Awake(){
		windowRect1 =new Rect (Screen.width/2-305,Screen.height/2-140,380,280);
		windowRect2 =new Rect (Screen.width/2+110,Screen.height/2-140,220,100);
		MainMenu = GetComponent<MainMenuScript>();

	}

	//enables joining menu
	public void EnableMenu(bool quickplay ){
		showMenu=true;
		tryingToConnectquickplayNumber = 0;
	
		tryingToConnectquickplay=false;
		
		quickplayMode=quickplay;
		
		if(quickplay){		
			quickplayModeStarted=Time.time;
		}
		
		MasterServer.RequestHostList (gameName);
		lastHostListRequest = Time.realtimeSinceStartup;
		
		remoteIP[0]="127.0.0.1";//sets ip
		remotePort[0]=serverPort;
	}
	
	
	void OnFailedToConnectToMasterServer(NetworkConnectionError info)
	{
		//Dont request the MS list the next 60 sec
		lastHostListRequest = Time.realtimeSinceStartup + 60;
	}
	
	void OnFailedToConnect(NetworkConnectionError info)
	{
		Debug.Log("Failed To Connect info: "+info);
		
		bool invalidPass  = false;
		if(info==NetworkConnectionError.InvalidPassword){
			invalidPass=true;
		}
		//FailedConnRetry(invalidPass);	
	}
	
	
	void FailedConnRetry(bool invalidPassword ){
		if(tryingToConnectquickplay){
			//Try again without NAT if we used NAT
			
			if(Network.useNat && lastMSConnectionAttemptForcedNat){
				Debug.Log("Failed 1A");
				
				lastMSConnectionAttemptForcedNat=false;
				Network.useNat=false;
				remotePort[0]=serverPort;//Fall back to default server port
				Network.Connect(remoteIP, remotePort[0]);
				lastquickplayConnectionTime=Time.time;
			}else{
				Debug.Log("Failed 1B");
				
				//Reset NAT to org value
				Network.useNat=officialNATstatus;
				
				tryingToConnectquickplayNumber++;
				tryingToConnectquickplay=false;
			}
		}else{
			
			//Direct connect or host list
			connectionInfo="Failed to connect!";
			
			if(Network.useNat && lastMSConnectionAttemptForcedNat){
				Debug.Log("Failed 2A");
				
				Network.useNat=false;
				Network.Connect(remoteIP, remotePort[0]);
				lastquickplayConnectionTime=Time.time;
			}else{
				Debug.Log("Failed 2B");
				
				errorMessage="Failed to connect to "+remoteIP[0]+":"+remotePort[0];
				if(invalidPassword){
					errorMessage+="\nYou used the wrong password (or you didn't need to enter one!).";
				}
				//reset default port
				remotePort[0]=serverPort;
				
				//Reset nat to tested value
				Network.useNat=officialNATstatus;
				
			}
		}	
	}
	
	
	void OnConnectedToServer(){//pnce connected to a selected lobbyeneters lobby script
		Debug.Log("Connected to lobby!");
		showMenu=false;
		GameLobby gameLobbyScript  = GetComponent<GameLobby>();
		gameLobbyScript.EnableLobby();
		
	}
	
	void OnGUI ()
	{		
		if(!showMenu){
			return;
		}

		//Back to main menu
		if(GUI.Button(new Rect(40,10,150,20), "Back to main menu")){
			showMenu=false;
			MainMenu.OpenMenu("multiplayer");
		}
		

		if(errorMessage.Length!=0 ){	
			GUI.Box(new Rect(Screen.width/2-100,Screen.height/2-30,200,60), "Error");
			GUI.Label(new Rect(Screen.width/2-90,Screen.height/2-15,180,50), errorMessage);
			if(GUI.Button(new Rect(Screen.width/2+40,Screen.height/2+5,50,20), "Close")){
				errorMessage="";
			}
		}
		

		if(quickplayMode){//join a game
			quickplayFunction();
		}else{
			if(errorMessage.Length==0){ //Hide windows on error
				windowRect1 = GUILayout.Window (0, windowRect1, listGUI, "Join a game via the list");	
				windowRect2 = GUILayout.Window (1, windowRect2, directConnectGUIWindow, "Directly join a game via an IP");	
				//windowRect3 = GUILayout.Window (2, windowRect3, hostGUI, "Host a game");
			}	
		}
		
		
		
	}
	
	
	void quickplayFunction(){
		
		GUI.Box(new Rect(Screen.width/4+180,Screen.height/2-30,280,50), "");

		int i =0;
		HostData[] data   = MasterServer.PollHostList();
		foreach (HostData element in data)
		{
			// Do not display NAT enabled games if we cannot do NAT punchthrough
			if ( !(NATtester.filterNATHosts && element.useNat) && !element.passwordProtected )
			{
				//aHost=1;
				
				if(element.connectedPlayers<element.playerLimit)
				{					
					if(tryingToConnectquickplay){
						string natText;
						if(Network.useNat){
							natText=" with option 1/2";
						}else{
							natText=" with option 2/2";
						}
						GUI.Label(new Rect(Screen.width/4+200,Screen.height/2-25,285,50), "Trying to connect to host "+(tryingToConnectquickplayNumber+1)+"/"+data.Length+" "+natText);
						if(!Network.useNat && lastquickplayConnectionTime+0.75<=Time.time || lastquickplayConnectionTime+5.00<=Time.time){
							FailedConnRetry(false);							
						}
						return;	
					}		
					if(!tryingToConnectquickplay && tryingToConnectquickplayNumber<=i){
						Debug.Log("Trying to connect to game nr "+i+" & "+tryingToConnectquickplayNumber);
						tryingToConnectquickplay=true;
						tryingToConnectquickplayNumber=i;
						// Enable NAT functionality based on what the hosts if configured to do
						lastMSConnectionAttemptForcedNat=element.useNat;
						remoteIP=element.ip;
						remotePort[0]=element.port;
						Network.useNat = element.useNat;
						int connectPort =element.port;
						if (Network.useNat){
							print("Using Nat punchthrough to connect");
						}else{
							print("Connecting directly to host");
						}
						Debug.Log("connecting to "+element.gameName+" "+element.ip+":"+connectPort);
						Network.Connect(element.ip, connectPort);	
						lastquickplayConnectionTime=Time.time;
					}
					i++;		
				}
			}
			
		}
		
		//List is empty, d ont give up yet: Give MS 5 seconds to feed the list
		if(Time.time<quickplayModeStarted+5){
			//Debug.Log("PlayNow: delay up to 3 sec (no hosts yet)" );		
			GUI.Label(new Rect(Screen.width/4+200,Screen.height/2-25,280,50), "Waiting for masterserver..."+Mathf.Ceil((quickplayModeStarted+5)-Time.time) );
			return;	
		}
		
		if(!tryingToConnectquickplay){
			Debug.Log("PlayNow: No games hosted, so hosting one ourselves");			
			showMenu=false;
			GameLobby gameLobbyScript  = GetComponent<GameLobby>();
			gameLobbyScript.EnableLobby();	
		}
		
		
	}
	
	
	//public string password = "";
	//sets gui functions
	void directConnectGUIWindow(int id ){
	
		GUILayout.BeginVertical();
		GUILayout.Space(5);
		GUILayout.EndVertical();
		GUILayout.Label(connectionInfo);
		
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(10);
		remoteIP[0] = GUILayout.TextField(remoteIP[0], GUILayout.MinWidth(70));
		remotePort[0] =System.Convert.ToInt32(GUILayout.TextField(remotePort[0]+""));

		GUILayout.Space(10);
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(10);
		GUILayout.Label("Password");	
		password = GUILayout.TextField(password, GUILayout.MinWidth(50));
		GUILayout.Space(10);
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(10);
		Network.useNat = GUILayout.Toggle(Network.useNat, "Advanced: Use NAT");
		GUILayout.EndHorizontal();
		
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(10);
		GUILayout.FlexibleSpace();
		if (GUILayout.Button ("Connect"))
		{
			lastMSConnectionAttemptForcedNat=Network.useNat;
			connectionInfo="";
			Network.Connect(remoteIP, remotePort[0], password);
		}		
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
	}
	
	Vector2 scrollPosition;
	//sets gui settings for menu
	void listGUI (int id ) {
		GUI.color = Color.yellow;
		GUILayout.BeginVertical();
		GUILayout.Space(6);
		GUILayout.EndVertical();
		
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(200);
		
		// Refresh hosts
		if (GUILayout.Button ("Refresh available Servers") || Time.realtimeSinceStartup > lastHostListRequest + hostListRefreshTimeout)
		{
			if(Time.realtimeSinceStartup > lastHostListRequest + 5){//max once per 5 sec
				MasterServer.RequestHostList (gameName);
				lastHostListRequest = Time.realtimeSinceStartup;
			}
		}
		
		GUILayout.FlexibleSpace();
		
		GUILayout.EndHorizontal();
		
		
		
		scrollPosition = GUILayout.BeginScrollView (scrollPosition);
		
		int aHost =0;

		HostData[] data   = MasterServer.PollHostList();
		foreach (HostData element in data)
		{
			
			GUILayout.BeginHorizontal();

			// Do not display NAT enabled games if we cannot do NAT punchthrough
			if ( !(NATtester.filterNATHosts && element.useNat) )
			{
				aHost=1;
				string name = element.gameName + " ";

				GUILayout.Label(name);	
				GUILayout.FlexibleSpace();
				GUILayout.Label(element.connectedPlayers + "/" + element.playerLimit);
				
				if(element.useNat){
					GUILayout.Label(".");
				}
				GUILayout.FlexibleSpace();
				GUILayout.Label("[" + element.ip[0] + ":" + element.port + "]");	
				GUILayout.FlexibleSpace();
				
				if(element.passwordProtected){
					GUILayout.Label("PASSWORD");
					if (GUILayout.Button("Connect"))
					{
						if(password==""){
							errorMessage="You must enter a password if you want to join this game!";
							return;	
						}// Enable NAT functionality based on what the hosts if configured to do
						Network.useNat = element.useNat;
						lastMSConnectionAttemptForcedNat = element.useNat;
						if (Network.useNat){
							print("Using Nat punchthrough to connect");
						}else{
							print("Connecting directly to host");
						}
						remoteIP=element.ip;
						remotePort[0]=element.port;
						int connectPort2 =element.port;
						Network.Connect(element.ip, connectPort2, password);			
					}
					
				}else{
					if (GUILayout.Button("Connect"))
					{
						// Enable NAT functionality based on what the hosts if configured to do
						Network.useNat = element.useNat;
						lastMSConnectionAttemptForcedNat = element.useNat;
						if (Network.useNat){
							print("Using Nat punchthrough to connect");
						}else{
							print("Connecting directly to host");
						}
						remoteIP=element.ip;
						remotePort[0]=element.port;
						int connectPort =element.port;	
						Network.Connect(element.ip, connectPort);			
					}
				}
				GUILayout.Space(15);
			}
			GUILayout.EndHorizontal();	
		}
		if(aHost==0){
			GUILayout.Label("No games hosted at the moment..");
		}
		
		GUILayout.EndScrollView ();
	}

}
