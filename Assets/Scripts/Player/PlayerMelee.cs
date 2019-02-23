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
    private PlayerMeleeCollider activeAttack;

    public float slideFadeTimeout;
    private float slideFadeTimer;


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
        
        // Stop melee if falling, otherwise animator will not call exit melee
        if (player.state == PlayerController.State.melee && !player.grounded) {
            ExitMelee();
        }
        
        if (player.state == PlayerController.State.melee) {
            if (activeAttack != null) {
                float t = player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                float force = activeAttack.GetHorizontalSlide(t) * (player.facingRight ? 1 : -1);
                if (slideFadeTimer > 0) {
                    force = Mathf.Lerp(force, player.rb.velocity.x, slideFadeTimer / slideFadeTimeout);
                    slideFadeTimer -= Time.deltaTime;
                }
                player.rb.velocity = new Vector2(force, player.rb.velocity.y);
            } else {
                // This code is triggered on first frame of a new combo, tigger fade into slide
                slideFadeTimer = slideFadeTimeout;

                //player.rb.velocity = new Vector2(0f, player.rb.velocity.y);

            }
            
        }
        player.animator.SetBool("continueCombo", continueCombo);
	}

	/* Melee hitbox controls, used by player animator
     * Each melee animation should have three phases:
     * Buildup, Contact, Recovery
     */
    public void EnterMelee(string attack) {
        // Player is locked down, animation starts
        player.state = PlayerController.State.melee;
        continueCombo = false;
        comboAllowed = false;
        comboAdvance = false;
        activeAttack =  attackHitboxParent.Find(attack).GetComponent<PlayerMeleeCollider>();
    }

    public void EnableMeleeHitbox() {
        // Melee hitbox is enabled
        activeAttack.SetEnabled(true);
    }

    public void AllowCombo() {
        comboAllowed = true;
    }

    public void DisableMeleeHitbox() {
        // Melee hitbox is disabled
        activeAttack.SetEnabled(false);
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
        activeAttack.SetEnabled(false);
        activeAttack = null;
        
    }
}
