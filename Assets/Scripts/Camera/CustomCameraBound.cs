using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class CustomCameraBound : MonoBehaviour {

	private Vector3 cameraOffset = new Vector3(0,0,-10);
	
	public int priority = 1;
	public float cameraSize = 5;
	public CinemachineVirtualCamera virtualCamera;

	void Start() {
		virtualCamera.Follow = PlayerController.instance.transform;
		virtualCamera.Priority = -1;
		virtualCamera.enabled = false;
		virtualCamera.m_Lens.OrthographicSize = cameraSize;

		GameManager.instance.LateRefresh += CheckCollider;
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

	void OnDisable() {
		GameManager.instance.LateRefresh -= CheckCollider;
	}

	// Manually check if the player is in the collider (when TimeScale = 0)
	public void CheckCollider() {
		Collider2D[] l = new Collider2D[1];
		ContactFilter2D filter2D = new ContactFilter2D();
		filter2D.layerMask = LayerMask.NameToLayer("Player");
		filter2D.useLayerMask = true;
		Debug.Log(GetComponent<Collider2D>().OverlapCollider(filter2D, l));

		if (l[0].tag == "Player") {
			virtualCamera.Priority = priority;
			virtualCamera.enabled = true;
		} else {
			virtualCamera.Priority = -1;
			virtualCamera.enabled = false;
		}
	}

}
