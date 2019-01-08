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
	[HideInInspector] public CinemachineVirtualCamera defaultVirtualCamera;

	private Vector3 cameraOffset = new Vector3(0,0,-10);

	private CinemachineBlendDefinition defaultBlendTemp;

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

		// Subscribe to game manager's OnSceneChange
        GameManager.instance.Refresh += Refresh;
		GameManager.instance.OnPause += OnPause;
		GameManager.instance.OnUnpause += OnUnpause;
	}

	

	public void Refresh() {
		//Create a new default virtual camera centered on the player, destroy the previous one
		Vector3 targetPos = PlayerController.instance.transform.position + cameraOffset;
		GameObject newDefaultCamera = Instantiate(defaultVirtualCameraPrefab, targetPos, Quaternion.identity);
		DontDestroyOnLoad(newDefaultCamera);

		CinemachineVirtualCamera vc = newDefaultCamera.GetComponent<CinemachineVirtualCamera>();
		vc.Follow = PlayerController.instance.transform;
		vc.Priority = 0;

		Destroy(defaultVirtualCamera.gameObject);
		defaultVirtualCamera = vc;
	}

	public void ApplyShake(float timeout, float intensity) {
		CinemachineVirtualCamera vcam = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
		CinemachineBasicMultiChannelPerlin noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		StartCoroutine(ShakeCoroutine(noise, timeout, intensity));
	}

	IEnumerator ShakeCoroutine(CinemachineBasicMultiChannelPerlin noise, float timeout, float intensity) {
		SetNoise(noise, intensity);
		yield return new WaitForSeconds(timeout);
		SetNoise(noise, 0);
	} 

	private void SetNoise(CinemachineBasicMultiChannelPerlin noise, float intensity) {
		noise.m_AmplitudeGain = intensity * .2f;
		noise.m_FrequencyGain = intensity * 2f;
	}

	public void OnPause() {
		HideWorldSpaceUI();

		// Set default camera mode to Cut
		defaultBlendTemp = brain.m_DefaultBlend;
		brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
	}

	public void OnUnpause() {
		ShowWorldSpaceUI();

		// Restore default camera mode
		brain.m_DefaultBlend = defaultBlendTemp;
	}

	public void HideWorldSpaceUI() {
		mainCamera.cullingMask &=  ~(1 << LayerMask.NameToLayer("World Space UI"));
	}

	public void ShowWorldSpaceUI() {
		mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("World Space UI");
	}
}
