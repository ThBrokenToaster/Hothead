using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileController : MonoBehaviour {

    Rigidbody2D proRB;

    public float projectileSpeed;
    

	// Use this for initialization
	void Start () {
        proRB = GetComponent<Rigidbody2D>();
        
        
        if (transform.localRotation.z != 0)
        {
            proRB.AddForce(new Vector2(-1, 0) * projectileSpeed, ForceMode2D.Impulse);
        } else
        {
            proRB.AddForce(new Vector2(1, 0) * projectileSpeed, ForceMode2D.Impulse);
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void removeForce()
    {
        proRB.velocity = new Vector2(0, 0);
    }
}
