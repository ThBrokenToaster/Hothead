using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeCollider : MonoBehaviour {

    public float damage;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<DamageableAbstract>() != null) {
            other.GetComponent<DamageableAbstract>().Damage(damage);
        }
    }
}
