using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Subcomponent of PlayerController managing melee attacks
 */
public class PlayerDash : MonoBehaviour {

	
	private PlayerController player;

	public float dashTime = 2;
	public float dashSpeed = 5;
	public AnimationCurve dashAnim;
	private float dashTimer;
	private Vector2 dashDirection;

	void Awake() {
		player = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	public void DashUpdate () {
		if (Input.GetButtonDown("Dash") && player.grounded && player.state == PlayerController.State.idle) {
            player.state = PlayerController.State.dash;
			dashDirection = player.facingRight ? new Vector2(1,0) : new Vector2(-1,0);
			player.animator.SetBool("dash", true);
			StartCoroutine("Dash");
			MainCameraController.instance.ApplyShake(dashTime, 1);
        }
		if (player.state == PlayerController.State.dash) {
			float multiplier = dashAnim.Evaluate(dashTimer / dashTime);
			player.rb.velocity = dashDirection.normalized * dashSpeed * multiplier;
			dashTimer += Time.deltaTime;
		}
	}

	IEnumerator Dash() {
		dashTimer = 0;
		yield return new WaitForSeconds(dashTime);
		player.animator.SetBool("dash", false);
		player.state = PlayerController.State.idle;
	}
}
