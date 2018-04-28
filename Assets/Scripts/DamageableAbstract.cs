using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Abstract class of objects that can be damaged
 */
abstract public class DamageableAbstract : MonoBehaviour {
    abstract public void Damage(float amount);
}