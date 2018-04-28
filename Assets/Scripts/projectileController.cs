using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Controller for projectiles (currently only player projectiles)
 */
public class ProjectileController : MonoBehaviour {

    Rigidbody2D proRB;
    public float damage;

    public GameObject explosionEffect; // Optional
    public float projectileSpeed; 
    
	void Start () {
        proRB = GetComponent<Rigidbody2D>();
        
        if (transform.localRotation.z != 0) {
            proRB.AddForce(new Vector2(-1, 0) * projectileSpeed, ForceMode2D.Impulse);
        } else {
            proRB.AddForce(new Vector2(1, 0) * projectileSpeed, ForceMode2D.Impulse);
        }

	}

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<DamageableAbstract>() != null && other.tag != "Player") {
            other.GetComponent<DamageableAbstract>().Damage(damage);
            Explode();
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            Explode();
        }
    }

    private void Explode() {
        Destroy(gameObject);
        if (explosionEffect != null) Instantiate(explosionEffect, transform.position, transform.rotation);
    }

}
