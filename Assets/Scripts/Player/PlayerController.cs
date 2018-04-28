using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { idle, walk, melee };

/*
 * The main player component. Mostly manages movement
 */
public class PlayerController : MonoBehaviour {
    
    // Player components
    [HideInInspector]
    public PlayerHealth health;
    [HideInInspector]
    public PlayerMelee melee;
    [HideInInspector]
    public PlayerProjectile projectile;
    
    private Rigidbody2D rb;
    [HideInInspector]
    public Animator animator;
    public AudioSource audioSource;

    // General
    public float maxSpeed;
    public bool facingRight = true;
    public State state = State.idle;

    // PlasmaBolts
    public Transform hand;
    

    public int equipped = 0;

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
        projectile = GetComponent<PlayerProjectile>();
        audioSource = GetComponent<AudioSource>();
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

        // Component Updates
        melee.MeleeUpdate();
        projectile.ProjectileUpdate();
        
        // Switching Items
        if (Input.GetButtonDown("Fire3")) {
            if (equipped < 2) {
                equipped++;
            } else {
                equipped = 0;
            }
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

    
}
