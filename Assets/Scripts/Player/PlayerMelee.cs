using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour {

	private PlayerController player;

	// Attacking
    public Collider2D[] attackHitboxes;


	void Start () {
		player = GetComponent<PlayerController>();
	}
	
	public void MeleeUpdate() {
		// Melee attack
        if (Input.GetButtonDown("Melee") && player.grounded && player.state != State.melee) {
            attack(attackHitboxes[0]);
            player.animator.SetTrigger("melee");
        }
	}

	// Melee hit
    void attack(Collider2D hitbox) {
        StartCoroutine(attackForTime(hitbox));
    }

    public IEnumerator attackForTime(Collider2D hitbox) {
        player.state = State.melee;
        hitbox.enabled = true;
        yield return new WaitForSeconds(.5f);
        hitbox.enabled = false;
        player.state = State.idle;
    }
}
