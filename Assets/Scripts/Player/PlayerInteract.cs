using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {

    private PlayerController player;
    private List<Interactable> interactables = new List<Interactable>();
    private Interactable focusedInteractable;
    private bool usePressed = false;

	void Start() {
		player = GetComponent<PlayerController>();
	}

	void Update () {
        Interactable closest = GetClosestInteractable();
        if (focusedInteractable != closest) {
            if (focusedInteractable != null) {
                focusedInteractable.LoseFocus();
            }
            if (closest != null) {
                closest.GainFocus();
            }
            focusedInteractable = closest;
        }

        if (Input.GetAxisRaw("Use") > 0) {
            if (!usePressed) {
                usePressed = true;
                if (focusedInteractable != null) {
                    focusedInteractable.Interact();
                } 
            }
        } else {
            if (usePressed) {
                usePressed = false;
            }
        }

	}

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Interactable>() != null) {
            interactables.Add(other.GetComponent<Interactable>());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (interactables.Contains(other.GetComponent<Interactable>())) {
            interactables.Remove(other.GetComponent<Interactable>());
        }
    }

    private Interactable GetClosestInteractable() {
        if (interactables.Count == 0) {
            return null;
        }
        Interactable closest = interactables[0];
        foreach (Interactable i in interactables) {
            if (i.CanInteract() && DistanceFromPlayer(i) < DistanceFromPlayer(closest)) {
                closest = i;
            }
        }
        if (!closest.CanInteract()) {
            return null;
        }
        return closest;
    }

    private float DistanceFromPlayer(Interactable i) {
        return Vector2.Distance(i.transform.position, player.transform.position);
    }
}
