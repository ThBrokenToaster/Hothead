using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public float damage;

    //public Collider[] used;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("A");
        if (other.gameObject.layer == LayerMask.NameToLayer("Shootable"))
        {
            Debug.Log("B");
            if (other.tag == "Enemy")
            {
                Debug.Log("C");
                EnemyHealth hurt = other.gameObject.GetComponent<EnemyHealth>();
                hurt.doDamage(damage);
            }
        }
    }

    /*private void OnTriggerStay2D(Collider2D other)
       {
           if (other.gameObject.layer == LayerMask.NameToLayer("Shootable"))
           {
               if (other.tag == "Enemy")
               {
                   EnemyHealth hurt = other.gameObject.GetComponent<EnemyHealth>();
                   hurt.doDamage(damage);
               }
           }
      }*/
}
