using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour{

    [SerializeField] TextMeshProUGUI keyMoveUpText;
    [SerializeField] TextMeshProUGUI keyMoveDownText;
    [SerializeField] TextMeshProUGUI keyMoveLeftText;
    [SerializeField] TextMeshProUGUI keyMoveRightText;
    [SerializeField] TextMeshProUGUI keyGamepadMoveText;
    [SerializeField] TextMeshProUGUI keyInteractText;
    [SerializeField] TextMeshProUGUI keyGamepadInteractText;
    [SerializeField] TextMeshProUGUI keyAlternateInteractText;
    [SerializeField] TextMeshProUGUI keyGamepadAlternateInteractText;
    [SerializeField] TextMeshProUGUI keyPauseText;
    [SerializeField] TextMeshProUGUI keyGamepadPauseText;


    private void Start() {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        UpdateVisual();
        Show();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (KitchenGameManager.Instance.IsCountdownToStartActive()) {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        keyInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        keyGamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Interact);
        keyAlternateInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        keyGamepadAlternateInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_InteractAlternate);
        keyPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        keyGamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Pause);
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject?.SetActive(false); 
    }
}


