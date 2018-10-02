using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Implementation of DamageableAbstract designed for enemies
 */
public class EnemyHealth : DamageableAbstract {

    public float enemyMaxHealth;
    private float health;

    public GameObject deathFx; // Optional
    public GameObject drop; // Optional

    public float knockbackMultiplier = 0f;
    private Rigidbody2D rb;

	void Start () {
        health = enemyMaxHealth;
        rb = GetComponent<Rigidbody2D>(); // Optional
	}

    override public void Damage(float amount) {
        health -= amount;
        if (health <= 0) {
            Kill();
        }
    }

    override public void ApplyKnockback(Vector2 dir, float amount) {
        if (rb != null) rb.velocity = dir * amount * knockbackMultiplier;
    }

    void Kill() {   
        Destroy(gameObject);
        if (deathFx != null) Instantiate(deathFx, transform.position, transform.rotation);
        if (drop != null) Instantiate(drop, transform.position + new Vector3(0, .5f), transform.rotation);
    }
}
