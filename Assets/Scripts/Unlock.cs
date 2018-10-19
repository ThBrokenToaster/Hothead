using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Represents an unlockable item, such as a powerup
 */
[System.Serializable]
public class Unlock {
	public string title;
	public bool enabled;

	public Unlock(string t, bool e) {
		title = t;
		enabled = e;
	}

	public Unlock(string t) {
		title = t;
		enabled = true;
	}
}
