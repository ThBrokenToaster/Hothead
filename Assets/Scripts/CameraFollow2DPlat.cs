using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2DPlat : MonoBehaviour {

    public Transform target;
    public float cameraSmoothingEffect;
    Vector3 offset;
    float yLowerBound;



	// Use this for initialization
	void Start () {
        offset = transform.position - target.position;

        yLowerBound = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSmoothingEffect * Time.deltaTime);

        if (transform.position.y < yLowerBound)
        {
            transform.position = new Vector3(transform.position.x, yLowerBound, transform.position.z);
        }
    }
}
