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
    private bool meleeButtonDown;

	void Awake() {
		player = GetComponent<PlayerController>();
	}
	
    // single frame inputs
    void Update() {
        if (!meleeButtonDown)
            meleeButtonDown = Input.GetButtonDown("Melee");
    }

	public void MeleeUpdate() {
		// Melee attack
        if (meleeButtonDown && player.grounded && player.state != PlayerController.State.melee) {
            meleeButtonDown = false;
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
        transform.Find(hitboxName).GetComponent<PlayerMeleeCollider>().SetEnabled(true);
    }

    public void DisableMeleeHitbox(string hitboxName) {
        transform.Find(hitboxName).GetComponent<PlayerMeleeCollider>().SetEnabled(false);
    }
    
    public void ExitMelee() {
        player.state = PlayerController.State.idle;
    }
}
