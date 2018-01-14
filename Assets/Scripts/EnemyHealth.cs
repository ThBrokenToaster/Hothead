using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public float enemyMaxHealth;

    public GameObject deathFx;

    float enemyHealth;

    public bool dropsPickup;

    public GameObject drop;

	// Use this for initialization
	void Start () {
        enemyHealth = enemyMaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void doDamage(float damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            kill();
        }
    }

    void kill()
    {
        Instantiate(deathFx, transform.position, transform.rotation);
        Destroy(gameObject);
        if (dropsPickup)
        {
            Instantiate(drop, transform.position + new Vector3(0, .5f), transform.rotation);
        }
    }
}
