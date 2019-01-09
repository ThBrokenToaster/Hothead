using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Triggers attached SceneLink to unload connected scene
 */
public class AdditiveUnloadTrigger : MonoBehaviour {

	public SceneLink link;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			link.UnloadScene();
		}
	}
}
