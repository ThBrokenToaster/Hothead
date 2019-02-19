using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



/*
 * Global game managers, persists between scenes
 * Currently used to switch scenes
 */
public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public enum LoadState { firstLoad, loaded, loadingToDoor, additive };
	public enum GameState { running, pauseMenu, paused };
	public static LoadState loadState = LoadState.firstLoad;
	private static GameState gameState = GameState.running;

	public bool paused = false;
	private string loadToDoorName;
	private string loadToScene;

	public EventManager eventManager;
	public SaveManager saveManager;
	
	public delegate void Event();

	public event Event Refresh;
	public event Event LateRefresh;
	public event Event OnPause;
	public event Event OnUnpause;

	public static List<string> openScenes = new List<string>();
	public static string linkName;
	public static SceneLink sceneLink = null;

	void Awake() {
		// ensures only one GameManager can exist
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);    
		}
		DontDestroyOnLoad(gameObject);

		eventManager = GetComponent<EventManager>();
		saveManager = GetComponent<SaveManager>();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (gameState == GameState.running) {
				PauseGame(true);
			} else if (gameState == GameState.pauseMenu) {
				UnPauseGame();
			}
		}
	}

	public void PauseGame(bool showPauseMenu) {
		paused = true;
		
		if (showPauseMenu) {
			gameState = GameState.pauseMenu;
			HUDController.instance.ShowPauseMenu();
		} else {
			gameState = GameState.paused;
		}

		Time.timeScale = 0f;

		OnPause();
	}

	public void UnPauseGame() {
		gameState = GameState.running;
		paused = false;
		HUDController.instance.HidePauseMenu();

		Time.timeScale = 1f;

		OnUnpause();
	}

	// load new scene, w/ player starting at a certain door
	public void LoadSceneToDoor(string sceneName, string doorName) {
		loadState = LoadState.loadingToDoor;
		loadToDoorName = doorName;
		loadToScene = sceneName;

		PauseGame(false);
		// Trigger fade out, load scene after fade
		HUDController.instance.TriggerFadeOut(LoadScene);
	}

	// moves the player to a door within the same scene
	public void MoveToDoor(string doorName) {
		loadToDoorName = doorName;

		PauseGame(false);
		HUDController.instance.TriggerFadeOut(PostMoveToDoor);
	}

	// Moves player to door and triggers fade in
	public void PostMoveToDoor() {
		DoorController.FindDoor(loadToDoorName).MovePlayer();
		Refresh();
		HUDController.instance.TriggerFadeIn(UnPauseGame);
	}

	public void LoadScene() {
		SceneManager.LoadScene(loadToScene);
	}
	
	void OnEnable() {
      	SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.sceneUnloaded += OnSceneUnloaded;
  	}
 
  	void OnDisable() {
    	SceneManager.sceneLoaded -= OnSceneLoaded;
		SceneManager.sceneUnloaded -= OnSceneUnloaded;
  	}
 
	// Called when a new scene starts
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		Debug.Log("Scene loaded: " + scene.name);

		openScenes.Add(scene.name);

		// move player to door
		if (loadState == LoadState.loadingToDoor) {
			DoorController.FindDoor(loadToDoorName).MovePlayer();
			HUDController.instance.TriggerFadeIn(UnPauseGame);
		}

		// refresh unless its the first load
		if (loadState != LoadState.firstLoad && loadState != LoadState.additive) {
			// Everything that has subscribed to the event will be notified
			StartCoroutine(RefreshWithLate());
		}

		loadState = LoadState.loaded;
	}

	private void OnSceneUnloaded(Scene scene) {
		Debug.Log("Scene unloaded: " + scene.name);
		openScenes.Remove(scene.name);
	}

	IEnumerator RefreshWithLate() {
		if (Refresh != null) Refresh();
		yield return null;
		if (LateRefresh != null) LateRefresh();
	}
}
