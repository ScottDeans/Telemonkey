/*----------------
 Scott Deans-1700147
 GrantMacewan University
 4th Year Computer Science
------------------*/
using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{

	public GUISkin skin;
	
	private float gldepth = -0.5f;
	private float startTime = 0.1f;
	private bool ison=false;
	public Material mat;
	
	private long tris = 0;
	private long verts = 0;
	private float savedTimeScale;
	
	private float levelTime;
	private string nextLevel;

	private bool showfps;
	private bool triangles;
	private bool Vertices;

	public Color lowFPSColor = Color.red;
	public Color highFPSColor = Color.cyan;
	public Color statColor = Color.yellow;
	public int lowFPS = 30;
	public int highFPS = 50;
	
	public GameObject start;

	public enum MenuBar {
		None,Save,Main,Options,Quit,Restart,Home,Complete,GameOver
	}
	
	private MenuBar currentPage;
	
	private float[] fpsarray;
	private float fps;
	
	private int toolbarInt = 0;
	private string[]  toolbarstrings =  {"Audio","Graphics","Stats","System"};
	
	
	void Start() {
		fpsarray = new float[Screen.width];
		Time.timeScale = 1;
		//PauseGame();
	}
	//scrolling threw various fps
	void ScrollFPS() {
		for (int x = 1; x < fpsarray.Length; ++x) {
			fpsarray[x-1]=fpsarray[x];
		}
		if (fps < 1000) {
			fpsarray[fpsarray.Length - 1]=fps;
		}
	}

	//show fps if activated and uses escape to pause menu thus calling up menu
	void LateUpdate () {
		if (showfps ) {
			FPSUpdate();
		}
		
		if (Input.GetKeyDown("escape")) 
		{

			switch (currentPage) 
			{
			case MenuBar.None: 
				PauseGame(); 
				break;
				
			case MenuBar.Main: 
				if (!IsBeginning()) 
					UnPauseGame(); 
				break;
				
			case MenuBar.Complete:
				break;
				
			case MenuBar.GameOver:
				break;
				
			default: 

				currentPage = MenuBar.Main;
				break;
			}
		}
	}
	//checks if menu is called and if game is paused which is started by esc will bring up the options
	void OnGUI () {
		if (!ison) {
			//return;
				}
		if (skin != null) {
			GUI.skin = skin;
		}
		ShowStatNums();
		if (IsGamePaused()) {
			GUI.color = statColor;
			switch (currentPage) {
			case MenuBar.Restart: Restart(); break;
			case MenuBar.Save: Save(); break;
			case MenuBar.Main: MainPauseMenu(); break;
			case MenuBar.Options: ShowToolbar(); break;
			case MenuBar.Quit: Quit(); break;
			case MenuBar.Home: Home(); break;
			case MenuBar.Complete: Complete(); break;
			case MenuBar.GameOver: GameOverMenu(); break;
			}
		}   
	}
	//save name and sets current state which saves it to be called bu saveload.save, and can be recovered saveload.load
	string saveNameInput="";
	void Save(){
		MenuStart(300,300);


		saveNameInput = GUILayout.TextField(saveNameInput,15);
		if (saveNameInput.Length >= 1) {
						if (GUILayout.Button ("Save")) {
								Game.current = new Game ();
								Game.current.SaveName = saveNameInput;
								SaveLoad.Save ();
								currentPage = MenuBar.None;
						}

				}else{
				GUILayout.Label("Enter a name to continue...");
			}
		MenuEnd ();

		}
	//depending on the coption clicksed that is available in toolbar will send to the correct options
	void ShowToolbar() {
		MenuStart(300,300);
		toolbarInt = GUILayout.Toolbar (toolbarInt, toolbarstrings);
		switch (toolbarInt) {
		case 0: VolumeControl(); break;
		case 3: ShowComputerSpecs(); break;
		case 1: GraphicalQualities(); GraphicalQualityControl(); break;
		case 2: StatControl(); break;
		}
		MenuEnd();
	}
	//when called returns to mai nscreen
	void Home() {
		Application.LoadLevel("Main Menu");
	}
	//when called quits application
	void Quit() {
		Application.Quit ();
	}
	//return to main menu
	void ShowBackButton() {
		if (GUI.Button(new Rect(20, Screen.height - 50, 50, 20),"Back")) {
			currentPage = MenuBar.Main;
		}
	}
	//displays computer specs
	void ShowComputerSpecs() {
		GUILayout.Label("Unity player version "+Application.unityVersion);
		GUILayout.Label("Graphics: "+SystemInfo.graphicsDeviceName+" "+
		                SystemInfo.operatingSystem+"\n"+
		                SystemInfo.graphicsMemorySize+"MB\n"+
		                SystemInfo.graphicsDeviceVersion+"\n"+
		                SystemInfo.graphicsDeviceVendor);

		GUILayout.Label("Shadows: "+SystemInfo.supportsShadows);
		GUILayout.Label("Image Effects: "+SystemInfo.supportsImageEffects);
		GUILayout.Label("Render Textures: "+SystemInfo.supportsRenderTextures);
	}
	
	void GraphicalQualities() {
		switch (QualitySettings.currentLevel) 
		{
		case QualityLevel.Fastest:
			GUILayout.Label("Fastest");
			break;
		case QualityLevel.Fast:
			GUILayout.Label("Fast");
			break;
		case QualityLevel.Simple:
			GUILayout.Label("Simple");
			break;
		case QualityLevel.Good:
			GUILayout.Label("Good");
			break;
		case QualityLevel.Beautiful:
			GUILayout.Label("Beautiful");
			break;
		case QualityLevel.Fantastic:
			GUILayout.Label("Fantastic");
			break;
		}
	}
	//function to set  the quality of graphics quality
	void GraphicalQualityControl() {
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Decrease")) {
			QualitySettings.DecreaseLevel();
		}
		if (GUILayout.Button("Increase")) {
			QualitySettings.IncreaseLevel();
		}
		GUILayout.EndHorizontal();
	}
	//reduces volume in area
	void VolumeControl() {
		GUILayout.Label("Volume");
		AudioListener.volume = GUILayout.HorizontalSlider(AudioListener.volume, 0, 1);
	}
	//controlls the toggle if activeated displays info
	void StatControl() {
		GUILayout.BeginHorizontal();
		showfps = GUILayout.Toggle(showfps,"FPS");
		triangles = GUILayout.Toggle(triangles,"Triangles");
		Vertices = GUILayout.Toggle(Vertices,"Vertices");
		GUILayout.EndHorizontal();
	}
	
	void FPSUpdate() {
		float delta = Time.smoothDeltaTime;
		if (!IsGamePaused() && delta !=0.0) {
			fps = 1 / delta;
		}
	}
	//will display gotten fps , vertices, and triangle numbers on top right screen
	void ShowStatNums() {
		GUILayout.BeginArea( new Rect(Screen.width - 100, 10, 100, 200));
		if (showfps) {
			string fpsstring= fps.ToString ("#,##0 fps");
			GUI.color = Color.Lerp(lowFPSColor, highFPSColor,(fps-lowFPS)/(highFPS-lowFPS));
			GUILayout.Label (fpsstring);
		}
		if (triangles || Vertices) {
			GetObjectStats();
			GUI.color = statColor;
		}
		if (triangles) {
			GUILayout.Label (tris+"tri");
		}
		if (Vertices) {
			GUILayout.Label (verts+"vtx");
		}
		GUILayout.EndArea();
	}
	//sets gui screen settings for menu
	void MenuStart(int width, int height) {
		if (skin != null) {
			GUI.skin = skin;
		}
		GUI.skin.label.alignment = TextAnchor.UpperCenter;
		GUILayout.BeginArea( new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));
		if (currentPage == MenuBar.Main) {
			GUI.Box (new Rect (0, 0, 200, 180), "Paused");
			GUILayout.Space (25);
			GUILayout.BeginHorizontal ();
			GUILayout.Space (25);
			GUILayout.EndHorizontal ();
		}
		if (currentPage == MenuBar.Options) {
			GUI.Box (new Rect (0, 0, 400, 350), "options");
			GUILayout.Space (45);
			GUILayout.BeginHorizontal ();
			GUILayout.Space (45);
			GUILayout.EndHorizontal ();
		}
		if (currentPage == MenuBar.Save) {
			GUI.Box (new Rect (0, 0, 350, 100), "Save");
			GUILayout.Space (45);
			GUILayout.BeginHorizontal ();
			GUILayout.Space (45);
			GUILayout.EndHorizontal ();
		}
		if (currentPage == MenuBar.Complete) {
			GUI.Box (new Rect (0, 0, 250, 125), "Success!");
			if(levelTime < 60){
				GUI.Label(new Rect (0, 25, 250, 50),"You completed the level in " + (int)(levelTime % 60)+" seconds!");
			}
			else if(levelTime % 60 < 10){
				GUI.Label(new Rect (0, 25, 250, 50),"You completed the level in " +
					(int)(levelTime / 60) + ":0" + (int)(levelTime % 60) + "!");
			}
			else{
				GUI.Label(new Rect (10, 20, 250, 50),"You completed the level in " +
					(int)(levelTime / 60) + ":" + (int)(levelTime % 60) + "!");
			}
			GUILayout.Space (70);
			GUILayout.BeginHorizontal ();
			GUILayout.Space (20);
			GUILayout.EndHorizontal ();
		}
		if (currentPage == MenuBar.GameOver) {
			GUI.Box (new Rect (0, 0, 250, 125), "Game Over");
			GUI.Label(new Rect (0, 25, 250, 50),"Would you like to retry?");
			GUILayout.Space (70);
			GUILayout.BeginHorizontal ();
			GUILayout.Space (20);
			GUILayout.EndHorizontal ();
		}
	}
	//ends gui and adds back button
	void MenuEnd() {
		GUILayout.EndArea();
		if (currentPage != MenuBar.Main && currentPage != MenuBar.Complete && currentPage != MenuBar.GameOver) {
			ShowBackButton();
		}
	}
	
	bool IsBeginning() {
		return (Time.time < startTime);
	}
	
	
	void MainPauseMenu() {

		MenuStart(200,200);
		if (GUILayout.Button (IsBeginning() ? "Resume" : "Continue")) {
			UnPauseGame();	
		}
		if (GUILayout.Button ("Restart")) {
			Restart();
		}
		if (GUILayout.Button ("Saves")) {
			currentPage = MenuBar.Save;
		}

		if (GUILayout.Button ("Options")) {
			currentPage = MenuBar.Options;
		}
		if (GUILayout.Button ("Quit Level")) {
			currentPage = MenuBar.Home;
		}
		if (GUILayout.Button ("Quit Program")) {
			Application.Quit();
		}
		MenuEnd();
	}
	//this function gets all objects available and starts the check for certicies and triangles
	void GetObjectStats() {
		verts = 0;
		tris = 0;
		GameObject[] ob = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject obj in ob) {
			GetObjectStats(obj);
		}
	}
	//this function will do the math of the gotten components to determine triangles and vertices
	void GetObjectStats(GameObject obj) {
		Component[] filters;
		filters = obj.GetComponentsInChildren<MeshFilter>();
		foreach( MeshFilter f  in filters )
		{
			tris += f.sharedMesh.triangles.Length/3;
			verts += f.sharedMesh.vertexCount;
		}
	}
	
	void SetNextLevel(string next){
		nextLevel = next;
	}
	
	void LevelComplete(float completedTime){
		PauseGame();
		currentPage = MenuBar.Complete;
		levelTime = completedTime;
	}
	
	void Complete(){
		MenuStart(250,200);
		if (GUILayout.Button ("Retry")) {
			Restart();
		}
		if(nextLevel != null){
			if (GUILayout.Button ("Next Level")) {
				UnPauseGame();
				Application.LoadLevel(nextLevel);
			}
		}
		else{
			if (GUILayout.Button ("Main Menu")) {
				currentPage = MenuBar.Home;
			}
		}
		MenuEnd();
	}
	
	void GameOver(){
		PauseGame();
		currentPage = MenuBar.GameOver;
	}
	
	void GameOverMenu(){
		MenuStart(250,200);
		if (GUILayout.Button ("Retry")) {
			Restart();
		}
		else{
			if (GUILayout.Button ("Main Menu")) {
				currentPage = MenuBar.Home;
			}
		}
		MenuEnd();
	}
	
	void Restart(){
		UnPauseGame();
		Application.LoadLevel(Application.loadedLevel);
	}
	//pauseGame function sets timescale to 0 to pause
	void PauseGame() {
		savedTimeScale = Time.timeScale;
		Time.timeScale = 0;
		//AudioListener.pause = true;
		BroadcastMessage("PauseAll", SendMessageOptions.DontRequireReceiver);
		currentPage = MenuBar.Main;
	}
	//pauseGame function resets timescale previous timescale
	void UnPauseGame() {
		Time.timeScale = savedTimeScale;
		//AudioListener.pause = false;
		BroadcastMessage("UnpauseAll", SendMessageOptions.DontRequireReceiver);
		currentPage = MenuBar.None;
		ison=!ison;
		if (IsBeginning() && start != null) {
			start.SetActive(true);
		}
	}
	
	bool IsGamePaused() {
		return (Time.timeScale == 0);
	}
	
	void OnApplicationPause(bool pause) {
		if (IsGamePaused()) {
			AudioListener.pause = true;
		}
	}
}

