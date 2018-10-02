using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Controller for projectiles (currently only player projectiles)
 */
public class ProjectileController : MonoBehaviour {

    Rigidbody2D proRB;
    public float damage;
    [HideInInspector]
    public bool facingRight;

    public GameObject explosionEffect; // Optional
    public float projectileSpeed; 
    
	void Start () {
        GetComponent<SpriteRenderer>().flipX = !facingRight;
        proRB = GetComponent<Rigidbody2D>();
        if (facingRight) {
            proRB.velocity = Vector2.right;
        } else {
            proRB.velocity = Vector2.left;
        }
        proRB.velocity *= projectileSpeed;
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
