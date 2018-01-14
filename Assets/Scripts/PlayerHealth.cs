using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public float maxHealth;
    public GameObject deathFx;
    public AudioClip hurtNoise;
    AudioSource playerASS;

    float currentHealth;

    PlayerController controller;

    //HUD
    public Slider healthBar;
    public Image damagedEffect;
    Color damagedColor = new Color(0f, 0f, 0f, .5f);
    float smoothColor = 5f;

    bool damaged = false;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;

        controller = GetComponent<PlayerController>();

        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;

        playerASS = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (damaged)
        {
            damagedEffect.color = damagedColor;
        }
        else
        {
            damagedEffect.color = Color.Lerp(damagedEffect.color, Color.clear, smoothColor * Time.deltaTime);
        }
        damaged = false;
		
	}

    public void doDamage(float damage)
    {
        if (damage > 0)
        {
            currentHealth -= damage;
            playerASS.clip = hurtNoise;
            playerASS.Play();
            damaged = true;
            healthBar.value = currentHealth;
            if (currentHealth <= 0)
            {
                kill();
            }
        }
        
    }

    public void heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.value = currentHealth;
    }

    public void kill()
    {
        Instantiate(deathFx, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
