using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Attach to enemy trigger collider to cause damage and knockback to DamageableAbstract
 */
public class EnemyDamage : MonoBehaviour {

    public float damageAmount;
    public float damageRate;
    public float knockbackAmount;


    private void OnTriggerEnter2D(Collider2D other) {
        DamageableAbstract d = other.GetComponent<DamageableAbstract>();
        if (d != null) {
            d.Damage(damageAmount);
            Vector2 dPos = d.transform.position;
            Vector2 dir = new Vector2(dPos.x - transform.position.x, dPos.y - transform.position.y).normalized;
            d.ApplyKnockback(dir, knockbackAmount);
        }
    }

    // outdated knockback method
    // to be deleted
    void Knockback(Transform other) {
        Vector2 direction = new Vector2(other.position.x - transform.position.x, other.position.y - transform.position.y).normalized;
        direction *= knockbackAmount;
        Rigidbody2D pushRb = other.gameObject.GetComponent<Rigidbody2D>();
        pushRb.velocity = Vector2.zero;
        pushRb.AddForce(direction, ForceMode2D.Impulse);
    }
}
