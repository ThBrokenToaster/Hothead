using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Attach to Camera to follow target smoothly
 */
public class CameraFollow2DPlat : MonoBehaviour {

    public Transform target;
    public float cameraSmoothingEffect;
    Vector3 offset;
    public float yLowerBound;

	void Start () {
        offset = transform.position - target.position;
        // yLowerBound = transform.position.y;
	}
	
    void FixedUpdate() {
        if (target == null) return;
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSmoothingEffect * Time.deltaTime);

        if (transform.position.y < yLowerBound) {
            transform.position = new Vector3(transform.position.x, yLowerBound, transform.position.z);
        }
    }

    public void Refresh() {
        transform.position = target.position + offset;
    }
}
