using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Controls a dialogue event, stores a collection of dialogue panels
 */
[System.Serializable]
public class DialogueEvent : EventAbstract {
	public DialoguePanel[] panels;
	private EventSequence seq;
	private int index;

	override public void BeginEvent(EventSequence callback) {
		seq = callback;
		index = 0;
		HUDController.instance.dialogue.ShowDialoguePanel(panels[0], this);
	}

	public void NextPanel() {
		index++;
		if (index < panels.Length) {
			HUDController.instance.dialogue.ShowDialoguePanel(panels[index], this);
		} else {
			EndEvent();
		}
	}

	override public void EndEvent() {
		HUDController.instance.dialogue.HideDialoguePanel();
		seq.NextEvent();
	}

}
