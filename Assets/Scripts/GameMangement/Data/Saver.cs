using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saver : MonoBehaviour {

	public string uniqueID;
	private string scene;

	void Awake() {
		scene = gameObject.scene.name;
	}

	public void SetData(DataAbstract data) {
		GameManager.instance.saveManager.SetData(scene, uniqueID, data);
	}

	public DataAbstract GetData() {
		return GameManager.instance.saveManager.GetData(scene, uniqueID);
	}
}
