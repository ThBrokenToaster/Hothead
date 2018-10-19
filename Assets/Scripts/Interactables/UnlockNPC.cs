using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NPC that triggers an unlock for the player during dialogue
 * Child object is a marker showing the player they can interact
 */
public class UnlockNPC : InteractableAbstract {
	private SpriteRenderer marker;

	public DialogueEvent preUnlockDialogue;
	public UnlockEvent unlock;
	public DialogueEvent postUnlockDialogue;

	private bool givenUnlock = false;
	public bool repeatPostUnlockDialogue = true;


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
				(!givenUnlock || repeatPostUnlockDialogue);
	}

	override public void Interact() {
		if (!givenUnlock) {
			givenUnlock = true;
			GameManager.instance.eventManager.StartEventSequence(preUnlockDialogue, unlock, postUnlockDialogue);
		} else if (repeatPostUnlockDialogue) {
			GameManager.instance.eventManager.StartEventSequence(postUnlockDialogue);
		}
	}
}
