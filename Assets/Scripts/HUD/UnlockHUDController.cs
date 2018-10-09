using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnlockHUDController : MonoBehaviour {

	private UnlockEvent unlockEvent;
	private HUDController hud;
	public TextMeshProUGUI unlockText;

	[TextArea] public string text;

	void Awake() {
		hud = GetComponent<HUDController>();
	}

	public void ShowUnlockText(string title, UnlockEvent callback) {
		unlockEvent = callback;
		unlockText.text = string.Format(text, title);
		hud.animator.SetTrigger("Unlock");
	}

	public void PostUnlockText() {
		unlockEvent.EndEvent();
	}
}
