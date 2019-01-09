using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Simple NPC that can be talked to
 * Child object is a marker showing the player they can interact
 */
public class DialogueNPC : InteractableAbstract {

	[System.Serializable]
	public class DialogueData : DataAbstract {
		public int index;
	}
	public DialogueData data = new DialogueData();
	public Saver saver;

	private SpriteRenderer marker;

	public DialogueEvent[] dialogue;
	private int index = 0;
	public bool repeatLastDialogue;

	void Awake() {
		saver = GetComponent<Saver>();
	}

	void Start() {
		marker = transform.GetChild(0).GetComponent<SpriteRenderer>();
		marker.gameObject.layer = LayerMask.NameToLayer("World Space UI");
		marker.enabled = false;

		if (saver != null) {
			DialogueData d = (DialogueData) saver.GetData();
			if (d != null) {
				index = d.index;
			}
		}
		
	}

	override public void GainFocus() {
		marker.enabled = true;
	}

	override public void LoseFocus() {
		marker.enabled = false;
	}

	override public bool CanInteract() {
		return PlayerController.instance.grounded && 
			   PlayerController.instance.state != PlayerController.State.melee &&
			   (repeatLastDialogue || index < dialogue.Length);
	}

	override public void Interact() {
		if (index < dialogue.Length) {
			GameManager.instance.eventManager.StartEventSequence(dialogue[index]);
			index++;
			UpdateData();
		} else if(repeatLastDialogue) {
			GameManager.instance.eventManager.StartEventSequence(dialogue[dialogue.Length - 1]);
		}
	}

	public void UpdateData() {
		data.index = index;
		if (saver != null) {
			saver.SetData(data);
		}
	}
}
