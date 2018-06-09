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
        if (Input.GetButtonDown("Melee") && player.grounded && player.state != PlayerController.State.melee) {
            player.animator.SetTrigger("melee");
        }
	}

	/* Melee hitbox controls, used by player animator
     * Each melee animation should have three phases:
     * Buildup, Contact, Recovery
     */
    public void EnterMelee() {
        player.state = PlayerController.State.melee;
    }

    public void EnableMeleeHitbox(string hitboxName) {
        transform.Find(hitboxName).GetComponent<Collider2D>().enabled = true;
    }

    public void DisableMeleeHitbox(string hitboxName) {
        transform.Find(hitboxName).GetComponent<Collider2D>().enabled = false;
    }
    
    public void ExitMelee() {
        player.state = PlayerController.State.idle;
    }
}
