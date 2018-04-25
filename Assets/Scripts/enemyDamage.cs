using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDamage : MonoBehaviour {

    public float damage;
    public float damageRate;
    public float knockbackAmount;

    float nextDamage;

	void Start () {
        nextDamage = 0;
	}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && nextDamage < Time.time)
        {
            PlayerHealth health = other.gameObject.GetComponent<PlayerHealth>();
            health.Damage(damage);
            nextDamage = Time.time + damageRate;

            Knockback(other.transform);
        }
    }

    void Knockback(Transform other)
    {
        Vector2 direction = new Vector2(other.position.x - transform.position.x, other.position.y - transform.position.y).normalized;
        direction *= knockbackAmount;
        Rigidbody2D pushRb = other.gameObject.GetComponent<Rigidbody2D>();
        pushRb.velocity = Vector2.zero;
        pushRb.AddForce(direction, ForceMode2D.Impulse);
    }
}
