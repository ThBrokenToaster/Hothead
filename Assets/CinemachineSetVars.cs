using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineSetVars : MonoBehaviour {

	CinemachineVirtualCamera cm;

	// Use this for initialization
	void Start () {
		cm = GetComponent<CinemachineVirtualCamera>();
	}
	
	// Update is called once per frame
	void Update () {
		if (cm.Follow == null) {
			cm.Follow = GameObject.FindGameObjectWithTag("Player").transform;
		}
	}
}
