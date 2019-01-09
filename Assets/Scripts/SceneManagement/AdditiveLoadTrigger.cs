using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Triggers attached SceneLink to load connected scene
 */
public class AdditiveLoadTrigger : MonoBehaviour {

	public SceneLink link;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			link.LoadScene();
		}
	}
}
