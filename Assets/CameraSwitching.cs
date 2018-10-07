using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitching : MonoBehaviour {

	public CinemachineVirtualCamera fromCamera;
	public CinemachineVirtualCamera toCamera;


	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("Switch");
		if (other.tag == "Player") {
			Debug.Log("Yeet");
			toCamera.enabled = true;
			fromCamera.enabled = false;
		}
	}
}
