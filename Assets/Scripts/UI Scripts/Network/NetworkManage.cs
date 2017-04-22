/*----------------
 Scott Deans-1700147
 GrantMacewan University
 4th Year Computer Science
script for futrure entrys
------------------*/
using UnityEngine;
//using UnityEngine.Network;
using System.Collections;

public class NetworkManage : MonoBehaviour {
	/*
	private bool isRefreshingHostList = false;
	private HostData[] hostList;
	private const string typeName = "UniqueGameName";
	private const string gameName = "TeleMonkey";
	public NetworkView networkView;   
	int lastLevelPrefix;
	//private void update(){
		//if(networkView.group==1){
			//Application.LoadLevel ("Co-opLevel1");
	//}
		//}

	public void start(){
		DontDestroyOnLoad (this);
		networkView = new NetworkView ();
		networkView.group = 1;
		}
	void Update()
	{
		if (isRefreshingHostList && MasterServer.PollHostList().Length > 0)
		{
			isRefreshingHostList = false;
			hostList = MasterServer.PollHostList();
		}
	}
	private void StartServer()
	{
		Network.InitializeServer(5, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
		//MasterServer.ipAddress = "127.0.0.1";
	}
	void OnServerInitialized()
	{
		Debug.Log("Server Initializied");
		Application.LoadLevel ("Co-opLevel1");
	}
	void StartConnection()
	{
		if (!Network.isClient && !Network.isServer)
		{
				StartServer();
				
		}
	}
	private void RefreshHostList()
	{
		if (!isRefreshingHostList)
		{
			isRefreshingHostList = true;
			MasterServer.RequestHostList(typeName);
		}
	}
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}
	void OnConnectedToServer()
	{
		Debug.Log("Server Joined");
		Application.LoadLevel ("Co-opLevel1");
		//networkView.RPC ("LoadLevel", RPCMode.All);
		//Application.LoadLevel ("Co-opLevel1");
		//Network.RemoveRPCsInGroup(0);
		//Network.RemoveRPCsInGroup(1);
		//networkView.RPC( "LoadLevel", RPCMode.AllBuffered, "Co-opLevel1",  1);


	
	}
	//void OnServerInitilzed(){
		//Application.LoadLevel ("Co-opLevel1");
	//}
	//void loadlevel(){
		//Application.LoadLevel ("Co-opLevel1");
		//}
	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}
	void QuitApplication() {
		Application.Quit();	
		Debug.Log ("3000");
	}
	void ReturnScreen(){
		GUI.Label(new Rect(10, 10, 100, 20), "2 player");
		Debug.Log ("Main");
		Application.LoadLevel ("Main Menu");
	}
}
	/*@RPC
	public void LoadLevel (string level , int levelPrefix )
	{
		//lastLevelPrefix = levelPrefix;
		// There is no reason to send any more data over the network on the default channel,
		// because we are about to load the level, thus all those objects will get deleted anyway
		//Network.SetSendingEnabled(0, false);    
		// We need to stop receiving because first the level must be loaded first.
		// Once the level is loaded, rpc's and other state update attached to objects in the level are allowed to fire
		//Network.isMessageQueueRunning = false;
		// All network views loaded from a level will get a prefix into their NetworkViewID.
		// This will prevent old updates from clients leaking into a newly created scene.
		//Network.SetLevelPrefix(levelPrefix);
		Application.LoadLevel(level);
		//yield;
		//yield;
		
		// Allow receiving data again
		//Network.isMessageQueueRunning = true;
		// Now the level has been loaded and we can start sending out data to clients
		//Network.SetSendingEnabled(0, true);
		
		
		//for (var go in FindObjectsOfType(GameObject))
			//go.SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver); 
	}*/
	/*[RPC]
	public void LoadLevel(string level, int levelPrefix)
	{
		StartCoroutine(loadLevel(level, levelPrefix));
	}
	
	private IEnumerator loadLevel(string level, int levelPrefix)
	{
		// omitted code
		
		Application.LoadLevel(level);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		
		// Allow receiving data again
		Network.isMessageQueueRunning = true;
		// Now the level has been loaded and we can start sending out data
		Network.SetSendingEnabled(0, true);
		
		// Notify our objects that the level and the network is ready
		foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
			go.SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);
	}
}*/
}