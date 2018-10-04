using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
 * Door interactable. Leads the player to another door, even in a different scene
 * Child object is a marker showing the player they can interact
 */
public class DoorController : InteractableAbstract {

	private SpriteRenderer marker;
	public string doorName;
	public bool loadsNewScene;
	public string sceneToLoad;
	public string doorToLoad;

	void Start() {
		marker = transform.GetChild(0).GetComponent<SpriteRenderer>();
		marker.gameObject.layer = LayerMask.NameToLayer("World Space UI");
		marker.enabled = false;
	}

	override public void GainFocus() {
		marker.enabled = true;
	}

	override public void LoseFocus() {
		marker.enabled = false;
	}

	override public bool CanInteract() {
		return PlayerController.instance.grounded && PlayerController.instance.state != PlayerController.State.melee;
	}

	override public void Interact(){
		if (loadsNewScene) {
			GameManager.instance.LoadSceneToDoor(sceneToLoad, doorToLoad);
		} else {
			GameManager.instance.MoveToDoor(doorToLoad);
		}
	}

	// moves the player to the door
    public void MovePlayer() {
		PlayerController.instance.MoveTo(transform.position);
	}

	// returns door within scene with a certain name
	public static DoorController FindDoor(string findDoorName) {
		DoorController targetDoor = GameObject.FindObjectOfType<DoorController>();
		foreach(DoorController door in GameObject.FindObjectsOfType<DoorController>()) {
			if (door.doorName == findDoorName) {
				targetDoor = door.GetComponent<DoorController>();
			}
		}
		return targetDoor;
	}
}

/*
 * CustomEditor to toggle the correct fields
 */
[CustomEditor(typeof(DoorController))]
public class DoorControllerEditor : Editor {
	override public void OnInspectorGUI() {
		DoorController door = target as DoorController;
		door.doorName = EditorGUILayout.TextField("Name", door.doorName);
		GUILayout.Space(5);

		door.loadsNewScene = GUILayout.Toggle(door.loadsNewScene, "Loads New Scene");
		if (door.loadsNewScene) {
			door.sceneToLoad = EditorGUILayout.TextField("Scene To Load", door.sceneToLoad);
		}
		GUILayout.Space(5);

		door.doorToLoad = EditorGUILayout.TextField("Leads To Door", door.doorToLoad);	
	}
}