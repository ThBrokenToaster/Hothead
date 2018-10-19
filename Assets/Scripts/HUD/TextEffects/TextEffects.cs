using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * Applies text effects on a TMPro object
 * Currently used in dialogue text
 */
public class TextEffects : MonoBehaviour {

	public float speedMultiplier = 1.0f;
	public float effectMultiplier = 1.0f;

	public enum EffectType { none, shake, wave };
	public List<EffectType> charEffect;

	private TMP_Text m_TextComponent;
	private DialogueHUDController dialogueHUD;
	private IEnumerator runningEffect;

	private bool hasTextChanged;
	private bool disabled = false;
	private bool finished = false;

	void Awake() {
		m_TextComponent = GetComponent<TMP_Text>();
	}

	public void SetDialogueHUD(DialogueHUDController d) {
		dialogueHUD = d;
	} 

	public void SetText(string text) {
		EffectType currentEffect = EffectType.none;
		string meshText = "";
		charEffect.Clear();

		string[] effectTags = { "shake", "wave" };
		EffectType[] effectTypes = { EffectType.shake, EffectType.wave };

		while (text.Length > 0) {
			for (int i = 0; i < effectTags.Length; i++) {
				if (text.StartsWith("<" + effectTags[i] + ">")) {
					currentEffect = effectTypes[i];
					text = text.Substring(("<" + effectTags[i] + ">").Length);
				} else if (text.StartsWith("</" + effectTags[i] + ">")) {
					currentEffect = EffectType.none;
					text = text.Substring(("</" + effectTags[i] + ">").Length);
				}
			}
			if (text.StartsWith("<")) {
				while (!text.StartsWith(">")) {
					meshText += text.ToCharArray()[0];
					text = text.Substring(1);
				}
				meshText += text.ToCharArray()[0];
				text = text.Substring(1);
			} else {
				meshText += text.ToCharArray()[0];
				charEffect.Add(currentEffect);
				text = text.Substring(1);
			}
		}

		m_TextComponent.SetText(meshText);
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

		if (runningEffect != null) {
			StopCoroutine(runningEffect);
		}
		runningEffect = AnimateVertexColors();
		StartCoroutine(runningEffect);

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

	IEnumerator AnimateVertexColors() {
		yield return new WaitForSecondsRealtime(.01f);
		TMP_TextInfo textInfo = m_TextComponent.textInfo;

		int loopCount = 0;
		hasTextChanged = false;

		// Cache the vertex data of the text object as the effect is applied to the original position of the characters.
		TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();

		while (true) {
			if (disabled) break;

			// Get new copy of vertex data if the text has changed.
			if (hasTextChanged) {
				yield break;
			}

			int characterCount = textInfo.characterCount;

			for (int i = 0; i < characterCount; i++) {
				TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

				// Skip characters that are not visible and thus have no geometry to manipulate.
				if (!charInfo.isVisible || charEffect[i] == EffectType.none)
					continue;

				// Get the index of the material used by the current character.
				int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

				// Get the index of the first vertex used by this text element.
				int vertexIndex = textInfo.characterInfo[i].vertexIndex;

				// Get the cached vertices of the mesh used by this text element (character or sprite).
				Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;

				Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;

				Vector3 jitterOffset = Vector3.zero;
				if (charEffect[i] == EffectType.shake) {
					jitterOffset = new Vector3(Random.Range(-.25f, .25f), Random.Range(-.25f, .25f), 0);
				} else if (charEffect[i] == EffectType.wave) {
					jitterOffset = new Vector3(0, Mathf.Sin((i/2)+loopCount) / 2f, 0);
				}
				
				jitterOffset *= effectMultiplier;

				destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] + jitterOffset;
				destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] + jitterOffset;
				destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] + jitterOffset;
				destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] + jitterOffset;

			}

			// Push changes into meshes
			for (int i = 0; i < textInfo.meshInfo.Length; i++) {
				textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
				m_TextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
			}

			loopCount += 1;

			yield return new WaitForSecondsRealtime(0.1f);
		}
	}
}
