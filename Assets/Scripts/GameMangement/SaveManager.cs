using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SaveManager : MonoBehaviour {

	[System.Serializable]
	public class SceneData {
		public Dictionary<string, DataAbstract> data = new Dictionary<string, DataAbstract>();

		public void SetData(string key, DataAbstract d) {
			data.Remove(key);
			data.Add(key, d);
		}

		public DataAbstract GetData(string key) {
			DataAbstract d;
			data.TryGetValue(key, out d);
			return d;
		}
	}
	public Dictionary<string, SceneData> sceneData = new Dictionary<string, SceneData>();

	private string savePath;
	
	[System.Serializable]
	public class GameData {
		public Dictionary<string, SceneData> sceneData;
		public PlayerData playerData;
		public string sceneLoad;
	}

	void Awake() {
		savePath = Application.persistentDataPath + "/save.dat";
		Debug.Log("Save path: " + savePath);
	}

	public void SetData(string scene, string id, DataAbstract d) {
		SceneData s;
		sceneData.TryGetValue(scene, out s);
		if (s == null) {
			s = new SceneData();
			sceneData.Add(scene, s);
		}
		s.SetData(id, d);
	}

	public DataAbstract GetData(string scene, string id) {
		SceneData s;
		sceneData.TryGetValue(scene, out s);
		if (s == null) {
			return null;
		}
		return s.GetData(id);
	}

	public void SaveGame() {
		GameData data = new GameData();
		data.sceneData = sceneData;
		data.playerData = PlayerData.CreatePlayerData();
		data.sceneLoad = GameManager.openScenes[GameManager.openScenes.Count - 1];

		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(savePath, FileMode.Create);
		formatter.Serialize(stream, data);
		stream.Close();
	}

	public void LoadGame() {
		if (File.Exists(savePath)) {
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(savePath, FileMode.Open);
			GameData data = (GameData) formatter.Deserialize(stream);
			stream.Close();

			sceneData = data.sceneData;
			SceneManager.LoadScene(data.sceneLoad);
			PlayerData.ApplyPlayerData(data.playerData);
		}
	}
}
