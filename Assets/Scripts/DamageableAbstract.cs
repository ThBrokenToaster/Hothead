using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Abstract class of objects that can be damaged
 */
abstract public class DamageableAbstract : MonoBehaviour {
    abstract public void Damage(float amount);
    virtual public void ApplyKnockback(Vector2 dir, float amount) {
        // By default, no knockback is applied
    }
}