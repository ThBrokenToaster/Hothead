using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			if (player.facingRight) {
				Instantiate(projectiles[0], player.hand.position, Quaternion.Euler(new Vector3(0, 0, 0)));
			} else if (!player.facingRight) {
				Instantiate(projectiles[0], player.hand.position, Quaternion.Euler(new Vector3(0, 0, 180)));
			}
           
        }
    }
}
