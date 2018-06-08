using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Controller for UI canvas, persits between scenes
 */
public class UICanvasController : MonoBehaviour {

	public static UICanvasController instance = null;
	public Slider healthBar;
    public Image damagedEffect;
	public Animator animator;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);    
		}
		DontDestroyOnLoad(gameObject);
		animator = GetComponent<Animator>();
	}

	void Start () {
        healthBar.maxValue = PlayerController.instance.health.maxHealth;
        healthBar.value = PlayerController.instance.health.maxHealth;
	}

	public void UpdateHealthUI() {
        healthBar.value = PlayerController.instance.health.currentHealth;
    }

	public void TriggerDamageAnimation() {
		animator.SetTrigger("Damage");
	}
}
