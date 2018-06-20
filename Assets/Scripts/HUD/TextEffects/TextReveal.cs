using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextReveal : MonoBehaviour {

	public float speedMultiplier = 1.0f;

	private TMP_Text m_TextComponent;
	private DialogueHUDController dialogueHUD;

	private bool hasTextChanged;
	private bool disabled = false;
	private bool finished = false;

	void Awake() {
		m_TextComponent = GetComponent<TMP_Text>();
	}

	public void SetDialogueHUD(DialogueHUDController d) {
		dialogueHUD = d;
	} 

	void OnEnable() {
		disabled = false;
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
	}

	void OnDisable() {
		disabled = true;
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
	}

	void ON_TEXT_CHANGED(Object obj) {
		if (obj == m_TextComponent)
			hasTextChanged = true;
	}

	public void StartReveal() {
		finished = false;
		StartCoroutine(RevealText());
	}

	public void FinishReveal() {
		int numCharacters = m_TextComponent.textInfo.characterCount; 
		TMP_TextInfo textInfo = m_TextComponent.textInfo;

		for (int i = 0; i < numCharacters; i++) {
			int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

			// Get the vertex colors of the mesh used by this text element (character or sprite).
			Color32[] newVertexColors = textInfo.meshInfo[materialIndex].colors32;

			// Get the index of the first vertex used by this text element.
			int vertexIndex = textInfo.characterInfo[i].vertexIndex;

			// Set new alpha values.
			newVertexColors[vertexIndex + 0].a = 255;
			newVertexColors[vertexIndex + 1].a = 255;
			newVertexColors[vertexIndex + 2].a = 255;
			newVertexColors[vertexIndex + 3].a = 255;
		}
		m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

		finished = true;
	}

	IEnumerator RevealText() {
		m_TextComponent.ForceMeshUpdate();

		TMP_TextInfo textInfo = m_TextComponent.textInfo;	
		int numCharacters = textInfo.characterCount; 
		
		Color32[] newVertexColors;
		
		// Turn everything invisible
		for (int i = 0; i < numCharacters; i++) {
			int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
			newVertexColors = textInfo.meshInfo[materialIndex].colors32;
			int vertexIndex = textInfo.characterInfo[i].vertexIndex;

			// Set new alpha values.
			newVertexColors[vertexIndex + 0].a = 0;
			newVertexColors[vertexIndex + 1].a = 0;
			newVertexColors[vertexIndex + 2].a = 0;
			newVertexColors[vertexIndex + 3].a = 0;
		}
		m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

		for (int i = 0; i < numCharacters; i++) {
			if (disabled || finished) break;

			if (!textInfo.characterInfo[i].isVisible) continue;

			int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
			newVertexColors = textInfo.meshInfo[materialIndex].colors32;
			int vertexIndex = textInfo.characterInfo[i].vertexIndex;

			// Set new alpha values.
			newVertexColors[vertexIndex + 0].a = 255;
			newVertexColors[vertexIndex + 1].a = 255;
			newVertexColors[vertexIndex + 2].a = 255;
			newVertexColors[vertexIndex + 3].a = 255;
			
			m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
			yield return new WaitForSecondsRealtime(0.03f / speedMultiplier);
		}

		finished = true;

		dialogueHUD.OnDialogueRevealEnd();
	}
}
