using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Subcomponent of PlayerController managing health, extends DamageableAbstract
 */
public class PlayerHealth : DamageableAbstract {

    private PlayerController player;

    public float maxHealth;
    public AudioClip hurtNoise;

    [HideInInspector]
    public float currentHealth;

	void Awake() {
        player = GetComponent<PlayerController>();
    }
    
	void Start () {
        currentHealth = maxHealth;
	}

    override public void Damage(float amount) {
        if (amount > 0) {
            currentHealth -= amount;
            player.audioSource.clip = hurtNoise;
            player.audioSource.Play();
            if (currentHealth <= 0) {
                Kill();
            }
            HUDController.instance.TriggerDamageAnimation();
            HUDController.instance.UpdateHealthUI();
        }
        
    }

    override public void ApplyKnockback(Vector2 dir, float amount) {
        player.knockback.ApplyKnockback(dir, amount);
    }

    public void Heal(float amount) {
        currentHealth += amount;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        HUDController.instance.UpdateHealthUI();
    }

    public void Kill() {
        // This code needs to change, as deleting the player causes various errors
        Destroy(gameObject);
    }
}
