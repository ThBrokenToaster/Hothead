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
	public enum LoadState { firstLoad, loaded, loadingToDoor };
	public enum GameState { running, paused };

	private LoadState loadState = LoadState.firstLoad;
	private GameState gameState = GameState.running;

	private string loadToDoorName;
	private string loadToScene;
	
	public delegate void Event();

	void Awake() {
		// ensures only one GameManager can exist
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);    
		}
		DontDestroyOnLoad(gameObject);
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (gameState == GameState.running) {
				PauseGame(true);
			} else {
				UnPauseGame();
			}
		}
	}

	public void PauseGame(bool showPauseMenu) {
		gameState = GameState.paused;
		if (showPauseMenu) {
			UICanvasController.instance.ShowPauseMenu();
		}

		Time.timeScale = 0f;
	}

	public void UnPauseGame() {
		gameState = GameState.running;
		UICanvasController.instance.HidePauseMenu();

		Time.timeScale = 1f;
	}

	// load new scene, w/ player starting at a certain door
	public void LoadSceneToDoor(string sceneName, string doorName) {
		loadState = LoadState.loadingToDoor;
		loadToDoorName = doorName;
		loadToScene = sceneName;

		PauseGame(false);
		// Trigger fade out, load scene after fade
		UICanvasController.instance.TriggerFadeOut(LoadScene);
	}

	public void LoadScene() {
		SceneManager.LoadScene(loadToScene);
	}
	
	void OnEnable() {
      	SceneManager.sceneLoaded += OnSceneLoaded;
  	}
 
  	void OnDisable() {
    	SceneManager.sceneLoaded -= OnSceneLoaded;
  	}
 
	// Called when a new scene starts
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		// move player to door
		if (loadState == LoadState.loadingToDoor) {
			DoorController.FindDoor(loadToDoorName).MovePlayer();
			UICanvasController.instance.TriggerFadeIn(null);
		}

		// refresh unless its the first load
		if (loadState != LoadState.firstLoad) {
			Refresh();
		}

		loadState = LoadState.loaded;
		UnPauseGame();
	}

	// Refresh is called when the player is reloaded (resets player)
	public void Refresh() {
		PlayerController.instance.Refresh();
		MainCameraController.instance.Refresh();
	}
}
