using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Controller for UI canvas, persits between scenes
 */
public class HUDController : MonoBehaviour {

	public static HUDController instance = null;

	public Slider healthBar;
    public Image damagedEffect;
	public GameObject pauseMenu;

	public Animator animator;
	[HideInInspector] public DialogueHUDController dialogue;
	[HideInInspector] public UnlockHUDController unlock;
	[HideInInspector] public PauseHUDController pause;

	private GameManager.Event postFadeEvent;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);    
		}
		DontDestroyOnLoad(gameObject);

		animator = GetComponent<Animator>();
		dialogue = GetComponent<DialogueHUDController>();
		unlock = GetComponent<UnlockHUDController>();
		pause = GetComponent<PauseHUDController>();
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
