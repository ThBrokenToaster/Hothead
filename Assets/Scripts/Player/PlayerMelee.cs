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

    // Combo logistics
    private int comboIndex = 0;
    public float comboTimeout = 1f;
    private float timeSinceLastAttack = 0f;
    private bool comboActive = false;

	void Awake() {
		player = GetComponent<PlayerController>();
	}

	public void MeleeUpdate() {
        if (comboActive) {
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack > comboTimeout) {
                 comboActive = false;
                 comboIndex = 0;
            }
        }
		// Melee attack
        if (Input.GetButtonDown("Melee") && player.grounded && player.state == PlayerController.State.idle) {
            timeSinceLastAttack = 0f;
            comboActive = true;
            comboIndex++;

            player.state = PlayerController.State.melee;

            if (comboIndex == 3 && player.unlock.Has("meleeCombo")) {
                player.animator.SetTrigger("meleeKick");
                comboIndex = 0;
            } else {
                player.animator.SetTrigger("meleePunch");
            }
        }

        if (player.state == PlayerController.State.melee) {
            player.rb.velocity = new Vector2(0f, player.rb.velocity.y);
        }
	}

	/* Melee hitbox controls, used by player animator
     * Each melee animation should have three phases:
     * Buildup, Contact, Recovery
     */
    public void EnterMelee() {
        // Player is locked down, animation starts
        player.state = PlayerController.State.melee;
    }

    public void EnableMeleeHitbox(string hitboxName) {
        // Melee hitbox is enabled
        transform.Find(hitboxName).GetComponent<PlayerMeleeCollider>().SetEnabled(true);
    }

    public void DisableMeleeHitbox(string hitboxName) {
        // Melee hitbox is disabled
        transform.Find(hitboxName).GetComponent<PlayerMeleeCollider>().SetEnabled(false);
    }
    
    public void ExitMelee() {
        // Player is released, allowed to move
        player.state = PlayerController.State.idle;
    }
}
