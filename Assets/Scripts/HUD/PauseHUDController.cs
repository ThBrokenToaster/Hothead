using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHUDController : MonoBehaviour {

	public void SaveGame() {
		GameManager.instance.saveManager.SaveGame();
	}

	public void LoadGame() {
		GameManager.instance.saveManager.LoadGame();
	}
}
