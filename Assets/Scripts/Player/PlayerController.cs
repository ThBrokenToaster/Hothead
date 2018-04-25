using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { idle, walk, melee };

public class PlayerController : MonoBehaviour {
    
    // Player components
    [HideInInspector]
    public PlayerHealth health;
    [HideInInspector]
    public PlayerMelee melee;
    
    private Rigidbody2D rb;
    [HideInInspector]
    public Animator animator;

    // General
    public float maxSpeed;
    public bool facingRight = true;
    public State state = State.idle;

    // PlasmaBolts
    public Transform hand;
    public GameObject projectile;
    public GameObject arrowFire;
    public float fireRate = 1f;
    public float nextFire = 0;

    int equiped = 0;

    // Jumping
    [HideInInspector]
    public bool grounded;
    private float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float jumpHeight;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health =  GetComponent<PlayerHealth>();
        melee = GetComponent<PlayerMelee>();
	}

	void FixedUpdate () {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        float move = Input.GetAxis("Horizontal");

        // Flip player if needed
        if (move != 0 && move > 0 != facingRight) {
            flip();
        }

        // Set playerState
        if (state != State.melee) {
            if (move == 0f) {
                state = State.idle;
            } else {
                state = State.walk;
            }
        }

        // Apply horizontal movement
        if (state == State.walk) {
            rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);
            // rb.AddForce(new Vector2(move * maxSpeed, 0));
        } else if (state == State.melee) {
            rb.velocity = new Vector2(0 , rb.velocity.y);
        }

        // Jumping
        if (grounded && Input.GetButtonDown("Jump")) {
            grounded = false;
            rb.AddForce(new Vector2(0, jumpHeight));
        }

        // Smart Jump
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        melee.MeleeUpdate();
        
        // Switching Items
        if (Input.GetButtonDown("Fire3")) {
            if (equiped < 2) {
                equiped++;
            } else {
                equiped = 0;
            }
        }

        // Plasma Bolt
        if (Input.GetButtonDown("Fire1")) {
            firePlasma();
        }

        // Set playAnim triggers
        animator.SetBool("isWalking", state == State.walk);
        animator.SetBool("isGrounded", grounded);
        animator.SetFloat("verticalSpeed", rb.velocity.y);
	}

    void flip() {
        if (state != State.melee) {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void firePlasma() {
        if (Time.time > nextFire) {
            nextFire = Time.time + fireRate;
            if (equiped == 0) {
                if (facingRight) {
                    Instantiate(projectile, hand.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                } else if (!facingRight) {
                    Instantiate(projectile, hand.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                }
            } else if (equiped == 1) {
                if (facingRight) {
                    Instantiate(arrowFire, hand.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                } else if (!facingRight) {
                    Instantiate(arrowFire, hand.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                }
            } else {
                if (facingRight) {
                    Instantiate(projectile, hand.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                } else if (!facingRight) {
                    Instantiate(projectile, hand.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                }
            }
        }
    }
}
