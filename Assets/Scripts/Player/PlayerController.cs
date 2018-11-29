﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The main player component. Mostly manages movement
 */
public class PlayerController : MonoBehaviour {
    
    public static PlayerController instance;
    public enum State { idle, melee, dash };

    // Player components
    [HideInInspector]
    public PlayerHealth health;
    [HideInInspector]
    public PlayerMelee melee;
    [HideInInspector]
    public PlayerDash dash;
    [HideInInspector]
    public PlayerProjectile projectile;
    [HideInInspector]
    public PlayerInteract interact;
    [HideInInspector]
    public PlayerUnlock unlock;
    
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public AudioSource audioSource;

    // General
    public float xSpeed;
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

    void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);    
		}
		DontDestroyOnLoad(gameObject);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health =  GetComponent<PlayerHealth>();
        melee = GetComponent<PlayerMelee>();
        dash = GetComponent<PlayerDash>();
        projectile = GetComponent<PlayerProjectile>();
        interact = GetComponent<PlayerInteract>();
        unlock = GetComponent<PlayerUnlock>();
        audioSource = GetComponent<AudioSource>();
	}
  
  void Start() {
        GameManager.instance.Refresh += Refresh;
  }

	void Update () {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        float velX = Input.GetAxis("Horizontal") * xSpeed; // * Time.deltaTime;
        float velY = rb.velocity.y;

        if (velX != 0 && state == State.idle) {
            facingRight = velX > 0;
            Vector3 scale = transform.localScale;
            scale.x = facingRight ? 1 : -1;
            transform.localScale = scale;
        }

        // Jumping
        if (grounded && Input.GetButtonDown("Jump") && state == State.idle) {
            grounded = false;
            velY = jumpHeight;
        }

        // Smart Jump
        if (velY < 0) {
            velY += Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (velY > 0 && !Input.GetButton("Jump")) {
            velY += Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        rb.velocity = new Vector2(velX , velY);

        // Component Updates
        melee.MeleeUpdate();
        projectile.ProjectileUpdate();
        dash.DashUpdate();
        
        // Switching Items
        if (Input.GetButtonDown("Fire3")) {
            if (equipped < 2) {
                equipped++;
            } else {
                equipped = 0;
            }
        }

        // Set playAnim triggers
        animator.SetBool("isWalking", velX != 0f);
        animator.SetBool("isGrounded", grounded);
        animator.SetFloat("verticalSpeed", rb.velocity.y);
	}

    void Flip() {
        if (state != State.melee) {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    public void Refresh() {
        rb.velocity = Vector2.zero;
        interact.Refresh();
    }

    public void MoveTo(Vector3 position) {
        transform.position = position;

    }
}
