using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    public float healthAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            health.Heal(healthAmount);
            Destroy(gameObject);
        }
    }
}
