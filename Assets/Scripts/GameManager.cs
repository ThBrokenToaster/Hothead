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
	private LoadState loadState = LoadState.firstLoad;
	private string loadToDoorName;

	void Awake() {
		// ensures only one GameManager can exist
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);    
		}
		DontDestroyOnLoad(gameObject);
	}

	// load new scene, w/ player starting at a certain door
	public void LoadSceneToDoor(string sceneToLoad, string doorName) {
		loadState = LoadState.loadingToDoor;
		loadToDoorName = doorName;
		SceneManager.LoadScene(sceneToLoad);
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
