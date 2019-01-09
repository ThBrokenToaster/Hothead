using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/*
 * Subcomponent of PlayerController managing unlockables
 */
public class PlayerUnlock : MonoBehaviour {

	private PlayerController player;
	public List<Unlock> unlocks = new List<Unlock>();

	void Awake() {
        player = GetComponent<PlayerController>();
    }
	
	public bool Has(string title) {
		return unlocks.Any(u => u.enabled && u.title == title);
	}

	public bool Contains(string title) {
		return unlocks.Any(u => u.title == title);
	}

	public void AddUnlock(string title) {
		AddUnlock(new Unlock(title, true));
	}

	public void AddUnlock(string title, bool enabled) {
		AddUnlock(new Unlock(title, enabled));
	}

	public void AddUnlock(Unlock u) {
		if (!Contains(u.title)) {
			unlocks.Add(u);
		}
	}

	public void SetUnlockEnable(string title, bool enabled) {
		unlocks.Where(u => u.title == title).First().enabled = enabled;
	}

}
