using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The main player component. Mostly manages movement
 */
public class PlayerController : MonoBehaviour {
    
    public static PlayerController instance;
    public enum State { idle, melee, dash, knockback, walljump, offwalljump };

    // Player components
    [HideInInspector] public PlayerHealth health;
    [HideInInspector] public PlayerMelee melee;
    [HideInInspector] public PlayerDash dash;
    [HideInInspector] public PlayerProjectile projectile;
    [HideInInspector] public PlayerInteract interact;
    [HideInInspector] public PlayerUnlock unlock;
    [HideInInspector] public PlayerKnockback knockback;
    
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public AudioSource audioSource;

    // General
    public float xSpeed;
    public bool facingRight = true;
    public State state = State.idle;

    // PlasmaBolts
    public Transform hand;

    public int equipped = 0;

    // Jumping
    [HideInInspector] public bool grounded;
    [HideInInspector] public bool OnWall;
    [HideInInspector] public bool OnWallBack;
    private float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public Transform wallCheck;
    public Transform wallCheckBack;

    public float jumpHeight;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public Vector2 wallJumpSpeed;
    public float wallJumpTime;
    private float wallJumpTimer;
    public float wallStickTime;
    private float wallStickTimer;
    
    public Vector2 longJumpMultipliers;
    public bool longJump;
    
    public AnimationCurve wallJumpCurve;

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
        knockback = GetComponent<PlayerKnockback>();
        audioSource = GetComponent<AudioSource>();
	}
  
  void Start() {
        GameManager.instance.Refresh += Refresh;
  }

	void Update () {
        rb.gravityScale = 1f;

        float velX = Input.GetAxis("Horizontal") * xSpeed;
        float velY = rb.velocity.y;

        // Flip Player
        if (velX != 0 && state == State.idle) {
            facingRight = velX > 0;
            Vector3 scale = transform.localScale;
            scale.x = facingRight ? 1 : -1;
            transform.localScale = scale;
        }

        // Check ground and walls
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        OnWall = Physics2D.OverlapCircle(wallCheck.position, groundCheckRadius, groundLayer);
        OnWallBack = Physics2D.OverlapCircle(wallCheckBack.position, groundCheckRadius, groundLayer);

        
        if (unlock.Has("WallJump")) {
            // Trigger Wall Jump
            if (state == State.idle && !grounded && Input.GetButtonDown("Jump")) {
                if (OnWall) {
                    // normal
                    OnWall = false;
                    OnWallBack = false;
                    state = State.walljump;
                    wallJumpTimer = wallJumpTime;
                    wallStickTimer = 0f;
                    velY = wallJumpSpeed.y;
                }
                if (OnWallBack) {
                    // facing away
                    OnWall = false;
                    OnWallBack = false;
                    state = State.offwalljump;
                    wallJumpTimer = wallJumpTime;
                    wallStickTimer = 0f;
                    velY = wallJumpSpeed.y * (longJump ? longJumpMultipliers.y : 1);
                }
            }

            if (state == State.idle) {
                if (OnWall && !grounded && !OnWallBack) {
                    wallStickTimer = wallStickTime;
                }
                if (wallStickTimer > 0) {
                    wallStickTimer -= Time.deltaTime;
                    velX = 0;
                }
                if (OnWall && Input.GetAxisRaw("Horizontal") != 0 && velY <= 0f) {
                    if (unlock.Has("WallStick")) {
                        // user sticks to wall
                        velY = 0;
                        rb.gravityScale = 0f;
                    } else if (unlock.Has("WallSlide")) {
                        // user slides down wall
                        rb.gravityScale = .3f;
                    }
                }
            } 
        }

        // Jumping
        if (grounded && Input.GetButtonDown("Jump") && state == State.idle) {
            grounded = false;
            velY = jumpHeight;
        }

        // Smart Jump
        if (state == State.idle && !OnWall && !OnWallBack) {
            if (velY < 0) {
                velY += Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            } else if (velY > 0 && !Input.GetButton("Jump")) {
                velY += Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
        
        // Apply Wall Jump
        if (state == State.walljump || state == State.offwalljump) {
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer <= 0f) {
                state = State.idle;
            } else {
                float newVelX;
                if (state == State.walljump) {
                    newVelX = wallJumpSpeed.x * (!facingRight ? 1f : -1f);
                } else {
                    newVelX = wallJumpSpeed.x * (!facingRight ? -1f : 1f) * (longJump ? longJumpMultipliers.x : 1);
                }
                float i = wallJumpCurve.Evaluate((wallJumpTime - wallJumpTimer) / wallJumpTime);
                velX = (i * newVelX) + ((1f - i) * velX);
            }
        }
        
        // Set Velocity
        rb.velocity = new Vector2(velX , velY);

        // Component Updates
        melee.MeleeUpdate();
        projectile.ProjectileUpdate();
        dash.DashUpdate();
        knockback.KnockbackUpdate();
        
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
