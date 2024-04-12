using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour {

    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;

    [SerializeField] private Button moveUpButton;
    [SerializeField] private TextMeshProUGUI moveUpButtonText;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private TextMeshProUGUI moveDownButtonText;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private TextMeshProUGUI moveLeftButtonText;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private TextMeshProUGUI moveRightButtonText;
    [SerializeField] private Button interactButton;
    [SerializeField] private TextMeshProUGUI interactButtonText;
    [SerializeField] private Button interactAlternateButton;
    [SerializeField] private TextMeshProUGUI interactAlternateButtonText;
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI pauseButtonText;
    [SerializeField] private Transform pressToRebindKeyTransform;

    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private TextMeshProUGUI gamepadInteractButtonText;
    [SerializeField] private Button gamepadInteractAlternateButton;
    [SerializeField] private TextMeshProUGUI gamepadInteractAlternateButtonText;
    [SerializeField] private Button gamepadPauseButton;
    [SerializeField] private TextMeshProUGUI gamepadPauseButtonText;

    private Action OnCloseButtonAction;

    private void Awake() {

        Instance = this;

        soundEffectsButton.onClick.AddListener(() => {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() => {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() => {
            Hide();
            OnCloseButtonAction();
        });
        moveUpButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Move_Up);
        });
        moveDownButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Move_Down);
        });
        moveLeftButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Move_Left);
        });
        moveRightButton.onClick.AddListener(() => { 
        RebindBinding(GameInput.Binding.Move_Right);
        });
        interactButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Interact);
        });
        interactAlternateButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.InteractAlternate);
        });
        pauseButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Pause);
        });
        gamepadInteractButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.GamePad_Interact);
        });
        gamepadInteractAlternateButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.GamePad_InteractAlternate);
        });
        gamepadPauseButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.GamePad_Pause);
        });
    }

    private void Start() {
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;
        UpdateVisual();
        Hide();
        HidePressToRebindKey();
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e) {
        Hide();
    }

    private void UpdateVisual() {
        soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);
        moveUpButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlternateButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        pauseButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        gamepadInteractButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Interact);
        gamepadInteractAlternateButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_InteractAlternate);
        gamepadPauseButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Pause);
    }

    public void Show(Action OnCloseButtonAction) {
        this.OnCloseButtonAction = OnCloseButtonAction;
        gameObject.SetActive(true);

        soundEffectsButton.Select();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey() {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey() {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }


    private void RebindBinding(GameInput.Binding binding) {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () => {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}
