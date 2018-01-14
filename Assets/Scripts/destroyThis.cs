using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyThis : MonoBehaviour {

    public float timer;

	// Use this for initialization
	void Awake () {
        Destroy(gameObject, timer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
