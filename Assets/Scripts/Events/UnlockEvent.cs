using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Event where the player unlocks something.
 * Currently triggers HUD animation with text
 */
[System.Serializable]
public class UnlockEvent : EventAbstract {

	public string title;
	public string unlockName;
	public bool apply = true;
	private EventSequence seq;

	override public void BeginEvent(EventSequence callback) {
		seq = callback;
		PlayerController.instance.unlock.AddUnlock(title, apply);
		HUDController.instance.unlock.ShowUnlockText(unlockName, this);
	}

	override public void EndEvent() {
		seq.NextEvent();
	}
}
