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
	private Animator animator;
	public GameObject pauseMenu;

	private GameManager.Event postFadeEvent;

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

	// Begins fade out animation. Stores GameManager.Event to call after completion
	public void TriggerFadeOut(GameManager.Event afterFadeOut) {
		postFadeEvent = afterFadeOut;
		animator.SetTrigger("FadeOut");
	}

	// Called when animator is done fading out
	public void OnFadeOutComplete() {
		if (postFadeEvent != null) {
			postFadeEvent();
		}
	}

	// Begins fade in animation. Stores GameManager.Event to call after completion
	public void TriggerFadeIn(GameManager.Event afterFadeIn) {
		postFadeEvent = afterFadeIn;
		animator.SetTrigger("FadeIn");
	}

	// Called when animator is done fading in
	public void OnFadeInComplete() {
		if (postFadeEvent != null) {
			postFadeEvent();
		}
	}

	public void ShowPauseMenu() {
		pauseMenu.SetActive(true);
	}

	public void HidePauseMenu() {
		pauseMenu.SetActive(false);
	}
}
