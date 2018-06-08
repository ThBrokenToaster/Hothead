using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Subcomponent of PlayerController managing melee attacks
 */
public class PlayerMelee : MonoBehaviour {

	private PlayerController player;

	// Attacking
    public Collider2D[] attackHitboxes;


	void Awake() {
		player = GetComponent<PlayerController>();
	}
	
	public void MeleeUpdate() {
		// Melee attack
        if (Input.GetButtonDown("Melee") && player.grounded && player.state != State.melee) {
            player.animator.SetTrigger("melee");
        }
	}

	// Melee hitbox controls, used by player animator
    public void EnableMeleeHitbox(string hitboxName) {
        player.state = State.melee;
        transform.Find(hitboxName).GetComponent<Collider2D>().enabled = true;
    }

    public void DisableMeleeHitbox(string hitboxName) {
        player.state = State.idle;
        transform.Find(hitboxName).GetComponent<Collider2D>().enabled = false;
    }
}
