using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Destroys gameObject after set amount of time
 */
public class DestroyThis : MonoBehaviour {

    public float timer;

	// Use this for initialization
	void Awake () {
        Destroy(gameObject, timer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
