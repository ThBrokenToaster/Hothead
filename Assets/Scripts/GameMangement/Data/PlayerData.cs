using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {
	public float maxHealth;
	public float currentHealth;
	public List<Unlock> unlocks;

	public static PlayerData CreatePlayerData() {
		PlayerData data = new PlayerData();
		PlayerController p = PlayerController.instance;
		data.maxHealth = p.health.maxHealth;
		data.currentHealth = p.health.currentHealth;
		data.unlocks = p.unlock.unlocks;
		return data;
	}

	public static void ApplyPlayerData(PlayerData data) {
		PlayerController p = PlayerController.instance;
		p.health.maxHealth = data.maxHealth;
		p.health.currentHealth = data.currentHealth;
		p.unlock.unlocks = data.unlocks;
		HUDController.instance.UpdateHealthUI();
	}
}
