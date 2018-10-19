using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
 * Single dialogue window
 */
[System.Serializable]
public class DialoguePanel {
	public string nameText;
	[TextArea(3,3)]
	public string bodyText;
	public DialoguePanelSettings settings;
}
