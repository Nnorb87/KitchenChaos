using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour{

    public static GameInput Instance;
    private const string PLAYER_PREFS_BINDINGS = "InputBinding";

    public enum Binding {
        Move_Up, 
        Move_Down, 
        Move_Left, 
        Move_Right, 
        Interact, 
        InteractAlternate,
        Pause,
        GamePad_Interact,
        GamePad_InteractAlternate,
        GamePad_Pause
    }

    private PlayerInputActions playerInputAction;
    public event EventHandler OnInteract;
    public event EventHandler OnInteractAlternate;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;
    private void Awake() {
        Instance = this;
        playerInputAction = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS)) {
            playerInputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        playerInputAction.Player.Enable();
        playerInputAction.Player.Interact.performed += Interact_performed;
        playerInputAction.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputAction.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy() {
        playerInputAction.Player.Interact.performed -= Interact_performed;
        playerInputAction.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputAction.Player.Pause.performed -= Pause_performed;

        playerInputAction.Dispose();

    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAlternate?.Invoke(this,EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {

        Vector2 inputVector = playerInputAction.Player.Move.ReadValue<Vector2>();
        
        inputVector = inputVector.normalized;

        return inputVector;

    }

    public string GetBindingText (Binding binding) {
     
        switch (binding) {

            default:
            case Binding.Interact:
                return playerInputAction.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInputAction.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputAction.Player.Pause.bindings[0].ToDisplayString();
            case Binding.Move_Up:
                return playerInputAction.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputAction.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputAction.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputAction.Player.Move.bindings[4].ToDisplayString();
            case Binding.GamePad_Interact:
                return playerInputAction.Player.Interact.bindings[1].ToDisplayString();
            case Binding.GamePad_InteractAlternate:
                return playerInputAction.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Binding.GamePad_Pause:
                return playerInputAction.Player.Pause.bindings[1].ToDisplayString();
        }

    }

    public void RebindBinding (Binding binding, Action OnActionRebound) {

        InputAction inputAction;
        int bindingIndex;

        switch (binding) {
            default:
            case Binding.Move_Up:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputAction.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = playerInputAction.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputAction.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.GamePad_Interact:
                inputAction = playerInputAction.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.GamePad_InteractAlternate:
                inputAction = playerInputAction.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.GamePad_Pause:
                inputAction = playerInputAction.Player.Pause;
                bindingIndex = 1;
                break;
        }



        playerInputAction.Player.Disable();
        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback => {
                callback.Dispose();
                playerInputAction.Player.Enable();
                OnActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputAction.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();


       

    }

}
