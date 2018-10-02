using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Subcomponent of PlayerController managing projectiles
 */
public class PlayerProjectile : MonoBehaviour {

	private PlayerController player;

	public List<GameObject> projectiles;
    public float fireRate = 1f;
    private float fireTimer;

	void Start () {
		player = GetComponent<PlayerController>();
		fireTimer = fireRate;
	}
	
	void Update() {
		if (fireTimer > 0f) {
			fireTimer -= Time.deltaTime;
		}
	}
	public void ProjectileUpdate () {
        if (Input.GetButtonDown("Fire1")) {
            firePlasma();
        }
	}

	private void firePlasma() {
        if (fireTimer <= 0f) {
            fireTimer = fireRate;

			// different projectiles were removed, as the inventory system will change
			projectiles[0].GetComponent<ProjectileController>().facingRight = player.facingRight;
			GameObject p = Instantiate(projectiles[0], player.hand.transform.position, Quaternion.identity);

           
        }
    }
}
