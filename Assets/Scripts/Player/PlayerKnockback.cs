using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour {

	private PlayerController player;

	public Vector2 dir;
	public float amount;
	public AnimationCurve curve;

	public float knockbackTime;
	public float knockbackMultiplier = 1;
	private float knockbackTimer;
	
	void Awake() {
        player = GetComponent<PlayerController>();
    }

	public void KnockbackUpdate() {
		if (player.state == PlayerController.State.knockback) {
			float velX = dir.x * amount;
			float i = curve.Evaluate(knockbackTimer / knockbackTime);
			velX = (i * velX) + ((1f - i) * player.rb.velocity.x);
			player.rb.velocity = new Vector2(velX, player.rb.velocity.y);
			knockbackTimer += Time.deltaTime;
		}
	}

	public void ApplyKnockback(Vector2 dir, float amount) {
		this.dir = dir.normalized;
		this.amount = amount * knockbackMultiplier;
		StartCoroutine("Knockback");
	}

	IEnumerator Knockback() {
		player.state = PlayerController.State.knockback;
		knockbackTimer = 0f;
		player.rb.velocity = new Vector2(player.rb.velocity.x, dir.y * amount);
		player.animator.SetBool("knockback", true);
		yield return new WaitForSeconds(knockbackTime);
		player.animator.SetBool("knockback", false);
		player.state = PlayerController.State.idle;
	}
}
