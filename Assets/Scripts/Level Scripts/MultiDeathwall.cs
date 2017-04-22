/*----------------
 Scott Deans-1700147
 GrantMacewan University
 4th Year Computer Science
 
------------------*/
using UnityEngine;
using System.Collections;

public class MultiDeathwall : MonoBehaviour {

	public bool launchingGame;
	
	void Start() {
		launchingGame = false;

	}
	void OnGUI () {
				if (launchingGame) {
						launchingGameGUI ();
				}
		}
	void OnTriggerStay2D(Collider2D target) {
		if (target.tag == "Player") {
						Restart ();
				}
	}
	[RPC]//launches game for both players
	void launchGame(){
		Network.isMessageQueueRunning=false;
		launchingGame=true;
	}
	void Restart(){
		networkView.RPC("launchGame",RPCMode.All);
		
	}
	public void launchingGameGUI(){
		//Show loading progress, ADD LOADINGSCREEN?
		Debug.Log ("launchingGame");
		GUI.Box(new Rect(Screen.width/4+180,Screen.height/2-30,280,50), "");
		if(Application.CanStreamedLevelBeLoaded ((Application.loadedLevel))){
			GUI.Label(new Rect(Screen.width/4+200,Screen.height/2-25,285,150), "Loaded, starting the game!");
			Application.LoadLevel( (Application.loadedLevel) );
			
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
			GUI.Label(new Rect(Screen.width/4+200,Screen.height/2-25,285,150), "Starting..Loading the game: "+Mathf.Floor(Application.GetStreamProgressForLevel((Application.loadedLevel))*100)+" %");
		}	
	}
}
