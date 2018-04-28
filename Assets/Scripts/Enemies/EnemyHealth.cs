using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : DamageableAbstract {

    public float enemyMaxHealth;
    private float health;

    public GameObject deathFx; // Optional
    public GameObject drop; // Optional

	void Start () {
        health = enemyMaxHealth;
	}

    override public void Damage(float amount) {
        health -= amount;
        if (health <= 0) {
            Kill();
        }
    }

    void Kill() {   
        Destroy(gameObject);
        if (deathFx != null) Instantiate(deathFx, transform.position, transform.rotation);
        if (drop != null) Instantiate(drop, transform.position + new Vector3(0, .5f), transform.rotation);
    }
}
