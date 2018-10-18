using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/*
 * Main Camera Controller, persits between scenes
 */
public class MainCameraController : MonoBehaviour {

	public static MainCameraController instance = null;
	
	[HideInInspector] public Camera mainCamera;
	[HideInInspector] public CinemachineBrain brain;

	public GameObject defaultVirtualCameraPrefab;
	public CinemachineVirtualCamera defaultVirtualCamera;

	private Vector3 cameraOffset = new Vector3(0,0,-10);

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);    
		}
		DontDestroyOnLoad(gameObject);
		mainCamera = GetComponent<Camera>();
		brain = GetComponent<CinemachineBrain>();
	}

	void Start() {
		// Instantiate the default Virtual Camera
		GameObject g = Instantiate(defaultVirtualCameraPrefab, transform.position, Quaternion.identity);
		CinemachineVirtualCamera vc = g.GetComponent<CinemachineVirtualCamera>();
		vc.Follow = PlayerController.instance.transform;
		vc.Priority = 0;
		defaultVirtualCamera = vc;
		DontDestroyOnLoad(g);
	}

	public void Refresh() {
		//Create a new default virtual camera centered on the player, destroy the previous one
		Vector3 targetPos = PlayerController.instance.transform.position + cameraOffset;
		GameObject g = Instantiate(defaultVirtualCameraPrefab, targetPos, Quaternion.identity);
		CinemachineVirtualCamera vc = g.GetComponent<CinemachineVirtualCamera>();
		vc.Follow = PlayerController.instance.transform;
		vc.Priority = 0;

		Destroy(defaultVirtualCamera.gameObject);
		defaultVirtualCamera = vc;
		DontDestroyOnLoad(g);
	
	}

	public void HideWorldSpaceUI() {
		mainCamera.cullingMask &=  ~(1 << LayerMask.NameToLayer("World Space UI"));
	}

	public void ShowWorldSpaceUI() {
		mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("World Space UI");
	}
}
