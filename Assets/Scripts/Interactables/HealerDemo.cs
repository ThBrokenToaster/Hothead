using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * INCOMPLETE - going to be an NPC that heals player during dialogue
 */
public class HealerDemo : InteractableAbstract {
	private SpriteRenderer marker;
	public DialogueEvent dialogue;
	public EventSequence test;

	void Start() {
		marker = transform.GetChild(0).GetComponent<SpriteRenderer>();
		marker.enabled = false;
	}

	override public void GainFocus() {
		marker.enabled = true;
	}

	override public void LoseFocus() {
		marker.enabled = false;
	}

	override public bool CanInteract() {
		return PlayerController.instance.grounded && PlayerController.instance.state != PlayerController.State.melee;
	}

	override public void Interact() {
		GameManager.instance.eventManager.StartEventSequence(dialogue);
	}
}
