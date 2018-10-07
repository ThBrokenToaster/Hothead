using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Main Camera Controller, persits between scenes
 */
public class MainCameraController : MonoBehaviour {

	public static MainCameraController instance = null;
	
	private Camera mainCamera;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);    
		}
		//DontDestroyOnLoad(gameObject);
		mainCamera = GetComponent<Camera>();
	}

	public void Refresh() {
		GetComponent<CameraFollow2DPlat>().Refresh();
	}

	public void HideWorldSpaceUI() {
		mainCamera.cullingMask &=  ~(1 << LayerMask.NameToLayer("World Space UI"));
	}

	public void ShowWorldSpaceUI() {
		mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("World Space UI");
	}
}
