using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Main Camera Controller, persits between scenes
 */
public class MainCameraController : MonoBehaviour {

	public static MainCameraController instance = null;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);    
		}
		DontDestroyOnLoad(gameObject);
	}

	public void Refresh() {
		GetComponent<CameraFollow2DPlat>().Refresh();
	}
}
