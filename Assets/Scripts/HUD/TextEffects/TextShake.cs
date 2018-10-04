using UnityEngine;
using System.Collections;
using TMPro;

/*
 * This is a modified script of VertexJitter from the TMP Examples
 */
public class TextShake : MonoBehaviour {

	public float speedMultiplier = 1.0f;
	public float offsetMultiplier = 1.0f;

	public enum EffectType { none, shake, wave };
	public EffectType type = EffectType.none;

	private TMP_Text m_TextComponent;
	private bool hasTextChanged;
	private bool disabled;

	void Awake() {
		m_TextComponent = GetComponent<TMP_Text>();
	}

	void OnEnable() {
		disabled = false;
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
		StartCoroutine(AnimateVertexColors());
	}

	void OnDisable() {
		disabled = true;
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
	}


	void Start() {
		StartCoroutine(AnimateVertexColors());
	}


	void ON_TEXT_CHANGED(Object obj) {
		if (obj == m_TextComponent)
			hasTextChanged = true;
	}

	/// <summary>
	/// Method to animate vertex colors of a TMP Text object.
	/// </summary>
	/// <returns></returns>
	IEnumerator AnimateVertexColors() {
		yield return new WaitForSecondsRealtime(.01f);
		TMP_TextInfo textInfo = m_TextComponent.textInfo;

		int loopCount = 0;
		hasTextChanged = true;

		// Cache the vertex data of the text object as the effect is applied to the original position of the characters.
		TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();

		while (true) {
			if (disabled) break;

			// Get new copy of vertex data if the text has changed.
			if (hasTextChanged) {
				// Update the copy of the vertex data for the text object.
				cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
				hasTextChanged = false;
			}

			int characterCount = textInfo.characterCount;

			for (int i = 0; i < characterCount; i++) {
				TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

				// Skip characters that are not visible and thus have no geometry to manipulate.
				if (!charInfo.isVisible)
					continue;

				
				// Get the index of the material used by the current character.
				int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

				// Get the index of the first vertex used by this text element.
				int vertexIndex = textInfo.characterInfo[i].vertexIndex;

				// Get the cached vertices of the mesh used by this text element (character or sprite).
				Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;

				Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;

				Vector3 jitterOffset = Vector3.zero;

				FontStyles style = textInfo.characterInfo[i].style;
				Debug.Log(style.ToString());

				if (type == EffectType.shake) {
					jitterOffset = new Vector3(Random.Range(-.25f, .25f), Random.Range(-.25f, .25f), 0);
				} else if (type == EffectType.wave) {
					jitterOffset = new Vector3(0, Mathf.Sin((i/2)+loopCount) / 2f, 0);
				}
				
				jitterOffset *= offsetMultiplier;

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

			yield return new WaitForSecondsRealtime(0.1f / speedMultiplier);
		}
	}

}