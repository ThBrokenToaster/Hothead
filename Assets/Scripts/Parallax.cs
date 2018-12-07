using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

	public Transform[] backgrounds;
	public Transform playerLocation;

	private float[] parralaxRatios;
	public float smoothing = 1f;

	private Transform cameraTransform;
	private Vector3 previousCamPosition;

	// Use this for initialization
	void Start () {
		cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
		playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
		previousCamPosition = cameraTransform.position;
		parralaxRatios = new float[backgrounds.Length];

		for(int i = 0; i < backgrounds.Length; i++) {
			parralaxRatios[i] = backgrounds[i].position.z * -1;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < backgrounds.Length; i++) {
			float parallax = (previousCamPosition.x - cameraTransform.position.x) * parralaxRatios[i];

			float backgroundTargetX = backgrounds[i].position.x + parallax;

			Vector3 backgroundTargetPos = new Vector3(backgroundTargetX, playerLocation.position.y, backgrounds[i].position.z);

			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}

		previousCamPosition = cameraTransform.position;
	}
}
