using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * Subcomponent of HUDController controls the UI for dialogue panels
 */
public class DialogueHUDController : MonoBehaviour {

	private HUDController hud;
	public GameObject dialogueUI;
	public TextMeshProUGUI nameTMP, bodyTMP;

	private DialogueEvent dialogueEvent;
	private TextEffects textController;

	private enum DialogueState { disabled, loading, finished } 
	private DialogueState state = DialogueState.disabled;
	private float speedMultiplier = 1f;
	private bool firstFrameEnabled = false;
	private bool ableToContinue = false;

	void Awake() {
		hud = GetComponent<HUDController>();
		textController = bodyTMP.GetComponent<TextEffects>();
		textController.SetDialogueHUD(this);
	}

	void LateUpdate() {
		if (!firstFrameEnabled) {
			if (Input.GetButtonDown("Use")) {
				switch (state) {
					case DialogueState.loading:
						textController.FinishReveal();
						break;
					case DialogueState.finished:
						dialogueEvent.NextPanel();
						break;
				}
			}
		} else {
			firstFrameEnabled = false;
		}
	}

	public void ShowDialoguePanel(DialoguePanel panel, DialogueEvent callback) {
		state = DialogueState.loading;
		dialogueUI.SetActive(true);

		nameTMP.SetText(panel.nameText);

		textController.SetText(panel.bodyText);

		if (panel.settings.useCustomSpeed) {
			textController.speedMultiplier = panel.settings.speedMultiplier;
		} else {
			textController.speedMultiplier = 1f;
		}
		
		ableToContinue = false;
		firstFrameEnabled = true;
		dialogueEvent = callback;

		textController.StartReveal();
	}


	public void HideDialoguePanel() {
		state = DialogueState.disabled;
		dialogueUI.SetActive(false);
	}

	public void OnDialogueRevealEnd() {
		state = DialogueState.finished;
	}
}
