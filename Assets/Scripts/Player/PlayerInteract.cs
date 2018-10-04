using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Subcomponent of PlayerController managing interactables
 */
public class PlayerInteract : MonoBehaviour {

    private PlayerController player;
    private List<InteractableAbstract> interactables = new List<InteractableAbstract>();
    private InteractableAbstract focusedInteractable;

	void Start() {
		player = GetComponent<PlayerController>();
	}

	void Update () {
        InteractableAbstract closest = GetClosestInteractable();
        if (focusedInteractable != closest) {
            if (focusedInteractable != null) {
                focusedInteractable.LoseFocus();
            }
            if (closest != null) {
                closest.GainFocus();
            }
            focusedInteractable = closest;
        }

        if (!GameManager.instance.paused && Input.GetButtonDown("Use")) {
            if (focusedInteractable != null) {
                focusedInteractable.Interact();
            } 
        }

	}

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<InteractableAbstract>() != null) {
            interactables.Add(other.GetComponent<InteractableAbstract>());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (interactables.Contains(other.GetComponent<InteractableAbstract>())) {
            interactables.Remove(other.GetComponent<InteractableAbstract>());
        }
    }

    private InteractableAbstract GetClosestInteractable() {
        if (interactables.Count == 0) {
            return null;
        }
        InteractableAbstract closest = interactables[0];
        foreach (InteractableAbstract i in interactables) {
            if (i.CanInteract() && DistanceFromPlayer(i) < DistanceFromPlayer(closest)) {
                closest = i;
            }
        }
        if (!closest.CanInteract()) {
            return null;
        }
        return closest;
    }

    private float DistanceFromPlayer(InteractableAbstract i) {
        return Vector2.Distance(i.transform.position, player.transform.position);
    }

    public void Refresh() {
        if (focusedInteractable != null) {
            focusedInteractable.LoseFocus();
            focusedInteractable = null;
        }
        interactables.Clear();
    }
}
