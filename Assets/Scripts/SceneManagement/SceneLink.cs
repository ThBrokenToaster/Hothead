using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Marks where to connect seperate scenes
 * Needs triggers to call Load/Unload Scene
 */
public class SceneLink : MonoBehaviour {

	public string linkName;
	public string sceneToLoad;
	public string linkInSceneName;

	void Awake() {
		if (GameManager.linkName == linkName) {
			Vector3 offset = GameManager.sceneLink.transform.position - transform.position;
			foreach (GameObject g in gameObject.scene.GetRootGameObjects()) {
				g.transform.position += offset;
			}
			GameManager.linkName = null;
		}
	}

	public void LoadScene() {
		if (!GameManager.openScenes.Contains(sceneToLoad)) {

			GameManager.sceneLink = this;
			GameManager.linkName = linkInSceneName;
			GameManager.loadState = GameManager.LoadState.additive;

			SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
		}
	}

	public void UnloadScene() {
		if (GameManager.openScenes.Contains(sceneToLoad)) {
			SceneManager.UnloadSceneAsync(sceneToLoad);
		}
	}
}
