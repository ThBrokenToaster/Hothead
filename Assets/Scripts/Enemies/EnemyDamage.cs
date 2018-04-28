using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour {

    public float damageAmount;
    public float damageRate;
    public float knockbackAmount;

    float nextDamage;

	void Start () {
        nextDamage = 0;
	}

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<DamageableAbstract>() != null) {
            other.GetComponent<DamageableAbstract>().Damage(damageAmount);
            nextDamage = Time.time + damageRate;

            Knockback(other.transform);
        }
    }

    void Knockback(Transform other) {
        Vector2 direction = new Vector2(other.position.x - transform.position.x, other.position.y - transform.position.y).normalized;
        direction *= knockbackAmount;
        Rigidbody2D pushRb = other.gameObject.GetComponent<Rigidbody2D>();
        pushRb.velocity = Vector2.zero;
        pushRb.AddForce(direction, ForceMode2D.Impulse);
    }
}
