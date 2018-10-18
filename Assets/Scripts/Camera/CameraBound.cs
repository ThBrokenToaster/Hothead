using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBound : MonoBehaviour {

	private Vector3 cameraOffset = new Vector3(0,0,-10);
	
	public int priority = 1;
	[HideInInspector] public CinemachineVirtualCamera virtualCamera;
	public GameObject virtualCameraPrefab;

	void Start() {
		GameObject g = Instantiate(virtualCameraPrefab, transform.position, Quaternion.identity, transform);
		virtualCamera = g.GetComponent<CinemachineVirtualCamera>();


		virtualCamera.transform.position = cameraOffset;
		virtualCamera.Follow = PlayerController.instance.transform;
		virtualCamera.Priority = -1;
		virtualCamera.enabled = false;

		CinemachineConfiner confine = virtualCamera.GetComponent<CinemachineConfiner>();
		confine.m_ConfineMode = CinemachineConfiner.Mode.Confine2D;
		confine.m_BoundingShape2D = GetComponent<Collider2D>();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			virtualCamera.Priority = priority;
			virtualCamera.enabled = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			virtualCamera.Priority = -1;
			virtualCamera.enabled = false;
		}
	}

}
