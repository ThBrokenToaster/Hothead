using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Simple NPC that can be talked to
 * Child object is a marker showing the player they can interact
 */
public class DialogueNPC : InteractableAbstract {
	private SpriteRenderer marker;

	public DialogueEvent[] dialogue;
	private int index = 0;
	public bool repeatLastDialogue;


	void Start() {
		marker = transform.GetChild(0).GetComponent<SpriteRenderer>();
		marker.gameObject.layer = LayerMask.NameToLayer("World Space UI");
		marker.enabled = false;
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
		} else if(repeatLastDialogue) {
			GameManager.instance.eventManager.StartEventSequence(dialogue[dialogue.Length - 1]);
		}
	}
}
