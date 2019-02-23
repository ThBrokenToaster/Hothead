using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Attaches to the trigger collider of a melee attack
 */
public class PlayerMeleeCollider : MonoBehaviour {

    public float damage;
    public float knockback;
    public float horizontalSlideForce;
    public AnimationCurve horizontalSlideCurve;

    public void SetEnabled(bool e) {
        GetComponent<Collider2D>().enabled = e;

        // There is a glitch where onTriggerEnter wont
        // be called on stationary objects when first
        // enabled, so this just moves the collider
        // slightly so it will always work. 
        if (e) {
            transform.position += Vector3.forward * .001f;
        } else {
            transform.position -= Vector3.forward * .001f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        DamageableAbstract d = other.GetComponent<DamageableAbstract>();
        if (d != null) {
            d.Damage(damage);
            Vector2 dir = (other.transform.position - transform.position).normalized;
            d.ApplyKnockback(dir, knockback);
        }
    }

    public float GetHorizontalSlide(float time) {
        return horizontalSlideCurve.Evaluate(time) * horizontalSlideForce;
    }
}
