using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileHit : MonoBehaviour {

    public float damage;

    projectileController pc;

    public GameObject explosionEffect;

	// Use this for initialization
	void Awake () {
        pc = GetComponentInParent<projectileController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shootable"))
        {
            pc.removeForce();
            Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(gameObject);
            if(other.tag == "Enemy")
            {
                EnemyHealth hurt = other.gameObject.GetComponent<EnemyHealth>();
                hurt.doDamage(damage);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shootable"))
        {
            pc.removeForce();
            Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(gameObject);
            if (other.tag == "Enemy")
            {
                EnemyHealth hurt = other.gameObject.GetComponent<EnemyHealth>();
                hurt.doDamage(damage);
            }
        }
    }
}
