/*----------------
 Scott Deans-1700147
 GrantMacewan University
 4th Year Computer Science
------------------*/
using UnityEngine;
using System.Collections;

public class MultiSpawner : MonoBehaviour {
	public Transform playerPrefab;
	public Transform playerPrefab2;
	public int countPlayers;
	//public Transform[] PlayerCharacters;
	//public Transform[] spawnp;
	void OnEnable(){
		countPlayers=0;
		//gameObject.GetComponentsInChildren<Transform>();

	}

	void Awake(){
		int player = System.Int32.Parse (Network.player.ToString ());
		
		//Debug.Log (System.Int32.Parse (Network.player.ToString ()));
		//Network.Instantiate(PlayerCharacters[player],spawnp[].Pos), transform.rotation, 0);
		//Network.Instantiate(PlayerCharacters[player],new Vector3(0,0,0), transform.rotation, 0);
		//if ((Network.peerType != NetworkPeerType.Server)) {

		//if network is mine will instantiate for me
		if ((networkView.isMine)) {
			Network.Instantiate (playerPrefab, new Vector3 (0, 0, 0), transform.rotation, 0);
		}
	
		else{
			
			Network.Instantiate (playerPrefab2, new Vector3 (10, 0, 0), transform.rotation, 0);
		}
				//}
	}
	

	//void OnNetworkLoadedLevel()//alternate spawns
	//{
			//Network.Instantiate(playerPrefab,new Vector3(0,0,0), transform.rotation, 0);
	//}
//	void OnNetworkLoadedLevel2 ()
//	{
//		Network.Instantiate(playerPrefab2,new Vector3(30,0,0), transform.rotation, 0);
//	}
	void OnPlayerDisconnected (NetworkPlayer player)//if a network player disconects
	{
		Debug.Log("Server destroying player");
		Network.RemoveRPCs(player, 0);
		Network.DestroyPlayerObjects(player);
	}

}
