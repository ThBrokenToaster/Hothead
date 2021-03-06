﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



/*
 * Global game managers, persists between scenes
 * Currently used to switch scenes
 */
public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	private enum LoadState { firstLoad, loaded, loadingToDoor };
	private enum GameState { running, pauseMenu, paused };
	private LoadState loadState = LoadState.firstLoad;
	private GameState gameState = GameState.running;

	public bool paused = false;
	private string loadToDoorName;
	private string loadToScene;

	public EventManager eventManager;
	
	public delegate void Event();

	void Awake() {
		// ensures only one GameManager can exist
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);    
		}
		DontDestroyOnLoad(gameObject);

		eventManager = GetComponent<EventManager>();
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

		MainCameraController.instance.HideWorldSpaceUI();
	}

	public void UnPauseGame() {
		gameState = GameState.running;
		paused = false;
		HUDController.instance.HidePauseMenu();

		Time.timeScale = 1f;

		MainCameraController.instance.ShowWorldSpaceUI();
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
  	}
 
  	void OnDisable() {
    	SceneManager.sceneLoaded -= OnSceneLoaded;
  	}
 
	// Called when a new scene starts
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		// move player to door
		if (loadState == LoadState.loadingToDoor) {
			DoorController.FindDoor(loadToDoorName).MovePlayer();
			HUDController.instance.TriggerFadeIn(UnPauseGame);
		}

		// refresh unless its the first load
		if (loadState != LoadState.firstLoad) {
			Refresh();
		}

		loadState = LoadState.loaded;
	}

	// Refresh is called when the player is reloaded (resets player)
	public void Refresh() {
		PlayerController.instance.Refresh();
		MainCameraController.instance.Refresh();
	}
}
