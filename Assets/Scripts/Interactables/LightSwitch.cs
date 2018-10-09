using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * OUTDATED - interactable that turns on/off connected lights
 */
public class LightSwitch : InteractableAbstract {

	public Light[] connectedLights;
	private SpriteRenderer marker; 

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
		foreach(Light l in connectedLights) {
			l.enabled = !l.enabled;
		}
	}

}
