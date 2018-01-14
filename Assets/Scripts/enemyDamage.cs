using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDamage : MonoBehaviour {

    public float damage;
    public float damageRate;
    public float knockbackAmount;

    float nextDamage;

	// Use this for initialization
	void Start () {
        nextDamage = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && nextDamage < Time.time)
        {
            PlayerHealth health = other.gameObject.GetComponent<PlayerHealth>();
            health.doDamage(damage);
            nextDamage = Time.time + damageRate;

            knockback(other.transform);
        }
    }

    void knockback(Transform other)
    {
        Vector2 direction = new Vector2(other.position.x - transform.position.x, other.position.y - transform.position.y).normalized;
        direction *= knockbackAmount;
        Rigidbody2D pushRb = other.gameObject.GetComponent<Rigidbody2D>();
        pushRb.velocity = Vector2.zero;
        pushRb.AddForce(direction, ForceMode2D.Impulse);
    }
}
