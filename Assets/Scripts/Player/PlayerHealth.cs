using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    private PlayerController player;

    public float maxHealth;
    public GameObject deathFx;
    public AudioClip hurtNoise;
    AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();

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
            audioSource.clip = hurtNoise;
            audioSource.Play();
            damaged = true;
            healthBar.value = currentHealth;
            if (currentHealth <= 0) {
                Kill();
            }
        }
        
    }

    public void Heal(float amount) {
        currentHealth += amount;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        healthBar.value = currentHealth;
    }

    public void Kill() {
        Instantiate(deathFx, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
