/*----------------
 Scott Deans-1700147
 GrantMacewan University
 4th Year Computer Science
 this script deal with the introduction load
------------------*/
using UnityEngine;
using System.Collections;

public class MenuLoadingScreen : MonoBehaviour {
	public float barDisplay; //current progress
	public int timer=0;
	public Vector2 pos = new Vector2(50,50);
	public Vector2 size = new Vector2(50,50);
	public Texture LoadStart;
	public Texture loadStage1;
	public Texture loadStage2;
	public Texture loadStage3;
	public Texture loadStage4;
	public Texture loadStage5;
	private float LoadIng;
	void Start(){
		LoadIng = 5f;
		}

	void OnGUI() {
		
	//creates the load ball until full then loads main menu.
				switch (Mathf.CeilToInt (LoadIng)) {
				case 0:
						GUI.DrawTexture (new Rect ((Screen.width-150) / 2, (Screen.height-150) / 2, 150f, 60f), loadStage5);
						Application.LoadLevel (("Main Menu"));
						break;
				case 1:
						GUI.DrawTexture (new Rect ((Screen.width-150) / 2, (Screen.height-150) / 2, 150f, 60f), loadStage4);
						break;
				case 2:
						GUI.DrawTexture (new Rect ((Screen.width-150) / 2, (Screen.height-150) / 2, 150f, 60f), loadStage3);
						break;
				case 3:
						GUI.DrawTexture (new Rect ((Screen.width-150) / 2, (Screen.height-150) / 2, 150f, 60f), loadStage2);
						break;
				case 4:
						GUI.DrawTexture(new Rect ((Screen.width-150) / 2, (Screen.height-150) / 2, 150f, 60f), loadStage1);
						break;
				default:
						GUI.DrawTexture(new Rect ((Screen.width-150) / 2, (Screen.height-150) / 2, 150f, 60f), LoadStart);
						break;
				}
		}

	void Update() {

			if (LoadIng > 0) {
				LoadIng -= Time.smoothDeltaTime;
			
			}
			if(LoadIng <= 0) {
				LoadIng = 0f;
			}

		/*if (Application.CanStreamedLevelBeLoaded (("Main Menu"))) {
						GUI.Label (new Rect (Screen.width / 4 + 200, Screen.height / 2 - 25, 285, 150), "Loaded, starting the game!");
			Application.LoadLevel (("Main Menu"));
				} else {
						GUI.Label (new Rect (Screen.width / 4 + 200, Screen.height / 2 - 25, 285, 150), "Starting..Loading the game: " + Mathf.Floor (Application.GetStreamProgressForLevel (("Main Menu")) * 100) + " %");
				}*/
	}
}
