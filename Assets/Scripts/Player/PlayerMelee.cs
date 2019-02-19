using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Subcomponent of PlayerController managing melee attacks
 */
public class PlayerMelee : MonoBehaviour {

	private PlayerController player;

	// Attacking
    public Transform attackHitboxParent;

    // Combo logistics
    // private int comboIndex = 0;
    // public float comboTimeout = 1f;
    // private float timeSinceLastAttack = 0f;
    // private bool comboActive = false;
    private bool continueCombo = false;
    private bool comboAllowed = false;
    private bool comboAdvance = false;



	void Awake() {
		player = GetComponent<PlayerController>();
	}

	public void MeleeUpdate() {
        // if (comboActive) {
        //     timeSinceLastAttack += Time.deltaTime;
        //     if (timeSinceLastAttack > comboTimeout) {
        //          comboActive = false;
        //          comboIndex = 0;
        //     }
        // }
		// Melee attack
        if (player.unlock.Has("MeleeSlice") && Input.GetButtonDown("Melee")) {
            if (player.grounded && player.state == PlayerController.State.idle) {
                // timeSinceLastAttack = 0f;
                // comboActive = true;
                // comboIndex++;

                player.state = PlayerController.State.melee;
                player.animator.SetTrigger("meleeSlice");
            } else if (comboAllowed) {
                continueCombo = true;
                if (comboAdvance) {
                    player.animator.SetTrigger("comboNext");
                }
            }
        }
        

        if (player.state == PlayerController.State.melee) {
            player.rb.velocity = new Vector2(0f, player.rb.velocity.y);
        }
        player.animator.SetBool("continueCombo", continueCombo);
	}

	/* Melee hitbox controls, used by player animator
     * Each melee animation should have three phases:
     * Buildup, Contact, Recovery
     */
    public void EnterMelee() {
        // Player is locked down, animation starts
        player.state = PlayerController.State.melee;
        continueCombo = false;
        comboAllowed = false;
        comboAdvance = false;
    }

    public void EnableMeleeHitbox(string hitboxName) {
        // Melee hitbox is enabled
        attackHitboxParent.Find(hitboxName).GetComponent<PlayerMeleeCollider>().SetEnabled(true);
    }

    public void AllowCombo() {
        comboAllowed = true;
    }

    public void DisableMeleeHitbox(string hitboxName) {
        // Melee hitbox is disabled
        attackHitboxParent.Find(hitboxName).GetComponent<PlayerMeleeCollider>().SetEnabled(false);
    }
    
    public void AdvanceCombo() {
        comboAdvance = true;
        if (continueCombo && comboAllowed) {
            player.animator.SetTrigger("comboNext");
        }
    }

    public void UnallowCombo() {
        comboAllowed = false;
    }
    
    public void ExitMelee() {
        // Player is released, allowed to move
        player.state = PlayerController.State.idle;
    }
}
