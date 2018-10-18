using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Subcomponent of GameManager, handles events sequences (dialogue, cutscenes)
 */
public class EventManager : MonoBehaviour {

	private EventSequence currentSeq;
	private GameManager gameManager; 

	void Awake() {
		gameManager = GetComponent<GameManager>();
	}

	public void StartEventSequence(EventSequence seq) {
		currentSeq = seq;
		gameManager.PauseGame(false);
		currentSeq.StartSequence();
	}

	public void StartEventSequence(params EventAbstract[] events) {
		StartEventSequence(new EventSequence(events));
	}


	public void EndEventSequence() {
		gameManager.UnPauseGame();
	}
}
