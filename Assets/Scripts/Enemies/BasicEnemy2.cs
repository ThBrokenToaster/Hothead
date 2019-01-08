using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy2 : DamageableAbstract {

    public float windUpTime = .5f;
    public float waittime = 1.5f;
    int state = 0;
	public float enemyMaxHealth;
    private float health;
    public GameObject detect;
    public GameObject wallDetect;
    public LayerMask ground;

    //Direction of movement
    float dir = 1;
    public float speed = 5f;

    public float platDetect = 1f;

    public GameObject deathFx; // Optional
    public GameObject drop; // Optional

    public float knockbackMultiplier = 0f;
    private Rigidbody2D rb;
    bool wall = false;
    RaycastHit2D hit;
    bool facingRight = true;
    public float damageAmount;
    public float damageRate;
    public float knockbackAmount;
    public GameObject frontSense;
    public bool attacking;
    bool wait = false;
    bool noRepeat = false;
    

    float nextDamage;
    [HideInInspector]
    public Animator animator;
    

	void Awake () {
        animator = GetComponent<Animator>();
        health = enemyMaxHealth;
        rb = GetComponent<Rigidbody2D>(); // Optional
        Physics2D.queriesStartInColliders = false;
        nextDamage = 0;
	}

    override public void Damage(float amount) {
        health -= amount;
        if (health <= 0) {
            Kill();
        }
    }

    override public void ApplyKnockback(Vector2 dir2, float amount) {
        if (rb != null) rb.velocity = dir2 * amount * knockbackMultiplier;
    }

    void Kill() {   
        Destroy(gameObject);
        if (deathFx != null) Instantiate(deathFx, transform.position, transform.rotation);
        if (drop != null) Instantiate(drop, transform.position + new Vector3(0, .5f), transform.rotation);
    }

    void FixedUpdate() {
        if (state == 0) {
            if (rb != null) rb.velocity = new Vector2(speed * dir, rb.velocity.y);
            hit = Physics2D.Raycast(detect.transform.position, Vector2.down, platDetect, ground);
            wall = Physics2D.OverlapCircle(wallDetect.transform.position, .2f, ground);
            if (hit.collider == null || wall || hit.collider.gameObject.tag == "Player") {
                flip();
            }
        } else if (state == 1) {
            if (!attacking && !wait && !noRepeat) {
                noRepeat = true;
                rb.velocity = new Vector2(0, rb.velocity.y);
                StartCoroutine(swipe());
            }
        }

        if (attacking == false && noRepeat == false) {
            state = 0;
        }
        if (wait == true) {
            StartCoroutine(waiting());
        }

        animator.SetBool("Attacking", attacking);
    }

    void flip() {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        dir = dir * -1;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(detect.transform.position, detect.transform.position + Vector3.down*platDetect);
    }
/* 
    private void OnTriggerStay2D(Collider2D other) {
        if (other.GetComponent<DamageableAbstract>() != null && nextDamage < Time.time) {
            other.GetComponent<DamageableAbstract>().Damage(damageAmount);
            nextDamage = Time.time + damageRate;

            Knockback(other.transform);
        }
    }*/

    void Knockback(Transform other) {
        Vector2 direction = new Vector2(other.position.x - transform.position.x, other.position.y - transform.position.y).normalized;
        direction *= knockbackAmount;
        Rigidbody2D pushRb = other.gameObject.GetComponent<Rigidbody2D>();
        pushRb.velocity = Vector2.zero;
        pushRb.AddForce(direction, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            state = 1;
        }
    }

    IEnumerator swipe() {
        yield return new WaitForSeconds(windUpTime);
        attacking = true;
        yield return new WaitForSeconds(1.5f);
    }

    public void EndSwipe() {
        attacking = false;
        wait = true;
        noRepeat = false;
    }

    IEnumerator waiting() {
        yield return new WaitForSeconds(waittime);
        wait = false;
    }

}
