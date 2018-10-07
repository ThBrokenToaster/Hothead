using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour {
	public GameObject player;
	public GameObject playerCamera;
	public GameObject[] otherGlobals;
	
	
	public Vector3 cameraOffset = new Vector3(0, 0, -10);
	private static bool loaded = false;

	void Awake() {
		if(loaded) { 
			Destroy(gameObject);
		} else {
			loaded = true;
		}

		player = Instantiate(player, transform.position, Quaternion.identity);
		//playerCamera = Instantiate(playerCamera, transform.position + cameraOffset, Quaternion.identity);
		//playerCamera.GetComponent<CameraFollow2DPlat>().target = player.transform;

		foreach(GameObject obj in otherGlobals) {
			Instantiate(obj, transform.position, Quaternion.identity);
		}
		Destroy(gameObject);
	}
}
