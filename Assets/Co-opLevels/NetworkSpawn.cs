using UnityEngine;
using System.Collections;

public class NetworkSpawn : MonoBehaviour {
	public GameObject playerPrefab;
	// Use this for initialization
	void Start () {
		SpawnPlayer ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	private void SpawnPlayer()
	{
		//if(networkView.ismine) {
		//	Network.Instantiate (playerPrefab, Vector3.up * 5, Quaternion.identity, 0);
		//}
	}
}
