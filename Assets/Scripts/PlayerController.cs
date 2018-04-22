using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //Variables for moving
    public float maxSpeed;

    Rigidbody2D playerRB;
    Animator playerAnim;
    bool facingRight;


    //Vars for PlasmaBolts
    public Transform hand;
    public GameObject projectile;
    public GameObject arrowFire;
    public float fireRate = 1f;
    float nextFire = 0;

    int equiped = 0;



    //attacking
    public Collider2D[] attackHitboxes;



    //jumping stuffs
    bool grounded = false;
    float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float jumpHeight;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

	// Use this for initialization
	void Start () {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();

        facingRight = true;
	}
	

	void FixedUpdate () {

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        playerAnim.SetBool("isGrounded", grounded);

        playerAnim.SetFloat("verticalSpeed", playerRB.velocity.y);

        float move = Input.GetAxis("Horizontal");

        playerAnim.SetFloat("speed", Mathf.Abs(move));

        playerRB.velocity = new Vector2(move * maxSpeed, playerRB.velocity.y);
        //playerRB.AddForce(new Vector2(move * maxSpeed, 0));

        if (move > 0 && !facingRight)
        {
            flip();
        }
        else if (move < 0 && facingRight)
        {
            flip();
        }
	}

    void flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Update()
    {
        //jumping



        if (grounded && Input.GetButtonDown("Jump"))
        {
            grounded = false;
            playerAnim.SetBool("isGrounded", grounded);
            playerRB.AddForce(new Vector2(0, jumpHeight));
        }


        //Smart Jump
        if (playerRB.velocity.y < 0)
        {
            playerRB.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (playerRB.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            playerRB.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }


        //melee attack
        if (Input.GetButtonDown("Melee"))
        {
            attack(attackHitboxes[0]);
        }





        //switching Items
        if (Input.GetButtonDown("Fire3"))
        {
            if (equiped < 2)
            {
                equiped++;
            }
            else
            {
                equiped = 0;
            }
        }

        //Plasma Bolt
        if (Input.GetButtonDown("Fire1"))
        {
            firePlasma();
        }
    }

    //melee hit
    void attack(Collider2D hitbox)
    {
        StartCoroutine(attackForTime(hitbox));
    }

    public IEnumerator attackForTime(Collider2D hitbox)
    {
        hitbox.enabled = true;
        yield return new WaitForSeconds(.5f);
        hitbox.enabled = false;
    }




    void firePlasma()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            if (equiped == 0)
            {
                if (facingRight)
                {
                    Instantiate(projectile, hand.position, Quaternion.Euler(new Vector3(0,0,0)));
                }
                else if (!facingRight)
                {
                    Instantiate(projectile, hand.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                }
            }
            else if (equiped == 1)
            {
                if (facingRight)
                {
                    Instantiate(arrowFire, hand.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                }
                else if (!facingRight)
                {
                    Instantiate(arrowFire, hand.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                }
            }
            else
            {
                if (facingRight)
                {
                    Instantiate(projectile, hand.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                }
                else if (!facingRight)
                {
                    Instantiate(projectile, hand.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                }
            }
        }
    }
}
