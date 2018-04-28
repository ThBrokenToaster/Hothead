using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    private PlayerController player;

    public float maxHealth;
    public AudioClip hurtNoise;

    float currentHealth;

    //HUD
    public Slider healthBar;
    public Image damagedEffect;
    Color damagedColor = new Color(0f, 0f, 0f, .5f);
    float smoothColor = 5f;

    bool damaged = false;

	// Use this for initialization
	void Start () {
        player = GetComponent<PlayerController>();

        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        if (damaged) {
            damagedEffect.color = damagedColor;
        } else {
            damagedEffect.color = Color.Lerp(damagedEffect.color, Color.clear, smoothColor * Time.deltaTime);
        }
        damaged = false;
	}

    public void Damage(float damage) {
        if (damage > 0) {
            currentHealth -= damage;
            player.audioSource.clip = hurtNoise;
            player.audioSource.Play();
            damaged = true;
            if (currentHealth <= 0) {
                Kill();
            }
            UpdateHealthUI();
        }
        
    }

    public void Heal(float amount) {
        currentHealth += amount;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        UpdateHealthUI();
    }

    public void UpdateHealthUI() {
        healthBar.value = currentHealth;
    }
    public void Kill() {
        // This code needs to change, as deleting the player causes various errors

        // Instantiate(deathFx, transform.position, transform.rotation);
        // Destroy(gameObject);
    }
}
